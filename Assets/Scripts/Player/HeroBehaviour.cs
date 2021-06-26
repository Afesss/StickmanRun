using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class HeroBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnimationController animController;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    private bool isDead;

    private const float rayOffset = 1f;
    private const float rayDistance = 3f;

    [Inject] private PlayerSettings playerSettings;
    
    private void OnEnable()
    {
        isDead = false;
        meshRenderer.materials[0] = playerSettings.startMaterial;
        _rigidbody.useGravity = false;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (!CheckFloor(HumanBodyBones.LeftFoot) || !CheckFloor(HumanBodyBones.RightFoot))
        {
            HeroDead();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == playerSettings.obstacleLayerNumber)
        {
            HeroDead();
        }
    }
    private bool CheckFloor(HumanBodyBones foot)
    {
        Vector3 footPos = animController.Animator.GetBoneTransform(foot).position;

        return Physics.Raycast(footPos + Vector3.up * rayOffset, Vector3.down, rayDistance);
    }

    private void HeroDead()
    {
        isDead = true;
        meshRenderer.material = playerSettings.deadMaterial;
        animController.EnableRagdoll();
    }
}
