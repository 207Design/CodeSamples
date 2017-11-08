using UnityEngine;

namespace Tile
{
    public class BackgroundTile : MonoBehaviour
    {
        [SerializeField]
        MeshRenderer meshRenderer;

        // <summary>
        // Set material for this background tile
        // </summary>
        public void SetTileData(Material material)
        {
            meshRenderer.material = material;
        }
    }
}