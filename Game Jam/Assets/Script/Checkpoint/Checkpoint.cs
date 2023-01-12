using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float CamHeight;
    
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
            Debug.Log(gameObject.name);
            GameManager.Inst.Player.CurrentCheckpoint = this;
        }
    }
}
