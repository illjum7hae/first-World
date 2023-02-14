using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileAtlas", menuName = "TileAtlas")]
public class TileAtlas : ScriptableObject
{
    [Header("Tile Sprites")]
    public TileClass grass;
    public TileClass dirt;
    public TileClass stone;
    public TileClass treeRoot;
    public TileClass log;
    public TileClass leaf;
}
