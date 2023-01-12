using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] private float speed;

    private void Update()
    {
        
        if (transform.localPosition.y >= maxHeight && speed > 0)
        {
            speed = -1 * speed;
        }
        else if (transform.localPosition.y <= minHeight && speed < 0)
        {
            speed = -1 * speed;
        }

        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(position, currentTarget, speed * Time.deltaTime);
    }
}
