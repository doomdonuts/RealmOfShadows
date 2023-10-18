using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ShowGameStats : MonoBehaviour
{

    public Button returnButton;
    public TextMeshProUGUI deathNumber;
    public TextMeshProUGUI goldSpent;
    public TextMeshProUGUI ownSacrificed;
    // Start is called before the first frame update
    void Start()
    {
        deathNumber.text = "Number of Deaths: " + GameData.totalDeaths;
        goldSpent.text = "Money Spent: " + GameData.moneySpent;
        ownSacrificed.text = "Own monsters sacrificed: " + GameData.totalSacrificed;
        returnButton.onClick.AddListener(returnToMain);
    }

    private void returnToMain()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
