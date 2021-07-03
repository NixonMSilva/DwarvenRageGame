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

    private GameObject healthBarFrame;
    public Slider healthBar;

    private GameObject characterFrame;
    private Image characterImage;

    private GameObject armorFrame;
    private GameObject armorIndicator;
    private TextMeshProUGUI armorValue;

    private Image damageFrame;

    private GameObject weaponSlot;
    private Image weaponSlotIcon;

    private GameObject deathScreen;
    //private CanvasGroup deathScreen;

    private List<GameObject> _itemSlots;

    private List<Image> _itemSprites;
    private List<TextMeshProUGUI> _itemStackTexts;

    private GameObject goldFrame;
    private TextMeshProUGUI goldCount;

    private string[] slotKeyCode = { "[1]", "[2]", "[3]", "[4]", "[5]" };

    private float[] inventoryXCoor = { 768f, 896f, 1024f, 1152f };

    [SerializeField] private GameObject weaponSlotPrefab;
    [SerializeField] private GameObject itemSlotPrefab;

    [SerializeField] private Sprite defaultItemSprite;

    [SerializeField] private Sprite playerNormal;
    [SerializeField] private Sprite playerHurt;
    [SerializeField] private Sprite playerVeryHurt;
    [SerializeField] private Sprite playerVeryDrunk;

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

        healthBarFrame = GameObject.Find("HealthBar");
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        characterFrame = GameObject.Find("PlayerPhotoSquare");
        characterImage = GameObject.Find("PlayerPhoto").GetComponent<Image>();

        armorFrame = GameObject.Find("PlayerArmorSquare");
        //armorIndicator = GameObject.Find("PlayerArmorIcon");
        armorValue = armorFrame.GetComponentInChildren<TextMeshProUGUI>();

        damageFrame = GameObject.Find("BloodPanel").GetComponent<Image>();

        weaponSlotParent = GameObject.Find("WeaponSlots");
        itemSlotParent = GameObject.Find("ItemSlots");

        weaponSlot = GameObject.Find("EquippedWeaponIcon");
        weaponSlotIcon = GameObject.Find("WeaponIcon").GetComponent<Image>();

        goldFrame = GameObject.Find("GoldPanel");
        goldCount = GameObject.Find("GoldCount").GetComponent<TextMeshProUGUI>();

        deathScreen = GameObject.Find("DeathMenu");
        //deathScreen = GameObject.Find("DeathMenu").GetComponent<CanvasGroup>();

        _itemSlots = new List<GameObject>();

        _itemSprites = new List<Image>();
        _itemStackTexts = new List<TextMeshProUGUI>();
    }

    private void Start ()
    {
        HideTooltip();
        HidePauseMenu();
        HideDeathMenu();

        InputHandler.instance.OnEscapePressed += PauseMenu;
    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnEscapePressed -= PauseMenu;
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

    public void DeathMenu ()
    {
        ShowDeathMenu();
        InputHandler.instance.LockCursor(false);
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
        HideItemSlots();
        HideArmorFrame();
        HideHealthBar();
        HideCharacterFrame();
    }

    public void ShowPlayerInterface ()
    {
        ShowWeaponSlots();
        ShowItemSlots();
        ShowArmorFrame();
        ShowHealthBar();
        ShowCharacterFrame();
    }

    /*
    public void CreateWeaponSlots (int count)
    {
        float startX = 600f, startY = 1000;

        for (int i = 0; i < count; ++i)
        {
            AddWeaponSlot(inventoryXCoor[i], startY);
            startX += 150f;
        }
    } */

    public void CreateItemSlots (int count)
    {
        float startY = 192f;

        for (int i = 0; i < count; ++i)
        {
            AddItemSlot(inventoryXCoor[i], startY, i);
        }
    }

    /*
    public void AddWeaponSlot (float coorX, float coorY)
    {
        GameObject newSlot = Instantiate(weaponSlotPrefab, weaponSlotParent.transform);
        RectTransform slotPosition = newSlot.GetComponent<RectTransform>();
        slotPosition.position = new Vector3(coorX, coorY, 0f);
        _weaponSlots.Add(newSlot);
    } */

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


    public void UpdateWeaponSlot (Sprite icon)
    {
        weaponSlotIcon.sprite = icon;
        weaponSlotIcon.color = Color.white;
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
            _itemSprites[i].sprite = null;
            _itemSprites[i].color = new Color(1f, 1f, 1f, 0f);
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

    public void UpdateHealthBar (float newHealthValue)
    {
        healthBar.value = newHealthValue;
    }

    public void ShowHealthBar ()
    {
        healthBarFrame.SetActive(true);
    }

    public void HideHealthBar ()
    {
        healthBarFrame.SetActive(false);
    }

    public void ShowCharacterFrame ()
    {
        characterFrame.SetActive(true);
    }

    public void UpdateCharacterFrame (float health, float maxHealth, float armor, float maxArmor)
    {
        if (health / maxHealth <= 0.5f)
        {
            // Hurt
            characterImage.sprite = playerHurt;
        }
        else if (health/maxHealth <= 0.25f)
        {
            // Very hurt
            characterImage.sprite = playerVeryHurt;
        }
        else if (health / maxHealth == 1f)
        {
            if (armor / maxArmor >= 0.5f)
            {
                // Drunk
                characterImage.sprite = playerVeryDrunk;
            }
        }
        else
        {
            Debug.Log("Normal!");
            // Normal
            characterImage.sprite = playerNormal;
        }
    }

    public void HideCharacterFrame ()
    {
        characterFrame.SetActive(false);
    }

    public void ShowArmorFrame () 
    {
        armorFrame.SetActive(true);
    }

    public void UpdateArmor (float newArmorValue) 
    {
        armorValue.text = Mathf.FloorToInt(newArmorValue).ToString() + "%";
    }

    public void HideArmorFrame ()
    {
        armorFrame.SetActive(false);
    }

    public void ShowGoldFrame ()
    {
        goldFrame.SetActive(true);
    }

    public void UpdateGoldCount (int value)
    {
        goldCount.text = value.ToString();
    }

    public void HideGoldFrame ()
    {
        goldFrame.SetActive(false);
    }

    public void ShowDamagePanel ()
    {
        StartCoroutine(CanvasFadeUpDown(damageFrame, 0.1f, damageFrame.color.a, 0.25f));
    }

    public void HideDeathMenu ()
    {
        //deathScreen.alpha = 0f;
        deathScreen.gameObject.SetActive(false);
    }

    public void ShowDeathMenu ()
    {
        CanvasGroup deathMenuCanvas = deathScreen.GetComponent<CanvasGroup>();
        deathScreen.gameObject.SetActive(true);
        deathMenuCanvas.alpha = Mathf.Lerp(0f, 1f, 5f);
        Time.timeScale = 0f;
    }

    // 0 - Out | 1 - In
    IEnumerator CanvasFade (Image element, float duration, float startAlpha, float endAlpha)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;
        float elapsedTime = 0f;

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            if (startAlpha > endAlpha) // if we are fading out/down 
            {
                element.color = new Color(element.color.r, element.color.g, element.color.b, startAlpha - percentage);
            }
            else // if we are fading in/up
            {
                element.color = new Color(element.color.r, element.color.g, element.color.b, startAlpha + percentage);
            }
            yield return new WaitForEndOfFrame();
        }
        element.color = new Color(element.color.r, element.color.g, element.color.b, endAlpha);
    }

    IEnumerator CanvasFadeUpDown (Image element, float duration, float startAlpha, float endAlpha)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;
        float elapsedTime = 0f;

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            if (startAlpha > endAlpha) // if we are fading out/down 
            {
                element.color = new Color(element.color.r, element.color.g, element.color.b, startAlpha - percentage);
            }
            else // if we are fading in/up
            {
                element.color = new Color(element.color.r, element.color.g, element.color.b, startAlpha + percentage);
            }
            yield return new WaitForEndOfFrame();
        }
        element.color = new Color(element.color.r, element.color.g, element.color.b, endAlpha);
        StartCoroutine(CanvasFade(element, duration, element.color.a, 0f));
    }


}
