using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContinue : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource audioSource;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateGameState(int updateGameStateTo)
    {
        GameData.gameState = updateGameStateTo;
    }

    public void continueScene(int sceneToGoTo)
    {
        StartCoroutine(FadeAudioSource.StartFade(audioSource, 1.0f, 0.0f));
        SceneManager.LoadScene(sceneToGoTo);
    }

}
