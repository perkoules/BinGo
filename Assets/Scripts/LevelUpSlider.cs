using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class LevelUpSlider : MonoBehaviour
{
    private Slider levelSlider;

	private PlayerDataSaver playerDataSaver;
	private Dictionary<int, int> levelRubbishCombo;

    private void OnEnable()
    {
		playerDataSaver = GetComponent<PlayerDataSaver>();
		levelRubbishCombo = new Dictionary<int, int>()
		{ 
			{ 1, 20   },
			{ 2, 50   },
			{ 3, 100  },
			{ 4, 200  },
			{ 5, 300  },
			{ 6, 500  },
			{ 7, 700  },
			{ 8, 1000 },
			{ 9, 1250 },
			{ 10,1500 },
			{ 11,1750 },
			{ 12,2000 },
			{ 13,2500 },
			{ 14,3000 },
			{ 15,5000 } 
		};
		levelSlider = GetComponent<Slider>();
		Filling();
    }

    public void Filling()
    {
		int rub = playerDataSaver.GetWasteCollected() + playerDataSaver.GetRecycleCollected();
		int lvl = playerDataSaver.GetProgressLevel();
		int max = levelRubbishCombo[lvl + 1];
		int min = levelRubbishCombo[lvl];

		levelSlider.maxValue = max;
		levelSlider.minValue = min;

		levelSlider.value = rub;
	}
}
