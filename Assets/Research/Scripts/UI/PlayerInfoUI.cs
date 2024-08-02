using Quantum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : QuantumMonoBehaviour
{
    public TMP_Text nicknameText;
    public TMP_Text healthText;           
    public Image healthImage;

    [Header("Test")]
    public string nickname;
    public float health;
    public float maxHealth;

    private void Start()
    {
        SetPlayer(nickname);
        UpdateHealth(health, maxHealth);
    }

    public void UpdateHealth(float health, float maxHealth = 100)
    {
        float healthPercentage = (health / maxHealth) * 100f;
        healthText.text = $"{healthPercentage:F1}%";

        float normalizedScale = Mathf.Clamp01(health / maxHealth);
        healthImage.rectTransform.localScale = new Vector3(normalizedScale, 1f, 1f);
    }

    public void SetPlayer(string nickname)
    {
        nicknameText.text = nickname;
    }
}
