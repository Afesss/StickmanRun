using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct SpawnerSettings
{
    [Header("Hero")]
    public GameObject heroPrefab;
    [Range(0, 20)]public int maxHeroNumber;
}
