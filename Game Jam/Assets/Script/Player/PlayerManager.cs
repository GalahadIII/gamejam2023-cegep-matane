using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]

public class PlayerManager : MonoBehaviour
{
    
    Vector3 positionChute;

    bool fallingLastFrame;
    public float distanceMort = 10f;
    public int currentCheckpoint;

    public GameObject modeleVivant;
    public GameObject deadBody;

    float distanceChute;

    public static bool isDead;
    public MovementController moveController;
    public InteractionModule interactionModule;

    void OnEnable()
    {
        moveController = GetComponent<MovementController>();
        interactionModule = GetComponentInChildren<InteractionModule>();
    }
    // Update is called once per frame
    void Update()
    {
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
    void LateUpdate()
    {
        fallingLastFrame = moveController.Falling;
        isDead = false;
    }
    void Die()
    {
        Debug.Log("Dead");
        modeleVivant.SetActive(false);
        deadBody.SetActive(true);
        isDead = true;
    }

    public void ResetVel()
    {
        moveController.ResetVelocity();
    }

    public void FreezePosition(FreezePositionAxis axis)
    {
        moveController.FreezePosition(axis);
    }
    
    private Quaternion _targetQuat = Quaternion.identity;
    private float _rotationSpeed = 1;
    public void SetQuaternion(Quaternion quaternion, float rotationSpeed)
    {
        _rotationSpeed = rotationSpeed;
        _targetQuat = quaternion;
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetQuat, _rotationSpeed * Time.deltaTime);
    }
}
