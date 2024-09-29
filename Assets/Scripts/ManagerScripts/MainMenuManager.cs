using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    private void Start() {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    
    public void StartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    
    public void ShowOptions() {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    public void HideOptions() {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    
    public void QuitApplication() {
        Application.Quit();
    }
    
    public void AdjustMusic(float volume) {
        // adjust player prefs
        PlayerPrefs.SetFloat("MusicVolume", volume);
        // TODO: adjust music volume in audio source.
    }

    public void AdjustSound(float volume) {
        // adjust player prefs
        PlayerPrefs.SetFloat("SoundVolume", volume);
        // TODO: adjust sound volume in audio source.
    }
}
