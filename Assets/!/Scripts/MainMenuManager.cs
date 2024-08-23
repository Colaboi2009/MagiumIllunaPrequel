using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    #region Variables

    public GameObject m_mainMenu;
    public GameObject m_mainTextCanvas;

    public GameObject m_resetEverythingConfirmationMenu;

    public SaveLoadManager m_saveLoadManager;

    #endregion

    #region Functions

    public void btn_mainMenu(){
        m_mainMenu.SetActive(true);
        m_mainTextCanvas.SetActive(false);
    }

    public void btn_returnToGame(){
        m_mainMenu.SetActive(false);
        m_mainTextCanvas.SetActive(true);
    }

    public void btn_newGame(){
        GameManager.RestartGame();
        btn_returnToGame();
    }

    public void btn_loadFromLastCheckpoint(){
        GameManager.LoadFromlastCheckpoint();
        btn_returnToGame();
    }

    public void btn_saveLoadGame(){
        m_saveLoadManager.OpenMenu();
    }

    public void btn_resetEverything(){
        m_resetEverythingConfirmationMenu.SetActive(true);
    }

    public void btn_resetEverythingConfirmation(){
        m_saveLoadManager.ResetEverything();
    }

    public void btn_resetEverythingCancel(){
        m_resetEverythingConfirmationMenu.SetActive(false);
    }

    #endregion    
}
