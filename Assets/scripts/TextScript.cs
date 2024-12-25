using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    
    public TextMeshProUGUI textMeshPro;
    public Slider slider;
    void Start()
    {
        textMeshPro.text = $"Charge : {slider.value:F2}";
    }

    void Update()
    {
        textMeshPro.text = $"Charge : {slider.value:F2}";
    }
}
