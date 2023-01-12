using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraShake cameraShake;

    [SerializeField] private TowerContext facingDirection;
    [SerializeField] private float degree;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float magnitude;

    [SerializeField] private float duration;

    private TowerContext lastFacing;
    // Start is called before the first frame update
    void Start()
    {
        facingDirection = TowerContext.South;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(cameraShake.Shake(duration, magnitude));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            lastFacing = facingDirection;
            degree -= 90f;
            facingDirection += 1;
            if ((int)(facingDirection) > 3)
            {
                facingDirection = 0;
            }


        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lastFacing = facingDirection;
            degree += 90f;
            facingDirection -= 1;
            if ((int)(facingDirection) < 0)
            {
                facingDirection = TowerContext.West;
            }
        }
        Rotate();
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, degree, 0), rotationSpeed * Time.deltaTime);
    }

    // private enum FacingDirection
    // {
    //     South = 0,
    //     East = 1,
    //     North = 2,
    //     West = 3
    // }
}
