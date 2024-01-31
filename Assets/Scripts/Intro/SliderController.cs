using UnityEngine;
using UnityEngine.UI;

public enum SettingSliderType
{
    BGM,
    EFT,
}

public class SliderController : MonoBehaviour
{
    public SettingSliderType Type;
    private string sliderType;
    private string sliderType_mute;

    private float volume;

    private Button muteBtn;
    private GameObject muteObj;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        muteBtn = transform.GetChild(1).GetComponent<Button>();
        muteObj = transform.GetChild(1).GetChild(0).gameObject;
        slider = transform.GetChild(2).GetComponent<Slider>();

        sliderType = Type.ToString();
        sliderType_mute = sliderType + "_m";

        if (PlayerPrefs.HasKey(sliderType))
            volume = PlayerPrefs.GetFloat(sliderType);
        else
        {
            volume = slider.maxValue;
        }
        slider.value = volume;

        if (PlayerPrefs.HasKey(sliderType_mute) && PlayerPrefs.GetFloat(sliderType_mute) == 1)
        {
            muteObj.SetActive(true);
            slider.value = 0;
        }
        else
        {
            muteObj.SetActive(false);
        }

        slider.onValueChanged.AddListener(delegate
        {
            OnSliderMove(slider.value);
        });
        muteBtn.onClick.AddListener(delegate
        {
            MuteBtnOnClick();
        });
    }

    private void OnDisable()
    {
        if (muteObj.activeSelf)
            PlayerPrefs.SetFloat(sliderType_mute, 1);
        else
            PlayerPrefs.SetFloat(sliderType_mute, 0);

        PlayerPrefs.SetFloat(sliderType, volume);
    }

    private void OnSliderMove(float _sliderValue)
    {
        AudioManager.Instance.SetVolume(Type, _sliderValue);

        if (_sliderValue == 0)
        {
            muteObj.SetActive(true);
        }
        else
        {
            volume = _sliderValue;
            muteObj.SetActive(false);
        }
    }

    private void MuteBtnOnClick()
    {
        // muted -> sound
        if (muteObj.activeSelf)
        {
            muteObj.SetActive(false);

            if (volume == 0) volume = 0.5f;
            slider.value = volume;
            AudioManager.Instance.SetVolume(Type, volume);
        }
        else
        {
            muteObj.SetActive(true);
            slider.value = 0;
            AudioManager.Instance.SetVolume(Type, 0);
        }
    }
}
