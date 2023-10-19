// Scene ��ȯ �� ���� ���� �����ϴ� manager script
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private GameState gameState = GameState.INTRO;
    public string PlayerName = "";
    private void Start()
    {
        if (PlayerPrefs.HasKey(StaticVars.PREFS_NICKNAE))
            PlayerName = PlayerPrefs.GetString(StaticVars.PREFS_NICKNAE);
    }

    public void ChangeScene (GameState _state)
    {
        if (gameState == _state) return;

        if (_state == GameState.INTRO) { NetworkManager.Instance.DisconnectAndDestroy(); }

        AudioManager.Instance.ChangeBGM(_state);

        gameState = _state;
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)gameState);
        while (!asyncLoad.isDone) { yield return null; }
    }

    // ������ ��������� ���� ���� ��ư�� ���� �� �Ҹ��� �Լ�
    public void EnterGame()
    {
        
    }
}
