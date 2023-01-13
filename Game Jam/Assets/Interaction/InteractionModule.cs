using UnityEngine;

public class InteractionModule : ColliderDetector, IPosition
{
    public Vector3 WorldPosition => transform.position + new Vector3(0, 1, 0);

    public IInteractable ClosestInteractable = null;
    private bool _canInteract = false;

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 position = transform.position;
        float lastClosestDistance = -1;
        ClosestInteractable = null;

        foreach (GameObject o in Objects)
        {
            IInteractable oInteractable = o.GetComponent<IInteractable>();
            if (oInteractable == null) continue;

            float distance = (position - o.transform.position).magnitude;

            if (distance < lastClosestDistance || lastClosestDistance < 0)
            {
                ClosestInteractable = oInteractable;
                lastClosestDistance = distance;
            }
        }

        if (ClosestInteractable == null)
        {
            InteractionGUI.Inst.Active = this;
            _canInteract = false;
            return;
        }

        InteractionGUI.Inst.Active = ClosestInteractable;
        ClosestInteractable.ShowHint();
        if (!_canInteract) return;
        ClosestInteractable.Interact();
        _canInteract = false;

    }

    public void TriggerInteraction()
    {
        _canInteract = true;
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
