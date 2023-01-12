using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update=
    private float _rotationSpeed = 5f;
    private Quaternion _targetRotation = Quaternion.identity;
    
    Vector3 positionChute;

    bool fallingLastFrame;
    public float distanceMort = 10f;

    float distanceChute;

    public static bool isDead;
    MovementController moveController;
    private InteractionModule _interactionModule;

    void OnEnable()
    {
        moveController = gameObject.GetComponent<MovementController>();
        _interactionModule = GetComponentInChildren<InteractionModule>();
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
        if (InputManager.PlayerInputs.Interact.OnDown)
        {
            _interactionModule.TriggerInteraction();
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
    
    public void SetQuaternion(Quaternion quaternion)
    {
        _targetRotation = quaternion;
        // Debug.Log($"{_targetRotation.eulerAngles}");
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
    }
    
}
