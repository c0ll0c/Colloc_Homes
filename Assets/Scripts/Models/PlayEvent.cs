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
        return "[�̺�Ʈ �߻�!] ��� ���̷����� ���ĵǾ����ϴ�. ���ѷ� �޽ļҿ��� ����� ��������!";
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
        return "[�̺�Ʈ �߻�!] £�� �Ȱ��� �þ߸� �����մϴ�. ġ�췯 �������?";
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
        return "[�̺�Ʈ �߻�!] �� ���� ���Ⱑ �����մϴ�. �����ҿ��� ���⸦ �����غ��ƿ�!";
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
        return "[�̺�Ʈ �߻�!] �氨���� �������⿡ �������ϴ�. �氨���� ���Ϸ� ������!";
    }
}