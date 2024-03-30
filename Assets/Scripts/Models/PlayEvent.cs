using UnityEngine;
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
    void Init();
    string Play();
    void MiniGame()
    {
        Distraction.gameObject.SetActive(false);
        NetworkManager.Instance.PlaySceneManager.TurnOnSolution();
    }
    void CloseMiniGame(bool _clear)
    {
        NetworkManager.Instance.PlaySceneManager.MiniGameSceneObj.SetActive(false);
        Distraction.gameObject.SetActive(true);
        if (_clear) Clear();
    }
    void Clear()
    {
        NetworkManager.Instance.PlaySceneManager.MiniGameSceneObj = null;
        SceneManager.UnloadSceneAsync(SceneName);
        Distraction.StopEvent();
    }
}

class EventHungry : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    public string SceneName { get; set; }

    public void Init()
    {
        SceneName = "MiniGameHungryScene";
    }
    public string Play()
    {
        Distraction.StartHungry();
        return "[이벤트 발생!] 기아 바이러스가 전파되었습니다. 서둘러 급식소에서 배급을 받으세요!";
    }
}
class EventFog : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    public string SceneName { get; set; }

    public void Init()
    {
        SceneName = "MiniGameFogScene";
    }
    public string Play()
    {
        Distraction.StartFog();
        return "[이벤트 발생!] 짙은 안개가 시야를 방해합니다. 치우러 가볼까요?";
    }
}
class EventElec : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    public string SceneName { get; set; }

    public void Init()
    {
        SceneName = "MiniGameElecScene";
    }
    public string Play()
    {
        Distraction.StartElec();
        return "[이벤트 발생!] 섬 내의 전기가 부족합니다. 발전소에서 전기를 생산해보아요!";
    }

}
class EventSaveNPC : IPlayEvent
{
    public DistractionController Distraction { get; set; }
    public EventSolutionHandler Solution { get; set; }
    public string SceneName { get; set; }

    public void Init()
    {
        SceneName = "MiniGameSaveNPCScene";
    }
    public string Play()
    {
        Distraction.StartSaveNPC();
        return "[이벤트 발생!] 경감님이 돌무더기에 갇혔습니다. 경감님을 구하러 가세요!";
    }
}