using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float CamHeight;
    public Animator animator;

    private void Start()
    {

    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("isTriggered");
            Debug.Log(gameObject.name);
            GameManager.Inst.Player.CurrentCheckpoint = this;
        }
    }
}
