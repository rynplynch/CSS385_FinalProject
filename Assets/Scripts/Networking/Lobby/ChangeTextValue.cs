using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeTextValue : MonoBehaviour
{
    private TextMeshProUGUI text;
    public Slider slider;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void ChangeValue()
    {
        text.text = "" + slider.value;
    }
}
