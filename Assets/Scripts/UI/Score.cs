using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class Score : IInitializable
{
    public ReactiveProperty<int> gameScore { get; private set; } = new ReactiveProperty<int>();
    public void Initialize()
    {
        gameScore.Value = 0;
    }
    public void UpdateScore(int value)
    {
        gameScore.Value += value;
    }
    public void ResetScore()
    {
        gameScore.Value = 0;
    }
}
