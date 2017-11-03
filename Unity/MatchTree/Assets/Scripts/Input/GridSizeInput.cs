using UnityEngine;
using UnityEngine.UI;
using Manager;

namespace GameMenu
{
    public class GridSizeInput : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager;

        [SerializeField]
        Text xText;

        [SerializeField]
        Text yText;

        [SerializeField]
        Button xIncrease;

        [SerializeField]
        Button xDecrease;

        [SerializeField]
        Button yIncrease;

        [SerializeField]
        Button yDecrease;

        int currentXValue;

        int currentYValue;

        int maxX;
        int minX;

        int maxY;
        int minY;

        Vector2 gridSize;

        public Vector2 GridSize
        {
            get
            {
                return gridSize;
            }
        }

        // Use this for initialization
        void Start()
        {
            Vector2 _maxGridSize = gameManager.MaxGridSize;
            Vector2 _minGridSize = gameManager.MinGridSize;
            maxX = (int)_maxGridSize.x;
            maxY = (int)_maxGridSize.y;
            minX = (int)_minGridSize.x;
            minY = (int)_minGridSize.y;

            currentXValue = minX;
            xText.text = currentXValue.ToString();

            currentYValue = minY;
            yText.text = currentYValue.ToString();

            CheckButtonViability("x");
            CheckButtonViability("y");
        }

        // <summary>
        // Increase the X value size by 1, but if larger than max X set it to min X value
        // </summary>
        public void IncreaseAxisValue(string axis)
        {
            switch (axis)
            {
                case "x":
                    currentXValue += 1;
                    xText.text = currentXValue.ToString();
                    break;
                case "y":
                    currentYValue += 1;
                    yText.text = currentYValue.ToString();
                    break;
                default:
                    break; 
            }

            gridSize = new Vector2(currentXValue, currentYValue);
            CheckButtonViability(axis);
        }

        // <summary>
        // Decrease the X value size by 1, but if smaller than min X set it to max X value
        // </summary>
        public void DecreaseAxisValue(string axis)
        {
            switch (axis)
            {
                case "x":
                    currentXValue -= 1;
                    xText.text = currentXValue.ToString();
                    break;
                case "y":
                    currentYValue -= 1;
                    yText.text = currentYValue.ToString();
                    break;
                default:
                    break;
            }

            gridSize = new Vector2(currentXValue, currentYValue);
            CheckButtonViability(axis);
        }

        // <summary>
        // Set if buttons for either x or y in grid vector should be active based on max and min values
        // </summary>
        private void CheckButtonViability(string value)
        {
            switch (value)
            {
                case "x":
                    if (currentXValue + 1 > maxX)
                    {
                        xIncrease.interactable = false;
                    }
                    else
                    {
                        xIncrease.interactable = true;
                    }

                    if (currentXValue - 1 < minX)
                    {
                        xDecrease.interactable = false;
                    }
                    else
                    {
                        xDecrease.interactable = true;
                    }
                    break;

                case "y":
                    if (currentYValue + 1 > maxY)
                    {
                        yIncrease.interactable = false;
                    }
                    else
                    {
                        yIncrease.interactable = true;
                    }

                    if (currentYValue - 1 < minY)
                    {
                        yDecrease.interactable = false;
                    }
                    else
                    {
                        yDecrease.interactable = true;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}