using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelle : Trap
{

    public BoxCollider children;
    public Animator animator;

    public AudioClip _ac;

    private AudioSource _as;

    void Start()
    {
        _as = gameObject.GetComponent<AudioSource>();
        children.enabled = false;

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _as.PlayOneShot(_ac);
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
