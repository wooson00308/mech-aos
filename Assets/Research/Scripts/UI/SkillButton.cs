using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class SkillButton : MonoBehaviour
{
    private Button button;

    public Image cooltimeImage;

    bool isCooldown = false;

    private void Awake()
    {
        button = GetComponent<Button>(); 
    }

    private void Update()
    {
        button.interactable = isCooldown;
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public void UpdateCooltime(float currentCooltime, float maxCooltime)
    {
        isCooldown = currentCooltime > 0;
        cooltimeImage.fillAmount = Mathf.Clamp01(currentCooltime / maxCooltime);
    }
}
