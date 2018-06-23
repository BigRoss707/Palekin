using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject {

    public struct Inputs
    {
        //Axis
        public float VertAxis;
        public float HoriAxis;

        //Jump
        public bool Jump;
        public bool JumpUp;
        public bool JumpDown;

        //Keys
        public bool W;
        public bool W_Down;
        public bool W_Up;
        public bool A;
        public bool A_Down;
        public bool A_Up;
        public bool S;
        public bool S_Down;
        public bool S_Up;
        public bool D;
        public bool D_Down;
        public bool D_Up;
    }

    #region PublicVariables
    public bool haltInput = false;
    public float maxSpeed = 30;
    public float speed = 18;
    public float speedScalar = 1;
    public float jumpSpeed = 8;
    public float jumpSpeedScalar = 1;
    #endregion

    #region PrivateVariables
    private Inputs input;
    private SpriteRenderer spriteRenderer;

    //private Animator animator;
    #endregion


    void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        //animator = this.GetComponent<Animator>();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        //Initialization of public members
        haltInput = false;

        //Initialization of private members
        input = new Inputs();
    }

    protected override void FixedUpdate()
    {
        TakeInput(haltInput);
        base.FixedUpdate();
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();
        Vector2 move = Vector2.zero;
        move.x = input.HoriAxis;

        if(input.JumpDown && grounded)
        {
            velocity.y = jumpSpeed * jumpSpeedScalar;
        }
        else if(input.JumpUp)
        {
            if(velocity.y  > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x >  0.01f) : (move.x < 0.01f));
        if(flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        //animator.SetBool("grounded", grounded);
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        targetVelocity = move * speed;
        //Debug.Log("Target Velocity: " + targetVelocity.ToString());
        Debug.Log("Velocity: " + velocity.ToString());
    }

    void TakeInput(bool haltInput)
    {
        if(!haltInput)
        {
            input.VertAxis = Input.GetAxis("Vertical");
            input.HoriAxis = Input.GetAxis("Horizontal");

            input.Jump = Input.GetKey(KeyCode.Space);
            input.JumpUp = Input.GetKeyUp(KeyCode.Space);
            input.JumpDown = Input.GetKeyDown(KeyCode.Space);

            input.W = Input.GetKey(KeyCode.W);
            input.W_Down = Input.GetKeyDown(KeyCode.W);
            input.W_Up = Input.GetKeyUp(KeyCode.W);

            input.A = Input.GetKey(KeyCode.A);
            input.A_Down = Input.GetKeyDown(KeyCode.A);
            input.A_Up = Input.GetKeyUp(KeyCode.A);

            input.S = Input.GetKey(KeyCode.S);
            input.S_Down = Input.GetKeyDown(KeyCode.S);
            input.S_Up = Input.GetKeyUp(KeyCode.S);

            input.D = Input.GetKey(KeyCode.D);
            input.D_Down = Input.GetKeyDown(KeyCode.D);
            input.D_Up = Input.GetKeyUp(KeyCode.D);

            
        }
        
    }

}
