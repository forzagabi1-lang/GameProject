using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public PlayerAttacks attacks;
    public float bosshealth;
    public Animator anim;
    public Boss bossScript;
    public GameObject finalDoorOpener;
    public DetectPlayerByBoss detectPlayerByBoss;
    public SoundMaker soundMaker;
    public GameObject finalLadders;

    public void Damage(float damageAmount)
    {
        bosshealth -= damageAmount;
        anim.SetTrigger("Bosstakehit");
        soundMaker.BossHurtSound();

        if (bosshealth <= 0)
        {
            anim.SetBool("Bossdeath", true);
            GetComponent<CapsuleCollider2D>().enabled = false;
            bossScript.rb.bodyType = RigidbodyType2D.Static;
            bossScript.enabled = false;
            bossScript.StopAllCoroutines();
            finalDoorOpener.SetActive(true);
            finalLadders.SetActive(true);
            detectPlayerByBoss.blockedDoors.SetActive(false);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
