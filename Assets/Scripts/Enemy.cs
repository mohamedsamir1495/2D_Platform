using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    private IEnemyState currentState;

    public GameObject Target { get; set; }
    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private float throwRange;

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

    private Canvas healthCanvas;

    private bool dropItem=true;

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            else
                return false;
        }
    }
    [SerializeField]
    public bool InThrowRange
    {
        get
        {
            if (Target != null)
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            else
                return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return healthStat.CurrentVal <= 0;
        }
    }

    // Use this for initialization
    public override void Start () {
        base.Start();
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());

        healthCanvas = transform.GetComponentInChildren<Canvas>();
	}

    
	
	// Update is called once per frame
	void Update () {
        if (!IsDead)
        {
            if(!TakingDamage)
             currentState.Execute();

            LookAtTarget();
        }
	}

    public void RemoveTarget()
    {
        Target = null;
        healthCanvas.enabled = false;
        ChangeState(new PatrolState());
    }
    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
                ChangeDirection();

        }
    }
    public void ChangeState(IEnemyState newState)
    {
        if(currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                MyAnimator.SetFloat("Speed", 1);
                transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);

            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
                Target = null;
                healthCanvas.enabled = false;
            }
            else if (currentState is RangedState)
            {
                Target = null;
                ChangeState(new IdleState());
            }
        }
    }
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        if (!healthCanvas.isActiveAndEnabled)
            healthCanvas.enabled = true;

        healthStat.CurrentVal -= 10;
        if (!IsDead)
            MyAnimator.SetTrigger("Damage");
        else
        {
            if (dropItem)
            {
                dropItem = false;
                GameObject coin = Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x, transform.position.y + 2), Quaternion.identity);
                Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }
            MyAnimator.SetTrigger("Die");
            yield return null;
        }
    }

    /// <summary>
    /// Removes the enemy from the game
    /// </summary>
    public override void Death()
    {
        //   Destroy(gameObject);  
        dropItem = true;
        MyAnimator.ResetTrigger("Die");
        MyAnimator.SetTrigger("Idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
        healthCanvas.enabled = false;
    }
    public override void ChangeDirection()
    {
        //Makes a reference to the enemys canvas
        Transform tmp = transform.Find("Enemy Canvas").transform;

        //Stores the position, so that we know where to move it after we have flipped the enemy
        Vector3 pos = tmp.position;

        ///Removes the canvas from the enemy, so that the health bar doesn't flip with it
        tmp.SetParent(null);

        ///Changes the enemys direction
        base.ChangeDirection();

        //Puts the health bar back on the enemy.
        tmp.SetParent(transform);

        //Pits the health bar back in the correct position.
        tmp.position = pos;
    }
}
