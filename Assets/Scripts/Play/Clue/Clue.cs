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

    public Clue (ClueType _type, int _index, int _typeIndex, string _nickname, string _code)
    {
        ClueType = _type;
        Index = _index;
        TypeIndex = _typeIndex;
        UserNickName = _nickname;
        UserCode = _code;
    }
}
