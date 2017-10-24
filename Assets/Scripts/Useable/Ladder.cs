using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour ,IUseable{

    [SerializeField]
    private Collider2D platformCollider;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Use()
    {
        if (Player.Instance.OnLadder)
        {
            //We need to stop climbing
            UseLadder(false,1,0,1,"Land");
        }
        else
        {
            //We need to start climbing
            UseLadder(true,0,1,0,"Reset");
            Physics2D.IgnoreCollision(Player.Instance.GetComponent<Collider2D>(),platformCollider,true);
        }
    }

    private void UseLadder(bool onLadder,int gravity,int layerWeight,int animSpeed,string trigger)
    {
        Player.Instance.OnLadder = onLadder;
        Player.Instance.MyRigidBody.gravityScale = gravity;
        Player.Instance.MyAnimator.SetLayerWeight(2, layerWeight);
        Player.Instance.MyAnimator.speed = animSpeed;
        Player.Instance.MyAnimator.SetTrigger(trigger);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            UseLadder(false, 1,0,1,"Land");
            Physics2D.IgnoreCollision(Player.Instance.GetComponent<Collider2D>(), platformCollider, false);

        }
    }
}
