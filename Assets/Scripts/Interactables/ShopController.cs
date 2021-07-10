using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.ConfigureHelper;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    [SerializeField] private Animator sellerAnim;
    [SerializeField] private ShopType type = ShopType.normalShop;

    private bool boughtSomething = false;

    private int noBeersBought = 0;

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
        }
    }

    private void PlayRudeComment ()
    {   
        Debug.Log("Cheapskate");
    }

    private void PlayHello ()
    {
        switch (type)
        {
            case ShopType.postBoss1Shop:
                break;
            default:
                PlayDefaultHello();
                break;
        }
        manager.TimesShopped++;
    }

    private void PlayDefaultHello()
    {
        if (manager.TimesShopped == 0)
        {
            // Intro Hello
            Debug.Log("First hello");
        }
        else
        {
            // Random hello
            Debug.Log("Hello");
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
    }
}
