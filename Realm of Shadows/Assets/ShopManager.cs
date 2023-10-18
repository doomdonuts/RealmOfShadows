using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Dictionary<string, string> itemDescription;
    public Dictionary<string, int> itemPrice;
    public PlayerHealth player;
    public AudioClip buySound;
    public AudioClip errorSound;
    private AudioSource audioSource;

    public Button monsterButton;
    public Button spellButton;
    public Button trapButton;
    public Button extraButton;
    public Button choiceButton;
    public Button speedButton;
    public Button restoreButton;
    public Button removeButton;
    public Button leaveShopButton;
    public Button buyButton;

    public TextMeshProUGUI blurbText;

    public GameObject shopUI;

    private string lastClicked;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        itemDescription = new Dictionary<string, string>();
        itemPrice = new Dictionary<string, int>();

        itemDescription.Add("monsterPack", "$700: A pack of 7 random monster cards, one of each from level 1 to 7. You can add any amount to your deck.");
        itemDescription.Add("spellPack", "$500: A pack of 5 random spell cards (may include 1 field card if you wish). You can add any amount to your deck.");
        itemDescription.Add("trapPack", "$500: A pack of 5 random trap cards. You can add any amount to your deck.");
        itemDescription.Add("extraPack", "$3000: You can choose any one Extra Deck monster to add to your deck. Also add up to 5 cards that are needed for summoning or supporting this monster.");
        itemDescription.Add("choicePack", "$2000: A pack of 3 cards chosen entirely by you!");
        itemDescription.Add("speedPack", "$1000: A limited edition pack that's actually a pack in real life. You can add any amount to your deck.");
        itemDescription.Add("restoreLP", "$100: The new pizza crepe taco pancake chili bag, only at Taco Town... and also this store. Restores 1000 LP instantly");
        itemDescription.Add("removeCard", "$100: Get rid of that shitty card in your deck. You must have at least 40 cards in your main deck to use this.");

        itemPrice.Add("monsterPack", 700);
        itemPrice.Add("spellPack", 500);
        itemPrice.Add("trapPack", 500);
        itemPrice.Add("extraPack", 3000);
        itemPrice.Add("choicePack", 2000);
        itemPrice.Add("speedPack", 1000);
        itemPrice.Add("restoreLP", 100);
        itemPrice.Add("removeCard", 100);

        monsterButton.onClick.AddListener(delegate { updateBlurbText("monsterPack"); });
        spellButton.onClick.AddListener(delegate { updateBlurbText("spellPack"); });
        trapButton.onClick.AddListener(delegate { updateBlurbText("trapPack"); });
        extraButton.onClick.AddListener(delegate { updateBlurbText("extraPack"); });
        choiceButton.onClick.AddListener(delegate { updateBlurbText("choicePack"); });
        speedButton.onClick.AddListener(delegate { updateBlurbText("speedPack"); });
        restoreButton.onClick.AddListener(delegate { updateBlurbText("restoreLP"); });
        removeButton.onClick.AddListener(delegate { updateBlurbText("removeCard"); });
        leaveShopButton.onClick.AddListener(closeShopMenu);
        buyButton.onClick.AddListener(buyItem);

    }

    private void buyItem()
    {
        int itemCost = 10000;
        itemPrice.TryGetValue(lastClicked, out itemCost);
        if (GameData.playerMoney >= itemCost)
        {
            audioSource.PlayOneShot(buySound);
            GameData.playerMoney = GameData.playerMoney- itemCost;

            if (lastClicked == "restoreLP")
            {
                if ((player.currentHealth + 1000 >= 8000) && (player.allowOverheal == false)) player.currentHealth = 8000;
                else player.currentHealth += 1000;
            }

            GameData.moneySpent += itemCost;

        } else
        {
            buyButton.gameObject.SetActive(false);
            audioSource.PlayOneShot(errorSound);
            blurbText.text = "You don't have enough money!";
        }
    }

    private void closeShopMenu()
    {
        buyButton.gameObject.SetActive(false);
        shopUI.SetActive(false);
        GameManager.isPaused = false;
        GameSaveHandler.SaveGame();
    }

    public void updateBlurbText(string itemName)
    {
        string itemText = "";
        itemDescription.TryGetValue(itemName, out itemText);
        blurbText.text = itemText;
        lastClicked = itemName;
        buyButton.gameObject.SetActive(true);
    }

    public void showShop()
    {
        blurbText.text = "Wanna buy something?";
        shopUI.SetActive(true);
        GameManager.isPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
