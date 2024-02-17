using UnityEngine;
using UnityEngine.UI;

public class StartBtnOnClick : MonoBehaviour
{
    // Intro Scene���� ���� ��ư ���� �� Loading Text Ȱ��ȭ ��Ű��
    // ��ư�� ��� Ŭ���� ���ϵ��� ���´�.
    public void StartGame()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            AlertManager.Instance.NoNetworkAlert();
            return;
        }
        NetworkManager.Instance.Connect();
        transform.GetComponent<Button>().interactable = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        if (string.IsNullOrEmpty(GameManager.Instance.PlayerName)){
            string playerName = "P#" + Random.Range(10000, 100000).ToString();
            PlayerPrefs.SetString(StaticVars.PREFS_NICKNAE, playerName);
            GameManager.Instance.PlayerName = playerName;
        }
    }
}