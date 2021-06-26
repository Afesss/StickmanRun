using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using PathCreation;
public class Spawner : MonoBehaviour
{
    public event System.Action OnHeroStartGameSpawn;
    public event System.Action OnHeroSpawn;

    private GameObject[] heroes;
    private Vector3[] startPos;

    private SpawnerSettings spawnerSettings;

    [Inject]
    private void Construct(SpawnerSettings spawnerSettings)
    {
        this.spawnerSettings = spawnerSettings;
    }
    private void Start()
    {
        startPos = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,1),
            new Vector3(-1,0,1)
        };

        heroes = new GameObject[spawnerSettings.maxHeroNumber];

        for (int i = 0; i < heroes.Length; i++)
        {
            heroes[i] = Instantiate(spawnerSettings.heroPrefab, transform);
            heroes[i].SetActive(false);
        }
        StartSpawn();
    }
    public void StartSpawn()
    {
        for (int i = 0; i < startPos.Length; i++)
        {
            OnHeroStartGameSpawn?.Invoke();
            heroes[i].SetActive(true);
            heroes[i].transform.localPosition = startPos[i];
        }
    }
    public GameObject GetFreeHero()
    {
        
        for (int i = 0; i < heroes.Length; i++)
        {
            if (!heroes[i].activeSelf)
            {
                OnHeroSpawn?.Invoke();
                return heroes[i];
            }
        }
        return null;
    }
}
