using System.Collections.Generic;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    [SerializeField] public List<GameObject> Objects = new();
    [SerializeField] public GameObject Closest = null;

    protected void FixedUpdate()
    {
        SetClosest();
    }

    protected void SetClosest()
    {
        Vector3 position = transform.position;
        float lastClosestDistance = -1;
        Closest = null;

        foreach (GameObject o in Objects)
        {
            float distance = (position - o.transform.position).magnitude;

            if (distance < lastClosestDistance || lastClosestDistance < 0)
            {
                Closest = o;
                lastClosestDistance = distance;
            }
        }
    }
    
    private void OnCollisionEnter(Collision col)
    {
        Objects.Add(col.gameObject);
    }
    private void OnCollisionExit(Collision col)
    {
        Objects.Remove(col.gameObject);
    }
    private void OnTriggerEnter(Collider col)
    {
        Objects.Add(col.gameObject);
    }
    private void OnTriggerExit(Collider col)
    {
        Objects.Remove(col.gameObject);
    }
}
