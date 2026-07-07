using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerByBoss : MonoBehaviour
{
    Boss boss;
    public GameObject blockedDoors;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            boss.InvokeRepeating("BossAttacks", 0, 2);
            blockedDoors.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        boss.CancelInvoke("BossAttacks");
    }
}
