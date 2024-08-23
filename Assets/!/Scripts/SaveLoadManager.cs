using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    #region Variables

    static SaveLoadManager m_instance;

    public GameObject m_saveLoadMenu;

    public GameObject m_saveLoadListingPrefab;
    public Transform m_saveLoadListingParent;

    public TextMeshProUGUI m_temp;

    GameManager m_gameM;
    MainMenuManager m_mainMenuM;

    string Filepath => Application.persistentDataPath + "/SaveSlots.json";

    List<SaveSlot> m_saveSlots;

    #endregion

    void Awake(){
        if (m_instance == null){
            m_instance = this;
        } else {
            Destroy(m_instance.gameObject);
            m_instance = this;
        }

        m_gameM = GetComponent<GameManager>();
        m_mainMenuM = GetComponent<MainMenuManager>();

        m_saveSlots = new();

        if (System.IO.File.Exists(Filepath)){
            string json = System.IO.File.ReadAllText(Filepath);
            AllSaveSlots game = JsonUtility.FromJson<AllSaveSlots>(json);
            m_gameM.SetGame(game.activeSlot.choiceList);
            m_saveSlots = game.slots;
            m_temp.text = game.activeSlot.slotName;
        }
    }

    void OnApplicationPause(){
        SaveSlot activeSlot = new() {choiceList = m_gameM.m_choiceList, slotName = $"Current {m_saveSlots.Count}"};
        AllSaveSlots game = new() {slots = m_saveSlots, activeSlot = activeSlot};
        string json = JsonUtility.ToJson(game);

        if (!System.IO.File.Exists(Filepath))
            System.IO.File.Create(Filepath);
        System.IO.File.WriteAllText(Filepath, json);       
    }

    #region Functions

    void displayLoadSlots(){
        foreach (Transform trans in m_saveLoadListingParent){
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < m_saveSlots.Count; i++) {
            SlotButton sb = Instantiate(m_saveLoadListingPrefab, m_saveLoadListingParent).GetComponent<SlotButton>();
            sb.m_slotName = m_saveSlots[i].slotName;
            sb.m_index = i;
        }
    }

    void save(string slotName, int slotIndex){
        m_saveSlots[slotIndex].slotName = slotName;
        m_saveSlots[slotIndex].choiceList = m_gameM.m_choiceList.Copy();
    }

    void load(int slotIndex){
        Debug.Assert(slotIndex < m_saveSlots.Count, "How tf did u get a higher slot index!!!");
        m_gameM.SetGame(m_saveSlots[slotIndex].choiceList.Copy());
        btn_return();
        m_mainMenuM.btn_returnToGame();
    }

    void changeName(string newName, int slotIndex){
        m_saveSlots[slotIndex].slotName = newName;
    }

    public void OpenMenu(){
        m_saveLoadMenu.SetActive(true);
        displayLoadSlots();
    }

    public void ResetEverything(){
        m_saveSlots = new();
        System.IO.File.Delete(Filepath);
    }

    public void btn_return(){
        m_saveLoadMenu.SetActive(false);
    }

    public void btn_saveNew(){
        SaveSlot slot = new(){
            choiceList = m_gameM.m_choiceList,
            slotName = "New Slot"
        };
        m_saveSlots.Add(slot);
        displayLoadSlots();
    }

    public static void btn_Save(string slotName, int slotIndex){
        m_instance.save(slotName, slotIndex);
    }

    public static void btn_Load(int slotIndex){
        m_instance.load(slotIndex);
    }

    public static void btn_changeName(string newName, int slotIndex){
        m_instance.changeName(newName, slotIndex);
    }

    #endregion    
}

[Serializable]
public class AllSaveSlots{
    public List<SaveSlot> slots;
    public SaveSlot activeSlot;
}

[Serializable]
public class SaveSlot{
    public ChoiceList choiceList;
    public string slotName;
}
