using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
public class GameManager : MonoBehaviour
{
    public event System.Action OnHeroRun;
    public event System.Action OnGameStart;
    public event System.Action OnGameOver;


    public bool GameStarted { get; private set; }

    public ReactiveProperty<string> startTimer { get; private set; } = new ReactiveProperty<string>();

    private int availableHeroesNumber;

    private float timeToRunGame = 3;
    private float timer;

    private Finish finish;
    private Movement movement;
    private Strafe strafe;
    private Spawner spawner;
    private Score score;
    private PlayerSettings playerSettings;
    
    [Inject]
    private void Construct(Finish finish, Movement movement, Strafe strafe, Spawner spawner, 
        Score score, PlayerSettings playerSettings)
    {
        this.finish = finish;
        this.strafe = strafe;
        this.movement = movement;
        this.spawner = spawner;
        this.score = score;
        this.playerSettings = playerSettings;
    }
    private void Start()
    {
        GameStarted = false;
        finish.ResetFinish();
        finish.OnFinished += FinishedGame;
        spawner.OnHeroSpawn += Spawner_OnHeroSpawn;
        spawner.OnHeroStartGameSpawn += Spawner_OnHeroStartSpawn;
        
        StartGame();
    }
    private void OnDestroy()
    {
        finish.OnFinished -= FinishedGame;
        spawner.OnHeroSpawn -= Spawner_OnHeroSpawn;
        spawner.OnHeroStartGameSpawn += Spawner_OnHeroStartSpawn;
    }
    private void Spawner_OnHeroStartSpawn()
    {
        availableHeroesNumber++;
    }

    private void Spawner_OnHeroSpawn()
    {
        availableHeroesNumber++;
        score.UpdateScore(playerSettings.playerScore);
    }
    public void HeroDead()
    {
        score.UpdateScore(-playerSettings.playerScore);
        availableHeroesNumber--;

        if (availableHeroesNumber == 0)
        {
            score.ResetScore();
            OnGameOver?.Invoke();
            FinishedGame();
        }
    }
    
    private void FinishedGame()
    {
        movement.EnableMovement(false);
        strafe.EnableStrafe(false);
        GameStarted = false;
    }
    private void StartGame()
    {
        timer = timeToRunGame;
        OnGameStart?.Invoke();
        StartCoroutine(WaitToRun());
        availableHeroesNumber = 0;
        spawner.StartSpawn();
    }
    private IEnumerator WaitToRun()
    {
        while (timer > 0)
        {
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
            int time = (int)timer + 1;
            startTimer.Value = time.ToString();
        }
        startTimer.Value = "GO";
        OnHeroRun?.Invoke();
        movement.EnableMovement(true);
        strafe.EnableStrafe(true);
        GameStarted = true;
    }
    public void CheckGameOver()
    {

    }
}
