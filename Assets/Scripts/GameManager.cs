// Scene 전환 등 게임 전반 관리하는 manager script
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

    // 방장이 레디씬에서 게임 시작 버튼을 누를 때 불리는 함수
    public void EnterGame()
    {
        
    }
}
