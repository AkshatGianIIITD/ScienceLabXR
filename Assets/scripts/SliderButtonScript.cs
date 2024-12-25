using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderButtonScript : MonoBehaviour
{
    public Slider targetSlider; 

    public void IncrementValue()
    {
        if (targetSlider != null)
        {
            // Increase the slider value by 1, clamping it to the slider's maximum value
            targetSlider.value = Mathf.Min(targetSlider.value + 1, targetSlider.maxValue);
        }
        else
        {
            Debug.LogError("Target Slider is not assigned.");
        }
    }
    public void DecrementValue()
    {
        if (targetSlider != null)
        {
            // Increase the slider value by 1, clamping it to the slider's maximum value
            targetSlider.value = Mathf.Max(targetSlider.value - 1, targetSlider.minValue);
        }
        else
        {
            Debug.LogError("Target Slider is not assigned.");
        }
    }
}
