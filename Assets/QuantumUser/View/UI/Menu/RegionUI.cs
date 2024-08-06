using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace QuantumUser
{
    public class RegionUI : MonoBehaviour
    {
        readonly string[] optionsKeys =		{ "USA East",	"Europe",	"Asia"};
        readonly string[] optionsValues =	{ "us",			"eu",		"asia"};

        private void Start()
        {
            if (TryGetComponent(out TMP_Dropdown dropdown))
            {
                dropdown.AddOptions(new List<string>(optionsKeys));
                dropdown.onValueChanged.AddListener((index) =>
                {
                    Matchmaker.AppSettings.FixedRegion = optionsValues[index];
                    PlayerPrefs.SetString("FixedRegion", optionsValues[index]);
                });

                string regionPref = PlayerPrefs.GetString("FixedRegion", null);
                if (regionPref != null)
                {
                    Matchmaker.AppSettings.FixedRegion = regionPref;
                }
			
                int curIndex = optionsValues.ToList().IndexOf(Matchmaker.AppSettings.FixedRegion);
                if (curIndex != -1)
                {
                    dropdown.value = curIndex;
                }
            }
        }
    }
}