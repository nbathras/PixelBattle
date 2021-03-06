﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject {
    public List<UnitTypeSO> constructableUnits;
    public Sprite sprite;
    public GameObject prefab;
    public int gridSizeX;
    public int gridSizeY;
}
