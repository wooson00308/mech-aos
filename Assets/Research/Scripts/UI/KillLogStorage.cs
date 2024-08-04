using System.Collections.Generic;
using UnityEngine;

public class KillLogStorage : MonoBehaviour
{
    [SerializeField] private KillLogUI killLogPrefab;
    private List<KillLogUI> killLogs = new List<KillLogUI>();

    // Method to get a KillLogUI instance
    public KillLogUI GetKillLogUI()
    {
        // Look for an inactive KillLogUI instance
        foreach (var killLogUI in killLogs)
        {
            if (!killLogUI.gameObject.activeSelf)
            {
                killLogUI.gameObject.SetActive(true);
                return killLogUI;
            }
        }

        // Create a new KillLogUI instance if none are available
        KillLogUI newKillLogUI = Instantiate(killLogPrefab, transform);
        killLogs.Add(newKillLogUI);
        return newKillLogUI;
    }
}
