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
        return "[�̺�Ʈ �߻�!] ��� ���̷����� ���ĵǾ����ϴ�. ���ѷ� �޽ļҿ��� ����� ��������!";
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
        return "[�̺�Ʈ �߻�!] £�� �Ȱ��� �þ߸� �����մϴ�. ġ�췯 �������?";
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
    public string SceneName { get; set; }

    public string Play()
    {
        SceneName = "MiniGameSaveNPCScene";
        Distraction.StartSaveNPC();
        return "[�̺�Ʈ �߻�!] �氨���� �������⿡ �������ϴ�. �氨���� ���Ϸ� ������!";
    }
    public void MiniGame()
    {
        Distraction.gameObject.SetActive(false);
        Solution.SolutionSaveNPC();
    }

}