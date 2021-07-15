using System.Data.Common;
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
    private GameObject rangedSlotParent;

    private GameObject healthBarFrame;
    public Slider healthBar;

    private GameObject characterFrame;
    private Image characterImage;

    private GameObject armorFrame;
    private GameObject armorIndicator;
    private TextMeshProUGUI armorValue;

    private Image damageFrame;
    private CanvasGroup damageCanvas;

    private GameObject weaponSlot;
    private Image weaponSlotIcon;

    private GameObject rangedSlot;
    private Image rangedSlotIcon;
    private TextMeshProUGUI rangedKey;
    private Slider rangedCooldownSlider;

    private GameObject deathScreen;
    //private CanvasGroup deathScreen;

    private GameObject shopScreen;

    private List<GameObject> _itemSlots;

    private List<Image> _itemSprites;
    private List<TextMeshProUGUI> _itemStackTexts;

    private GameObject goldFrame;
    private TextMeshProUGUI goldCount;
    private GameObject goldAnimPoint;

    private GameObject progressFrame;
    private TextMeshProUGUI progressTitle;
    private Slider progressSlider;

    private GameObject sceneLoader;
    private Slider sceneSlider;

    private readonly string[] slotKeyCode = { "[1]", "[2]", "[3]", "[4]", "[5]" };

    private readonly float[] inventoryXCoor = { 768f, 896f, 1024f, 1152f };

    [SerializeField] private GameObject weaponSlotPrefab;
    [SerializeField] private GameObject itemSlotPrefab;

    [SerializeField] private GameObject goldAnimPrefab;

    [SerializeField] private Sprite defaultItemSprite;

    [SerializeField] private Sprite playerNormal;
    [SerializeField] private Sprite playerHurt;
    [SerializeField] private Sprite playerVeryHurt;
    [SerializeField] private Sprite playerVeryDrunk;
    
    private GameObject warningRoot;
    [SerializeField] private GameObject warningPrefab;

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

        //DontDestroyOnLoad(gameObject);

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
        damageCanvas = damageFrame.GetComponent<CanvasGroup>();

        weaponSlotParent = GameObject.Find("WeaponSlots");
        itemSlotParent = GameObject.Find("ItemSlots");
        rangedSlotParent = GameObject.Find("RangedSlots");

        weaponSlot = GameObject.Find("EquippedWeaponIcon");
        weaponSlotIcon = GameObject.Find("WeaponIcon").GetComponent<Image>();

        rangedSlot = GameObject.Find("EquippedRangedIcon");
        rangedSlotIcon = GameObject.Find("RangedIcon").GetComponent<Image>();
        rangedKey = rangedSlot.GetComponentInChildren<TextMeshProUGUI>();
        rangedCooldownSlider = rangedSlot.GetComponentInChildren<Slider>();

        goldFrame = GameObject.Find("GoldPanel");
        goldCount = GameObject.Find("GoldCount").GetComponent<TextMeshProUGUI>();
        goldAnimPoint = GameObject.Find("GoldAnimation");

        progressFrame = GameObject.Find("ProgressBar");
        progressSlider = progressFrame.GetComponent<Slider>();
        progressTitle = progressFrame.GetComponentInChildren<TextMeshProUGUI>();

        deathScreen = GameObject.Find("DeathMenu");

        shopScreen = GameObject.Find("ShopMenu");

        _itemSlots = new List<GameObject>();

        _itemSprites = new List<Image>();
        _itemStackTexts = new List<TextMeshProUGUI>();
        
        warningRoot = GameObject.Find("WarningTextPoint");
        
        sceneLoader = GameObject.Find("LoadingMenu");
        sceneSlider = sceneLoader.GetComponentInChildren<Slider>();
    }

    private void Start ()
    {
        WipeInterface();

        SetRangedSlotKey();

        InputHandler.instance.OnEscapePressed += PauseMenu;
    }

    public void WipeInterface()
    {
        HideTooltip();
        HideDeathMenu();
        HideShopMenu();
        HideProgressMenu();
        HideLoadingMenu();
        HidePauseMenu();
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
        pauseMenu.SetActive(false);
        ShowDeathMenu();
        InputHandler.instance.LockCursor(false);
    }

    public void ShowShopMenu (GameObject seller)
    {
        shopScreen.SetActive(true);
        shopScreen.GetComponent<ShopInterface>().ShowShopInterface(seller);
        HidePlayerInterface();
        ShowGoldFrame();
        InputHandler.instance.LockCursor(false);
    }

    public void HideShopMenu ()
    {
        shopScreen.SetActive(false);
        ShowPlayerInterface();
        InputHandler.instance.LockCursor(true);
    }

    public void HideShopMenu (bool hasRanged)
    {
        HideShopMenu();
        if (!hasRanged)
            HideRangedSlot();
    }

    public void DrawPauseMenu ()
    {
        pauseMenu.SetActive(true);
        HidePlayerInterface();
        Time.timeScale = 0f;
        isPauseMenuOn = true;
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
        HideRangedSlot();
        HideArmorFrame();
        HideHealthBar();
        HideCharacterFrame();
        HideGoldFrame();
    }

    public void ShowPlayerInterface ()
    {
        ShowWeaponSlots();
        ShowItemSlots();
        ShowRangedSlot();
        ShowArmorFrame();
        ShowHealthBar();
        ShowCharacterFrame();
        ShowGoldFrame();
    }

    public void CreateItemSlots (int count)
    {
        float screenX = Screen.width;
        float screenY = Screen.height;
        float startXLeft, startXRight, startY;

        float centerX = screenX / 2f;
        float centerY = screenY / 2f;

        //Debug.Log("X: " + screenX + " Y: " + screenY);

        startXLeft = centerX - (centerX * 0.21f);
        startXRight = centerX + (centerX * 0.07f);
        startY = centerY - (centerY * 0.6f);

        int i, j;
        
        for (i = 0; i < count / 2; ++i)
        {
            AddItemSlot (startXLeft, startY, i);
            startXLeft = startXLeft + (centerX * 0.14f);
        }

        for (j = i; j < count; ++j)
        {
            AddItemSlot (startXRight, startY, j);
            startXRight = startXRight + (centerX * 0.14f);
        }
        
        /*
        if (screenX >= 1920f && screenY >= 1080f)
        {
            Debug.Log("Here 1!");
            startY = 192f;
            startX = 768f;
            for (int i = 0; i < count; ++i)
            {
                AddItemSlot(startX, startY, i);
                startX += 128f;
            }
        }
        else
        {
            Debug.Log("Here 2!");
            startX = centerX - 128f;
            startY = centerY - 240f;
            int i, j;
            for (i = 0; i < count / 2; ++i)
            {
                AddItemSlot(startX, startY, i);
                startX += 86f;
            }

            startX = centerX + 43;
            for (j = i; j < count; ++j)
            {
                AddItemSlot(startX, startY, j);
                startX += 86f;
            }
        } */
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


    public void UpdateRangedSlot (Sprite icon)
    {
        if (icon != null)
        {
            rangedSlotIcon.sprite = icon;
            rangedSlotIcon.color = Color.white;
            ShowRangedSlot();
        }
    }

    public void ShowItemSlots ()
    {
        if (rangedSlotIcon != null)
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

    public void ShowRangedSlot ()
    {        
        rangedSlotParent.SetActive(true);
    }

    public void HideRangedSlot()
    {
        rangedSlotParent.SetActive(false);
    }

    public void SetRangedSlotKey ()
    {
        string keyText = InputHandler.instance.GetRangedKey().ToString();
        rangedKey.text = "[" + keyText + "]";
    }

    public void UpdateRangedCooldownSlider (float value)
    {
        rangedCooldownSlider.value = value;
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
            else
            {
            }
        }
        else
        {
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

    public void UpdateGoldCount (int value, int delta)
    {
        goldCount.text = value.ToString();
        PlayGoldAnimation(delta);
    }

    public void PlayGoldAnimation (int value)
    {
        // Don't do anything if there's no change
        if (value == 0)
            return;
        
        if (value > 0)
            AudioManager.instance.PlaySound("gold_pickup");
        else
            AudioManager.instance.PlaySound("gold_sell");

        // Play animation
        GameObject goldAnimation = Instantiate(goldAnimPrefab, goldAnimPoint.transform);
        TextMeshProUGUI goldText = goldAnimation.GetComponent<TextMeshProUGUI>();

        // Adds a plus sign if the value is positive
        if (value >= 0)
            goldText.text = "+" + value.ToString();
        else
            goldText.text = value.ToString();

        goldAnimation.GetComponent<Animator>().SetInteger("value", value);
    }

    public void HideGoldFrame ()
    {
        goldFrame.SetActive(false);
    }

    public void ShowDamagePanel (Color color)
    {
        damageFrame.color = color;
        StartCoroutine(CanvasFadeUpDown(damageCanvas, 0.1f, damageCanvas.alpha, 0.25f));
    }

    public void HideDeathMenu ()
    {
        //deathScreen.alpha = 0f;
        deathScreen.SetActive(false);
    }

    public void ShowDeathMenu ()
    {
        CanvasGroup deathMenuCanvas = deathScreen.GetComponent<CanvasGroup>();
        deathScreen.SetActive(true);
        deathMenuCanvas.alpha = Mathf.Lerp(0f, 1f, 5f);
        Time.timeScale = 0f;
    }

    public void ShowProgressMenu (string title)
    {
        progressFrame.SetActive(true);
        progressSlider.value = 0f;
        progressTitle.text = title;
    }

    public void HideProgressMenu ()
    {
        progressFrame.SetActive(false);
    }

    public void UpdateProgressTitle (string title)
    {
        progressTitle.text = title;
    }

    public void UpdateProgressBar(float value)
    {
        progressSlider.value = value;
    }
    
    public void ThrowWarningMessage (string message)
    {
        GameObject messageObj = Instantiate(warningPrefab, warningRoot.transform);
        messageObj.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void ShowLoadingMenu()
    {
        UpdateLoadingSlider(0f);
        sceneLoader.SetActive(true);     
    }
    
    public void HideLoadingMenu()
    {
        sceneLoader.SetActive(false);
    }

    public void UpdateLoadingSlider(float value)
    {
        sceneSlider.value = value;
    }

    // 0 - Out | 1 - In
    IEnumerator CanvasFade (CanvasGroup canvasGroup, float duration, float startAlpha, float endAlpha)
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
                canvasGroup.alpha = startAlpha - percentage;
            }
            else // if we are fading in/up
            {
                canvasGroup.alpha = startAlpha + percentage;
            }
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = endAlpha;
    }

    IEnumerator CanvasFadeUpDown (CanvasGroup canvasGroup, float duration, float startAlpha, float endAlpha)
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
                canvasGroup.alpha = startAlpha - percentage;
            }
            else // if we are fading in/up
            {
                canvasGroup.alpha = startAlpha + percentage;
            }
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = endAlpha;
        StartCoroutine(CanvasFade(canvasGroup, duration, canvasGroup.alpha, 0f));
    }


}
