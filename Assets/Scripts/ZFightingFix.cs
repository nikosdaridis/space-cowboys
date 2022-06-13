using UnityEngine;

public class ZFightingFix : MonoBehaviour
{
    // Private
    private SpriteRenderer[] backgroundSprites;

    void Awake()
    {
        backgroundSprites = GetComponentsInChildren<SpriteRenderer>();

        FixSpritesZFighting();
    }

    void FixSpritesZFighting()
    {
        for (int i = 0; i < backgroundSprites.Length; i++)
        {
            backgroundSprites[i].sortingOrder = i + 1;
            backgroundSprites[i].transform.position -= new Vector3(0.0f, 0.0f, (i + 1.0f) / 10000.0f);
        }
    }
}