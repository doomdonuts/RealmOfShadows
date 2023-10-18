using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateLPText : MonoBehaviour
{
    public PlayerHealth player;
    private TextMeshProUGUI textMesh;
    public TMP_InputField inputField;
    public GameObject duelObjects;
    private GameObject LPEditor;
   
    // Start is called before the first frame update
    void Start()
    {
        LPEditor = inputField.gameObject;
        textMesh = this.GetComponent<TextMeshProUGUI>();
        inputField.onSubmit.AddListener(textChanged);
    }
 
void textChanged(string msg)
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { return; }
        int newLP;      

        if (int.TryParse(msg, out newLP) && newLP >= 0) player.currentHealth = newLP;
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = player.currentHealth.ToString();
        if (player.currentHealth <= 2000) textMesh.color = Color.red;
        else textMesh.color = Color.white;
    }

    private void OnMouseOver()
    {
        if (!LPEditor.activeSelf)
        {
            LPEditor.SetActive(true);
            GameManager.isPaused = true;
        }
    }

    private void OnMouseExit()
    {
        if (LPEditor.activeSelf) {
            LPEditor.SetActive(false);
            if (!duelObjects.activeSelf) GameManager.isPaused = false;
        }
    }
}
