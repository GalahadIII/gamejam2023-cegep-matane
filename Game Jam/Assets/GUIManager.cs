using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Inst { get; private set; }
    private GUIManager() { Inst = this; }
    private void OnEnable()
    {
        Inst = this;
    }

    public InteractionGUI InteractOverLay;
    public GameObject RestartMenuDeath;
    
    //

    public void Display_RestartMenuDeath()
    {
        RestartMenuDeath.SetActive(!RestartMenuDeath.activeSelf);
    }
    
    //

    public void Menu_ExitGame()
    {
        Application.Quit();
    }

    public void Menu_Restart()
    {
        SceneManager.LoadScene("niveau1");
    }
    
    
    
    

}
