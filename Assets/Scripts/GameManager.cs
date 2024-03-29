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

        StartCoroutine(CheckInternetConnection());
    }

    public void ChangeScene (GameState _state)
    {
        if (gameState == _state) return;

        if (_state == GameState.INTRO) { NetworkManager.Instance.DisconnectAndDestroy(); }

        AudioManager.Instance.ChangeBGM(_state);

        gameState = _state;
        StartCoroutine(LoadAsyncScene());
    }

    public void ChangeGameState(GameState _state)
    {
        if (gameState == _state) return;

        gameState = _state;
        AudioManager.Instance.ChangeBGM(_state);
    }

    private IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int)gameState);
        while (!asyncLoad.isDone) { yield return null; }
    }

    // 방장이 레디씬에서 게임 시작 버튼을 누를 때 불리는 함수
    public void EnterGame()
    {
        gameState = GameState.PLAY;
        AudioManager.Instance.PlayEffect(EffectAudioType.INFECT);
    }

    #region Check Internet Connection
    IEnumerator CheckInternetConnection()
    {
        while (true)
        {
            // 3초마다 인터넷 연결 확인
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                switch (gameState)
                {
                    case GameState.INTRO:
                        AlertManager.Instance.NoNetworkAlert();
                        break;
                    case GameState.LOBBY:
                    case GameState.READY:
                    case GameState.PLAY:
                    case GameState.WIN:
                    case GameState.LOSE:
                        ChangeScene(GameState.INTRO);
                        AlertManager.Instance.NoNetworkAlert();
                        break;
                }
            }
            yield return StaticFuncs.WaitForSeconds(3f);
        }
    }
    #endregion

    #region Android Backbutton Control
    // BackButton Disable
    private bool clickedBefore = false;
    private void Update()
    {
        // 안드로이드 back button
        // [TODO] iOS back button
        if (Input.GetKey(KeyCode.Escape))
        {
            if (clickedBefore)
            {
                Application.Quit();
                return;
            }
            clickedBefore = true;
            StaticFuncs.ShowAndroidToastMessage("게임을 종료하려면 뒤로 가기를 한 번 더 눌러주세요");
            StartCoroutine(QuitTimer());
        }
    }

    IEnumerator QuitTimer()
    {
        yield return StaticFuncs.WaitForSeconds(3f);
        clickedBefore = false;
    }
    #endregion
}
