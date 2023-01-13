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

    public void DisplayToggle_RestartMenuDeath()
    {
        Debug.Log($"DisplayToggle_RestartMenuDeath");
        RestartMenuDeath.SetActive(!RestartMenuDeath.activeSelf);
    }

    public void DisplayToggle_PauseMenu()
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

    public void Menu_Restart()
    {
        SceneManager.LoadScene("niveau1");
    }

    public void Menu_Respawn()
    {
        GameManager.Inst.Player.Respawn();
        DisplayToggle_RestartMenuDeath();
    }
    
    
    
    
    

}
