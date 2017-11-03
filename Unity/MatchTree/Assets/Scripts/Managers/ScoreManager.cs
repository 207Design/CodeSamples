using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        Text multiplierText;

        [SerializeField]
        List<int> multiplierLevels = new List<int>();

        int amountOfTilesSelected;

        int multiplierValue;

        public int MultiplierValue
        {
            get
            {
                return multiplierValue;
            }
        }

        // Use this for initialization
        void Start()
        {
            AmountSelectedTiles(0);
        }

        // <summary>
        // Set multiplier value based on amount of selected tiles and values in multiplierLevels
        // </summary>
        public void AmountSelectedTiles(int tiles)
        {
            amountOfTilesSelected = tiles;

            for (int i = 0; i < multiplierLevels.Count; i++)
            {
                if (amountOfTilesSelected < multiplierLevels[i])
                {
                    multiplierValue = i + 1;
                    break;
                }
            }
            multiplierText.text = multiplierValue.ToString();
        }
    }
}