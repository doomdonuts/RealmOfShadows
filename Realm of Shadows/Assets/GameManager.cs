using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public DungeonGenerator dungeonManager;
    public RoomProperties currentRoom;
    public int currentFloorNum;

    public GameObject duelObjects;
    public GameObject afterDuel;
    public GameObject winLoseButtons;
    public PlayerHealth player;

    public Button winButton;
    public Button endButton;
    public Button loseButton;
    public TextMeshProUGUI header;
    public TextMeshProUGUI specialOrEndText;
    public TextMeshProUGUI floorLabel;

    public GameObject specialDeck;
    public Button specialDeckButton;
    public TextMeshProUGUI specialDeckButtonText;

    public GameObject blood;

    public AudioSource audioSource;
    public AudioClip mainFloorMusic;
    public AudioClip shopMusic;
    public AudioClip sanctuaryMusic;
    public AudioClip minibossMusic;
    public AudioClip bossMusic;
    public AudioClip chestMusic;

    public static Dictionary<string, string> enemyWinText = new Dictionary<string, string>();
    public static Dictionary<string, string> enemyLossText = new Dictionary<string, string>();
    public static Dictionary<string, string> enemyDuelConditions = new Dictionary<string, string>();
    public static Dictionary<string, string> enemyHeaders = new Dictionary<string, string>();
    public static Dictionary<string, int> enemyMoneyRewards = new Dictionary<string, int>();
    public static Dictionary<string, int> enemyXPRewards = new Dictionary<string, int>();
    

    public static string[] floorOneBosses;
    public static string[] floorOneMiniBosses;
    public static string[] floorTwoBosses;
    public static string[] floorTwoMiniBosses;
    public static string[] floorThreeBosses;
    public static string[] floorThreeMiniBosses;
    public static string[] floorFourBosses;
    public static string[] floorFourMiniBosses;

    public static bool isPaused;
    public static bool setUpComplete;

    public float deathTime = 3.0f;
    public Light pointLight;
    
    void Start()
    {

        isPaused = false;
        if (!setUpComplete)
        {
            setUpBosses();
            updateEnemyText();
            setUpComplete = true;
        }

        if (GameData.gameState >= 3) specialDeck.SetActive(true);
        if (blood != null && GameData.gameState < 3) blood.SetActive(false);

        pointLight = player.GetComponentInChildren<Light>();
        audioSource = this.GetComponent<AudioSource>();
        floorLabel.text = "Floor " + currentFloorNum + "\nStart";
        GameSaveHandler.SaveGame();

    }
        
    private void Update()
    {
        if (player.currentHealth <= 0 && !duelObjects.activeSelf)
        {
            isPaused = true;
            pointLight.color = Color.red;
            floorLabel.text = "YOU DIED";
            deathTime -= Time.deltaTime;
            GameSaveHandler.SaveGame();

            if (deathTime <= 0.0f)
            {
                deathTime = 3.0f;
                SceneManager.LoadScene(0);
            }

        }
        
        
        
    }

    private void setUpBosses()
    {
        floorOneBosses = new string[] {"Gear", "Exodia2", "Pendulum"};
        floorOneMiniBosses = new string[] { "BattleCity", "Toon", "Exodia1" };
        floorTwoBosses = new string[] { "Kaiba", "Joey", "Fire" };
        floorTwoMiniBosses = new string[] { "Utopia", "Gold" };
        floorThreeBosses = new string[] { "Madolche", "CyberDragon", "Performapal" };
        floorThreeMiniBosses = new string[] { "DragonKnight", "Dice", "Monarch" };
        floorFourBosses = new string[] { "EgyptianGods" };
        floorFourMiniBosses = new string[] { "MadolcheMB", "DiceMB", "FireMB", "GoldMB" };
    }

    void updateEnemyText()
    {
        enemyHeaders.Add("Kuriboh", "Kuriboh - 20 cards, 2000 LP");
        enemyHeaders.Add("Watapon", "Watapon - 20 cards, 2000 LP");
        enemyHeaders.Add("ShineBall", "Shine Ball - 20 cards, 4000 LP");
        enemyHeaders.Add("Marshmallon", "Marshmallon - 20 cards, 6000 LP");
        enemyHeaders.Add("UFOTurtle", "UFO Turtle - 40 cards, 4000 LP");
        enemyHeaders.Add("BigEye", "Big Eye - 20 cards, 4000 LP");
        enemyHeaders.Add("RedEgg", "Agido - 40 cards, 6000 LP");
        enemyHeaders.Add("MokeyMokey", "Mokey Mokey - 40 cards, 8000 LP");

        enemyDuelConditions.Add("Kuriboh", "Once per duel, if your monster attacks, Kuriboh can discard a card from its hand to prevent battle damage from that battle.");
        enemyDuelConditions.Add("Watapon", "Once per duel, Watapon can special summon a card from its hand.");
        enemyDuelConditions.Add("ShineBall", "Absolutely normal duel for an absolutely normal ball. No special conditions.");
        enemyDuelConditions.Add("Marshmallon", "If you attack one of Marshmallon's face-down monsters, you take 1000 damage if the monster isn't destroyed by the end of your battle phase.");
        enemyDuelConditions.Add("UFOTurtle", "Once per duel, when a card controlled by UFO Turtle is destroyed by battle and sent to the graveyard, UFO Turtle can special summon a monster with 1500 ATK or less in attack position.");
        enemyDuelConditions.Add("Big Eye", "Once per duel, Big Eye can look at the top 5 cards of its deck and place them back on top of the deck in any order.");
        enemyDuelConditions.Add("RedEgg", "Once per duel, Agido can roll a 6-sided die and special summon a monster from the graveyard whose level matches the result (a roll of 6 works for any monster of level 6+).");
        enemyDuelConditions.Add("MokeyMokey", "All monsters on Mokey Mokey's field gain +100 attack for each monster on its side of the field.");
        // Bosses

        enemyDuelConditions.Add("Gear", "Both players can play all Magic cards as if they were hand traps.");
        enemyDuelConditions.Add("Exodia2", "All Dark monsters on the field gain +500 attack and defense.");
        enemyDuelConditions.Add("Pendulum", "Both players can place any monster in their hands into their pendulum zones. If the monster is not a pendulum monster, the pendulum scale values are set to the monster's level, rank, or link level.");
        enemyDuelConditions.Add("BattleCity", "Both players can play monster cards as though they were equip cards. Each monster on the field can only have one 'monster equip card' equipped at a time. The equipped monster gains the stats and effects of the 'monster equip card'.");
        enemyDuelConditions.Add("Toon", "All monsters do piercing damage.");
        enemyDuelConditions.Add("Exodia1", "Before the duel starts, each player can choose 1 card and place it on the top of their deck.");
        enemyDuelConditions.Add("Kaiba", "Normal monsters gain +500 attack and defense.");
        enemyDuelConditions.Add("Joey", "Effect monsters gain +500 attack and defense.");
        enemyDuelConditions.Add("Fire", "Whenever any player loses life points, they lose an additional 500 LP.");       
        enemyDuelConditions.Add("Utopia", "Any effect that increases a monster's attack increases it by 300 more than usual.");
        enemyDuelConditions.Add("Gold", "All monster cards with a rarity of 'Rare' or higher can be special summoned from your hand without any cost.");
        enemyDuelConditions.Add("Madolche", "Both players can choose to add monsters back to their deck rather than sending them to the graveyard.");
        enemyDuelConditions.Add("CyberDragon", "Player can use cards from the graveyard as materials to summon XYZ, Fusion, and Link monsters from their Extra Decks.");
        enemyDuelConditions.Add("Performapal", "Each player's monsters gain +500 attack during the player's own Battle Phase.");
        enemyDuelConditions.Add("DragonKnight", "Level 5 and 6 Dark type monsters can be played without tributing; Level 7+ Dark type monsters require only one tribute.");
        enemyDuelConditions.Add("Dice", "Attacking monsters must roll a die- if the roll is 1, the monster can't attack the rest of this turn. If the roll is 6, the monster can attack one more time this turn.");
        enemyDuelConditions.Add("Monarch", "Players can tribute magic and trap cards from the field in order to Tribute Summon monsters.");
        enemyDuelConditions.Add("EgyptianGods", "All monsters in the Graveyard become Gold Rare (not really) and their attribute becomes Divine.");

        enemyDuelConditions.Add("MadolcheMB", "Both players can choose to add monsters back to their deck rather than sending them to the graveyard.");
        enemyDuelConditions.Add("DiceMB", "Attacking monsters must roll a die- if the roll is 1, the monster can't attack the rest of this turn. If the roll is 6, the monster can attack one more time this turn.");
        enemyDuelConditions.Add("FireMB", "Whenever any player loses life points, they lose an additional 500 LP.");
        enemyDuelConditions.Add("GoldMB", "All monster cards with a rarity of 'Rare' or higher can be special summoned from your hand without any cost.");

        enemyHeaders.Add("Gear","BOSS: Red Gadget (Gear)");
        enemyHeaders.Add("Exodia2", "BOSS: The Legendary Exodia Incarnate (Exodia 2)");
        enemyHeaders.Add("Pendulum", "BOSS: Timegazer Magician (Pendulum)");
        enemyHeaders.Add("BattleCity", "MINIBOSS: Chimera the Flying Mystical Beast (Battle City)");
        enemyHeaders.Add("Toon", "MINIBOSS: Toon Mermaid (Toon)");
        enemyHeaders.Add("Exodia1", "MINIBOSS: Gaia the Dragon Champion (Exodia 1)");
        enemyHeaders.Add("Kaiba", "BOSS: Blue-Eyes Ultimate Dragon (Kaiba)");
        enemyHeaders.Add("Joey", "BOSS: Red-Eyes Flare Metal Dragon (Joey)");
        enemyHeaders.Add("Fire", "BOSS: Solar Flare Dragon (Fire)");
        enemyHeaders.Add("Utopia", "MINIBOSS: C39 Utopia Ray V (Utopia)");
        enemyHeaders.Add("Gold", "MINIBOSS: Black Luster Soldier- EotB (Gold)");
        enemyHeaders.Add("Madolche", "BOSS: Madolche Queen Tiaramisu (Madolche)");
        enemyHeaders.Add("CyberDragon", "BOSS: Cyber End Dragon (Cyber)");
        enemyHeaders.Add("Performapal", "BOSS: Starving Venom Fusion Dragon (Performapal)");
        enemyHeaders.Add("DragonKnight", "MINIBOSS: Dark Magician the Dragon Knight (Dragon Knight)");
        enemyHeaders.Add("Dice", "MINIBOSS: Dice Jar (Dice)");
        enemyHeaders.Add("Monarch", "MINIBOSS: Ehther the Heavenly Monarch (Monarch)");
        enemyHeaders.Add("EgyptianGods", "FINAL BOSS: The Legendary Egyptian Gods");

        enemyHeaders.Add("MadolcheMB", "MINIBOSS: Madolche Petingcessoeur (Madolche)");
        enemyHeaders.Add("DiceMB", "MINIBOSS: Dice Jar (Dice)");
        enemyHeaders.Add("GoldMB", "MINIBOSS: Black Luster Soldier- Eotb (Gold)");
        enemyHeaders.Add("FireMB", "MINIBOSS: Solar Flare Dragon (Fire)");

    }

}
