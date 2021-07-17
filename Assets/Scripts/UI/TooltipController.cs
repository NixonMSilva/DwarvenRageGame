using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TooltipController : MonoBehaviour
{
    [SerializeField] private UnityEvent OnPressEvent;

    [SerializeField] private string defaultTooltipText = "<tooltip text not set>";

    private string tooltipKeyText;

    private string currentTooltipText;

    private bool canPressButton = false;
    private bool isDisabled = false;

    [SerializeField] private bool hideOnAction = true;

    public bool IsDisabled
    {
        get => isDisabled;
        set => isDisabled = value;
    }

    private void Awake ()
    {
        currentTooltipText = defaultTooltipText;
    }

    // Start is called before the first frame update
    private void Start()
    {
        KeyCode tooltipKey = InputHandler.instance.GetInteractionKey();
        tooltipKeyText = "[" + tooltipKey.ToString().ToUpper() + "]";
        InputHandler.instance.OnInteractionPressed += OnButtonPressed;
    }

    public void OnButtonPressed (object sender, EventArgs e)
    {
        if (canPressButton && !isDisabled)
        {
            if (hideOnAction)
            {
                canPressButton = false;
                HideTooltip();
            }
                
            OnPressEvent?.Invoke();
        }
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isDisabled)
        {
            canPressButton = true;
            ShowTooltip();
        }
    }

    public void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isDisabled)
        {
            canPressButton = false;
            HideTooltip();
        }
    }

    public void DisableTooltip()
    {
        isDisabled = true;
    }

    public void ShowTooltip ()
    {
        UserInterfaceController.instance.DrawTooltip(currentTooltipText + " " + tooltipKeyText);
    }

    public void HideTooltip ()
    {
        UserInterfaceController.instance.HideTooltip();
    }

    public void SetTooltipText (string text)
    {
        currentTooltipText = text;
        UserInterfaceController.instance.DrawTooltip(currentTooltipText);
    }

    private void OnDisable ()
    {
        /*
        HideTooltip();
        canPressButton = false;
        */
    }

    public string GetTooltipBaseText () => defaultTooltipText;

    public string GetTooltipCurrentText () => currentTooltipText;
}
