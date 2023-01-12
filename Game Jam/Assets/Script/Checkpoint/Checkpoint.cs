using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointId;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Component col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit cp");
            PlayerManager.CurrentCheckpoint = checkpointId;
        }
    }
}
