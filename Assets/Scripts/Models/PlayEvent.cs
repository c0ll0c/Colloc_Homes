public interface IPlayEvent
{
    DistractionController distractionController { get; set; }
    EventSolutionHandler eventSolutionHandler { get; set; }
    void SetDistractionController(DistractionController _dc)
    {
        distractionController = _dc;
    }
    void SetSolutionController(EventSolutionHandler _esh)
    {
        eventSolutionHandler = _esh;
        eventSolutionHandler.SetClickable();
    }
    string Play();
    void MiniGame();
    void CloseMiniGame(bool _clear)
    {
        distractionController.gameObject.SetActive(true);
        if (_clear) Clear();
    }
    void Clear()
    {
        distractionController.StopEvent();
    }
}

class EventHungry : IPlayEvent
{
    public DistractionController distractionController { get; set; }
    public EventSolutionHandler eventSolutionHandler { get; set; }

    public string Play()
    {
        distractionController.StartHungry();
        return "[�̺�Ʈ �߻�!] ��� ���̷����� ���ĵǾ����ϴ�. ���ѷ� �޽ļҿ��� ����� ��������!";
    }
    public void MiniGame()
    {
        distractionController.gameObject.SetActive(false);
        eventSolutionHandler.SolutionHungry();
    }

}
class EventFog : IPlayEvent
{
    public DistractionController distractionController { get; set; }
    public EventSolutionHandler eventSolutionHandler { get; set; }
    
    public string Play()
    {
        distractionController.StartFog();
        return "[�̺�Ʈ �߻�!] £�� �Ȱ��� �þ߸� �����մϴ�. ġ�췯 �������?";
    }
    public void MiniGame()
    {
        distractionController.gameObject.SetActive(false);
        eventSolutionHandler.SolutionFog();
    }

}
class EventElec : IPlayEvent
{
    public DistractionController distractionController { get; set; }
    public EventSolutionHandler eventSolutionHandler { get; set; }
    
    public string Play()
    {
        distractionController.StartElec();
        return "[�̺�Ʈ �߻�!] �� ���� ���Ⱑ �����մϴ�. �����ҿ��� ���⸦ �����غ��ƿ�!";
    }
    public void MiniGame()
    {
        distractionController.gameObject.SetActive(false);
        eventSolutionHandler.SolutionElec();
    }

}
class EventSaveNPC : IPlayEvent
{
    public DistractionController distractionController { get; set; }
    public EventSolutionHandler eventSolutionHandler { get; set; }
    
    public string Play()
    {
        distractionController.StartSaveNPC();
        return "[�̺�Ʈ �߻�!] �氨���� �������⿡ �������ϴ�. �氨���� ���Ϸ� ������!";
    }
    public void MiniGame()
    {
        distractionController.gameObject.SetActive(false);
        eventSolutionHandler.SolutionSaveNPC();
    }

}