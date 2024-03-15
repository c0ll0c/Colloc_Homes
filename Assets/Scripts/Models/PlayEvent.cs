public interface IPlayEvent
{
    DistractionController Distraction { get; set; }
    EventSolutionHandler Solution { get; set; }
    void SetDistractionController(DistractionController _dc)
    {
        Distraction = _dc;
    }
    void SetSolutionController(EventSolutionHandler _esh)
    {
        Solution = _esh;
        Solution.SetClickable();
    }
    string Play();
    void MiniGame();
    void CloseMiniGame(bool _clear)
    {
        Distraction.gameObject.SetActive(true);
        if (_clear) Clear();
    }
    void Clear()
    {
        Distraction.StopEvent();
    }
}

class EventHungry : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }

    public string Play()
    {
        Distraction.StartHungry();
        return "[�̺�Ʈ �߻�!] ��� ���̷����� ���ĵǾ����ϴ�. ���ѷ� �޽ļҿ��� ����� ��������!";
    }
    public void MiniGame()
    {
        Distraction.gameObject.SetActive(false);
        Solution.SolutionHungry();
    }

}
class EventFog : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    
    public string Play()
    {
        Distraction.StartFog();
        return "[�̺�Ʈ �߻�!] £�� �Ȱ��� �þ߸� �����մϴ�. ġ�췯 �������?";
    }
    public void MiniGame()
    {
        Distraction.gameObject.SetActive(false);
        Solution.SolutionFog();
    }

}
class EventElec : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    
    public string Play()
    {
        Distraction.StartElec();
        return "[�̺�Ʈ �߻�!] �� ���� ���Ⱑ �����մϴ�. �����ҿ��� ���⸦ �����غ��ƿ�!";
    }
    public void MiniGame()
    {
        Distraction.gameObject.SetActive(false);
        Solution.SolutionElec();
    }

}
class EventSaveNPC : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    
    public string Play()
    {
        Distraction.StartSaveNPC();
        return "[�̺�Ʈ �߻�!] �氨���� �������⿡ �������ϴ�. �氨���� ���Ϸ� ������!";
    }
    public void MiniGame()
    {
        Distraction.gameObject.SetActive(false);
        Solution.SolutionSaveNPC();
    }

}