using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelle : Trap
{

    public BoxCollider children;
    public Animator animator;

    void Start()
    {

        children.enabled = false;

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.Inst.Player.Die();
        }
    }
    public override void Trigger()
    {
        children.enabled = true;
        animator.SetTrigger("isTriggered");

    }
    public void Disable()
    {
        children.enabled = false;
    }
}
