using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ChoiceList
{
    #region Variables

    [SerializeField] List<Choice> m_choiceList;

    #endregion

    public ChoiceList(){
        m_choiceList = new();
    }

    #region Functions

    public void Add(Choice choice){
        m_choiceList.Add(choice);
    }

    public bool ContainsChoices(List<Choice> list){
        bool textMatch = list.All(elem => m_choiceList.Any(elem2 => elem2.text == elem.text));
        bool referenceMatch = list.All( // essentially just checks if any referenceNames members matches in both choices
            elem => m_choiceList.Any(
                elem2 => elem2.referenceNames.Any(
                    elem3 => elem.referenceNames.Any(
                        elem4 => elem4 == elem3))));
        return textMatch || referenceMatch;
    }

    public bool TryGetLastCheckpoint(out Choice choice){
        for (int i = m_choiceList.Count - 1; i > -1; i--){
            if (m_choiceList[i].checkpointFlag){
                choice = m_choiceList[i];
                return true;
            }
        }
        choice = new();
        return false;
    }

    public void SetCheckpointPosition(Choice choice){
        int index;
        for (index = 0; index < m_choiceList.Count; index++){
            if (m_choiceList[index] == choice)
                break;
        }
        if (index + 1 != m_choiceList.Count)
            m_choiceList.RemoveRange(index, m_choiceList.Count - index);
    }

    public int GetChapterNumber(){
        int count = 0;
        for (int i = m_choiceList.Count - 1; i > -1; i--){
            if (m_choiceList[i].checkpointFlag){
                count++;
            }
        }
        return count + 1;
    }

    public ChoiceList Copy(){
        ChoiceList copy = new();
        foreach (Choice choice in m_choiceList){
            copy.Add(choice.Copy());
        }
        return copy;
    }

    public List<Choice> Array() => m_choiceList;

    #endregion
}
