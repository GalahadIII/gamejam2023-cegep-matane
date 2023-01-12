using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionModule : MonoBehaviour
{
    private Collider col;

    [SerializeField] private List<GameObject> _objectsInRange = new();
    private bool _canInteract = false;

    private void FixedUpdate()
    {
        Vector3 position = transform.position;
        float lastClosestDistance = -1;
        IInteractable closest = null;

        foreach (GameObject o in _objectsInRange)
        {
            IInteractable oInteractable = o.GetComponent<IInteractable>();
            if (oInteractable == null) continue;
            
            float distance = (position - o.transform.position).magnitude;

            if (distance < lastClosestDistance || lastClosestDistance < 0)
            {
                closest = oInteractable;
                lastClosestDistance = distance;
            }
        }

        if (closest == null)
        {
            _canInteract = false;
            return;
        }

        closest.ShowHint();
        if (!_canInteract) return;
        closest.Interact();
        _canInteract = false;
    }
    
    private void TriggerInteraction()
    {
        _canInteract = true;
    }

    private void OnCollisionEnter(Collision col)
    {
        _objectsInRange.Add(col.gameObject);
    }
    private void OnCollisionExit(Collision col)
    {
        _objectsInRange.Remove(col.gameObject);
    }
    private void OnTriggerEnter(Collider col)
    {
        _objectsInRange.Add(col.gameObject);
    }
    private void OnTriggerExit(Collider col)
    {
        _objectsInRange.Remove(col.gameObject);
    }
}
