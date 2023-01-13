using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Inst { get; private set; }
    private GUIManager() { Inst = this; }
    private void OnEnable()
    {
        Inst = this;
    }

    public InteractionGUI InteractOverLay;

}
