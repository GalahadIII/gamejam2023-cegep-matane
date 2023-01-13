using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody[] listRigidbody = GetComponentsInChildren<Rigidbody>();
        Debug.Log(listRigidbody.Length);
        foreach (var rb in listRigidbody)
        {
            Vector3 dir = (rb.transform.position - transform.position).normalized;
            Debug.Log(dir);
            // + Random.insideUnitSphere * 0.3f
            rb.AddForce(dir * Random.Range(9,13), ForceMode.Impulse);
        }
    }
}
