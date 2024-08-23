using System.Diagnostics;
using UnityEngine;

public class AlwaysOnBottom : MonoBehaviour
{
    #region Variables

    Transform m_trans;
    Transform m_parent;

    #endregion

    void Start(){
        m_trans = transform;
        m_parent = transform.parent;
    }

    void Update(){
        m_trans.SetAsLastSibling();
    }

    #region Functions

    #endregion    
}

