using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipSelectorUI : QuantumMonoBehaviour
{
    public FixPopupUI fixUI;
    public CanvasGroup canvasGroup;

    public PipUI pipPrefab;
    public Transform pipParent;

    private List<PipUI> _pipStorage = new();

    private bool _isShow;
    public bool IsShow => _isShow;

    public void Reset()
    {
        foreach(var pip in _pipStorage)
        {
            Destroy(pip.gameObject);
        }

        _pipStorage.Clear();
    }

    public void Setup(List<Item> itemList) 
    {
        foreach(var item in itemList)
        {
            var pip = Instantiate(pipPrefab, pipParent).GetComponent<PipUI>();
            pip.Setup(this, item);
            _pipStorage.Add(pip);
        }
    }

    public Item GetItem(string itemName)
    {
        if (_pipStorage.Count == 0) return null;

        var item = _pipStorage.Find(x => x.itemData.rootName == itemName);
        if (item == null) return null;

        return item.itemData;
    }

    public void Show(List<Item> itemList)
    {
        _isShow = true;
        Reset();
        canvasGroup.alpha = 1;

        Setup(itemList);
    }

    public void Close()
    {
        _isShow = false;
        canvasGroup.alpha = 0;
    }
}
