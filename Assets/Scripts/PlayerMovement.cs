using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController character;

    [SerializeField] private float defaultSpeed = 10f;

    [SerializeField] private float speed;

    [SerializeField] private float gravity = -9.81f;

    [SerializeField] private float jumpHeight = 4f;

    [SerializeField] private float maxSlopAngle = 50f;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;

    private Vector3 forward;
    private RaycastHit hitInfo;
    public bool isGrounded;
    private float angle;
    private float groundAngle;

    private Vector3 velocity;

    private Vector3 position;

    private float xMove = 1f, zMove = 1f;

    private Animator anim;

    public bool debugVector = false;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float DefaultSpeed
    {
        get { return defaultSpeed; }
    }

    private void Awake ()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        speed = defaultSpeed;
    }

    private void Start() 
    {
        if (gameObject.CompareTag("Player"))
        {
            InputHandler.instance.OnHorizontalPressed   += HandleHorizontalInput;
            InputHandler.instance.OnVerticalPressed     += HandleVerticalInput;
            /*
            InputHandler.instance.OnSprintPressed       += HandleSprintUp;
            InputHandler.instance.OnSprintReleased      += HandleSprintDown;
            */
            InputHandler.instance.OnJumpPressed         += HandleJump;
        }
    }

    private void HandleHorizontalInput (float value)
    {
        if (isGrounded)
            xMove = value;
    }

    private void HandleVerticalInput (float value)
    {
        if (isGrounded)
            zMove = value;
    }

    private void HandleSprintUp (object sender, EventArgs e)
    {
        speed = defaultSpeed * 2;
    }

    private void HandleSprintDown (object sender, EventArgs e)
    {
        speed = defaultSpeed;
    }

    private void HandleJump (object sender, EventArgs e)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGround();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //-2 pra garantir que o player vá para o chão, já que a esfera pode ou nao estar no chao exatamente
        }

        Vector3 movement = transform.right * xMove + transform.forward * zMove;

        character.Move(movement * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;     //velocidade vai ser o tempo*a gravidade

        Vector2 horizSpeed = new Vector2 (xMove, zMove);

        character.Move(velocity * Time.deltaTime); //DeltaY = 1/2*G*t*t, por isso repete o deltatime aqui

        anim.SetFloat("speed", horizSpeed.magnitude); 
    }

    private bool CheckGround ()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void OnDestroy ()
    {
        InputHandler.instance.OnHorizontalPressed   -= HandleHorizontalInput;
        InputHandler.instance.OnVerticalPressed     -= HandleVerticalInput;
        /*
        InputHandler.instance.OnSprintPressed       -= HandleSprintUp;
        InputHandler.instance.OnSprintReleased      -= HandleSprintDown;
        */
        InputHandler.instance.OnJumpPressed         -= HandleJump;
    }

    private void OnDrawGizmosSelected ()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundDistance);

    }


}