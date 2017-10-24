using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState {

    private Enemy enemy;

    private float throwTimer;
    private float throwCoolDown = 3f;
    private bool canThrow = true;

    public void Enter(Enemy Enemy)
    {
        enemy = Enemy;
    }

    public void Execute()
    {
        ThrowKnife();
        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
        else if (enemy.Target != null)
        {
            enemy.Move();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
    }

    private void ThrowKnife() //Throw knife , arrow , etc
    {
        throwTimer += Time.deltaTime;

        if (throwTimer >= throwCoolDown)
        {
            canThrow = true;
            throwTimer = 0;
        }
        if(canThrow)
        {
            canThrow = false;
            enemy.MyAnimator.SetTrigger("Throw");
        }
    }
}
