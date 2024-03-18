using UnityEngine.SceneManagement;

public interface IPlayEvent
{
    DistractionController Distraction { get; set; }
    EventSolutionHandler Solution { get; set; }
    string SceneName { get; set; }

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
    void MiniGame()
    {
        Distraction.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneName, mode: LoadSceneMode.Additive);
    }
    void CloseMiniGame(bool _clear)
    {
        Distraction.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(SceneName);
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
    public string SceneName { get; set; }

    public string Play()
    {
        SceneName = "MiniGameHungryScene";
        Distraction.StartHungry();
        return "[이벤트 발생!] 기아 바이러스가 전파되었습니다. 서둘러 급식소에서 배급을 받으세요!";
    }
}
class EventFog : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    public string SceneName { get; set; }

    public string Play()
    {
        SceneName = "MiniGameFogScene";
        Distraction.StartFog();
        return "[이벤트 발생!] 짙은 안개가 시야를 방해합니다. 치우러 가볼까요?";
    }
}
class EventElec : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    public string SceneName { get; set; }

    public string Play()
    {
        SceneName = "MiniGameElecScene";
        Distraction.StartElec();
        return "[이벤트 발생!] 섬 내의 전기가 부족합니다. 발전소에서 전기를 생산해보아요!";
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
    public string SceneName { get; set; }

    public string Play()
    {
        SceneName = "MiniGameSaveNPCScene";
        Distraction.StartSaveNPC();
        return "[이벤트 발생!] 경감님이 돌무더기에 갇혔습니다. 경감님을 구하러 가세요!";
    }
    public void MiniGame()
    {
        Distraction.gameObject.SetActive(false);
        Solution.SolutionSaveNPC();
    }

}