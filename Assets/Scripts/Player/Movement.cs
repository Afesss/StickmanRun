using UnityEngine;
using PathCreation;
using Zenject;

public class Movement : MonoBehaviour
{
    private bool isMove;

    private float dstTravelled;
    
    private Quaternion rotation;

    private PlayerSettings playerSettings;
    private PathCreator pathCreator;

    [Inject]
    private void Construct(PlayerSettings playerSettings, TouchPad touchPad, PathCreator pathCreator)
    {
        this.playerSettings = playerSettings;
        this.pathCreator = pathCreator;
    }

    private void Start()
    {
        dstTravelled = 0.7f;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled);
        isMove = false;
    }
    private void Update()
    {
        if (isMove)
        {
            dstTravelled += playerSettings.moveSpeed * Time.deltaTime;

            transform.position = pathCreator.path.GetPointAtDistance(dstTravelled);

            rotation = pathCreator.path.GetRotationAtDistance(dstTravelled) * Quaternion.Euler(0, 0, 90);
            transform.rotation = rotation;
        }
    }
    public void EnableMovement(bool enable)
    {
        dstTravelled = 0;
        isMove = enable;
    }
}
