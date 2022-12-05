using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] CombatEntity mAssociatedEntity;    //entity that this health bar is attached to
    [SerializeField] Slider mHealthSlider;              //slider component reference for health
    [SerializeField] Slider mShieldSlider;              //slider component reference for shield
    [SerializeField] Slider mAbilitySlider;             //slider component reference for cooldown/ability charge

    void LateUpdate()
    {
        //billboard, so the healthbar always faces the camera
        transform.LookAt(transform.position + GameManager.GMInstance.mMainCamera.transform.forward);
    }

    //set fill value for health
    public void SetHealthFill(float fraction)
    {
        mHealthSlider.value = Mathf.Clamp(fraction, 0, 1);
    }

    //set fill value for shield
    public void SetShieldFill(float fraction)
    {
        mShieldSlider.value = Mathf.Clamp(fraction, 0, 1);
    }

    //set fill value for ability charge
    public void SetAbilityFill(float fraction)
    {
        mAbilitySlider.value = Mathf.Clamp(fraction, 0, 1);
    }
}
