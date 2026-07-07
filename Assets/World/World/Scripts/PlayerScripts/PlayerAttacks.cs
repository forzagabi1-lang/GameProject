using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttacks : MonoBehaviour
{
    [Header("Attack")]
    float attackCoolDown;
    bool canAttack = true;

    [Header("Defend")]
    public bool defending = false;
    float defendCoolDown;
    public bool canDefend = true;
    public float holdTime;


    [Header("Components")]
    Animator anim;
    Rigidbody2D rb;
    PlayerMovementScript playerMoveScript;
    PlayerHealth playerHealth;
    public SoundMaker soundMaker;

    [Header("Strength")]
    public float damageModifier = 0f;

    [Header("HitBox")]
    public Transform hitBox;
    public float hitBoxRadius;
    public LayerMask bossLayer;
    public Collider2D bossToHit;
    public LayerMask enemyLayer;
    public Collider2D enemyToHit;
    float damageAmount;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitBox.position, hitBoxRadius);
    }


    public void HitEnemy()
    {
        enemyToHit = Physics2D.OverlapCircle(hitBox.position, hitBoxRadius, enemyLayer);
        if (enemyToHit != null)
        {
            enemyToHit.GetComponent<EnemyHealth>().Damage(damageAmount);
        }
    }
    public void HitBoss()
    {
        bossToHit = Physics2D.OverlapCircle(hitBox.position, hitBoxRadius, bossLayer);
        if (bossToHit != null)
        {
            bossToHit.GetComponent<BossHealth>().Damage(damageAmount);
        }
    }
    IEnumerator AttackCoolDown()
    {
        canAttack = false;
        playerMoveScript.enabled = false;
        playerMoveScript.rb.velocity = Vector2.zero;
        yield return null;
        float coolDown = anim.GetCurrentAnimatorStateInfo(0).length;
        attackCoolDown = coolDown;
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
        playerMoveScript.enabled = true;
    }

    IEnumerator DefendCoolDown()
    {
        yield return null;
        float coolDown = anim.GetCurrentAnimatorStateInfo(0).length;
        defendCoolDown = coolDown;
        yield return new WaitForSeconds(defendCoolDown);
        canDefend = true;
        canAttack = true;
        playerMoveScript.enabled = true;
    }

    public void StartDefending()
    {
        defending = true;
        canDefend = false;
        canAttack = false;
        playerMoveScript.enabled = false;
        playerMoveScript.rb.velocity = Vector2.zero;
    }
    void PlayerInputs()
    {
        if (Input.GetMouseButton(0) & canAttack)
        {
            damageAmount = 1 + damageModifier;
            anim.SetTrigger("Attack1");
            soundMaker.PlayerSwingSound();
            StartCoroutine(AttackCoolDown());
        }
        else if (Input.GetMouseButton(1) & canAttack)
        {
            damageAmount = 1.5f + damageModifier;
            anim.SetTrigger("Attack2");
            soundMaker.PlayerSwingSound();
            StartCoroutine(AttackCoolDown());
        }
        else if (Input.GetMouseButton(2) & canAttack)
        {
            damageAmount = 2 + damageModifier;
            anim.SetTrigger("Attack3");
            soundMaker.PlayerSwingSound();
            StartCoroutine(AttackCoolDown());
        }
        if (Input.GetKey(KeyCode.F))
        {

            holdTime = holdTime += Time.deltaTime;
            

            if (canDefend & holdTime >= 0.4f)
            {
                StopCoroutine(DefendCoolDown());
                anim.SetBool("IdleBlock", true);
            }

        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            holdTime = 0;
            defending = false;
            anim.SetBool("IdleBlock", false);
            StartCoroutine(DefendCoolDown());
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMoveScript = GetComponent<PlayerMovementScript>();

        if (PlayerPrefs.HasKey("PlayerStrength"))
        {
            damageModifier = PlayerPrefs.GetFloat("PlayerStrength");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
    }
}
