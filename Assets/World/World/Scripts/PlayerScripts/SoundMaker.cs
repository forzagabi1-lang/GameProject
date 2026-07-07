using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    [Header("PlayerSounds")]
    public AudioSource playerSounds;
    public AudioClip playerSwordSwing;
    public AudioClip playerHurt;
    public AudioClip coinPickUp;
    public AudioClip jumpSound;
    public AudioClip gameOver;
    public AudioClip levelUp;

    public AudioClip drowningSound;

    [Header("TrapSounds")]
    public AudioSource trapSounds;
    public AudioClip trapBreak;

    [Header("DoorSounds")]
    public AudioSource doorsounds;
    public AudioClip doorBreak;

    [Header("CrestSounds")]
    public AudioSource crestSounds;
    public AudioClip crestDissapear;

    [Header("ShopSounds")]
    public AudioSource shopSounds;
    public AudioClip shopBuy;

    [Header("EnemySounds")]
    public AudioSource enemySounds;
    public AudioClip enemySwordSwing;
    public AudioClip enemyHeavySwordSwing;

    [Header("BossSounds")]
    public AudioSource bossSounds;
    public AudioClip bossMagicSound1;
    public AudioClip bossMagicSound2;
    public AudioClip bossHurt;

    [Header("OtherPlaceMusics")]
    public AudioSource natureSounds;
    public AudioClip ForestMusic;
    public AudioClip dungeonMusic;
    public AudioClip bossFightMusic;


    public void TrapBreakSound()
    {
        trapSounds.PlayOneShot(trapBreak);
        trapSounds.pitch = Random.Range(0.8f, 1.2f);
    }

    public void GameOverSound()
    {
        playerSounds.PlayOneShot(gameOver);
        playerSounds.pitch = 1;
    }

    public void LevelUpSound()
    {
        playerSounds.PlayOneShot(levelUp);
    }

    public void DrowningSound()
    {
        playerSounds.PlayOneShot(drowningSound);
        playerSounds.pitch = Random.Range(0.8f, 1.2f);
    }

    public void PlayerSwingSound()
    {
        playerSounds.PlayOneShot(playerSwordSwing);
        playerSounds.pitch = Random.Range(0.8f, 1.4f);
    }

    public void PlayerHurtSound()
    {
        playerSounds.PlayOneShot(playerHurt);
        playerSounds.pitch = Random.Range(1, 1.2f);
    }
    public void PlayerJumpSound()
    {
        playerSounds.PlayOneShot(jumpSound);
        playerSounds.pitch = Random.Range(0.6f, 0.8f);
        playerSounds.volume = 0.5f;
    }

    public void CrestDissapear()
    {
        crestSounds.PlayOneShot(crestDissapear);
    }

    public void DoorBreakSound()
    {
        doorsounds.PlayOneShot(doorBreak);
    }

    public void CoinPickUpSound()
    {
        playerSounds.PlayOneShot(coinPickUp);
        playerSounds.pitch = Random.Range(1, 1.3f);
    }

    public void ShopBuying()
    {
        shopSounds.PlayOneShot(shopBuy);
    }

    public void BossMagicSound1()
    {
        bossSounds.PlayOneShot(bossMagicSound1);
        bossSounds.pitch = Random.Range(0.9f, 1.1f);
    }
    public void BossMagicSound2()
    {
        bossSounds.PlayOneShot(bossMagicSound2);
        bossSounds.pitch = Random.Range(0.6f, 1);
    }

    public void BossHurtSound()
    {
        bossSounds.PlayOneShot(bossHurt);
        bossSounds.pitch = Random.Range(0.7f, 1.2f);
    }


    public void EnemySwordSwing()
    {
        enemySounds.PlayOneShot(enemySwordSwing);
        enemySounds.pitch = Random.Range(0.8f, 1.4f);
    }

    public void EnemyHeavySwordSwing()
    {
        enemySounds.PlayOneShot(enemyHeavySwordSwing);
        enemySounds.pitch = Random.Range(0.8f, 1.2f);
    }

    public void PlayForestMusic()
    {
        natureSounds.PlayOneShot(ForestMusic);
    }

    public void PlayDungeonMusic()
    {
        natureSounds.PlayOneShot(dungeonMusic);
    }

    public void PlayBossFightMusic()
    {
        natureSounds.PlayOneShot(bossFightMusic);
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
