using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    Enemy enemy;

    public GameObject pointLeft, pointRight;
    public float tpTime;
    public GameObject myArea;

    private CircleCollider2D areaCollider;
    private bool isPlayerInside = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();

        if (myArea != null)
        {
            areaCollider = myArea.GetComponent<CircleCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isInArea = false;

        if (myArea != null && areaCollider != null && enemy != null)
        {
            Vector2 trueWorldCircleCenter = myArea.transform.TransformPoint(areaCollider.offset);
            float distanceToCenter = Vector2.Distance(enemy.transform.position, trueWorldCircleCenter);
            float trueWorldRadius = areaCollider.radius * Mathf.Max(myArea.transform.lossyScale.x, myArea.transform.lossyScale.y);
            if (distanceToCenter <= trueWorldRadius)
            {
                isInArea = true;
            }
        }
        bool isSafe = isInArea || isPlayerInside;


        if (!isSafe)
        {
            enemy.enabled = false;
            enemy.anim.SetBool("run", false);
            tpTime += Time.deltaTime;
            if (tpTime > 3f)
            {
                TeleportEnemyHome();
                enemy.enabled = true;
                enemy.rb.bodyType = RigidbodyType2D.Dynamic;
                enemy.anim.SetBool("run", true);
            }
        }
        else
        {
            enemy.enabled = true;
            tpTime = 0f;
        }
    }

    private void TeleportEnemyHome()
    {
        if (enemy == null) return;

        enemy.transform.position = enemy.returningPoint.position;

        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            enemyRb.velocity = Vector2.zero;
        }

        enemy.moving = true;
        enemy.returning = false;

        if (pointLeft != null) pointLeft.SetActive(true);
        if (pointRight != null) pointRight.SetActive(true);

        tpTime = 0f;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;

            pointLeft.SetActive(false);
            pointRight.SetActive(false);
            enemy.returning = false;
            enemy.moving = false;
            if (enemy.anim != null) enemy.anim.SetBool("run", false);
            enemy.InvokeRepeating("Attack", 0f, 2f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;

            enemy.CancelInvoke("Attack");
            if (enemy.anim != null) enemy.anim.SetBool("run", true);

            if (enemy.transform.position.x < pointLeft.transform.position.x ||
                enemy.transform.position.x > pointRight.transform.position.x)
            {
                enemy.returning = true;
            }
            else if (enemy.transform.position.x >= pointLeft.transform.position.x &&
                     enemy.transform.position.x <= pointRight.transform.position.x)
            {
                enemy.moving = true;
                pointLeft.SetActive(true);
                pointRight.SetActive(true);
            }
        }
    }
}
