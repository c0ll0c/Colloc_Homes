// 단서 상태 데이터를 저장하는 스크립트
// 랜덤으로 CODE, USER, FAKE 단서 타입 부여 (enum)
// 단서가 현재 숨겨져 있는지 아닌지
// 랜덤으로 할당되는 포지션
// 단서 내용

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
    public string UserNickName = "";
    public string UserCode = "";

    public Clue (ClueType type)
    {
        ClueType = type;
    }
}
