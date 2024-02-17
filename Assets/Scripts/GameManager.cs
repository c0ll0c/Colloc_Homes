// Scene 전환 등 게임 전반 관리하는 manager script
using Photon.Pun;
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
        gameState = GameState.PLAY;
        AudioManager.Instance.PlayEffect(EffectAudioType.INFECT);
    }

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
            ShowAndroidToastMessage("게임을 종료하려면 뒤로 가기를 한 번 더 눌러주세요");
            StartCoroutine(QuitTimer());
        }
    }

    IEnumerator QuitTimer()
    {
        yield return StaticFuncs.WaitForSeconds(3f);
        clickedBefore = false;
    }

    private void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
