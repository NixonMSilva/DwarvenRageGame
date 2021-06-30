using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;

    private bool isMenuActive = false;

    /*
     * 
    private void Start ()
    {
        InputHandler.instance.OnEscapePressed += OpenCloseMenu; 
    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnEscapePressed -= OpenCloseMenu;
    }

    private void OpenCloseMenu (object sender, EventArgs args)
    {
        if (isMenuActive)
        {
            menuPanel.SetActive(false);
            isMenuActive = false;
            InputHandler.instance.isOnMenu = false;
            //InputHandler.instance.SetCursorMode(CursorLockMode.Locked); 
        }
        else
        {
            menuPanel.SetActive(true);
            isMenuActive = true;
            InputHandler.instance.isOnMenu = true;
            //InputHandler.instance.SetCursorMode(CursorLockMode.None);
        }
    }
    */
}
