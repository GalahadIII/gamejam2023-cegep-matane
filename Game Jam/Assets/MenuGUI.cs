using UnityEngine;

public class MenuGUI : MonoBehaviour
{
    public static MenuGUI Inst { get; private set; }
    private MenuGUI() { Inst = this; }
    private void OnEnable()
    {
        Inst = this;
    }
    
    //

    private bool _canvasActive = true;
    public void ToggleCanvas()
    {
        _canvasActive = !_canvasActive;
        GetComponent<Canvas>().enabled = _canvasActive;
    }
    
    //

    public void MainMenu_Commencer()
    {
        Debug.Log("MenuGUI.Inst.ButtonCommencer()");
        GUIManager.Load_Game();
    }

    public void MainMenu_Credits()
    {
        Debug.Log("MenuGUI.Inst.ButtonCredits()");
        
    }
    
    //
    
}
