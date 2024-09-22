using JetBrains.Annotations;
using Quantum;
using Quantum.Mech;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
[Preserve]
public unsafe class FixPopupUI : QuantumViewComponent<CustomViewContext>
{
    public GameUI gameUI;
    public CanvasGroup canvasGroup;
    public List<ItmeGroup> itemGroups;
    public PipSelectorUI _pipSelector;

    [Header("Selected Items")]
    public ItmeUI cockpit;
    public ItmeUI armor;
    public ItmeUI weapon;

    private bool _isShow;

    private Frame f;

    private void Awake()
    {
        f = QuantumRunner.DefaultGame.Frames.Predicted;
        QuantumEvent.Subscribe(this, (EventFix e) => { ShowAndClose(); });
    }

    public void OnSelectCockpit()
    {
        OnSelectItem(ItemType.Cockpit);
    }

    public void OnSelectArmor()
    {
        OnSelectItem(ItemType.Armor);
    }

    public void OnSelectWeapon()
    {
        OnSelectItem(ItemType.Weapon);
    }

    public void OnSelectItem(ItemType type)
    {
        _pipSelector.Show();

        var itemGroup = itemGroups.Find(x => x.type == type);
        if (itemGroup == null) return;

        _pipSelector.Show(itemGroup.items);
    }

    public void ShowAndClose()
    {
        _isShow = !_isShow;

        if (_isShow) Show();
        else Close();
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
    }

    public void Close()
    {
        _pipSelector.Close();
        canvasGroup.alpha = 0;
    }
}

public enum ItemType
{
    Weapon, Armor, Cockpit
}

[Serializable]
public class ItmeGroup
{
    public ItemType type;
    public List<Item> items;
}

[Serializable]
public class ItmeUI
{
    public TMP_Text itemName;
    public TMP_Text description;
    public Image icon;
    public Image selectFx;

    public void Setup(Item item)
    {
        itemName.text = item.rootName;
        if(description != null)
        {
            description.text = item.description;
        }
        icon.sprite = item.icon;

        if(selectFx != null)
        {
            selectFx.enabled = item.isSelected;
        }
    }
}

[Serializable]
public class Item
{
    public string rootName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public bool isSelected;
}