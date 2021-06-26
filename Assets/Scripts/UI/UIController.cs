using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using UnityEngine.UI;
using UniRx;
public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;
    [SerializeField] private GameObject touchPad;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timerText;

    private GameManager gameManager;
    private Finish finish;
    private Score score;
    [Inject]
    private void Consruct(GameManager gameManager, Finish finish, Score score)
    {
        this.finish = finish;
        this.gameManager = gameManager;
        this.score = score;
    }
    private void Start()
    {
        touchPad.SetActive(true);
        win.SetActive(false);
        lose.SetActive(false);
        finish.OnFinished += Finish_OnFinished;
        gameManager.OnGameOver += GameManager_OnGameOver;
        gameManager.OnHeroRun += GameManager_OnHeroRun;
        score.gameScore.Select(gameScore => string.Format("Score: {0}", gameScore))
            .Subscribe(text => scoreText.text = text);
        gameManager.startTimer.Select(timer => string.Format("{0}", timer))
            .Subscribe(text => timerText.text = text);
    }

    private void OnDestroy()
    {
        finish.OnFinished -= Finish_OnFinished;
        gameManager.OnGameOver -= GameManager_OnGameOver;
        gameManager.OnHeroRun -= GameManager_OnHeroRun;
    }
    private void GameManager_OnHeroRun()
    {
        StartCoroutine(WatiSecToStart());
    }

    private IEnumerator WatiSecToStart()
    {
        yield return new WaitForSeconds(0.3f);
        timerText.gameObject.SetActive(false);
    }
    private void GameManager_OnGameOver()
    {
        touchPad.SetActive(false);
        lose.SetActive(true);
    }

    private void Finish_OnFinished()
    {
        touchPad.SetActive(false);
        win.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
    
    
}
