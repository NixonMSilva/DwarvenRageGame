using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private StatusController status;

    private bool canBlock = true;

    public bool CanBlock
    {
        get { return canBlock; }
        set { canBlock = value; }
    }

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        status = GetComponent<StatusController>();
    }

    private void Start ()
    {
        if (gameObject.CompareTag("Player"))
        {
            InputHandler.instance.OnBlockPressed += HandleBlockUp;
            InputHandler.instance.OnBlockReleased += HandleBlockDown;
        }
    }

    private void HandleBlockUp (object sender, EventArgs e)
    {
        if (canBlock)
        {
            status.IsBlocking = true;
            if (anim != null)
                anim.SetBool("isBlocking", true);
        }
    }

    private void HandleBlockDown (object sender, EventArgs e)
    {
        if (canBlock)
        {
            status.IsBlocking = false;
            if (anim != null)
                anim.SetBool("isBlocking", false);
        }
    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnBlockPressed -= HandleBlockUp;
        InputHandler.instance.OnBlockPressed -= HandleBlockDown;
    }
}
