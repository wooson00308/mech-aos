using Quantum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHUD : QuantumMonoBehaviour
{
    public TMP_Text nicknameText;
    public TMP_Text level;
    public Image healthImage;

    [Header("Test")]
    public string nickname;
    public float health;
    public float maxHealth;

    public EntityRef entityRef;

    public void UpdateHealth(float health, float maxHealth = 100)
    {
        float normalized = Mathf.Clamp01(health / maxHealth);
        healthImage.fillAmount = normalized;
    }

    public void SetPlayer(string nickname, EntityRef entityRef)
    {
        this.entityRef = entityRef;
        nicknameText.text = nickname;
    }
}
