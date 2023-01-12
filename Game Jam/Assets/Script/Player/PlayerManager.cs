using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]

public class PlayerManager : MonoBehaviour
{
    Vector3 positionChute;

    bool fallingLastFrame;
    public float distanceMort = 10f;

    public GameObject modeleVivant;
    public GameObject deadBody;

    float distanceChute;

    public static bool isDead;
    public MovementController moveController;
    public InteractionModule interactionModule;

    void Start()
    {
        moveController = gameObject.GetComponent<MovementController>();
        interactionModule = GetComponentInChildren<InteractionModule>();
    }
    // Update is called once per frame
    void Update()
    {
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
}
