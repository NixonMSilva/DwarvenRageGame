using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private Animator sellerAnim;
    [SerializeField] private ShopType type = ShopType.normalShop;

    [SerializeField] private AudioSource soundSource;
    
    private bool boughtSomething = false;

    private int noBeersBought = 0;

    private List<string> _soundsPlayed = new List<string>();

    private int timesBoughtHere = 0;

    public bool BoughtSomething
    {
        get => boughtSomething;
        set => boughtSomething = value;
    }

    public int BeersBought
    {
        get => noBeersBought;
        set => noBeersBought = value;
    }

    public void PlaySellerAnimation ()
    {
        sellerAnim.Play("Thank_you");
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayHello();
        }
    }

    public void OnTriggerExit (Collider other)
    { 
        if (other.gameObject.CompareTag("Player"))
        {
            if (!boughtSomething)
            {
                PlayRudeComment();
            }

            timesBoughtHere++;
        }
    }

    private void PlayRudeComment ()
    {   
        //Debug.Log("Cheapskate");
    }

    private void PlayHello ()
    {
        if (timesBoughtHere == 0)
        {
            switch (type)
            {
                case ShopType.postBoss1Shop:
                    AudioManager.instance.PlaySoundInVolume(soundSource, "belgren_e_hanok_dps_troll");
                    break;
                default:
                    PlayDefaultHello();
                    break;
            }
        }
        else
        {
            PlayDefaultHello();
        }
        
        GameManager.instance.timesShopped++;
    }

    private void PlayDefaultHello ()
    {
        if (GameManager.instance.timesShopped == 0)
        {
            // Intro Hello
            AudioManager.instance.PlaySoundInVolume(soundSource, "hello_first");
            
        }
        else
        {
            // Random hello
            AudioManager.instance.PlaySoundInVolumeRandom(soundSource, "remark");
        }
    }

    public void PlayPotionSell ()
    {
        Debug.Log("Potion sold!");
        if (noBeersBought == 0)
        {
            
        }

        noBeersBought++;
    }

    public void NewPurchase ()
    {
        noBeersBought = 0;
        boughtSomething = false;
        ClearPurchasedSounds();
    }

    public void PlayPurchaseSound (string purchasedItemAudioPurchaseName)
    {
        if (!_soundsPlayed.Contains(purchasedItemAudioPurchaseName))
        {
            AudioManager.instance.PlaySoundInVolume(soundSource, purchasedItemAudioPurchaseName);
            _soundsPlayed.Add(purchasedItemAudioPurchaseName);
        }
    }

    public void ClearPurchasedSounds ()
    {
        _soundsPlayed.Clear();
    }
}
