using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesMenuManager : MonoBehaviour
{
    #region Variables

    static ChoicesMenuManager m_instance;

    public GameManager m_gm;
    
    public GameObject m_mainTextCanvas;
    public GameObject m_choicesMenu;
    public GameObject m_buttonGameObjectForBackOnMainChoiceMenu;

    public GameObject m_textArea;
    public TextMeshProUGUI m_text;
    public ScrollRect m_scrollRect;
    
    public Transform m_choiceListingParent;
    public GameObject m_choiceListingPrefab;

    List<Choice> m_currentlyDisplayedChoiceList;

    #endregion

    void Awake(){
        if (m_instance == null){
            m_instance = this;
        } else {
            Destroy(m_instance.gameObject);
            m_instance = this;
        }
    }

    void Update(){
        if (m_currentlyDisplayedChoiceList == null){
            updateChoicesDisplay();
        }
    }

    #region Functions

    void updateChoicesDisplay(){
        foreach (Transform go in m_choiceListingParent){
            Destroy(go.gameObject);
        }

        List<Choice> choices = m_gm.m_choiceList.Array();
        foreach (Choice c in choices){
            GameObject go = Instantiate(m_choiceListingPrefab, m_choiceListingParent);
            go.GetComponentInChildren<TextMeshProUGUI>().text = c.text;
            go.GetComponent<ChoiceMenuButton>().m_choice = c;
        }
        m_currentlyDisplayedChoiceList = choices;
    }

    void displayText(Choice choice){
        m_buttonGameObjectForBackOnMainChoiceMenu.SetActive(false);
        m_choiceListingParent.gameObject.SetActive(false);

        m_textArea.SetActive(true);
        m_text.text = "";
        
        showNextTextSection(choice.associatedTextSection);
        m_text.text += "\n\n\n\n\n\n\n\n\n\n\n."; // padding lol
    }

    void showNextTextSection(TextSection sect){
        m_text.text += sect.text;

        if (sect.textInsert) {
            insertText(sect, sect.textInsert);
        } else if (sect.textInsertIfs != null && sect.textInsertIfs.Count > 0){
            foreach (TextSection section in sect.textInsertIfs){
                bool found = false;

                if (section.required == null || section.required.Count <= 0){// no requirements
                    insertText(sect, section);
                    break;
                }                     

                foreach (NestedChoiceList list in section.required){
                    if (m_gm.m_choiceList.ContainsChoices(list.and)){
                        found = true;
                        insertText(sect, section);
                        break;
                    }
                }
                if (found)
                    break;
            }
        }

        m_scrollRect.verticalNormalizedPosition = 1;
    }

    void insertText(TextSection displaying, TextSection section){
        displaying = section;
        if (m_text.text != "")
            m_text.text += "\n\n";
        showNextTextSection(displaying);  
    }

    public static void btn_displayText(Choice choice){
        m_instance.displayText(choice);
    }

    public void btn_choices(){
        m_choicesMenu.SetActive(true);
        m_mainTextCanvas.SetActive(false);
        updateChoicesDisplay();
    }

    public void btn_return(){
        m_choicesMenu.SetActive(false);
        m_mainTextCanvas.SetActive(true);
    }
    
    public void btn_backToChoicesMenu(){
        m_textArea.SetActive(false);
        m_choiceListingParent.gameObject.SetActive(true);
        m_buttonGameObjectForBackOnMainChoiceMenu.SetActive(true);
    }

    #endregion    
}
