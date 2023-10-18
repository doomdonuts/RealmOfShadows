using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button newGameButton;
    public Button floor1Button;
    public Button floor2Button;
    public Button floor3Button;
    public Button floor4Button;

    public TextMeshProUGUI newGameButtonText;
    private AudioSource audioSource;
    public AudioClip startSound;
    public AudioClip errorSound;

    public GameObject floorStartButtons;
    public TextMeshProUGUI floor2Text;
    public TextMeshProUGUI floor3Text;
    public TextMeshProUGUI floor4Text;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        GameSaveHandler.LoadGame();
    }

    void Start()
    {
        newGameButton.onClick.AddListener(delegate { startNewGame(GameData.unlockedFloors); });
        floor1Button.onClick.AddListener(delegate { startNewGame(1); });
        floor2Button.onClick.AddListener(delegate { startNewGame(2); });
        floor3Button.onClick.AddListener(delegate { startNewGame(3); });
        floor4Button.onClick.AddListener(delegate { startNewGame(4); });

        audioSource = GetComponent<AudioSource>();


    }

    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((scene.name == "MainMenu"))
        {
            if (GameData.gameState >= 1)
            {
                newGameButtonText.text = "New Run";
            }
  

            if (GameData.unlockedFloors >= 2 && (floorStartButtons != null)) floorStartButtons.SetActive(true);

            if (GameData.unlockedFloors >= 2 && (floor2Button != null)) floor2Text.text = "2";
            if (GameData.unlockedFloors >= 3 && (floor3Button != null)) floor3Text.text = "3";
            if (GameData.unlockedFloors >= 4 && (floor4Button != null)) floor4Text.text = "4";
        }
    }


    private void startNewGame(int areaNumber)
    {
        if (GameData.gameState < 1)
        {
            audioSource.PlayOneShot(startSound);
            StartCoroutine(FadeAudioSource.StartFade(audioSource, 1.0f, 0.0f));
            SceneManager.LoadScene("IntroScene");
        } else if (GameData.unlockedFloors >= areaNumber)
        {         
            audioSource.PlayOneShot(startSound);
            StartCoroutine(FadeAudioSource.StartFade(audioSource, 1.0f, 0.0f));
            SceneManager.LoadScene(areaNumber);
        }
        else
        {
            audioSource.PlayOneShot(errorSound);
            Debug.Log("Area not unlocked!");
        }
    }

}
