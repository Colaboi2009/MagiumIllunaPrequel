using UnityEngine;
using UnityEngine.UI;

public class ScrollAdjuster : MonoBehaviour
{
    #region Variables

    public float m_threshold;
    public ScrollRect m_scrollView;

    #endregion

    void Update(){
        if (Mathf.Abs(m_scrollView.velocity.y) < m_threshold){
            m_scrollView.StopMovement();
        }
    }

    #region Functions
    #endregion    
}
