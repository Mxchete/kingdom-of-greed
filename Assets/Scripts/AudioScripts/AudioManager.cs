using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource walkingSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip walkingGrass;

    [Header("Weapon SFX")]
    public AudioClip swordBasic;
    public AudioClip axeBasic;
    public AudioClip hornClawBasic;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();

    }

    public void PlaySFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }

    public void PlayWeaponSFX(Weapon.WeaponType type){
        AudioClip clipToPlay = null;

        switch(type){
            case Weapon.WeaponType.Sword:
                clipToPlay = swordBasic;
                break;
            case Weapon.WeaponType.Axe:
                clipToPlay = axeBasic;
                break;
            case Weapon.WeaponType.HornClaw:
                clipToPlay = hornClawBasic;
                break;
        }

        if(clipToPlay != null){
            SFXSource.PlayOneShot(clipToPlay);
        }
    }

    public void PlayWalkingLoop(){
        if(!walkingSource.isPlaying){
            walkingSource.clip = walkingGrass;
            walkingSource.loop = true;
            walkingSource.Play();
        }
    }

    public void StopWalkingLoop()
    {
        if (walkingSource.isPlaying)
        {
            walkingSource.Stop();
        }

    }
}
