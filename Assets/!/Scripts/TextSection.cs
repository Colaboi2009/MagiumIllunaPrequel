using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSection : MonoBehaviour
{
    #region Variables

    public string text;

    [Space]
    [Space]
    public TextSection textInsert;
    [Space]
    [Tooltip("Note: 2nd dimension array for ANDs, 1st dimension array for ORs")]
    public List<NestedChoiceList> required;
    [Tooltip("Note: Lower indices take priority")]
    public List<TextSection> textInsertIfs;
    [Space]
    [Space]

    public List<Choice> nextChoices;


    #endregion

    #region Functions

    public bool Matches(){
        throw new System.NotImplementedException();
    }

    #endregion    
}

[Serializable]
public class NestedChoiceList{
    public List<Choice> and;
}