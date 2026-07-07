using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{ 
    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;
    public DetectPlayerByBoss detectPlayerByBoss;

    [Header("HitBox")]
    public Transform hitBox;
    public Vector2 hitBoxSize;
    public Collider2D forBossPlayerToHit;
    public LayerMask forBossPlayerLayer;
    public Transform forBossPlayer;
    float damageAmount, staminaDamageAmount;
    public SoundMaker soundMaker;

    [Header("Slide")]
    public float slideForce;
    public float slideTime;

    public void HitPlayer()
    {
        forBossPlayerToHit = Physics2D.OverlapBox(hitBox.position, hitBoxSize, 0, forBossPlayerLayer);
        if (forBossPlayerToHit != null)
        {
            forBossPlayer.GetComponent<PlayerHealth>().Damage(damageAmount, staminaDamageAmount);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth.isAlive == false)
        {
            CancelInvoke("BossAttacks");
        }
    }

    public void BossAttacks()
    {
        StartLookingAtPlayer();
        int randomAttack = Random.Range(1, 3);
        switch (randomAttack)
        {
            case 1:
                StartCoroutine(BossSlideToPlayer());
                hitBoxSize = new Vector2(1.5f, 1);
                soundMaker.BossMagicSound1();
                damageAmount = 3;
                staminaDamageAmount = 20;
                anim.SetInteger("Bossattack", randomAttack);

                Invoke("StopAttacks", 0.02f);
                break;
            case 2:
                StartCoroutine(BossSlideToPlayer());
                hitBoxSize = new Vector2(3, 1);
                soundMaker.BossMagicSound2();
                damageAmount = 3;
                staminaDamageAmount = 20;
                anim.SetInteger("Bossattack", randomAttack);
                Invoke("StopAttacks", 0.02f);
                break;
        }
    }


    public void StopAttacks()
    {
        anim.SetInteger("Bossattack", 0);
    }

    public void StartLookingAtPlayer()
    {
        if (forBossPlayer.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (forBossPlayer.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(hitBox.position, hitBoxSize);
    }

    IEnumerator BossSlideToPlayer()
    {
        Vector2 direction = forBossPlayer.position - transform.position;
        rb.AddForce(direction.normalized * slideForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(slideTime);
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.2f);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
