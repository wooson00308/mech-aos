using Quantum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : QuantumMonoBehaviour
{
    public Team Team;

    public TMP_Text nicknameText;
    public TMP_Text healthText;           
    public Image healthImage;

    private float maxHealth;
    private bool isInitialized = false;
    public bool IsInitialized => isInitialized;

    public void Initialized(float health, Team team)
    {
        isInitialized = true;

        Team = team;
        maxHealth = health;
        UpdateHealth(health);
    } 

    public void UpdateHealth(float health)
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
