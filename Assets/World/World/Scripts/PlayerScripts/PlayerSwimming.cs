using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwimming : MonoBehaviour
{
    public bool swimming;
    public Animator anim;
    public float swimSpeed;
    public PlayerMovementScript playerMovementScript;
    public PlayerAttacks playerAttacks;
    public SoundMaker soundMaker;
    public Rigidbody2D rb;

    float swimX;
    float swimY;
    float originalGravity;

    public bool inFloater = false;
    public bool inAntiFalling = false;

    public void PlayerSwim()
    {
        swimX = Input.GetAxisRaw("Horizontal");
        swimY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(swimX * swimSpeed, swimY * swimSpeed);
        if (swimX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (swimX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Floater")
        {
            inFloater = true;
            swimming = true;
            anim.SetBool("Swimming", true);
            rb.gravityScale = 0;
            anim.Play("Swimming");
        }

        if (collision.name == "AntiFalling")
        {
            inAntiFalling = true;
            anim.SetBool("Swimming", true);
            anim.Play("Swimming");
        }

        if (inFloater || inAntiFalling)
        {
            playerMovementScript.enabled = false;
            playerAttacks.enabled = false;
            anim.SetBool("Run", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Floater")
        {
            inFloater = false;
            swimming = false;
            rb.gravityScale = originalGravity;
        }

        if (collision.name == "AntiFalling")
        {
            inAntiFalling = false;
            anim.SetBool("Swimming", false);
        }

        if (!inFloater && !inAntiFalling)
        {
            playerMovementScript.enabled = true;
            playerAttacks.enabled = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (rb != null) originalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (swimming)
        {
            PlayerSwim();
            anim.SetBool("Fall", false);
            anim.SetBool("Run", false);
        }
    }
}
