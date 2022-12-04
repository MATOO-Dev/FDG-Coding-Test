using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Slider mHealthSlider;
    [SerializeField] Slider mShieldSlider;
    [SerializeField] Slider mAbilitySlider;

    public void SetHealthSliderFill(float fraction)
    {
        mHealthSlider.value = Mathf.Clamp(fraction, 0, 1);
    }
    public void SetShieldSliderFill(float fraction)
    {
        mShieldSlider.value = Mathf.Clamp(fraction, 0, 1);
    }
    public void SetAbilitySliderFill(float fraction)
    {
        mAbilitySlider.value = Mathf.Clamp(fraction, 0, 1);
    }
}
