using System.Collections.Generic;
using UnityEngine;

namespace Tile
{
    public class PathRenderer : MonoBehaviour
    {
        [SerializeField]
        LineRenderer lineRenderer;

        // <summary>
        // Draw lines between selected tiles
        // </summary>
        public void SetLineRendererPositions(List<PlayableTile> positions)
        {
            lineRenderer.positionCount = positions.Count;
            for (int i = 0; i < positions.Count; i++)
            {
                lineRenderer.SetPosition(i, positions[i].transform.position);
            }
        }

        // <summary>
        // Set line renderer lenght to 0
        // </summary>
        public void ClearLineRendererPositions()
        {
            lineRenderer.positionCount = 0;
        }
    }
}
