using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rateau : ColliderDetector
{
    // Start is called before the first frame update
    void Start()
    {

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
        }
    }

}
