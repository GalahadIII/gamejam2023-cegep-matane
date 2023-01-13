using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    public AudioClip _ac;

    private AudioSource _as;
    // Start is called before the first frame update
    void Start()
    {
        _as = gameObject.GetComponent<AudioSource>();
        Rigidbody[] listRigidbody = GetComponentsInChildren<Rigidbody>();
        // Debug.Log(listRigidbody.Length);
        _as.PlayOneShot(_ac);
        foreach (var rb in listRigidbody)
        {
            Vector3 dir = (rb.transform.position - transform.position).normalized;
            // Debug.Log(dir);
            // + Random.insideUnitSphere * 0.3f
            rb.AddForce(dir * Random.Range(9, 13), ForceMode.Impulse);
        }
    }
}
