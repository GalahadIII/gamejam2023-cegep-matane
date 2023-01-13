using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Opened = false;
    public float PivotSpeed = 300f;
    public Vector3 PivotOffset = new (0,0,0);
    public Quaternion ClosedRotation = Quaternion.Euler(0,0,0);
    public Quaternion OpenedRotation = Quaternion.Euler(0,0,0);
    
    public Quaternion TargetRotation => Opened ? OpenedRotation : ClosedRotation;

    private Transform t;
    
    private void OnEnable()
    {
        t = transform;
        if (Opened) Pivot(true);
        // Debug.Log($"{t.rotation}");
        
        t.localPosition = PivotOffset;
    }

    private void FixedUpdate()
    {
        t.localRotation = Quaternion.RotateTowards(t.localRotation, TargetRotation, PivotSpeed / 50);
    }
    
    //

    public void DisplayInteractHint()
    {
        
    }

    public void SetScale(Vector3 scale)
    {
        t.localScale = scale;
    }

    public void Pivot(bool set = false)
    {
        Opened = !Opened;
        
        if (set) t.localRotation = TargetRotation;
    }

    public void SetOpen(bool opened)
    {
        Opened = opened;
    }
}