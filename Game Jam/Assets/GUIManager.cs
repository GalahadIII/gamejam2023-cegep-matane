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

    public GameObject RestartMenuDeath;
    public GameObject PauseMenu;
    
    //

    public void Toggle_RestartMenuDeath()
    {
        RestartMenuDeath.SetActive(!RestartMenuDeath.activeSelf);
    }

    public void Toggle_PauseMenu()
    {
        bool active = !PauseMenu.activeSelf;
        PauseMenu.SetActive(active);
        Time.timeScale = active ? 0 : 1;
    }
    
    //

    public void Menu_ExitGame()
    {
        Application.Quit();
    }

    public void Menu_Respawn()
    {
        GameManager.Inst.Player.Respawn();
        Toggle_RestartMenuDeath();
    }
    
    //

    public static void Load_MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void Load_Game()
    {
        SceneManager.LoadScene("niveau1-gamejam-level");
        Time.timeScale = 1;
    }
    
    
    
    
    

}
