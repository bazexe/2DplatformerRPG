using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockBackDirection;
    protected bool isKnocked;
    [SerializeField] protected float knockBackDuration;

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fX { get; private set; }
    #endregion

    public int facingDir { get; private set; } = 1;
    public bool facingRight = true;
    protected virtual void Awake()
    {

    }   
    
    protected virtual void Start()
    {
        //Get Components
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fX = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        Debug.Log(gameObject.name + " was damaged");
        StartCoroutine("HitKnockback");
        fX.StartCoroutine("FlashFX");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockBackDirection.x * -facingDir, knockBackDirection.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) { return; }
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }


    public void ZeroVelocity()
    {
        if (isKnocked) { return; }
        rb.velocity = new Vector2(0, 0);

    }

        #region Flip
        public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion Flip

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    
    #endregion Collision
}
