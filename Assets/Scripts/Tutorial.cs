using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<Canvas> pages;

    public void Awake()
    {
        var isTutorial = PlayerPrefs.GetInt("IsTutorial", 0);
        if(isTutorial == 1) gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            pages[0].gameObject.SetActive(true);
            for (var i = 1; i < pages.Count; i++)
            {
                pages[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnExit()
    {
        PlayerPrefs.SetInt("IsTutorial", 1);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}
