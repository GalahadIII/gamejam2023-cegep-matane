using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevierController : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        animator.ResetTrigger("isActivated");
        animator.ResetTrigger("isReactivated");
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            animator.SetTrigger("isActivated");

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            animator.SetTrigger("isReactivated");
        }
    }
}
