using System.Collections.Generic;
using UnityEngine;

public class BlockSlot : MonoBehaviour
{
    public List<Sprite> blocks;

    public BlockType activeBlock;
    public SpriteRenderer activeBlockSprite;

    GameManager gameManager;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        RandomizeActiveBlock();
    }

    public void RandomizeActiveBlock() {
        activeBlock = (BlockType)Random.Range(1, blocks.Count + 1);
        activeBlockSprite.sprite = blocks[(int)activeBlock - 1];
    }
}
