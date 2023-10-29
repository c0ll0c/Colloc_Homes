public enum ClueType
{
    CODE,
    USER,
    FAKE
}

public class Clue
{
    public ClueType ClueType;
    public bool IsHidden = false;
    public bool IsGot = false;
    public string UserNickName = "";
    public string UserCode = "";
    public int Index;
    public int TypeIndex;

    public Clue (ClueType type, int _index, int _typeIndex)
    {
        ClueType = type;
        Index = _index;
        TypeIndex = _typeIndex;
    }
}
