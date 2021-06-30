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
    private GameObject itemSlotParent;

    private List<GameObject> _weaponSlots;
    private List<GameObject> _itemSlots;

    private List<Image> _itemSprites;
    private List<TextMeshProUGUI> _itemStackTexts;

    private string[] slotKeyCode = { "[1]", "[2]", "[3]", "[4]", "[5]" };

    [SerializeField] private GameObject weaponSlotPrefab;
    [SerializeField] private GameObject itemSlotPrefab;

    [SerializeField] private Sprite defaultItemSprite;

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
        itemSlotParent = GameObject.Find("ItemSlots");

        _weaponSlots = new List<GameObject>();
        _itemSlots = new List<GameObject>();

        _itemSprites = new List<Image>();
        _itemStackTexts = new List<TextMeshProUGUI>();
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
        float startX = 250f, startY = 1000;

        for (int i = 0; i < count; ++i)
        {
            AddWeaponSlot(startX, startY);
            startX += 150f;
        }
    }

    public void CreateItemSlots (int count)
    {
        float startX = 250f, startY = 100;

        for (int i = 0; i < count; ++i)
        {
            AddItemSlot(startX, startY, i);
            startX += 150;
        }
    }

    public void AddWeaponSlot (float coorX, float coorY)
    {
        GameObject newSlot = Instantiate(weaponSlotPrefab, weaponSlotParent.transform);
        RectTransform slotPosition = newSlot.GetComponent<RectTransform>();
        slotPosition.position = new Vector3(coorX, coorY, 0f);
        _weaponSlots.Add(newSlot);
    }

    public void AddItemSlot (float coorX, float coorY, int count)
    {
        GameObject newSlot = Instantiate(itemSlotPrefab, itemSlotParent.transform);
        RectTransform slotPosition = newSlot.GetComponent<RectTransform>();

        TextMeshProUGUI keycodeText = newSlot.transform.Find("Keycode").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI stackText = newSlot.transform.Find("Stack").GetComponent<TextMeshProUGUI>();
        Image sprite = newSlot.transform.Find("Image").GetComponent<Image>();

        slotPosition.position = new Vector3(coorX, coorY, 0f);
        keycodeText.text = slotKeyCode[count];
        stackText.text = "";
        _itemSlots.Add(newSlot);
        _itemStackTexts.Add(stackText);
        _itemSprites.Add(sprite);
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

    public void UpdateItemSlot (List<Sprite> _sprites, List<int> _itemStacks)
    {
        int i;

        for (i = 0; i < _sprites.Count && i < _itemStacks.Count; ++i)
        {
            if (_sprites != null)
            {

                _itemSprites[i].sprite = _sprites[i];
                _itemSprites[i].color = Color.white;
                
                if (_itemStacks[i] > 0)
                {
                    // If the stack has more than 1 item, draw the text
                    _itemStackTexts[i].text = "x" + _itemStacks[i].ToString();
                }
                else
                {
                    // If it doesn't, the text should be empty
                    _itemStackTexts[i].text = "";
                }
                
            }
            else
            {
                _itemStackTexts[i].text = "";
            }
        }

        // Reset the icons of the slots on the right
        ResetInventoryIcons(i);

        /*
        // If there's nothing in these lists, reset the icons
        if (_sprites.Count == 0 && _itemStacks.Count == 0)
        {
            for (int i = 0; i < _itemSlots.Count; ++i)
            {
                ResetInventoryIcons();
            }
        } */
    }

    public void ResetInventoryIcons (int index)
    {
        for (int i = index; i < _itemSlots.Count; ++i)
        {
            _itemSprites[i].sprite = defaultItemSprite;
            _itemStackTexts[i].text = "";
        }
    }

    public void ShowItemSlots ()
    {
        itemSlotParent.SetActive(true);
    }

    public void HideItemSlots ()
    {
        itemSlotParent.SetActive(false);
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
