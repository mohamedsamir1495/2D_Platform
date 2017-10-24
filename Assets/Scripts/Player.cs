using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeadEventHandler();

public class Player : Character {

    private static Player instance;
    
    public event DeadEventHandler Dead;

    private IUseable useable;

    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    [SerializeField]
    private float climbSpeed;

    private float direction;

    private bool move;

    private float btnHorizontal;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float immortalTime;

    public Rigidbody2D MyRigidBody { get; set; }
    public bool OnLadder { get; set; }
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }

    }

    public override bool IsDead
    {
        get
        {
            if(healthStat.CurrentVal <= 0)
               OnDead();

            return healthStat.CurrentVal <= 0;
        }
    }

    public bool IsFalling
    {
        get
        {
            return MyRigidBody.velocity.y < 0;
        }
    }


    // Use this for initialization
    public override void Start() {

        base.Start();
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!TakingDamage && !IsDead)
        {
            if(transform.position.y <=-14f)
            {
                Death();
            }
            HandleInput();

        }
    }

    void FixedUpdate() {
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            OnGround = IsGrounded();

            if (move)
            {
                this.btnHorizontal = Mathf.Lerp(btnHorizontal, direction, Time.deltaTime*2);
                //HandleMovement(btnHorizontal);
                Flip(direction);
            }
            else
            {
                HandleMovement(horizontal,vertical);
                Flip(horizontal);
            }

            HandleLayers();
        }
    }

    public void OnDead()
    {
        if (Dead != null)
            Dead();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !OnLadder && !IsFalling)
            MyAnimator.SetTrigger("Jump");
        if (Input.GetKeyDown(KeyCode.LeftShift))
            MyAnimator.SetTrigger("Attack");
        if (Input.GetKeyDown(KeyCode.LeftControl))
            MyAnimator.SetTrigger("Slide");
        if (Input.GetKeyDown(KeyCode.V))
            MyAnimator.SetTrigger("Throw");
        if (Input.GetKeyDown(KeyCode.E))
            Use();
                   
    }
    private void HandleMovement(float horizontal,float vertical)
    {
        // Check if the player is falling
        if (IsFalling)
        {
            gameObject.layer = 12;
            MyAnimator.SetBool("Land", true);
        }
        if (!Attack && !Slide && (OnGround || airControl))
            MyRigidBody.velocity = new Vector2(horizontal * movementSpeed, MyRigidBody.velocity.y);

        if (Jump && MyRigidBody.velocity.y == 0 && !OnLadder)
            MyRigidBody.AddForce(new Vector2(0, jumpForce));

        if (OnLadder)
        {
            MyAnimator.speed = vertical !=0 ? Mathf.Abs(vertical):Mathf.Abs(horizontal);
            MyRigidBody.velocity = new Vector2(horizontal * climbSpeed, vertical * climbSpeed);
        }
            MyAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
    }


    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }



    private bool IsGrounded()
    {
        if (MyRigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                foreach (Collider2D j in colliders)
                    if (j.gameObject != gameObject)
                    {

                        return true;
                    }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
            MyAnimator.SetLayerWeight(1, 1);
        else
            MyAnimator.SetLayerWeight(1, 0);
    }

    public override void ThrowKnife(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            base.ThrowKnife(value);
        }

    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);

        }
    }
    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            healthStat.CurrentVal -= 10;
            if (!IsDead)
            {
                MyAnimator.SetTrigger("Damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("Die");
            }
        }
    }

    public override void Death()
    {
        MyRigidBody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("Idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
    }

    private void Use()
    {
        if(useable != null)
        {
            useable.Use();
        }
    }
    public void BtnJump()
    {
        MyAnimator.SetTrigger("Jump");
        Jump = true;
    }

    public void BtnAttack()
    {
        MyAnimator.SetTrigger("Attack");
    }
    public void BtnSlide()
    {
        MyAnimator.SetTrigger("Slide");
    }
    public void BtnThrow()
    {
        MyAnimator.SetTrigger("Throw");
    }
    public void BtnMove(float direction)
    {
        this.direction = direction;
        this.move = true;
    }
    public void BtnStopMove()
    {
        this.direction = 0;
        this.btnHorizontal = 0;
        this.move = false;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag.Equals("Coin")){
            GameManager.Instance.CollectedCoins++;
            Destroy(other.gameObject);
        }
        
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Useable"))
        {
            useable = other.GetComponent<IUseable>();
        }
        base.OnTriggerEnter2D(other);

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Useable"))
        {
            useable = null;
        }
    }

}
