using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private TMP_Dropdown graphicsDropdown;

    [Header("Sensitivty")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    [Header("Audio")]
    [SerializeField] private Slider audioSlider;
    [SerializeField] private TextMeshProUGUI audioText;

    private void Awake()
    {
        graphicsDropdown.value = PlayerPrefsWriter.GetGraphics();

        sensitivitySlider.value = PlayerPrefsWriter.GetSensitivity();
        sensitivityText.SetText("Sensitivity: " + PlayerPrefsWriter.GetSensitivity());

        audioSlider.value = PlayerPrefsWriter.GetMasterVolume();
        audioText.SetText("Volume: " + PlayerPrefsWriter.GetMasterVolume());
    }

    public void SetGraphics(int index)
    {
        PlayerPrefsWriter.SetGraphics(index);
    }

    public void SetSensitivity(float sensitivity)
    {
        sensitivityText.SetText("Sensitivity: " + sensitivity);
        PlayerPrefsWriter.SetSensitivity(sensitivity);
    }

    public void SetMasterAudio(float volume)
    {
        audioText.SetText("Volume: " + volume.ToString());
        PlayerPrefsWriter.SetMasterVolume(volume);
    }
}