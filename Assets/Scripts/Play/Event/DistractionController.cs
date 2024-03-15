using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DistractionController : MonoBehaviour
{
    // Distractions
    public GameObject HungryDistraction;
    public GameObject FogDistraction;
    public GameObject ElecDistraction;
    public GameObject SaveNPCDistraction;
    private Coroutine distractionLoop = null;

    // Pointer
    private PointerController pointerController;
    public Transform EventSolutions;

    private void Start()
    {
        pointerController = transform.GetChild(0).GetChild(4).GetComponent<PointerController>();
    }

    #region Distractions
    public void StartHungry()
    {
        distractionLoop = StartCoroutine(HungryDistractionLoop());
        NetworkManager.Instance.EventToPlay.SetSolutionController(EventSolutions.GetChild(0).GetComponent<EventSolutionHandler>());

        pointerController.Show(EventSolutions.GetChild(0).position);
    }
    IEnumerator HungryDistractionLoop()
    {
        HungryDistraction.SetActive(true);

        Image hungrySprite = HungryDistraction.GetComponent<Image>();
        float alpha = 0f;
        float fadeSpeed = 0.5f;
        while (true)
        {
            alpha += fadeSpeed * 0.1f;
            hungrySprite.color = new Color(1f, 1f, 1f, alpha);

            if (alpha >= 1f || alpha <= 0f)
            {
                fadeSpeed = -fadeSpeed;
            }
            yield return StaticFuncs.WaitForSeconds(0.1f);
        }
    }

    public void StartFog()
    {
        FogDistraction.SetActive(true);
        NetworkManager.Instance.EventToPlay.SetSolutionController(EventSolutions.GetChild(1).GetComponent<EventSolutionHandler>());

        pointerController.Show(EventSolutions.GetChild(1).position);
    }
    
    public void StartElec()
    {
        ElecDistraction.SetActive(true);
        NetworkManager.Instance.EventToPlay.SetSolutionController(EventSolutions.GetChild(2).GetComponent<EventSolutionHandler>());

        pointerController.Show(EventSolutions.GetChild(2).position);
    }

    public void StartSaveNPC()
    {
        distractionLoop = StartCoroutine(SaveNPCDistractionLoop());
        NetworkManager.Instance.EventToPlay.SetSolutionController(EventSolutions.GetChild(3).GetComponent<EventSolutionHandler>());
        
        pointerController.Show(EventSolutions.GetChild(3).position);
    }
    IEnumerator SaveNPCDistractionLoop()
    {
        while (true)
        {
            SaveNPCDistraction.SetActive(true);
            while (SaveNPCDistraction.activeSelf)
            {
                yield return StaticFuncs.WaitForSeconds(1f);
            }
            yield return StaticFuncs.WaitForSeconds(3f);
        }
    }
    #endregion

    public void StopEvent()
    {
        // Distraction Off
        if (distractionLoop != null)
        {
            StopCoroutine(distractionLoop);
        }
        HungryDistraction.SetActive(false);
        FogDistraction.SetActive(false);
        ElecDistraction.SetActive(false);
        SaveNPCDistraction.SetActive(false);

        // Arrow Off
        pointerController.Hide();
    }
}
