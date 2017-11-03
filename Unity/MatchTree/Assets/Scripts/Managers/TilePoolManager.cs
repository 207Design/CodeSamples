using System.Collections.Generic;
using UnityEngine;
using Tile;

namespace Manager
{
    public class TilePoolManager : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager;

        [SerializeField]
        GameObject backgroundTilePrefab;

        [SerializeField]
        GameObject playableTilePrefab;

        Vector2 gridSize;

        // Lists with instantiated tiles
        List<PlayableTile> playableTilesPool = new List<PlayableTile>();
        List<BackgroundTile> backgroundTilesPool = new List<BackgroundTile>();

        // Folder objects for tiles
        GameObject playablePoolParent;
        GameObject backgroundPoolParent;

        // Use this for initialization
        void Start()
        {
            gridSize = gameManager.MaxGridSize;
            FillPlayableTilePool();
            FillBackgroundTilePool();
        }

        // <summary>
        // Instantiate playable tiles and fill up the tile pool to cover the tile need based on max board size
        // </summary>
        private void FillPlayableTilePool()
        {
            if (playablePoolParent == null)
            {
                playablePoolParent = CreateFolder("PlayablePoolParent", transform);
            }
            int _poolSize = ((int)gridSize.x + 4) * ((int)gridSize.y);
            for (int i = playableTilesPool.Count; i < _poolSize; i++)
            {
                PlayableTile _playableTile = Instantiate(playableTilePrefab).GetComponent<PlayableTile>();
                TileToPlayablePool(_playableTile);
            }
        }

        // <summary>
        // Instantiate background tiles and fill up the background tiles pool to cover the tile need based on max board size
        // </summary>
        private void FillBackgroundTilePool()
        {
            if (backgroundPoolParent == null)
            {
                backgroundPoolParent = CreateFolder("BackgroundPoolParent", transform);
            }
            int _poolSize = ((int)gridSize.x) * ((int)gridSize.y);
            for (int i = backgroundTilesPool.Count; i < _poolSize; i++)
            {
                BackgroundTile _backgroundTile = Instantiate(backgroundTilePrefab).GetComponent<BackgroundTile>();
                TileToBackgroundPool(_backgroundTile);
            }
        }

        // <summary>
        // Return a tile from the playable tiles pool
        // </summary>
        public PlayableTile TileFromPlayablePool()
        {
            if (playableTilesPool.Count > 0)
            {
                PlayableTile _playableTile = playableTilesPool[0];
                playableTilesPool.RemoveAt(0);
                _playableTile.transform.gameObject.SetActive(true);
                return _playableTile;
            }
            else
            {
                PlayableTile _playableTile = Instantiate(playableTilePrefab, playablePoolParent.transform).GetComponent<PlayableTile>();
                return _playableTile;
            }
        }

        // <summary>
        // Add returned tile to pool of playable tiles
        // </summary>
        public void TileToPlayablePool(PlayableTile tile)
        {
            tile.transform.parent = playablePoolParent.transform;
            playableTilesPool.Add(tile);
            tile.transform.gameObject.SetActive(false);
        }

        // <summary>
        // Return a tile from the background tiles pool
        // </summary>
        public BackgroundTile TileFromBackgroundPool()
        {
            if (backgroundTilesPool.Count > 0)
            {
                BackgroundTile _backgroundTile = backgroundTilesPool[0];
                backgroundTilesPool.RemoveAt(0);
                _backgroundTile.transform.gameObject.SetActive(true);
                return _backgroundTile;
            }
            else
            {
                BackgroundTile _backgroundTile = Instantiate(backgroundTilePrefab, backgroundPoolParent.transform).GetComponent<BackgroundTile>();
                return _backgroundTile;
            }
        }

        // <summary>
        // Add returned tile to pool of background tiles
        // </summary>
        public void TileToBackgroundPool(BackgroundTile tile)
        {
            tile.transform.parent = backgroundPoolParent.transform;
            backgroundTilesPool.Add(tile);
            tile.transform.gameObject.SetActive(false);
        }

        // <summary>
        // Create a folder for specified set of tiles
        // </summary>
        public GameObject CreateFolder(string folderName, Transform parent)
        {
            GameObject _newFolder = new GameObject();
            _newFolder.transform.parent = parent;
            _newFolder.name = folderName;
            _newFolder.transform.localPosition = new Vector3(0, 0, 0);
            return _newFolder;
        }
    }
}