using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    bool moveRight = true;
    public bool moving = true;
    public bool returning;
    public float speed;
    public Transform returningPoint;



    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;
    public DetectPlayer detectPlayerScript;
    public SoundMaker soundMaker;


    [Header("HitBox")]
    public Transform hitBox;
    public Vector2 hitBoxSize;
    public Collider2D playerToHit;
    public LayerMask playerLayer;
    public Transform player;
    float damageAmount, staminaDamageAmount;

    [Header("Slide")]
    public float slideForce;
    public float slideTime;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hitBox.position, hitBoxSize);
    }

    public void HitPlayer()
    {
        playerToHit = Physics2D.OverlapBox(hitBox.position, hitBoxSize, 0, playerLayer);
        if (playerToHit != null)
        {
            playerToHit.GetComponent<PlayerHealth>().Damage(damageAmount, staminaDamageAmount);
        }
    }

    void LookAtPlayer()
    {
        if (player.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (player.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    IEnumerator SlideToPlayer()
    {
        Vector2 direction = player.position - transform.position;
        rb.AddForce(direction.normalized * slideForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(slideTime);
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.1f);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }


    public void Attack()
    {
        LookAtPlayer();
        int randomAttack = Random.Range(1, 3);
        switch (randomAttack)
        {
            case 1:
                StartCoroutine(SlideToPlayer());
                soundMaker.EnemySwordSwing();
                hitBoxSize = new Vector2(1.5f, 1);
                damageAmount = 1;
                staminaDamageAmount = 10;
                anim.SetInteger("attack", randomAttack);
                Invoke("StopAttack", 0.01f);
                break;
            case 2:
                StartCoroutine(SlideToPlayer());
                soundMaker.EnemyHeavySwordSwing();
                hitBoxSize = new Vector2(3, 1);
                damageAmount = 2;
                staminaDamageAmount = 10;
                anim.SetInteger("attack", randomAttack);
                Invoke("StopAttack", 0.01f);
                break;
        }
    }


    public void StopAttack()
    {
        anim.SetInteger("attack", 0);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (returning)
        {
            Vector2 direction = returningPoint.position - transform.position;
            rb.velocity = direction.normalized * speed;

            if (returningPoint.position.x >= transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (moving & !returning)
        {
            if (moveRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                rb.velocity = Vector2.right * speed;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                rb.velocity = Vector2.left * speed;
            }
        }

        if (PlayerHealth.isAlive == false)
        {
            CancelInvoke("Attack");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PointRight")
        {
            moveRight = false;
        }

        if (collision.gameObject.name == "PointLeft")
        {
            moveRight = true;
        }

        if (collision.gameObject.name == "PointReturn" & returning)
        {
            moving = true;
            returning = false;
            detectPlayerScript.pointLeft.SetActive(true);
            detectPlayerScript.pointRight.SetActive(true);
        }

    }
}
