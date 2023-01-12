using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update=

    Vector3 positionChute;

    bool fallingLastFrame;
    public float distanceMort = 10f;

    float distanceChute;

    public static bool isDead;
    MovementController moveController;

    void Start()
    {
        moveController = gameObject.GetComponent<MovementController>();
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
        isDead = true;
    }
}
