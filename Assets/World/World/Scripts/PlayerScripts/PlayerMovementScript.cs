using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpForce;
    public bool stunned;
    public float stunnedForce;
    float moveX;
    private bool wasGrounded;
    bool canJump = true;

    [Header("Roll")]
    public float rollForce;
    public float rollTime;
    public bool rolling, canRoll = true;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;
    public PhysicsMaterial2D physicsMat;
    public PhysicsMaterial2D bouncyMat;
    public Collider2D playerCol;
    public PlayerAttacks playerAttacksScript;
    public SoundMaker soundMaker;

    [Header("Checking Ground")]
    public Transform groundChecking;
    public float groundCheckingDistance;
    public LayerMask groundLayer;

    [Header("Interaction")]
    public GameObject shopPanel;
    bool nearShop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void PlayerMove()
    {
        bool grounded = isGrounded();
        moveX = Input.GetAxisRaw("Horizontal");
        if (!rolling)
        {
            rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
        }

        if (moveX > 0)
        {
            anim.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveX < 0)
        {
            anim.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump && isGrounded())
        {
             rb.velocity = new Vector2(rb.velocity.x, jumpForce);
             anim.SetBool("Jump", true);
             soundMaker.PlayerJumpSound();
        }

        if (rb.velocity.y < 0)
        {
             anim.SetBool("Fall", true);
             anim.SetBool("Jump", false);
             if (isGrounded())
             {
                 anim.SetBool("Fall", false);
                 anim.SetBool("Jump", false);
             }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll && isGrounded())
        {
             StartCoroutine(PlayerRoll());
        }


        Debug.DrawRay(groundChecking.position, Vector2.down, Color.green, 0.01f);
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E) & nearShop)
        {
            if (!shopPanel.activeSelf)
            {
                shopPanel.SetActive(true);
            }
            else
            {
                shopPanel.SetActive(false);
            }

        }
    }


    IEnumerator PlayerRoll()
    {
        playerAttacksScript.canDefend = false;
        canRoll = false;
        canJump = false;
        rolling = true;
        anim.SetBool("Roll", true);
        rb.velocity = transform.right * rollForce;
        yield return null;
        float coolDown = anim.GetCurrentAnimatorStateInfo(0).length;
        rollTime = coolDown;
        yield return new WaitForSeconds(rollTime);
        rolling = false;
        anim.SetBool("Roll", false);
        canRoll = true;
        canJump = true;
        playerAttacksScript.canDefend = true;
    }


    public bool isGrounded()
    {
        float extraBuffer = 0.1f;
        if (Physics2D.Raycast(groundChecking.position, Vector2.down, groundCheckingDistance + extraBuffer, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth.isAlive & !stunned)
        {
            PlayerMove();
            PlayerInput();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Shop")
        {
            nearShop = true;
            playerAttacksScript.enabled = false;
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Shop")
        {
            nearShop = false;
            shopPanel.SetActive(false);
            playerAttacksScript.enabled = true;
        }
    }
    IEnumerator BounceAfterStunned()
    {
        yield return new WaitForSeconds(0.7f);
        playerCol.sharedMaterial = physicsMat;
        stunned = false;
        anim.SetBool("Stunned", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" & stunned)
        {
            StartCoroutine(BounceAfterStunned());
        }
    }
}
