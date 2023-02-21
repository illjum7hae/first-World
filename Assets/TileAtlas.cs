using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileAtlas", menuName = "TileAtlas")]
public class TileAtlas : ScriptableObject
{
    [Header("Environment")]
    public TileClass grass;
    public TileClass dirt;
    public TileClass stone;
    public TileClass treeRoot;
    public TileClass log;
    public TileClass leaf;
    public TileClass tallGrass;

    [Header("Ores")]
    public TileClass sand;
    public TileClass snow;
    public TileClass coal;
    public TileClass iron;
    public TileClass gold;
    public TileClass diamond;
}