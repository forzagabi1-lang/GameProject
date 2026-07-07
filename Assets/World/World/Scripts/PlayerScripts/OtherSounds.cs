using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSounds : MonoBehaviour
{
    [Header("ChestSounds")]
    public AudioSource chestSounds;
    public AudioClip chestOpening;

    public void ChestOpeningSound()
    {
        chestSounds.PlayOneShot(chestOpening);
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
