// �ܼ� ���� �����͸� �����ϴ� ��ũ��Ʈ
// �������� CODE, USER, FAKE �ܼ� Ÿ�� �ο� (enum)
// �ܼ��� ���� ������ �ִ��� �ƴ���
// �������� �Ҵ�Ǵ� ������
// �ܼ� ����

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
