using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingScript : MonoBehaviour
{
    public AudioClip _ac;

    private AudioSource _as;

    void Start()
    {
        _as = gameObject.GetComponent<AudioSource>();
    }

    public void WalkSounds()
    {
        _as.PlayOneShot(_ac);
    }

}
