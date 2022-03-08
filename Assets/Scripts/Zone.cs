using System;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType { None, Rock, Knight, Bishop, Dragon }

public class Zone : MonoBehaviour {
    public bool isFilled;
    public BlockType blockFill;
    public bool isHovered;

    [HideInInspector] public SpriteRenderer zoneSprite;
    GameManager gameManager;
    public BlockSlot blockSlot;
    BoardManager boardManager;

    [HideInInspector] public SpriteRenderer markZoneToBeAttackedSprite;

    void Start() {
        isFilled = false;
        isHovered = false;

        zoneSprite = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        blockSlot = FindObjectOfType<BlockSlot>();
        boardManager = FindObjectOfType<BoardManager>();

        markZoneToBeAttackedSprite = zoneSprite.transform.GetChild(1).GetComponent<SpriteRenderer>();
        markZoneToBeAttackedSprite.color = new Color(1f, 1f, 1f, 0f);
    }

    public void PlaceBlock() {
        zoneSprite.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = blockSlot.blocks[(int)blockSlot.activeBlock - 1];
        if (CheckBlockOnAttackZone(boardManager.mousePositionOnBoard)) {
            return;
        }
        if(blockSlot.activeBlock == BlockType.Rock || blockSlot.activeBlock == BlockType.Bishop) {
            gameManager.AddScore(2);
        }
        else if(blockSlot.activeBlock == BlockType.Knight || blockSlot.activeBlock == BlockType.Dragon) {
            gameManager.AddScore(1);
        }
        isFilled = true;
        blockFill = blockSlot.activeBlock;
    }

    List<Vector2Int> GetAttackZone() {
        List<Vector2Int> attackZone;
        attackZone = new List<Vector2Int>();
        if(blockSlot.activeBlock == BlockType.Rock) {
            attackZone.Add(new Vector2Int(1, 0));
            attackZone.Add(new Vector2Int(2, 0));
            attackZone.Add(new Vector2Int(-1, 0));
            attackZone.Add(new Vector2Int(-2, 0));
            attackZone.Add(new Vector2Int(0, 1));
            attackZone.Add(new Vector2Int(0, 2));
            attackZone.Add(new Vector2Int(0, -1));
            attackZone.Add(new Vector2Int(0, -2));
        }
        else if(blockSlot.activeBlock == BlockType.Bishop) {
            attackZone.Add(new Vector2Int(1, 1));
            attackZone.Add(new Vector2Int(2, 2));
            attackZone.Add(new Vector2Int(-1, -1));
            attackZone.Add(new Vector2Int(-2, -2));
            attackZone.Add(new Vector2Int(1, -1));
            attackZone.Add(new Vector2Int(2, -2));
            attackZone.Add(new Vector2Int(-1, 1));
            attackZone.Add(new Vector2Int(-2, 2));
        }
        else if (blockSlot.activeBlock == BlockType.Knight) {
            attackZone.Add(new Vector2Int(1, 2));
            attackZone.Add(new Vector2Int(2, 1));
            attackZone.Add(new Vector2Int(-1, -2));
            attackZone.Add(new Vector2Int(-2, -1));
            attackZone.Add(new Vector2Int(1, -2));
            attackZone.Add(new Vector2Int(2, -1));
            attackZone.Add(new Vector2Int(-1, 2));
            attackZone.Add(new Vector2Int(-2, 1));
        }
        else if (blockSlot.activeBlock == BlockType.Dragon) {
            attackZone.Add(new Vector2Int(1, 1));
            attackZone.Add(new Vector2Int(0, 1));
            attackZone.Add(new Vector2Int(-1, 1));
            attackZone.Add(new Vector2Int(1, 0));
            attackZone.Add(new Vector2Int(-1, 0));
            attackZone.Add(new Vector2Int(1, -1));
            attackZone.Add(new Vector2Int(0, -1));
            attackZone.Add(new Vector2Int(-1, -1));
        }
        return attackZone;
    }

    public void Hover(Vector2Int coor) {
        isHovered = true;
        List<Vector2Int> attackZone = GetAttackZone();
        for (int i = 0; i < attackZone.Count; i++) {
            if(coor.x + attackZone[i].x >= 0 && coor.x + attackZone[i].x <= boardManager.boardSize.x - 1 && coor.y + attackZone[i].y >= 0 && coor.y + attackZone[i].y <= boardManager.boardSize.y - 1) {
                boardManager.zone[coor + attackZone[i]].markZoneToBeAttackedSprite.color = new Color(1f, 1f, 1f, 0.3f);
            }
        }
        zoneSprite.color = new Color(140f / 255f, 210f / 255f, 140f / 255f, 1f);
    }

    public void Unhover(Vector2Int coor) {
        isHovered = false;
        List<Vector2Int> attackZone = GetAttackZone();
        for (int i = 0; i < attackZone.Count; i++) {
            if (coor.x + attackZone[i].x >= 0 && coor.x + attackZone[i].x <= boardManager.boardSize.x - 1 && coor.y + attackZone[i].y >= 0 && coor.y + attackZone[i].y <= boardManager.boardSize.y - 1) {
                boardManager.zone[coor + attackZone[i]].markZoneToBeAttackedSprite.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        zoneSprite.color = new Color(1f, 1f, 1f, 1f);
    }

    bool CheckBlockOnAttackZone(Vector2Int coor) {
        List<Vector2Int> attackZone = GetAttackZone();
        for (int i = 0; i < attackZone.Count; i++) {
            if (coor.x + attackZone[i].x >= 0 && coor.x + attackZone[i].x <= boardManager.boardSize.x - 1 && coor.y + attackZone[i].y >= 0 && coor.y + attackZone[i].y <= boardManager.boardSize.y - 1) {
                if(boardManager.zone[coor + attackZone[i]].isFilled) {
                    gameManager.GameOver();
                    return true;
                }
            }
        }
        return false;
    }
}