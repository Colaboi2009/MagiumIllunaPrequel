using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceButton : MonoBehaviour, IPointerClickHandler
{
    #region Variables

    public Choice m_choice;

    public TextMeshProUGUI m_text;

    #endregion
    
    void Start(){
        m_text.text = m_choice.text;
    }

    #region Functions

    public void OnPointerClick(PointerEventData eventData){
        GameManager.ChoiceButtonClick(m_choice);
    }

    #endregion
}