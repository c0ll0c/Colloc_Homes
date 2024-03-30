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

    // ������ ��������� ���� ���� ��ư�� ���� �� �Ҹ��� �Լ�
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
            // 3�ʸ��� ���ͳ� ���� Ȯ��
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
        // �ȵ���̵� back button
        // [TODO] iOS back button
        if (Input.GetKey(KeyCode.Escape))
        {
            if (clickedBefore)
            {
                Application.Quit();
                return;
            }
            clickedBefore = true;
            StaticFuncs.ShowAndroidToastMessage("������ �����Ϸ��� �ڷ� ���⸦ �� �� �� �����ּ���");
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
