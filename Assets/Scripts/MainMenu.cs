using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer = null;
    [SerializeField] private AudioMixer _musicAudioMixer = null;

    public void ExitButton()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        GameMaster.Instance.Timer = true;
        SpawnManager.Instance.ResetWave();
        SceneManager.LoadScene("Excape");
    }

    public void StartTutorial()
    {
        Cursor.visible = false;
        SceneManager.LoadScene("Tutorial");
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetVolume(float volume)
    {
        if(_audioMixer)
            _audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeMusic(float volume)
    {
        if(_musicAudioMixer)
            _musicAudioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
}
