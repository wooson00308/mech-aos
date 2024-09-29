using Quantum;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.Scripting;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
[Preserve]
public unsafe class PipUI : QuantumMonoBehaviour
{
    public Item itemData;
    public ItmeUI ui;

    private PipSelectorUI selector;

    private Frame f;

    public SkillButton button;

    private void Awake()
    {
        f = QuantumRunner.DefaultGame.Frames.Predicted;
    }

    public void Setup(PipSelectorUI selector, Item itemData) 
    {
        if (itemData == null) return;

        this.selector = selector;
        this.itemData = itemData;

        ui.Setup(itemData);
    }

    public void OnSelect()
    {
        if(!ui.selectFx.enabled)
        {
            ui.selectFx.enabled = true;
            var items = selector.fixUI.itemGroups.Find(x => x.type == itemData.type).items;
            foreach (var item in items)
            {
                item.isSelected = item.rootName == itemData.rootName;
            }

            switch(itemData.type)
            {
                case ItemType.Cockpit:
                    selector.fixUI.cockpit.Setup(itemData);
                    break;
                case ItemType.Armor:
                    selector.fixUI.armor.Setup(itemData);
                    break;
                case ItemType.Weapon:
                    selector.fixUI.weapon.Setup(itemData);
                    button.OnEvent();
                    break;
            }
        }

        selector.Close();
    }
}
