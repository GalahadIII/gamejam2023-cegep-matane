using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraShake cameraShake;

    // [SerializeField] private TowerContext facingDirection;
    // [SerializeField] private float degree;
    private Quaternion targetQuat = Quaternion.identity;
    [SerializeField] public float rotationSpeed;
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
        // if ((int)++GameManager.Inst.TowerSide > 3) GameManager.Inst.TowerSide = TowerContext.South;
        //     degree -= 90f;
        // }
        // else if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        // if ((int)--GameManager.Inst.TowerSide < 0) GameManager.Inst.TowerSide = TowerContext.West;
        //     degree += 90f;
        // }
        Rotate();
    }

    public void SetPosY(float yPos)
    {
        Transform t = transform;
        Vector3 pos = t.position;
        pos.y = yPos;
        t.position = pos;
    }
    public void SetQuaternion(Quaternion quaternion)
    {
        targetQuat = quaternion;
    }
    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQuat, rotationSpeed * Time.deltaTime);
    }

    // private enum FacingDirection
    // {
    //     South = 0,
    //     East = 1,
    //     North = 2,
    //     West = 3
    // }
}
