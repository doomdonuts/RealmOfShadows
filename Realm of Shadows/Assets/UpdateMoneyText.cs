using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateMoneyText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    void Start()
    {
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = GameData.playerMoney.ToString();   
    }
}
