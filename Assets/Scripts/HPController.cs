using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour {
    public Slider hpSlider;

    public void UpdateValue(float ratio) {
        hpSlider.value = ratio;
    }

    public void Reset() {
        hpSlider.value = 1f;
    }
}
