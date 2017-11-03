using System.Collections.Generic;
using UnityEngine;
using Tile;

namespace Manager
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager;

        [SerializeField]
        AudioManager audioManager;

        [SerializeField]
        ScoreManager scoreManager;

        [SerializeField]
        TilePoolManager tilePoolManager; 

        [SerializeField]
        PathRenderer pathRenderer;

        [SerializeField]
        List<Material> tileTypes = new List<Material>();

        [SerializeField]
        Material backgroundNotSelected;

        [SerializeField]
        Material backgroundSelected;

        PlayableTile[,] activePlayableTiles;
        BackgroundTile[,] activeBackgroundTiles;
        int[,] tileLayoutAtStart;

        List<PlayableTile> selectedTiles = new List<PlayableTile>();
        BackgroundTile previousBackgroundTile;

        GameObject activeTilesParent;
        GameObject backgroundTilesParent;

        int selectedTileType;
        Vector2 selectedGridPTile;

        Vector2 gridSize = new Vector2(8, 6);

        public Vector2 GridSize
        {
            set
            {
                gridSize = value;
            }
        }

        bool canSelectTiles;

        public bool CanSelectTiles
        {
            get 
            {
                return canSelectTiles;
            }
        }

        // Helper List for when checking adjecent tiles
        List<Vector2> gridMath = new List<Vector2>(8) { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(-1, -1) };

        // <summary>
        // Initalize oard with provided settings
        // CreateStartLayout will be run until a board with at least one solution has been generated
        // Set this objects position to center board on screen
        // </summary>
        public void InitializeGameBoard()
        {
            transform.position = new Vector3(0 - (gridSize.x * 5 - 5), 5 - (gridSize.y * 10) / 2, 10);
            PrintBoard();
            while (!CreateStartLayout())
            {
            }
            AddPlayableTiles(true);
            canSelectTiles = true;
        }

        // <summary>
        // Set size of arrays for playable tiles and background tiles
        // Get background tile from pool and add to the board grid
        // </summary>
        private void PrintBoard()
        {
            if (backgroundTilesParent == null)
            {
                backgroundTilesParent = tilePoolManager.CreateFolder("BackgroundTiles", transform);
            }

            activePlayableTiles = new PlayableTile[(int)gridSize.x, (int)gridSize.y];
            activeBackgroundTiles = new BackgroundTile[(int)gridSize.x, (int)gridSize.y];

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    BackgroundTile _backgroundTile = tilePoolManager.TileFromBackgroundPool();
                    _backgroundTile.transform.parent = backgroundTilesParent.transform;
                    _backgroundTile.transform.localPosition = new Vector3(x * 10, y * 10, 15);
                    activeBackgroundTiles[x, y] = _backgroundTile;
                }
            }
        }

        // <summary>
        // Create the layout that will be used when displaying the board initially
        // Return true if new layout has a solution, and false if it does not
        // </summary>
        private bool CreateStartLayout()
        {
            tileLayoutAtStart = new int[(int)gridSize.x, (int)gridSize.y];
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    tileLayoutAtStart[x, y] = UnityEngine.Random.Range(0, tileTypes.Count);
                }
            }

            // Check so at least one solution is available
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    int _adjecentOfSameType = 0;
                    Vector2 _gridPosition = new Vector2(x, y);
                    int _intTileType = tileLayoutAtStart[x, y];

                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 _nextTile = _gridPosition + gridMath[i];
                        if (_nextTile.x >= 0 && _nextTile.x < gridSize.x && _nextTile.y >= 0 && _nextTile.y < gridSize.y)
                        {
                            if (tileLayoutAtStart[(int)_nextTile.x, (int)_nextTile.y] == _intTileType)
                            {
                                _adjecentOfSameType += 1;
                            }
                        }
                    }
                    if (_adjecentOfSameType >= 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // <summary>
        // Get playable tile from pool and add to the board grid
        // Assign random type to the tile
        // </summary>
        private void AddPlayableTiles(bool isInitialization)
        {
            Vector3 _spawnPositon;
            if (activeTilesParent == null)
            {
                activeTilesParent = tilePoolManager.CreateFolder("ActiveTiles", transform);
            }
            for (int y = 0; y < gridSize.y; y++) 
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    if (activePlayableTiles[x, y] == null)
                    {
                        int _newInt;
                        if (isInitialization)
                        {
                            _spawnPositon = new Vector3(x * 10, y * 10, 0);
                            _newInt = tileLayoutAtStart[x, y];
                        }
                        else
                        {
                            _spawnPositon = new Vector3(x * 10, y * 10 + (gridSize.y + 2) * 10, 0);
                            _newInt = Random.Range(0, tileTypes.Count);
                        }
                        PlayableTile _playableTile = tilePoolManager.TileFromPlayablePool();
                        _playableTile.transform.parent = activeTilesParent.transform;
                        _playableTile.transform.localPosition = _spawnPositon;
                        activePlayableTiles[x, y] = _playableTile;
                        _playableTile.SetTileData(tileTypes[_newInt], _newInt, new Vector2(x, y));
                    }
                }
            }
        }

        // <summary>
        // Tile that should be selected or deselected based on input
        // </summary>
        public void SelectTile(GameObject tile)
        {
            if (canSelectTiles)
            {
                PlayableTile _tile = tile.GetComponent<PlayableTile>();
                if (selectedTiles.Contains(_tile) == false)
                {
                    if (selectedTiles.Count == 0)
                    {
                        // Select a first tile and store tile type and position
                        selectedTileType = _tile.TileType;
                        selectedGridPTile = _tile.GridPosition;
                        _tile.SelectTile();
                        selectedTiles.Add(_tile);

                        HighlightBackgroundTile();

                        audioManager.PlayAudioClip(AudioManager.audioClipEnum.selected);
                        scoreManager.AmountSelectedTiles(selectedTiles.Count);
                    }
                    else
                    {
                        if (_tile.TileType == selectedTileType)
                        {
                            if (TileIsAdjecent(_tile.GridPosition))
                            {
                                // Select a tile adjecent to the previously selected one, and store tile position
                                _tile.SelectTile();
                                selectedTiles.Add(_tile);

                                HighlightBackgroundTile();

                                audioManager.PlayAudioClip(AudioManager.audioClipEnum.selected);
                                scoreManager.AmountSelectedTiles(selectedTiles.Count);
                                pathRenderer.SetLineRendererPositions(selectedTiles);
                            }
                        }
                    }
                }
                else
                {
                    if (selectedTiles.Count > 1)
                    {
                        if (_tile == selectedTiles[selectedTiles.Count - 2])
                        {
                            // A tile is deselected
                            PlayableTile _previousTileObject = selectedTiles[selectedTiles.Count - 1];
                            _previousTileObject.DeselectTile();
                            selectedTiles.Remove(_previousTileObject);
                            selectedGridPTile = _tile.GridPosition;

                            HighlightBackgroundTile();

                            audioManager.PlayAudioClip(AudioManager.audioClipEnum.deselected);
                            scoreManager.AmountSelectedTiles(selectedTiles.Count);
                            pathRenderer.SetLineRendererPositions(selectedTiles);

                        }
                    }
                }
            }
        }

        // <summary>
        // Check if the grid position is adjencent to the currently selected grid tile
        // If true, assign the new position to selected grid tile variable
        // </summary>
        private bool TileIsAdjecent(Vector2 gridPosition)
        {
            for (int i = 0; i < 8; i++)
            {
                if (selectedGridPTile + gridMath[i] == gridPosition)
                {
                    selectedGridPTile = gridPosition;
                    return true;
                }
            }
            return false;
        }

        // <summary>
        // Selection process has been completed, and tiles should be collected or deselected depending on if 3 or more are selected or not
        // </summary>
        public void CheckSelectedTiles()
        {
            if (canSelectTiles)
            {
                if (selectedTiles.Count != 0)
                {
                    if (selectedTiles.Count > 2)
                    {
                        audioManager.PlayAudioClip(AudioManager.audioClipEnum.collected);
                        for (int i = 0; i < selectedTiles.Count; i++)
                        {
                            Vector2 _gridPosition = selectedTiles[i].GridPosition;
                            activePlayableTiles[(int)_gridPosition.x, (int)_gridPosition.y] = null;
                            selectedTiles[i].CollectTile();
                        }
                        canSelectTiles = false;
                    }
                    else
                    {
                        audioManager.PlayAudioClip(AudioManager.audioClipEnum.deselected);
                        for (int x = 0; x < selectedTiles.Count; x++)
                        {
                            selectedTiles[x].DeselectTile();
                        }
                        selectedTiles.Clear();
                    }

                    if (previousBackgroundTile != null)
                    {
                        previousBackgroundTile.SetTileData(backgroundNotSelected);
                        previousBackgroundTile = null;
                    }

                    pathRenderer.ClearLineRendererPositions();
                }
            }
        }

        // <summary>
        // Collect when tiles have called in that they have been successfully collected
        // When all selected has been collected, clear selected and rearrange grid
        // </summary>
        int tilesRemoved;
        public void TileSuccessfullyCollected()
        {
            tilesRemoved += 1;
            if (tilesRemoved >= selectedTiles.Count)
            {
                RearrangeGrid();
                selectedTiles.Clear();
                canSelectTiles = true;
                tilesRemoved = 0;

                scoreManager.AmountSelectedTiles(selectedTiles.Count);
            }
        }

        // <summary>
        // Rearrange the grid, and tell tiles where to move, so that gaps further down in the grid are closed
        // </summary>
        int emptyRows;
        private void RearrangeGrid()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                emptyRows = 0;
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (activePlayableTiles[x, y] == null)
                    {
                        emptyRows += 1;
                    }
                    else
                    {
                        if (emptyRows > 0)
                        {
                            activePlayableTiles[x, y].NewGridPosition(new Vector2(x, y - emptyRows));
                            activePlayableTiles[x, y - emptyRows] = activePlayableTiles[x, y];
                            activePlayableTiles[x, y] = null;
                        }
                    }
                }
            }
            AddPlayableTiles(false);
            CheckIfSolutionIsAvailable();
        }

        // <summary>
        // Check if there are available solutions on the grid
        // For each tile, check how many adjecent tiles have the same tile type
        // If one tile has two or more adjecent tiles with same tile type, return true
        // </summary>
        private void CheckIfSolutionIsAvailable()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    int _adjecentOfSameType = 0;
                    int _intTileType = activePlayableTiles[x, y].TileType;
                    Vector2 _gridPosition = new Vector2(x, y);

                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 _nextTile = _gridPosition + gridMath[i];
                        if ((_nextTile.x >= 0) && (_nextTile.x < gridSize.x) && (_nextTile.y >= 0) && (_nextTile.y < gridSize.y))
                        {
                            if (activePlayableTiles[(int)_nextTile.x, (int)_nextTile.y].TileType == _intTileType)
                            {
                                _adjecentOfSameType += 1;
                            }
                        }
                    }
                    if (_adjecentOfSameType >= 2)
                    {
                        return;
                    }
                }
            }

            gameManager.SolutionNotAvailable();
        }

        // <summary>
        // Remove board from screen
        // Playable tiles and background tiles are moved to tile pools
        // </summary>
        public void RemoveBoard()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    tilePoolManager.TileToPlayablePool(activePlayableTiles[x, y]);
                    tilePoolManager.TileToBackgroundPool(activeBackgroundTiles[x, y]);
                }
            }
            activePlayableTiles = new PlayableTile[0, 0];
            activeBackgroundTiles = new BackgroundTile[0, 0];
        }

        // <summary>
        // Highlight the current background tile and remove highlight from previous if one is already highlighted
        // </summary
        private void HighlightBackgroundTile()
        {
            if (previousBackgroundTile != null)
            {
                previousBackgroundTile.SetTileData(backgroundNotSelected);
            }
            activeBackgroundTiles[(int)selectedGridPTile.x, (int)selectedGridPTile.y].SetTileData(backgroundSelected);
            previousBackgroundTile = activeBackgroundTiles[(int)selectedGridPTile.x, (int)selectedGridPTile.y];
        }
    }
}