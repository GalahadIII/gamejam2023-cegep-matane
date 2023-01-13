using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rateau : Trap
{
    // Start is called before the first frame update
    public AudioClip achSound;

    public AudioSource audioSource;
    public GameObject achPanel;

    public BoxCollider children;
    public Animator animator;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        children.enabled = false;

    }



    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.Inst.Player.Die();
            achPanel.SetActive(true);
            audioSource.PlayOneShot(achSound);
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
