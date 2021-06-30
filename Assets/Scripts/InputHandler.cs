using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    [Header("Key Binding")]
    [Space(10)]
    [SerializeField] private KeyCode attackKey          = KeyCode.Mouse0;
    [SerializeField] private KeyCode blockKey           = KeyCode.Mouse1;
    [SerializeField] private KeyCode jumpKey            = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey          = KeyCode.LeftShift;
    [SerializeField] private KeyCode interactionKey     = KeyCode.E;
    [SerializeField] private KeyCode escapeKey          = KeyCode.I;
    [SerializeField] private KeyCode weaponKey1         = KeyCode.Keypad1;
    [SerializeField] private KeyCode weaponKey2         = KeyCode.Keypad2;
    [SerializeField] private KeyCode weaponKey3         = KeyCode.Keypad3;

    [Space(20)]
    [SerializeField] private float powerAttackThreshold = 0.5f;

    private float horizontalInput, verticalInput;
    private float mouseX, mouseY;

    [SerializeField]
    [Range(400f, 1200f)]
    private float mouseSensitivity = 600f;

    public event Action<float> OnHorizontalPressed;
    public event Action<float> OnVerticalPressed;

    public event Action<float, float> OnMouseMove;

    public event EventHandler OnAttackUnleashed;
    public event EventHandler OnPowerAttackUnleashed;

    public event EventHandler OnBlockPressed;
    public event EventHandler OnBlockReleased;

    public event EventHandler OnJumpPressed;
    public event EventHandler OnSprintPressed;
    public event EventHandler OnSprintReleased;

    public event EventHandler OnInteractionPressed;
    public event EventHandler OnEscapePressed;

    public event Action<int> OnWeaponKeyPressed;

    private bool isPowerAttacking = false;
    private float powerAttackFill = 0f;

    [HideInInspector]
    public bool isOnMenu = false;

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
    }

    private void Update ()
    {
        //================================================================
        //  Mouse
        //================================================================

        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (!isOnMenu)
        {
            OnMouseMove?.Invoke(mouseX, mouseY);

            //================================================================
            //  Movement
            //================================================================

            // Directional Input
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");


            OnHorizontalPressed?.Invoke(Mathf.Abs(horizontalInput) >= 0.05f ? horizontalInput : 0f);
            OnVerticalPressed?.Invoke(Mathf.Abs(verticalInput) >= 0.05f ? verticalInput : 0f);

            // Sprint Key
            if (Input.GetKeyDown(sprintKey))
            {
                OnSprintPressed?.Invoke(this, EventArgs.Empty);
            }
            else if (Input.GetKeyUp(sprintKey))
            {
                OnSprintReleased?.Invoke(this, EventArgs.Empty);
            }

            // Jump Key
            if (Input.GetKeyDown(jumpKey))
            {
                OnJumpPressed?.Invoke(this, EventArgs.Empty);
            }

            //================================================================
            //  Combat
            //================================================================

            // Attack Key & Power Attack
            if (Input.GetKey(attackKey))
            {
                // Increment the power attack "bar"
                powerAttackFill += Time.deltaTime;

                // If the power attack "bar" has filled up
                if (powerAttackFill >= powerAttackThreshold && !isPowerAttacking)
                {
                    isPowerAttacking = true;
                    powerAttackFill = 0f;
                    OnPowerAttackUnleashed?.Invoke(this, EventArgs.Empty);
                }
            }

            if (Input.GetKeyUp(attackKey) && !isPowerAttacking)
            {
                powerAttackFill = 0f;
                OnAttackUnleashed?.Invoke(this, EventArgs.Empty);
            }
            else if (Input.GetKeyUp(attackKey))
            {
                powerAttackFill = 0f;
                isPowerAttacking = false;
            }

            // Block Key
            if (Input.GetKeyDown(blockKey))
            {
                powerAttackFill = 0f;
                OnBlockPressed?.Invoke(this, EventArgs.Empty);
            }
            else if (Input.GetKeyUp(blockKey))
            {
                OnBlockReleased?.Invoke(this, EventArgs.Empty);
            }

            //================================================================
            //  Interaction
            //================================================================

            // Interaction Key
            if (Input.GetKeyDown(interactionKey))
            {
                OnInteractionPressed?.Invoke(this, EventArgs.Empty);
            }

            //================================================================
            //  Inventory Weapon
            //================================================================

            // Weapon Key
            int weaponKey = 0;

            if (Input.GetKeyDown(weaponKey1))
                weaponKey = 1;
            else if (Input.GetKeyDown(weaponKey2))
                weaponKey = 2;
            else if (Input.GetKeyDown(weaponKey3))
                weaponKey = 3;

            if (weaponKey > 0)
                OnWeaponKeyPressed?.Invoke(weaponKey - 1);
        }

        // Escape key
        if (Input.GetKeyDown(escapeKey))
        {
            OnEscapePressed?.Invoke(this, EventArgs.Empty);
        }
    }

    public KeyCode GetInteractionKey () => interactionKey;

    public void LockCursor (bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            isOnMenu = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            isOnMenu = true;
        }
    }
}
