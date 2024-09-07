using Quantum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillLogUI : QuantumMonoBehaviour
{
    public TMP_Text killedPlayerText;
    public TMP_Text killerPlayerText;

    private bool isShow = false;
    private float time = 0;

    [SerializeField] private float duration = 3f;

    public void Show(string killedPlayer, string killerPlayer)
    {
        if (isShow) return;
        isShow = true;

        time = 0f;

        gameObject.SetActive(true);

        killedPlayerText.text = killedPlayer;
        killerPlayerText.text = killerPlayer;
    }

    public void Update()
    {
        if (isShow)
        {
            time += Time.deltaTime;
            if (time >= duration)
            {
                Hide();
            }
        }
    }

    private void Hide()
    {
        isShow = false;
        gameObject.SetActive(false);
    }
}
