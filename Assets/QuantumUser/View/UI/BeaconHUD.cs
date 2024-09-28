using Quantum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeaconHUD : MonoBehaviour
{
    public Image gage;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void UpdateHUD(Team team, float gageValue, float maxGage)
    {
        float normalize = Mathf.Clamp01(gageValue / maxGage);
        gage.fillAmount = normalize;
    }

    public void UpdatePosition(Camera camera, Transform beacon, Vector3 offset)
    {
        var screenPosition = camera.WorldToScreenPoint(beacon.position);

        if (screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height)
        {
            rect.gameObject.SetActive(true);
            rect.position = screenPosition + offset;
        }
        else
        {
            rect.gameObject.SetActive(false);
        }
    }
}
