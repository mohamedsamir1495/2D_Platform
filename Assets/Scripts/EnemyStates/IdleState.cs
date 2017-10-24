using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState {

    private Enemy enemy;

    private float idleTimer;

    private float idleDuration;

    public void Enter(Enemy Enemy)
    {
        idleDuration = Random.Range(1, 10);
        enemy = Enemy;
    }

    public void Execute()
    {
        Idle();

        if(enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag.Equals("Knife"))
            enemy.Target = Player.Instance.gameObject;
    }

    private void Idle()
    {
        enemy.MyAnimator.SetFloat("Speed", 0);

        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
