using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Rigidbody2D myRigidBody;

    private Vector2 direction;
	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
	}

    private void FixedUpdate()
    {
        myRigidBody.velocity = direction * speed;
    }
    public void Initialize(Vector2 Direction)
    {
        direction = Direction;
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
