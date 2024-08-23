using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceMenuButton : MonoBehaviour, IPointerClickHandler
{
    #region Variables

    public Choice m_choice;

    #endregion
    
    #region Functions

    public void OnPointerClick(PointerEventData eventData){
        ChoicesMenuManager.btn_displayText(m_choice);
    }

    #endregion
}
