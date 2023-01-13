using UnityEngine;

public class InteractOutlineLayer : InteractBase, IInteractable
{
    private bool _active = false;
    private int _activeTimeLeft = 0;
    
    public GameObject LayoutSetObject ;
    public int DefaultLayer = 0;
    public int OutlineLayer = 7;
    
    private void FixedUpdate()
    {
        _active = _activeTimeLeft-- > 0;
        int targetLayer = _active ? OutlineLayer : DefaultLayer;
        Debug.Log($"{OutlineLayer} {DefaultLayer}");
        if (LayoutSetObject.layer != targetLayer)
            SetGameLayerRecursive(LayoutSetObject, targetLayer);
    }

    private static void SetGameLayerRecursive(GameObject o, int layer)
    {
        o.layer = layer;
        foreach (Transform child in o.transform)
            if (child.GetComponentInChildren<Transform>() != null) SetGameLayerRecursive(child.gameObject, layer);
    }


    public new void ShowHint()
    {
        Debug.Log($"ShowHint");
        _activeTimeLeft = 5;
        base.ShowHint();
    }
}
