using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    protected Transform knifePos;
    [SerializeField]
    protected float movementSpeed;

    /// <summary>
    /// Indicates if the character is facing right
    /// </summary>
    protected bool facingRight;

    /// <summary>
    /// The knife prefab this is used for instantiating a knife
    /// </summary>
    [SerializeField]
    private GameObject KnifePrefab;

    /// <summary>
    ///  The Character's Health
    /// </summary>
    [SerializeField]
    protected Stat healthStat;

    protected Vector3 startPos;

    [SerializeField]
    private EdgeCollider2D swordCollider;

    [SerializeField]
    private List<string> damageSources;

    public abstract bool IsDead { get; }

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }
    public Animator MyAnimator { get; private set; }

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    // Use this for initialization
    public virtual void Start () {
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
        startPos = transform.position;
        healthStat.Initialize();
    }

    // Update is called once per frame
    void Update () {
		
	}
    public abstract IEnumerator TakeDamage();

    public abstract void Death();

    //ChangeDirection in Character
    public virtual void ChangeDirection()
    {
        //Changes the facingRight bool to its negative value
        facingRight = !facingRight;

        //Flips the character by changing the scale
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void ThrowKnife(int value)
    {
        if (facingRight)
        {
            GameObject x = Instantiate(KnifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            x.GetComponent<Knife>().Initialize(Vector2.right);
        }
        else
        {
            GameObject x = Instantiate(KnifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            x.GetComponent<Knife>().Initialize(Vector2.left);
        }
    }

   public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag))
            StartCoroutine(TakeDamage());
        
    }
}
