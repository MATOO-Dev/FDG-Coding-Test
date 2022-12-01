using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] CombatEntity mAssociatedEntity;
    [SerializeField] Slider mHealthSlider;
    [SerializeField] Slider mShieldSlider;
    [SerializeField] Slider mAbilitySlider;

    void LateUpdate()
    {
        transform.LookAt(transform.position + GameManager.GMInstance.mMainCamera.transform.forward);
    }

    public void SetHealthFill(float fillValue)
    {
        mHealthSlider.value = Mathf.Clamp(fillValue, 0, 1);
    }

    public void SetShieldFill(float fillValue)
    {
        mShieldSlider.value = Mathf.Clamp(fillValue, 0, 1);
    }

    public void SetAbilityFill(float fillValue)
    {
        mAbilitySlider.value = Mathf.Clamp(fillValue, 0, 1);
    }
}
