using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerAttacks playerAttacks;
    public InputField healthAmountInput;
    public InputField staminaAmountInput;
    public InputField strengthAmountInput;
    public SoundMaker soundMaker;
    public GameObject crest;


    public void HealthBuy()
    {
        if (!int.TryParse(healthAmountInput.text, out int healthAmount) || healthAmount <= 0)
        {
            Debug.LogWarning("Invalid health amount entered.");
            return;
        }
        int cost = healthAmount * 120;
        if (playerHealth.coinAmount < cost)
        {
            Debug.Log("Cannot buy: Not enough coins!");
            return;
        }
        if ((playerHealth.health + healthAmount) > 20)
        {
            Debug.Log("Cannot buy: This purchase would exceed the maximum limit of 20 health!");
            return;
        }
        soundMaker.ShopBuying();
        playerHealth.coinAmount -= cost;
        playerHealth.coinText.text = playerHealth.coinAmount.ToString();

        playerHealth.maxHealth += healthAmount;
        playerHealth.health = playerHealth.maxHealth;

        playerHealth.healthBar.maxValue = playerHealth.maxHealth;
        playerHealth.healthBar.value = playerHealth.health;

        PlayerPrefs.SetFloat("Coins", playerHealth.coinAmount);
        PlayerPrefs.SetFloat("CurrentMaxHealth", playerHealth.maxHealth);
        PlayerPrefs.SetFloat("CurrentHealth", playerHealth.health);
        PlayerPrefs.Save();
    }

    public void StaminaBuy()
    {
        if (!int.TryParse(staminaAmountInput.text, out int staminaAmount) || staminaAmount <= 0)
        {
            Debug.LogWarning("Invalid stamina amount entered.");
            return;
        }
        int cost = staminaAmount * 6;
        if (playerHealth.coinAmount < cost)
        {
            Debug.Log("Cannot buy: Not enough coins!");
            return;
        }
        if ((playerHealth.stamina + staminaAmount) > 200)
        {
            Debug.Log("Cannot buy: This purchase would exceed the maximum limit of 200 stamina!");
            return;
        }
        soundMaker.ShopBuying();
        playerHealth.coinAmount -= cost;
        playerHealth.coinText.text = playerHealth.coinAmount.ToString();

        playerHealth.maxStamina += staminaAmount;
        playerHealth.stamina = playerHealth.maxStamina;

        playerHealth.staminaBar.maxValue = playerHealth.maxStamina;
        playerHealth.staminaBar.value = playerHealth.stamina;

        PlayerPrefs.SetFloat("Coins", playerHealth.coinAmount);
        PlayerPrefs.SetFloat("CurrentMaxStamina", playerHealth.maxStamina);
        PlayerPrefs.SetFloat("CurrentStamina", playerHealth.stamina);
        PlayerPrefs.Save();
    }

    public void CrestBuy()
    {
        if (PlayerPrefs.GetInt("HasBoughtCrest", 0) == 1)
        {
            Debug.Log("You already bought the crest!");
            return;
        }

        float cost = 200;

        if (playerHealth.coinAmount < cost)
        {
            Debug.Log("Cannot buy: Not enough coins!");
            return;
        }
        if (soundMaker != null) soundMaker.ShopBuying();

        playerHealth.coinAmount -= cost;
        playerHealth.coinText.text = playerHealth.coinAmount.ToString();

        if (crest != null)
        {
            crest.SetActive(true);
        }

        PlayerPrefs.SetInt("HasBoughtCrest", 1);
        PlayerPrefs.SetFloat("Coins", playerHealth.coinAmount);
        PlayerPrefs.Save();
    }

    public void StrengthBuy()
    {
        if (!int.TryParse(strengthAmountInput.text, out int strengthAmount) || strengthAmount <= 0)
        {
            Debug.LogWarning("Invalid strength amount entered.");
            return;
        }

        if (playerAttacks.damageModifier >= 3)
        {
            Debug.Log("Cannot buy: Your strength is already at the maximum limit of 3!");
            return;
        }

        float cost = 220;

        if (playerHealth.coinAmount < cost)
        {
            Debug.Log("Cannot buy: Not enough coins for Strength upgrade!");
            return;
        }

        if (soundMaker != null) soundMaker.ShopBuying();
        playerHealth.coinAmount -= cost;
        playerHealth.coinText.text = playerHealth.coinAmount.ToString();

        playerAttacks.damageModifier += 1;
        int imageLevelIndex = Mathf.Clamp((int)playerAttacks.damageModifier, 0, 3);

        PlayerPrefs.SetFloat("Coins", playerHealth.coinAmount);
        PlayerPrefs.SetFloat("PlayerStrength", playerAttacks.damageModifier);
        PlayerPrefs.SetInt("SavedImageLevel", imageLevelIndex);
        PlayerPrefs.Save();

        soundMaker.LevelUpSound();

        if (playerHealth != null)
        {
            playerHealth.LoadAndApplySavedImage();
        }

        Debug.Log("Strength upgrade purchased and updated via PlayerHealth!");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth.maxHealth = PlayerPrefs.GetFloat("CurrentMaxHealth", 10);
        playerHealth.health = PlayerPrefs.GetFloat("CurrentHealth", 10);
        playerHealth.maxStamina = PlayerPrefs.GetFloat("CurrentMaxStamina", 100f);
        playerHealth.stamina = PlayerPrefs.GetFloat("CurrentStamina", 100f);
        playerHealth.coinText.text = playerHealth.coinAmount.ToString();
        playerHealth.healthBar.maxValue = playerHealth.maxHealth;
        playerHealth.healthBar.value = playerHealth.health;
        playerHealth.staminaBar.maxValue = playerHealth.maxStamina;
        playerHealth.staminaBar.value = playerHealth.stamina;

        if (PlayerPrefs.GetInt("HasBoughtCrest", 0) == 1 && PlayerPrefs.GetInt("HasCrestInInventory", 0) == 0)
        {
            if (crest != null)
            {
                crest.SetActive(true);
            }
        }
        else
        {
            if (crest != null)
            {
                crest.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
