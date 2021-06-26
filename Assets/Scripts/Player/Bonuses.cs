using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
public class Bonuses : MonoBehaviour
{
    private float tweenDuration = 0.5f;
    private const float overlapSphereRadius = 0.3f;

    private Vector3[] castOffset;

    private PlayerSettings playerSettings;
    private Spawner spawner;
    private Score score;

    [Inject]
    private void Construct(PlayerSettings playerSettings, Spawner spawner, Score score)
    {
        this.playerSettings = playerSettings;
        this.spawner = spawner;
        this.score = score;
    }
    private Vector3? FindFreePos()
    {
        Vector3 localPos = transform.localPosition;
        float zPosOffset = localPos.z < 0.1f ? 1 : -1;
        castOffset = new Vector3[]
        {
            new Vector3(-1, 0, zPosOffset),
            new Vector3(1, 0, zPosOffset),
            new Vector3(-1, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, zPosOffset)
        };
        Collider[] collider = new Collider[0];

        for(int i = 0; i < castOffset.Length; i++)
        {
            collider = Physics.OverlapSphere(transform.TransformPoint(Vector3.zero + castOffset[i] + Vector3.up), overlapSphereRadius,playerSettings.playerLayer);
            
            if (collider.Length == 0 &&
                Physics.Raycast(transform.TransformPoint(Vector3.zero + castOffset[i] + Vector3.up), Vector3.down))
            {
                return localPos + castOffset[i];
            }
        }
        return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == playerSettings.bonusLayerNumber)
        {
            other.gameObject.SetActive(false);
            GameObject newHero = spawner.GetFreeHero();
            Vector3? pos = FindFreePos();
            if (newHero != null && pos != null)
            {
                score.UpdateScore(playerSettings.bonusesScore);
                newHero.SetActive(true);
                newHero.transform.localScale = Vector3.zero;
                newHero.transform.DOScale(Vector3.one, tweenDuration);
                newHero.transform.localPosition = transform.localPosition + Vector3.up;
                newHero.transform.DOLocalMove((Vector3)pos, tweenDuration);
            }
        }
    }

}
