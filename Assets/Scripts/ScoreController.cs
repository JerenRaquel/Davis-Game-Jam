using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {
    #region Class Instance
    public static ScoreController instance = null;
    private void CreateInstance() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    private void Awake() => CreateInstance();

    public TMPro.TextMeshProUGUI textbox;

    private int score;

    public void TickScore() {
        score++;
        textbox.text = "Score: " + score.ToString();
    }
}
