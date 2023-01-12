using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MovementController))]

public class PlayerManager : MonoBehaviour
{
    Vector3 positionChute;

    [SerializeField] private Animator animator;
    bool fallingLastFrame;
    public float distanceMort = 10f;
    public Checkpoint CurrentCheckpoint;

    public GameObject modeleVivant;
    public GameObject deadBody;

    public GameObject outlineVivant;

    private Vector3 direction;



    float distanceChute;

    public static bool isDead;
    public MovementController moveController;
    public InteractionModule interactionModule;

    private void OnEnable()
    {
        _targetRot = Quaternion.Euler(0, 180, 0);
        Debug.Log($"OnEnable {_targetRot.eulerAngles}");

        moveController = GetComponent<MovementController>();
        interactionModule = GetComponentInChildren<InteractionModule>();
    }
    private void Update()
    {
<<<<<<< HEAD
        if (GameManager.Inst.ConvertVector(moveController.Speed).x > 0)
        {

            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (GameManager.Inst.ConvertVector(moveController.Speed).x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
=======
        
        GameManager.Inst.CameraController.SetPosY(Math.Max(transform.position.y, GameManager.Inst.CameraController.CameraMinHeight));
>>>>>>> 1b85d5495c421da5a3b8c56790d4b999c3776784

        if (moveController.Speed.magnitude > 0.1f)
        {

            animator.SetBool("isWalking", true);

        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        Rotate();
        if (moveController.Falling && !fallingLastFrame)
        {
            //commence a tomber
            positionChute = transform.position;
        }
        else if (!moveController.Falling && fallingLastFrame)
        {
            //on a touche le sol
            if (positionChute.y - transform.position.y > distanceMort)
            {
                Die();
            }
        }

    }
    private void LateUpdate()
    {
        fallingLastFrame = moveController.Falling;
        isDead = false;
    }
    private void Die()
    {
        Debug.Log("Dead");
        modeleVivant.SetActive(false);
        deadBody.SetActive(true);
        outlineVivant.SetActive(false);
        moveController.DisabledControls = true;
        isDead = true;
    }

    [ContextMenu("Respawn")]
    private void Respawn()
    {
        modeleVivant.SetActive(true);
        deadBody.SetActive(false);
        outlineVivant.SetActive(true);
        moveController.DisabledControls = false;
        isDead = false;
        gameObject.transform.position = CurrentCheckpoint.transform.position;
    }

    public void ResetVel()
    {
        moveController.ResetVelocity();
    }
    public void FreezePosition(FreezePositionAxis axis)
    {
        moveController.FreezePosition(axis);
    }

    private Quaternion _targetRot;
    private float _rotationSpeed = 1;
    public void SetQuaternion(Quaternion quaternion, float rotationSpeed)
    {
        _rotationSpeed = rotationSpeed;
        _targetRot = Quaternion.Euler(0, 180 + quaternion.eulerAngles.y, 0);
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, _rotationSpeed * Time.deltaTime);
    }
}
