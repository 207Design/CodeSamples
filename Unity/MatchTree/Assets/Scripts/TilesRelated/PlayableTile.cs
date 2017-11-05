using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Manager;

namespace Tile
{
    public class PlayableTile : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {

        BoardManager boardManager;

        TilePoolManager tilePoolManager;

        ScoreManager scoreManager;

        [SerializeField]
        MeshRenderer meshRenderer;

        [SerializeField]
        Animator animator;

        [SerializeField]
        BoxCollider boxCollider;

        [SerializeField]
        Canvas canvas;

        [SerializeField]
        Text scoreText;

        [SerializeField]
        float fallSpeed = 8f;

        int tileType;

        public int TileType
        {
            get
            {
                return tileType;
            }
        }

        Vector2 gridPosition;

        public Vector2 GridPosition
        {
            get
            {
                return gridPosition;
            }
        }

        // Use this for initialization
        public void Start() {
            boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();
            tilePoolManager = GameObject.FindGameObjectWithTag("TilePoolManager").GetComponent<TilePoolManager>();
            scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
            canvas.worldCamera = Camera.main;
            canvas.gameObject.SetActive(false);
        }

        // <summary>
        // On Pointer down, tell board manager that this wile wants to be selected
        // </summary>
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            boardManager.SelectTile(this);
        }

        // <summary>
        // On Pointer enter, tell board manager that this wile wants to be selected
        // </summary>
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                boardManager.SelectTile(this);
            }
        }

        // <summary>
        // Select the tile
        // </summary>
        public void SelectTile()
        {
            animator.SetBool("selected", true);
        }

        // <summary>
        // Deselect the tile
        // </summary>
        public void DeselectTile()
        {
            animator.SetBool("selected", false);
        }

        // <summary>
        // Set relevant data for this tile, such as material, type of tile and position along grid
        // </summary>
        public void SetTileData(Material material, int value, Vector2 newGridPosition)
        {
            meshRenderer.material = material;
            tileType = value;
            gridPosition = newGridPosition;

            StartCoroutine(MoveToPosition());
        }

        // <summary>
        // Set new tile position along grid
        // </summary>
        public void NewGridPosition(Vector2 newGridPosition)
        {
            gridPosition = newGridPosition;
            StartCoroutine(MoveToPosition());
        }

        // <summary>
        // Set tile as collected and send back to boardmanager to be added to pool
        // </summary>
        public void CollectTile()
        {
            scoreText.text = (scoreManager.MultiplierValue * 100).ToString();
            boxCollider.enabled = false;
            animator.SetBool("selected", false);
            animator.SetTrigger("collect");
            canvas.gameObject.SetActive(true);
        }

        // <summary>
        // Animation for tile being collected has been played, and board manager can add this tile to the pool again
        // </summary>
        public void CollectAnimationCompleted()
        {
            animator.SetTrigger("reset");
            boxCollider.enabled = true;
            canvas.gameObject.SetActive(false);

            tilePoolManager.TileToPlayablePool(this);
            boardManager.TileSuccessfullyCollected();
        }

        // <summary>
        // Move tile to its new position on the grid
        // </summary>
        IEnumerator MoveToPosition()
        {
            float _time = 0;
            Vector3 _previousPosition = transform.localPosition;
            while (_time < 1)
            {
                _time += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(_previousPosition, new Vector3(gridPosition.x * 10, gridPosition.y * 10, transform.localPosition.z), _time * fallSpeed);

                yield return null;
            }
        }
    }
}