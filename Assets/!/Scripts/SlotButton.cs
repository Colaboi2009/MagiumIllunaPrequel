using TMPro;
using UnityEngine;

public class SlotButton : MonoBehaviour
{
    #region Variables

    public TMP_InputField m_inputName;
    public string m_slotName;
    public int m_index;

    #endregion

    void Start(){
        m_inputName.text = m_slotName;
    }
    
    void Update(){
        if (m_inputName.text != m_slotName){
            m_slotName = m_inputName.text;
            updateName();
        }
    }

    #region Functions

    void updateName(){
        SaveLoadManager.btn_changeName(m_slotName, m_index);
    }

    public void btn_Save(){
        m_slotName = m_inputName.text;
        SaveLoadManager.btn_Save(m_slotName, m_index);
    }

    public void btn_Load(){
        SaveLoadManager.btn_Load(m_index);
    }

    #endregion
}