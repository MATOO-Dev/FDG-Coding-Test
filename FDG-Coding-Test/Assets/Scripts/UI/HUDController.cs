using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Slider mHealthSlider;          //slider component reference for healthbar
    [SerializeField] Slider mShieldSlider;          //slider component reference for shield bar
    [SerializeField] Slider mAbilitySlider;         //slider component reference for ability charge
    [SerializeField] RectTransform mInGameHUD;      //reference to ingame hud container
    [SerializeField] RectTransform mVictoryHUD;     //reference to victory hud container
    [SerializeField] Text mVictoryText;             //reference to text component of victory hud

    void Start()
    {  
        //make victory hud invisible
        mVictoryHUD.gameObject.SetActive(false);
    }


    //set fill value for health
    public void SetHealthSliderFill(float fraction)
    {
        mHealthSlider.value = Mathf.Clamp(fraction, 0, 1);
    }

    //set fill value for shield
    public void SetShieldSliderFill(float fraction)
    {
        mShieldSlider.value = Mathf.Clamp(fraction, 0, 1);
    }

    //set fill value for ability charge
    public void SetAbilitySliderFill(float fraction)
    {
        mAbilitySlider.value = Mathf.Clamp(fraction, 0, 1);
    }

    //reset shield fill
    public void ResetShieldSliderFill()
    {
        mShieldSlider.value = 0;
    }

    //deactivate normal hud and activate victory hud instead
    public void ActivateVictoryHud(CombatEntity winningEntity)
    {
        mInGameHUD.gameObject.SetActive(false);
        mVictoryHUD.gameObject.SetActive(true);
        mVictoryText.text = "!Victory!\n" + winningEntity.gameObject.name + " has won!";
    }
}
