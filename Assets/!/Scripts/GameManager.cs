using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables

    static GameManager instance;

    public TextSection m_firstTextSection;

    [Header("Prefabs")]
    public GameObject m_choiceButtonPrefab;

    [Header("References")]
    public TextMeshProUGUI m_activeText;
    public Transform m_choiceButtonsParent;
    public ScrollRect m_scrollRect;
    public TextMeshProUGUI m_chapterNumber;

    [Header("Death choices")]
    public List<Choice> m_deathChoices;
    public SaveLoadManager m_slm;
    public ChoiceList m_choiceList;

    TextSection m_currentlyDisplayingTextSection;
    
    bool m_gameStart;
    
    #endregion

    void Awake(){
        if (instance == null){
            instance = this;
        } else {
            Destroy(instance.gameObject);
            instance = this;
        }
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }

    public void Start(){
        if (m_choiceList != null && m_choiceList.Array().Count >= 1){
            m_currentlyDisplayingTextSection = m_choiceList.Array()[^1].associatedTextSection;
        } else { // cover case of first opening
            m_choiceList = new();
            m_choiceList.Add(new(){text = "Beginning", associatedTextSection = m_firstTextSection, referenceNames = new()});
            m_currentlyDisplayingTextSection = m_firstTextSection;
        }
        if (m_currentlyDisplayingTextSection == null || m_currentlyDisplayingTextSection.text == null){ // cover case of bug and mis-loading/saving
            m_choiceList = new();
            m_choiceList.Add(new(){text = "Beginning", associatedTextSection = m_firstTextSection, referenceNames = new()});
            m_currentlyDisplayingTextSection = m_firstTextSection;
        }

        updateChapterNumber();

        m_activeText.text = "";
        showNextTextSection();  
        m_gameStart = true;     
    }

    #region Functions

    void choiceButtonClick(Choice choice){
        if (handleDeath(choice))
            return;

        m_choiceList.Add(choice);
        updateChapterNumber();

        m_currentlyDisplayingTextSection = getNextTextSection(choice);
        showNextTextSection();
    }

    TextSection getNextTextSection(Choice choice){
        m_activeText.text = "";
        if (choice.associatedTextSection != null){
            return choice.associatedTextSection;
        } else {
            throw new System.NotImplementedException("Wut");
        }
    }

    void showNextTextSection(){
        string s = m_currentlyDisplayingTextSection.text;
        m_activeText.text += m_currentlyDisplayingTextSection.text; // += -> so that TextInserts remain, they reset in getNextTextSection()

        for (int i = 0; i < m_choiceButtonsParent.childCount; i++){
            Destroy(m_choiceButtonsParent.GetChild(i).gameObject);
        }

        if (m_currentlyDisplayingTextSection.textInsert) {
            insertText(m_currentlyDisplayingTextSection.textInsert);
        } else if (m_currentlyDisplayingTextSection.textInsertIfs != null && m_currentlyDisplayingTextSection.textInsertIfs.Count > 0){
            foreach (TextSection section in m_currentlyDisplayingTextSection.textInsertIfs){
                bool found = false;

                if (section.required == null || section.required.Count <= 0){// no requirements
                    insertText(section);
                    break;
                }                     

                foreach (NestedChoiceList list in section.required){
                    if (m_choiceList.ContainsChoices(list.and)){
                        found = true;
                        insertText(section);
                        break;
                    }
                }
                if (found)
                    break;
            }
        } else {
            showNextButtons();
        }

        m_scrollRect.verticalNormalizedPosition = 1;
    }

    void insertText(TextSection section){
        m_currentlyDisplayingTextSection = section;
        if (m_activeText.text != "")
            m_activeText.text += "\n\n";
        showNextTextSection();  
    }

    void showNextButtons(){
        List<Choice> choices = m_currentlyDisplayingTextSection.nextChoices;

        if (choices.Count == 1 && choices[0].text == "Dead"){
            choices = m_deathChoices;
        }

        foreach (Choice choice in choices){
            Instantiate(m_choiceButtonPrefab, m_choiceButtonsParent).GetComponent<ChoiceButton>().m_choice = choice;
        }
    }

    bool handleDeath(Choice choice){
        if (choice.text == "Restart game"){
            RestartGame();
            return true;
        } else if (choice.text == "Load from last checkpoint"){
            LoadFromlastCheckpoint();
            return true;
        } else if (choice.text == "Load game"){
            m_slm.OpenMenu();
            return true;
        } else{
            return false;
        }
    }

    void loadFromLastCheckpoint(){
        if (m_choiceList.TryGetLastCheckpoint(out Choice choice)){
            m_choiceList.SetCheckpointPosition(choice);
            choiceButtonClick(choice);
        } else {
            RestartGame();
        }
    }

    void restartGame(){
        m_choiceList = new();
        Start();
    }

    void updateChapterNumber(){
        m_chapterNumber.text = $"Chapter {m_choiceList.GetChapterNumber()}";
    }

    public void SetGame(ChoiceList list){
        m_choiceList = list;
        if (m_gameStart) // this is so that unity calls the first start
            Start();
    }

    public static void RestartGame(){
        instance.restartGame();
    }

    public static void LoadFromlastCheckpoint(){
        instance.loadFromLastCheckpoint();
    }

    public static void ChoiceButtonClick(Choice choice){
        instance.choiceButtonClick(choice);
    }

    #endregion    
}
