using System.Collections.Generic;
using UnityEngine;

public class BoardManager: MonoBehaviour {
    public Vector2Int boardSize;
    [HideInInspector] public Vector3 boardOffset;
    public GameObject zonePrefab;
    [HideInInspector] public Dictionary<Vector2Int, Zone> zone;

    [HideInInspector] public Vector3 mousePosition;
    [HideInInspector] public Vector2Int mousePositionOnBoard;

    GameManager gameManager;

    public AudioSource source;
    public List<AudioClip> clip;

    private void Awake() {
        zone = new Dictionary<Vector2Int, Zone>();
        boardOffset = new Vector3((float)-boardSize.x / 2f, (float)-boardSize.y / 2f, 0f);

        for (int i = 0; i < boardSize.x; i++) {
            for (int j = 0; j < boardSize.y; j++) {
                zone.Add(new Vector2Int(i, j), Instantiate(zonePrefab, new Vector3((float)i, (float)j, 0f) + boardOffset, Quaternion.identity, transform).GetComponent<Zone>());
                zone[new Vector2Int(i, j)].gameObject.name = i + ", " + j;
            }
        }

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update() {
        if (gameManager.gameIsRunning) {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionOnBoard = new Vector2Int(Mathf.FloorToInt(mousePosition.x - boardOffset.x + 0.5f), Mathf.FloorToInt(mousePosition.y - boardOffset.y + 0.5f));

            UnhoverUnhoveredZone();
            HoverHoveredZone();

            if (mousePositionOnBoard.x >= 0 && mousePositionOnBoard.x <= boardSize.x - 1 && mousePositionOnBoard.y >= 0 && mousePositionOnBoard.y <= boardSize.y - 1) {
                if (zone[mousePositionOnBoard].isHovered && !zone[mousePositionOnBoard].isFilled && Input.GetKeyDown(KeyCode.Mouse0)) {
                    zone[mousePositionOnBoard].PlaceBlock();
                    gameManager.timer = 10f;
                    source.PlayOneShot(clip[0]);
                    CheckForThreeBlockMatch();
                    zone[mousePositionOnBoard].blockSlot.RandomizeActiveBlock();
                    RemoveAllAttackMarkAndHoverZone();
                }
            }
        }
    }

    void RemoveAllAttackMarkAndHoverZone() {
        for (int i = 0; i < boardSize.x; i++) {
            for (int j = 0; j < boardSize.y; j++) {
                zone[new Vector2Int(i, j)].isHovered = false;
                zone[new Vector2Int(i, j)].markZoneToBeAttackedSprite.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    private void UnhoverUnhoveredZone() {
        for (int i = 0; i < boardSize.x; i++) {
            for (int j = 0; j < boardSize.y; j++) {
                if (zone[new Vector2Int(i, j)].isHovered == true) {
                    if (mousePositionOnBoard != new Vector2Int(i, j)) {
                        zone[new Vector2Int(i, j)].Unhover(new Vector2Int(i, j));
                    }
                }
            }
        }
    }

    private void HoverHoveredZone() {
        for (int i = 0; i < boardSize.x; i++) {
            for (int j = 0; j < boardSize.y; j++) {
                if (zone[new Vector2Int(i, j)].isHovered == false) {
                    if (mousePositionOnBoard == new Vector2Int(i, j)) {
                        zone[new Vector2Int(i, j)].Hover(new Vector2Int(i, j));
                    }
                }
            }
        }
    }

    private void CheckForThreeBlockMatch() {
        Dictionary<BlockType, int> blockCounter;
        blockCounter = new Dictionary<BlockType, int>();
        blockCounter.Add(BlockType.Rock, 0);
        blockCounter.Add(BlockType.Knight, 0);
        blockCounter.Add(BlockType.Bishop, 0);
        blockCounter.Add(BlockType.Dragon, 0);
        for (int i = 0; i < boardSize.x; i++) {
            for (int j = 0; j < boardSize.y; j++) {
                if (zone[new Vector2Int(i, j)].isFilled) {
                    blockCounter[zone[new Vector2Int(i, j)].blockFill] += 1;
                }
            }
        }
        for (int i = 1; i < blockCounter.Count + 1; i++) {
            if (blockCounter[(BlockType)i] >= 3) {
                source.PlayOneShot(clip[1]);
                for (int j = 0; j < boardSize.x; j++) {
                    for (int k = 0; k < boardSize.y; k++) {
                        if(zone[new Vector2Int(j, k)].blockFill == (BlockType)i) {
                            zone[new Vector2Int(j, k)].isFilled = false;
                            zone[new Vector2Int(j, k)].blockFill = BlockType.None;
                            zone[new Vector2Int(j, k)].zoneSprite.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                        }
                    }
                }
            }
        }
    }
}