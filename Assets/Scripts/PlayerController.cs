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

    public HPController playerHPBar;
    public Animator animator;

    [Header("Settings")]
    public float speed = 6f;
    public float dashForce = 2f;
    public float dashTime;
    public float dashDelay;
    public int maxHealth;
    public int Health { get { return this.currentHealth; } }

    private ControlMap inputMap;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float time;
    private bool dashEnabled = true;
    private bool isDashing = false;
    private int currentHealth;

    private void OnEnable() {
        inputMap.Enable();
        inputMap.Combat.Move.performed += ctx => OnMove(ctx);
        inputMap.Combat.Move.canceled += ctx => OnMove(ctx);
        inputMap.Combat.Dash.performed += _ => OnDash();
        inputMap.Combat.Dash.canceled += _ => OnDash();
        inputMap.Combat.Swing.performed += _ => OnSwing();
    }

    private void OnDisable() {
        inputMap.Combat.Move.performed -= ctx => OnMove(ctx);
        inputMap.Combat.Move.canceled -= ctx => OnMove(ctx);
        inputMap.Combat.Dash.performed -= _ => OnDash();
        inputMap.Combat.Dash.canceled -= _ => OnDash();
        inputMap.Combat.Swing.performed -= _ => OnSwing();
        inputMap.Disable();
    }

    private void Start() {
        time = Time.time;
        rb = this.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        playerHPBar.UpdateValue(this.currentHealth / maxHealth);
    }

    private void FixedUpdate() {
        if (!isDashing) {
            transform.position = transform.position + new Vector3(direction.x, direction.y, 0) * Time.deltaTime * speed;
            if (direction.x > 0) {
                transform.localScale = new Vector3(1, 1, 1);
            } else if (direction.x < 0) {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (time + dashDelay <= Time.time && !isDashing) {
                time = Time.time;
                dashEnabled = true;
            }
        }
    }

    public void TakeDamage(int damage) {
        this.currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        playerHPBar.UpdateValue(this.currentHealth / maxHealth);
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

    private void OnSwing() {
        animator.SetTrigger("Swing");
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
