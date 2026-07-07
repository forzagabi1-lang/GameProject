using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("PlayerSound")]
    public AudioSource playerSounds;
    public AudioClip walkSound;
    public AudioClip blockAttackSound;

    public void WalkingSound()
    {
        playerSounds.PlayOneShot(walkSound);
        playerSounds.pitch = Random.Range(0.7f, 0.9f);
    }

    public void PlayerShieldBlockSound()
    {
        playerSounds.PlayOneShot(blockAttackSound);
        playerSounds.pitch = Random.Range(0.6f, 0.8f);
        playerSounds.volume = 0.5f;
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
