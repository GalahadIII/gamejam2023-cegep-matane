using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTop : MonoBehaviour
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
            col.gameObject.transform.position = new Vector3(1.1f, 67.36f, -2.267f);
        }
    }
}
