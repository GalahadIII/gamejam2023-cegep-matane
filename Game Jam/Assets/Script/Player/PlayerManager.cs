using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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
        // Debug.Log($"OnEnable {_targetRot.eulerAngles}");

        moveController = GetComponent<MovementController>();
        interactionModule = GetComponentInChildren<InteractionModule>();
    }
    private void Update()
    {
        Transform t = transform;

        float velX = GameManager.Inst.ConvertVector(moveController.Speed).x;
        // Vector3 vel = moveController.Speed;
        Vector3 newScale = new(Mathf.Sign(velX), 1, 1);
        t.localScale = newScale;
        // t.localScale = GameManager.Inst.ConvertVector(newScale, true);
        // Debug.Log($"{velX} {newScale} {t.localScale}");

        GameManager.Inst.CameraController.SetPosY(Math.Max(t.position.y, GameManager.Inst.CameraController.CameraMinHeight));

        if (moveController.Speed.magnitude > 0.1f && moveController.Grounded)
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
            positionChute = t.position;
        }
        else if (!moveController.Falling && fallingLastFrame)
        {
            //on a touche le sol
            if (positionChute.y - t.position.y > distanceMort)
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
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        modeleVivant.SetActive(false);
        Instantiate(deadBody, transform.position, quaternion.identity);
        explode();
        outlineVivant.SetActive(false);
        moveController.DisabledControls = true;
        isDead = true;
    }

    [ContextMenu("Respawn")]
    private void Respawn()
    {
        isDead = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        modeleVivant.SetActive(true);
        Destroy(deadBody);
        outlineVivant.SetActive(true);
        moveController.DisabledControls = false;
        ResetVel();
        gameObject.transform.position = CurrentCheckpoint.transform.position;
    }

    public void explode()
    {
        Rigidbody[] listRigidbody = deadBody.GetComponentsInChildren<Rigidbody>();
        Debug.Log(listRigidbody.Length);
        foreach (var rb in listRigidbody)
        {
            Vector3 dir = (rb.transform.position - transform.position + Random.insideUnitSphere * 0.3f).normalized;
            
            rb.AddForce(dir * Random.Range(7, 13), ForceMode.Impulse);
        }
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
