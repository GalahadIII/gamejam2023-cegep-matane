using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rateau : ColliderDetector
{
    // Start is called before the first frame update
    public AudioClip achSound;

    public AudioSource audioSource;
    public GameObject achPanel;


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

}
