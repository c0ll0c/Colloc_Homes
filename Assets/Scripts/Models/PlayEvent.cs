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
        return "[이벤트 발생!] 기아 바이러스가 전파되었습니다. 서둘러 급식소에서 배급을 받으세요!";
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
        return "[이벤트 발생!] 짙은 안개가 시야를 방해합니다. 치우러 가볼까요?";
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
        return "[이벤트 발생!] 섬 내의 전기가 부족합니다. 발전소에서 전기를 생산해보아요!";
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
        return "[이벤트 발생!] 경감님이 돌무더기에 갇혔습니다. 경감님을 구하러 가세요!";
    }
    public void MiniGame()
    {
        distractionController.gameObject.SetActive(false);
        eventSolutionHandler.SolutionSaveNPC();
    }

}