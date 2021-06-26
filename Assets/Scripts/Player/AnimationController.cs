using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class AnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody[] allRigidbody;
    [Header("Settings")]
    [SerializeField] private AnimationCurve animationCurve;

    public Animator Animator { get { return animator; } }

    private float animatorValue, direction , currentTime;

    private int runId = Animator.StringToHash("Run");
    private int danceId = Animator.StringToHash("Dance");
    private int finishId = Animator.StringToHash("Finish");
    private int startRunId = Animator.StringToHash("StartRun");
    private int maxRandomNum = 3;

    private Strafe strafe;
    private Finish finish;
    private GameManager gameManager;
    private PlayerSettings playerSettings;

    [Inject]
    private void Construct(Strafe strafe, PlayerSettings playerSettings, Finish finish, GameManager gameManager)
    {
        this.strafe = strafe;
        this.playerSettings = playerSettings;
        this.finish = finish;
        this.gameManager = gameManager;
    }

    private void OnEnable()
    {
        animator.SetBool(finishId, false);
        animator.SetBool(startRunId, gameManager.GameStarted ? true : false);

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        for (int i = 0; i < allRigidbody.Length; i++)
        {
            allRigidbody[i].isKinematic = false;
            allRigidbody[i].collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        strafe.OnStrafeStartWithDirection += Strafe_OnStrafeDirection;
        strafe.OnStrafeUpdate += Strafe_OnStrafeUpdate;
        finish.OnFinished += Finish_OnFinished;
        gameManager.OnHeroRun += GameManager_OnHeroRun;
    }
    private void OnDisable()
    {
        gameManager.OnHeroRun += GameManager_OnHeroRun;
        strafe.OnStrafeStartWithDirection -= Strafe_OnStrafeDirection;
        strafe.OnStrafeUpdate -= Strafe_OnStrafeUpdate;
        finish.OnFinished -= Finish_OnFinished;
    }
    private void GameManager_OnHeroRun()
    {
        animator.SetBool(startRunId, true);
    }
    private void Finish_OnFinished()
    {
        animator.SetBool(finishId, true);
        animator.SetFloat(danceId, (float)Random.Range(0, maxRandomNum));
    }
    private void Strafe_OnStrafeDirection(float direction)
    {
        this.direction = direction;
        currentTime = 0;
        animationCurve.keys[0].value = animator.GetFloat(runId);
    }
    private void Strafe_OnStrafeUpdate()
    {
        currentTime += Time.deltaTime / playerSettings.strafeDuration;
        
        animatorValue = direction * animationCurve.Evaluate(currentTime);
        animator.SetFloat(runId, animatorValue);
    }

    public void EnableRagdoll()
    {
        gameManager.HeroDead();
        transform.parent = null;
        animator.enabled = false;
        for(int i = 0; i< allRigidbody.Length; i++)
        {
            allRigidbody[i].isKinematic = false;
        }
    }
}
