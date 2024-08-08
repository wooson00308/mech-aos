using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Quantum;
using Quantum.Mech;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
public class SkillButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Button button;
    public Image cooltimeImage;
    private bool isCooldown = false;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Update()
    {
        button.interactable = isCooldown;
    }
    public void OnActivate(bool value)
    {
        if (button == null) return;
        button.interactable = value;
    }
    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
    public void UpdateCooltime(float currentCooltime, float maxCooltime)
    {
        isCooldown = currentCooltime > 0;
        if (maxCooltime == 0)
        {
            cooltimeImage.fillAmount = 0;
            return;
        }
        cooltimeImage.fillAmount = Mathf.Clamp01(currentCooltime / maxCooltime);
    }
    [InputControl(layout = "Button")]
    [SerializeField]
    private string _controlPath;
    protected override string controlPathInternal
    {
        get => _controlPath;
        set => _controlPath = value;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!control.CheckStateIsAtDefault())
            SentDefaultValueToControl();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isCooldown)
            SendValueToControl(1.0f);
    }
}