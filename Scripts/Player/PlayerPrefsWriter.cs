using UnityEngine;

public static class PlayerPrefsWriter
{
    private static string sensitivity = "Sensitivity";
    private static string graphicsSetting = "Graphics Settings";
    private static string masterSoundVolume = "Master Sound Volume";

    public static void LoadPlayerPrefs()
    {
        // Default values
        if (!PlayerPrefs.HasKey(sensitivity))
        {
            SetSensitivity(2);
        }

        if (!PlayerPrefs.HasKey(graphicsSetting))
        {
            SetGraphics(5);
        }
        else
        {
            SetGraphics(GetGraphics());
        }

        if (!PlayerPrefs.HasKey(masterSoundVolume))
        {
            SetMasterVolume(50);
        }
        else
        {
            SetMasterVolume(GetMasterVolume());
        }


        PlayerPrefs.Save();
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(masterSoundVolume);
    }

    public static void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume / 100;
        PlayerPrefs.SetFloat(masterSoundVolume, volume);
    }

    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat(sensitivity);
    }

    public static void SetSensitivity(float sens)
    {
        PlayerPrefs.SetFloat(sensitivity, sens);
    }

    public static void SetGraphics(int index)
    {
        PlayerPrefs.SetInt(graphicsSetting, index);
        QualitySettings.SetQualityLevel(GetGraphics(), true);
    }

    public static int GetGraphics()
    {
        return PlayerPrefs.GetInt(graphicsSetting);
    }
}