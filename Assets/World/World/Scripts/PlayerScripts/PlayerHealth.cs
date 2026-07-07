using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("CheckPoints")]
    public Transform[] checkpoints;


    [Header("Player Health")]
    public static bool isAlive;
    public float health;
    public float maxHealth;
    public Slider healthBar;
    public float stamina;
    public float maxStamina;
    public Slider staminaBar;
    public GameObject gameOver;
    public GameObject gameEnding;
    private float healTimer = 0;
    private float healDelay = 10;
    private float staminaHealTimer = 0;
    private float staminaHealDelay = 5;
    private float drownTime;

    [Header("Player Coin")]
    public float coinAmount;
    public Text coinText;

    [Header("Components")]
    public Animator anim;
    public PlayerMovementScript playerMovementScript;
    public CameraShake cameraShakeScript;
    public PlayerAttacks playerAttacksScript;
    public Enemy enemy;
    public bool blockHit;
    private bool isTakingAcidDamage = false;
    public SoundMaker soundMaker;
    public EnemyHealth enemyHealth;
    public BossHealth bossHealth;
    public PlayerSwimming playerSwimming;
    public Image powerUpImageDisplay;
    public Sprite[] powerUpSprites;
    public GameObject ForestMusic;

    Rigidbody2D rb;

    [Header("DoorOpeners")]
    public bool doorCanBeOpened = false;
    public bool doorCanBeOpened2 = false;
    public bool canOpenFinalDoor = false;
    public bool canOpenTrap = false;
    public GameObject door;


    public void Damage(float damageAmount, float staminaDamageAmount)
    {
        if (isAlive)
        {
            if (!playerAttacksScript.defending)
            {
                soundMaker.PlayerHurtSound();
                health -= damageAmount;
                healthBar.value = health;
                anim.SetTrigger("Hurt");
                healTimer = 0f;

                if (damageAmount == 2 & !playerMovementScript.stunned)
                {
                    playerMovementScript.stunned = true;
                    playerMovementScript.rb.velocity = Vector2.up * playerMovementScript.stunnedForce;
                    anim.SetBool("Stunned", true);
                    playerMovementScript.playerCol.sharedMaterial = playerMovementScript.bouncyMat;
                }

                if (health <= 0)
                {
                    isAlive = false;
                    anim.SetBool("Roll", false);
                    anim.SetBool("Stunned", false);
                    anim.SetBool("Death", true);
                    GetComponent<CapsuleCollider2D>().isTrigger = true;
                    playerMovementScript.rb.bodyType = RigidbodyType2D.Static;
                    playerAttacksScript.enabled = false;
                    gameOver.SetActive(true);
                    soundMaker.GameOverSound();
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
               
            }
            if (playerAttacksScript.defending)
            {
                  stamina -= staminaDamageAmount;
                  staminaBar.value = stamina;
                  anim.SetTrigger("BlockHit");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;

        if (PlayerPrefs.HasKey("Coins"))
        {
            coinAmount = PlayerPrefs.GetFloat("Coins");
            coinText.text = coinAmount.ToString();
        }

        if (PlayerPrefs.HasKey("CurrentMaxHealth"))
        {
            maxHealth = PlayerPrefs.GetFloat("CurrentMaxHealth");
            health = PlayerPrefs.GetFloat("CurrentHealth");
            if (health <= 0)
            {
                health = maxHealth;
            }

            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth;
                healthBar.value = health;
            }
        }

        if (PlayerPrefs.HasKey("CurrentMaxStamina"))
        {
            stamina = PlayerPrefs.GetFloat("CurrentStamina");
            maxStamina = PlayerPrefs.GetFloat("CurrentMaxStamina");
            if (stamina <= 0)
            {
                stamina = maxStamina;
            }

            if (staminaBar != null)
            {
                staminaBar.maxValue = maxStamina;
                staminaBar.value = stamina;
            }
        }

        playerMovementScript = GetComponent<PlayerMovementScript>();    

        if (PlayerPrefs.HasKey("HasCheckpoint") && PlayerPrefs.GetInt("HasCheckpoint") == 1)
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            Vector3 savedPosition = new Vector3(x, y, z);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.position = savedPosition;
            }
            transform.position = savedPosition;
        }
        if (PlayerPrefs.HasKey("HasCheckpoint"))
        {
            int checkpointindex = PlayerPrefs.GetInt("HasCheckpoint");
            for (int i = 1; i <= checkpointindex; i++)
            {
                checkpoints[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        maxHealth = health;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        maxStamina = stamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = stamina;

        if (PlayerPrefs.GetInt("HasCrestInInventory", 0) == 1)
        {
            doorCanBeOpened = true;
        }

        LoadAndApplySavedImage();
    }


    // Update is called once per frame
    void Update()
    {
        healTimer += Time.deltaTime;
        staminaHealTimer += Time.deltaTime;

        if (healTimer >= healDelay)
        {
            healthBar.value = Mathf.Min(healthBar.value + 1, maxHealth);
            health = healthBar.value;
            healTimer = 0;
        }

        if (staminaHealTimer >= staminaHealDelay)
        {
            stamina += 10;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            staminaBar.value = stamina;
            staminaHealTimer = 0;
        }

        if (PlayerPrefs.GetInt("DoorDestroyed", 0) == 1)
        {
            door.SetActive(false);
        }

        if(playerSwimming.inFloater | playerSwimming.inAntiFalling)
        {
            drownTime += Time.deltaTime;
            if(drownTime > 2)
            {
                health -= 1;
                if(health > 0)
                {
                    soundMaker.PlayerHurtSound();
                    soundMaker.DrowningSound();
                }
                drownTime = 0;
                healTimer = 0;
                healthBar.value = health;

                if (health <= 0)
                {
                    isAlive = false;
                    anim.SetBool("Roll", false);
                    anim.SetBool("Stunned", false);
                    anim.SetBool("Death", true);
                    GetComponent<CapsuleCollider2D>().isTrigger = true;
                    playerMovementScript.rb.bodyType = RigidbodyType2D.Static;
                    playerAttacksScript.enabled = false;
                    gameOver.SetActive(true);
                    soundMaker.GameOverSound();
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "coin" & playerMovementScript.isGrounded())
        {
            soundMaker.CoinPickUpSound();
            coinAmount += Random.Range(1, 26);
            coinText.text = coinAmount.ToString();
            PlayerPrefs.SetFloat("Coins", coinAmount);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Door" & doorCanBeOpened)
        {
            Destroy(collision.gameObject, 0.5f);
            soundMaker.DoorBreakSound();
            
        }

        if (collision.gameObject.tag == "Door2" & doorCanBeOpened2)
        {
            Destroy(collision.gameObject, 0.5f);
            soundMaker.DoorBreakSound();
            PlayerPrefs.SetInt("DoorDestroyed", 1);
            PlayerPrefs.Save();
        }

        if(collision.gameObject.name == "FinalTrap" & canOpenTrap)
        {
            Destroy(collision.gameObject, 2);
            soundMaker.TrapBreakSound();
            anim.SetBool("Fall", false);
        }

        if(collision.gameObject.tag == "Trap")
        {
            Destroy(collision.gameObject, 1);
            soundMaker.TrapBreakSound();
            anim.SetBool("Fall", false);
        }

        if (collision.gameObject.name == "FinalDoor" & canOpenFinalDoor)
        {
            Destroy(collision.gameObject, 0.5f);
            soundMaker.DoorBreakSound();
        }
    }

    public void LoadAndApplySavedImage()
    {
        if (powerUpImageDisplay != null && powerUpSprites != null)
        {
            int savedLevel = PlayerPrefs.GetInt("SavedImageLevel", 0);

            if (savedLevel >= 0 && savedLevel < powerUpSprites.Length)
            {
                powerUpImageDisplay.sprite = powerUpSprites[savedLevel];
            }
        }
    }

    private void ApplyAcidDamage()
    {
        if (!isAlive)
        {
            StopAcidDamage();
            return;
        }

        health -= 1;
        anim.SetTrigger("Hurt");
        soundMaker.PlayerHurtSound();
        healTimer = 0f;
        healthBar.value = health;

        if (health <= 0)
        {
            isAlive = false;
            anim.SetBool("Roll", false);
            anim.SetBool("Stunned", false);
            anim.SetBool("Death", true);
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            playerMovementScript.rb.bodyType = RigidbodyType2D.Static;
            playerAttacksScript.enabled = false;
            soundMaker.GameOverSound();
            gameOver.SetActive(true);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            StopAcidDamage();
        }
    }

    private void StopAcidDamage()
    {
        CancelInvoke("ApplyAcidDamage");
        isTakingAcidDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "chest")
        {
            Chest chestScript = collision.GetComponent<Chest>();
            if (!chestScript.IsOpened)
            {
                chestScript.Open();
                Transform coin = collision.transform.GetChild(1);
                coin.gameObject.SetActive(true);

                Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
                coinRb.velocity = Vector2.up * 15;
            }
        }

        if(collision.gameObject.name == "Ending" & bossHealth.bosshealth <= 0)
        {
            gameEnding.SetActive(true);
            playerMovementScript.enabled = false;
            playerAttacksScript.enabled = false;
            ForestMusic.SetActive(false);
        }

        if (collision.gameObject.name == "AcidicWater" && !isTakingAcidDamage)
        {
            isTakingAcidDamage = true;
            InvokeRepeating("ApplyAcidDamage", 0f, 1);
        }

        if (collision.gameObject.tag == "Spike")
        {
            health -= 1;
            healthBar.value = health;
            soundMaker.PlayerHurtSound();
            anim.SetTrigger("Hurt");
            healTimer = 0f;
            StartCoroutine(cameraShakeScript.ShakeCamera(3, 1));
            if (health <= 0)
            {
                isAlive = false;
                anim.SetBool("Roll", false);
                anim.SetBool("Stunned", false);
                anim.SetBool("Death", true);
                playerAttacksScript.enabled = false;
                GetComponent<CapsuleCollider2D>().isTrigger = true;
                playerMovementScript.rb.bodyType = RigidbodyType2D.Static;
                gameOver.SetActive(true);
                soundMaker.GameOverSound();
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        if (collision.gameObject.name == "DoorOpener")
        {
            Destroy(collision.gameObject, 2f);
            doorCanBeOpened = true;
            soundMaker.CrestDissapear();
        }

        if (collision.gameObject.name == "DoorOpener2")
        {
            Destroy(collision.gameObject, 2);
            doorCanBeOpened2 = true;
            soundMaker.CrestDissapear();
        }

        if (collision.gameObject.CompareTag("CheckPoint"))
        {
            foreach (Transform child in collision.transform)
            {
                child.gameObject.SetActive(true);
            }
            Vector3 playerPosition = transform.position;
            PlayerPrefs.SetFloat("CheckpointX", playerPosition.x);
            PlayerPrefs.SetFloat("CheckpointY", playerPosition.y);
            PlayerPrefs.SetFloat("CheckpointZ", playerPosition.z);
            PlayerPrefs.SetInt("HasCheckpoint", 1);
            PlayerPrefs.Save();
        }

        if(collision.gameObject.name == "TrapOpener")
        {
            Destroy (collision.gameObject, 2);
            canOpenTrap = true;
            soundMaker.CrestDissapear();
        }

        if (collision.gameObject.name == "FinalDoorOpener")
        {
            Destroy(collision.gameObject, 2);
            canOpenFinalDoor = true;
            soundMaker.CrestDissapear();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AcidicWater")
        {
            StopAcidDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (playerMovementScript == null)
        {
            Debug.LogError("playerMovementScript is missing on " + gameObject.name);
            return;
        }
        if (playerMovementScript.isGrounded() && health <= 0)
        {
            playerMovementScript.rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
