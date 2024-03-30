using UnityEngine;
using UnityEngine.UI;

public class ShowButtonOnCollision : MonoBehaviour
{
    [SerializeField] private bool checkHomes = true;
    [SerializeField] private bool checkColloc = false;
    [SerializeField] private bool checkInfect = false;
    public GameObject ButtonToShow;
    public GameObject CanvasToShow;

    private void Start()
    {
        ButtonToShow.GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            CanvasToShow.SetActive(true);
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!enabled) return;
        if ((checkHomes && collision.gameObject.CompareTag(StaticVars.TAG_HOLMES)) ||
            (checkColloc && collision.gameObject.CompareTag(StaticVars.TAG_COLLOC)) ||
            (checkInfect && collision.gameObject.CompareTag(StaticVars.TAG_INFECT))
            )
        {
            AudioManager.Instance.PlayEffect(EffectAudioType.ENTER);
            ButtonToShow.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        ButtonToShow.SetActive(false);
    }
}
