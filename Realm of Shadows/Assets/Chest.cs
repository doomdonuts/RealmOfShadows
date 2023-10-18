using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;
    public AudioClip chestSound;
    public bool isOpen = false;
    private float notifTime = 1.0f;
    private CoinNotifMessage notif;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        notif = this.GetComponentInChildren<CoinNotifMessage>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen && notifTime >= 0.0f)
        {
            notifTime -= Time.deltaTime;
            notif.transform.position = new Vector2(notif.transform.position.x, notif.transform.position.y + (0.3f * Time.deltaTime));
        } else if (isOpen && notifTime < 0.0f)
        {
            notif.gameObject.SetActive(false);
        }
        
    }

    private void OnCollisionEnter2D (Collision2D other)
    {


        if (other.gameObject.tag == "Player" && !isOpen)
        {
            audioSource.PlayOneShot(chestSound);
            anim.SetBool("CollisionAction", true);
            isOpen = true;
            GameData.playerMoney += Random.Range(100, 200);
            notif.gameObject.SetActive(true);
            GameSaveHandler.SaveGame();
        }
        
    }

}
