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

    private bool willMoveOnLoad = false;

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

    public void SetExactPosition (Vector3 position)
    {
        transform.position = position;
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
            // Play the jump sound 75% of the times
            if (UnityEngine.Random.Range(0f, 1f) <= 0.5f)
                AudioManager.instance.PlaySoundRandom("jump");

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    
    private void Update ()
    {
        isGrounded = CheckGround();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //-2 pra garantir que o player v?? para o ch??o, j?? que a esfera pode ou nao estar no chao exatamente
        }

        // Normalize movement to avoid faster diagonal strafing 
        Vector3 movement = (transform.right * xMove + transform.forward * zMove).normalized;

        character.Move(speed * Time.deltaTime * movement);

        velocity.y += gravity * Time.deltaTime;     //velocidade vai ser o tempo*a gravidade

        Vector2 horizSpeed = new Vector2(xMove, zMove);

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