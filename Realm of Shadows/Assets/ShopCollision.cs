using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCollision : MonoBehaviour
{
    private ShopManager shopManager;
    // Start is called before the first frame update
    void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        shopManager.showShop();
    }
}
