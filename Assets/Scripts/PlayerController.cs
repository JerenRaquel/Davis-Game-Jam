using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    #region Class Instance
    public static PlayerController instance = null;
    private void CreateInstance() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    private void Awake() {
        CreateInstance();
        inputMap = new ControlMap();
    }

    [Header("Settings")]
    public float speed = 6f;
    public float dashForce = 2f;
    public float dashTime;
    public float dashDelay;

    private ControlMap inputMap;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float time;
    private bool dashEnabled = true;
    private bool isDashing = false;

    private void OnEnable() {
        inputMap.Enable();
        inputMap.Combat.Move.performed += ctx => OnMove(ctx);
        inputMap.Combat.Move.canceled += ctx => OnMove(ctx);
        inputMap.Combat.Dash.performed += _ => OnDash();
        inputMap.Combat.Dash.canceled += _ => OnDash();
    }

    private void OnDisable() {
        inputMap.Combat.Move.performed -= ctx => OnMove(ctx);
        inputMap.Combat.Move.canceled -= ctx => OnMove(ctx);
        inputMap.Combat.Dash.performed -= _ => OnDash();
        inputMap.Combat.Dash.canceled -= _ => OnDash();
        inputMap.Disable();
    }

    private void Start() {
        time = Time.time;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if (!isDashing) {
            transform.position = transform.position + new Vector3(direction.x, direction.y, 0) * Time.deltaTime * speed;

            if (time + dashDelay <= Time.time && !isDashing) {
                time = Time.time;
                dashEnabled = true;
            }
        }
    }

    private void OnMove(InputAction.CallbackContext ctx) {
        this.direction = ctx.ReadValue<Vector2>();
    }

    private void OnDash() {
        if (!dashEnabled || isDashing) return;
        if (direction == Vector2.zero) return;
        dashEnabled = false;
        StartCoroutine(Dash());
    }

    private IEnumerator Dash() {
        isDashing = true;
        rb.velocity = direction * dashForce;
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForEndOfFrame();
    }
}
