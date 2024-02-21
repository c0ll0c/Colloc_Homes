using UnityEngine;

public class HandleNPC : MonoBehaviour
{
    public GameObject EndingButton;
    public Canvas EndingCanvas;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(StaticVars.TAG_HOLMES))
        {
            AudioManager.Instance.PlayEffect(EffectAudioType.ENTER);
            EndingButton.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        EndingButton.SetActive(false);
    }

    public void OnClickEndingButton()
    {
        EndingCanvas.gameObject.SetActive(true);
    }
}
