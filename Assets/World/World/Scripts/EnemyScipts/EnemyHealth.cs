using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public PlayerAttacks attacks;
    public float health;
    public Animator anim;
    public Enemy enemyScript;
    public PlayerHealth playerHealth;
    public SoundMaker soundMaker;
    float destroyTime;

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        anim.SetTrigger("takehit");

        if (health <= 0)
        {
            anim.SetBool("death", true);
            GetComponent<CapsuleCollider2D>().enabled = false;
            enemyScript.rb.bodyType = RigidbodyType2D.Static;
            enemyScript.moving = false;
            enemyScript.enabled = false;
            enemyScript.StopAllCoroutines();
            GetCoinsAfterDeath();
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void GetCoinsAfterDeath()
    {
        soundMaker.CoinPickUpSound();
        playerHealth.coinAmount += Random.Range(1, 26);
        playerHealth.coinText.text = playerHealth.coinAmount.ToString();
        PlayerPrefs.SetFloat("Coins", playerHealth.coinAmount);
    }

    // Start is called before the first frame update
    void Start()
    {
        attacks = GetComponent<PlayerAttacks>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
