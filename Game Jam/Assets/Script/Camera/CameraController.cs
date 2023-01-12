using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraShake cameraShake;

    // [SerializeField] private TowerContext facingDirection;
    [SerializeField] private float degree;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float magnitude;

    [SerializeField] private float duration;

    // private TowerContext lastFacing;
    // Start is called before the first frame update
    // void Start()
    // {
        // facingDirection = TowerContext.South;
    // }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.isDead)
        {
            StartCoroutine(cameraShake.Shake(duration, magnitude));
        }
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //
        //
        // }
        // else if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        // }
        Rotate();
    }

    public void PivotRight()
    {
        degree -= 90f;
    }
    public void PivotLeft()
    {
        degree += 90f;
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
