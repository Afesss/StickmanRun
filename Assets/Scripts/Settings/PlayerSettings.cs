using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerSettings
{
    [Header("Layers")]
    public LayerMask playerLayer;
    [Range(0, 31)] public int obstacleLayerNumber;
    [Range(0, 31)] public int bonusLayerNumber;
    [Header("Move Settings")]
    [Range(0, 20)] public float moveSpeed;
    [Range(0, 3)] public float strafeStepOffset;
    [Range(0, 2)] public float strafeDuration;
    [Range(0, 1)] public float finishDelay;

    [Header("Input Settings")]
    [Range(0, 20)] public float minInputDragDistance;

    [Header("Materials")]
    public Material startMaterial;
    public Material deadMaterial;

    [Header("Score")]
    [Range(0, 50)] public int bonusesScore;
    [Range(0, 50)] public int playerScore;
}
