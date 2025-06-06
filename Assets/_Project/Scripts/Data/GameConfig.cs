using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Config File")]
public class GameConfig : ScriptableObject
{
    [field: Header("Size")]
    [field: SerializeField] public int width { get; private set; }
    [field: SerializeField] public int height { get; private set; }
    [field: SerializeField] public int depth { get; private set; }

    [field: Header("Tiles")]
    [field: SerializeField] public Sprite[] tileSpriteVariants { get; private set; }

    [field: Header("Visual")]
    [field: SerializeField] public float tileOffsetY { get; private set; } = 0.1f;
    [field: SerializeField] public Color blockedColor { get; private set; }
    [field: Header("Auto Solver")]
    [field: SerializeField] public float stepDelay { get; private set; } = 0.5f;
}
