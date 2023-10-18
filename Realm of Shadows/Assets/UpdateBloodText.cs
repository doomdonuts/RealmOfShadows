using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateBloodText : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI textMesh;
    void Start()
    {

        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.gameState >= 3) textMesh.text = GameData.playerXP.ToString() + "/100";
    }
}
