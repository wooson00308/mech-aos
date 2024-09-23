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

    [Header("Level")]
    public TMP_Text level;
    public TMP_Text levelPoint;
    public TMP_Text increaseHpPercent;
    public TMP_Text increaseAttackPercent;

    private bool _isShow;

    private Frame f;

    private float increaseHpPercentValue;
    private float increaseAttackPercentValue;
    private int levelPointValue;

    private void Awake()
    {
        f = QuantumRunner.DefaultGame.Frames.Predicted;
        QuantumEvent.Subscribe(this, (EventFix e) => { ShowAndClose(); });
    }

    public void Levelup(EntityRef entity)
    {
        if (gameUI.LocalEntityRef.ToString() != entity.ToString()) return;

        level.text = $"{int.Parse(level.text) + 1}";

        levelPointValue++;
        levelPoint.text = $"{levelPointValue:00}";
    }

    public void OnUpgradeHP()
    {
        if (levelPointValue <= 0) return;

        // 여기에 서버에 능력치가 오른 것을 호출해야함
        increaseHpPercentValue += 10;
        increaseHpPercent.text = $"{increaseHpPercentValue}%";
        levelPoint.text = $"{--levelPointValue}";
    }

    public void OnUpgradeAttack()
    {
        if (levelPointValue <= 0) return;

        // 위와 동일
        increaseAttackPercentValue += 10;
        increaseHpPercent.text = $"{increaseAttackPercentValue}%";
        levelPoint.text = $"{--levelPointValue}";
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
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        _pipSelector.Close();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
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