using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Slider mHealthSlider;
    [SerializeField] Slider mShieldSlider;
    [SerializeField] Slider mAbilitySlider;
    [SerializeField] RectTransform mInGameHUD;
    [SerializeField] RectTransform mVictoryHUD;
    [SerializeField] Text mVictoryText;

    void Start()
    {
        mVictoryHUD.gameObject.SetActive(false);
    }

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

    public void ResetShieldSliderFill()
    {
        mShieldSlider.value = 0;
    }

    public void ActivateVictoryHud(CombatEntity winningEntity)
    {
        mInGameHUD.gameObject.SetActive(false);
        mVictoryHUD.gameObject.SetActive(true);
        mVictoryText.text = "!Victory!\n" + winningEntity.gameObject.name + " has won!";
    }
}
