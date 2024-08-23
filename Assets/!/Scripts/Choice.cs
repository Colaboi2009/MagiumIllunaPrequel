using System.Collections.Generic;

[System.Serializable]
public class Choice
{
    #region Variables

    public string text;
    public TextSection associatedTextSection;
    public List<string> referenceNames;
    public bool checkpointFlag;

    #endregion

    #region Functions

    public Choice Copy(){
        Choice c = new() {
            text = text,
            associatedTextSection = associatedTextSection,
            referenceNames = new(),
            checkpointFlag = checkpointFlag,
        };
        
        referenceNames.CopyTo(c.referenceNames.ToArray());
        return c;
    }

    #endregion    
}
