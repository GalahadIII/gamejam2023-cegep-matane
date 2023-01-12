using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    
    private void Start()
    {
        
    }
    
    private void Update()
    {
    }

    private void OnTriggerEnter(Component col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit cp");
            GameManager.Inst.Player.CurrentCheckpoint = this;
        }
    }
}
