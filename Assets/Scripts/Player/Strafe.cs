using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
public class Strafe : MonoBehaviour
{
    public event System.Action<float> OnStrafeStartWithDirection;
    public event System.Action OnStrafeUpdate;

    private bool isMoving;
    private bool onEnable = false;

    private float direction;

    private Vector3 strafeOffset;

    private TouchPad touchPad;
    private PlayerSettings playerSettings;
    private Tween tween;

    [Inject]
    private void Construct(PlayerSettings playerSettings, TouchPad touchPad)
    {
        this.playerSettings = playerSettings;
        this.touchPad = touchPad;
    }

    private void Start()
    {
        touchPad.OnPointerUpWithDragVector += TouchPad_OnPointerUpWithDragVector;
    }
    private void OnDestroy()
    {
        touchPad.OnPointerUpWithDragVector -= TouchPad_OnPointerUpWithDragVector;
    }
    private void TouchPad_OnPointerUpWithDragVector(Vector2 dragVector)
    {
        if (onEnable &&!isMoving && dragVector.sqrMagnitude >
            playerSettings.minInputDragDistance * playerSettings.minInputDragDistance)
        {
            isMoving = true;
            direction = Mathf.Sign(dragVector.x);

            OnStrafeStartWithDirection?.Invoke(direction);
            strafeOffset += Vector3.right * direction * playerSettings.strafeStepOffset;

            tween.Kill();

            tween = transform.DOLocalMove(strafeOffset, playerSettings.strafeDuration);
            tween.OnUpdate(() =>
            {
                OnStrafeUpdate?.Invoke();
            });
            StartCoroutine(WaitToNewInput());
        }
    }
    private IEnumerator WaitToNewInput()
    {
        yield return new WaitForSeconds(playerSettings.strafeDuration * 0.7f);
        isMoving = false;
    }
    public void EnableStrafe(bool value)
    {
        onEnable = value;
    }

}
