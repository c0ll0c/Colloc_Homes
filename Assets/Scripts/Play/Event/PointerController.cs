using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    private Camera UICamera;

    private Vector3 targetPosition;
    private RectTransform pointerTransform;
    private float borderSize;

    public Sprite ArrowSprite;
    public Sprite HereSprite;
    private Image pointerImg;

    private void Awake()
    {
        pointerTransform = GetComponent<RectTransform>();
        pointerImg = GetComponent<Image>();
        borderSize = Screen.height * 0.1f;
    }

    private void Update()
    {
        if (targetPosition.x <= borderSize ||
            targetPosition.x >= Screen.width - borderSize ||
            targetPosition.y <= borderSize ||
            targetPosition.y >= Screen.height - borderSize
            )
        {
            pointerImg.sprite = ArrowSprite;

            // rotate
            RotatePointer();

            // set position
            Vector3 pointerPosition = targetPosition;
            if (pointerPosition.x <= borderSize) pointerPosition.x = borderSize;
            if (pointerPosition.x >= Screen.width - borderSize) pointerPosition.x = Screen.width - borderSize;
            if (pointerPosition.y <= borderSize) pointerPosition.y = borderSize;
            if (pointerPosition.y >= Screen.height - borderSize) pointerPosition.y = Screen.height - borderSize;

            pointerTransform.position = UICamera.ScreenToWorldPoint(pointerPosition);
            pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0f);
        }
        else
        {
            pointerImg.sprite = HereSprite;
            
            // rotate
            pointerTransform.localEulerAngles = Vector3.zero;

            // set position
            pointerTransform.position = UICamera.ScreenToWorldPoint(targetPosition);
            pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0f);
        }
    }

    private void RotatePointer()
    {
        Vector3 dir = targetPosition - Camera.main.transform.position;
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        pointerTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Vector3 _target)
    {
        targetPosition = Camera.main.WorldToScreenPoint(_target);
        gameObject.SetActive(true);
    }
}
