using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterfaceController : MonoBehaviour
{
    public static UserInterfaceController instance;

    private TextMeshProUGUI tooltipText;

    private GameObject pauseMenu;
    private bool isPauseMenuOn;

    private GameObject inventory;
    private GameObject weaponSlotParent;
    private List<GameObject> _weaponSlots;

    [SerializeField] private GameObject weaponSlotPrefab;

    private void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        tooltipText = GameObject.Find("TooltipText").GetComponent<TextMeshProUGUI>();

        pauseMenu = GameObject.Find("PauseMenu");

        inventory = GameObject.Find("Inventory");

        weaponSlotParent = GameObject.Find("WeaponSlots");

        _weaponSlots = new List<GameObject>();
    }

    private void Start ()
    {
        HideTooltip();
        HidePauseMenu();

        InputHandler.instance.OnEscapePressed += PauseMenu;
    }

    private void OnDestroy ()
    {

    }

    public void DrawTooltip (string text)
    {
        tooltipText.text = text;
        tooltipText.gameObject.SetActive(true);
    }

    public void HideTooltip ()
    {
        tooltipText.gameObject.SetActive(false);
    
    }

    public void PauseMenu (object sender, EventArgs args)
    {
        if (isPauseMenuOn)
        {
            HidePauseMenu();
            InputHandler.instance.LockCursor(true);
        }
        else
        {
            DrawPauseMenu();
            InputHandler.instance.LockCursor(false);
        }
    }

    public void DrawPauseMenu ()
    {
        pauseMenu.SetActive(true);
        HidePlayerInterface();
        isPauseMenuOn = true;
        Time.timeScale = 0f;
    }

    public void HidePauseMenu ()
    {
        pauseMenu.SetActive(false);
        ShowPlayerInterface();
        isPauseMenuOn = false;
        Time.timeScale = 1f;
    }

    public void HidePlayerInterface ()
    {
        HideWeaponSlots();
    }

    public void ShowPlayerInterface ()
    {
        ShowWeaponSlots();
    }

    public void CreateWeaponSlots (int count)
    {
        float startX = 250f, startY = 100;

        for (int i = 0; i < count; ++i)
        {
            AddWeaponSlot(startX, startY);
            startX += 150f;
        }
    }

    public void AddWeaponSlot (float coorX, float coorY)
    {
        GameObject newSlot = Instantiate(weaponSlotPrefab, weaponSlotParent.transform);
        RectTransform slotPosition = newSlot.GetComponent<RectTransform>();
        slotPosition.position = new Vector3(coorX, coorY, 0f);
        _weaponSlots.Add(newSlot);
    }

    public void UpdateWeaponSlot (List<Sprite> _sprites)
    {
        for (int i = 0; i < _sprites.Count; ++i)
        {
            if (_sprites != null)
            {
                Image[] sprites = _weaponSlots[i].GetComponentsInChildren<Image>();
                for (int j = 0; j < sprites.Length; ++j)
                {
                    if (!sprites[j].gameObject.Equals(_weaponSlots[i]))
                    {
                        sprites[j].sprite = _sprites[i];
                        // For some reason the sprites might start with a wrong color?
                        sprites[j].color = Color.white;
                    }
                }
                //_weaponSlots[i].GetComponentInChildren<Image>().sprite = _weapons[i].icon;
            }
        }
    }

    public void ShowWeaponSlots ()
    {
        weaponSlotParent.SetActive(true);
    }

    public void HideWeaponSlots ()
    {
        weaponSlotParent.SetActive(false);
    }

    public void ShowBerserkIcon ()
    {

    }

    public void HideBerserkIcon ()
    {

    }

    public void ShowFortuneIcon()
    {

    }

    public void HideFortuneIcon()
    {

    }

    public void ShowFireResistanceIcon ()
    {

    }

    public void HideFireResistanceIcon ()
    {

    }

    public void ShowPoisonResistanceIcon ()
    {

    }

    public void HidePoisonResistanceIcon ()
    {

    }
}
