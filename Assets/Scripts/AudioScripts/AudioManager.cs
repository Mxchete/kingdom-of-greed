using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource walkingSource;
    

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip walkingGrass;
    public AudioClip bossBattleMusic;
    public AudioClip StartRoomMusic;
    public AudioClip TownMusic;
    [Header("Weapon SFX")]
    public AudioClip swordBasic;
    public AudioClip axeBasic;
    public AudioClip hornClawBasic;

    [Header("Other SFX")]
    public AudioClip playerIsHitSFX;

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


    /*public void ChangeMusic(AudioClip newMusic)
    {
        if (musicSource.clip == newMusic) return;

        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.Play();
    }*/

    public void ChangeMusic(AudioClip newMusic)
    {
        StartCoroutine(FadeAndPlay(newMusic));
    }

    private IEnumerator FadeAndPlay(AudioClip newClip)
    {
        // Fade out
        for (float vol = 1f; vol >= 0f; vol -= Time.deltaTime)
        {
            musicSource.volume = vol;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        for (float vol = 0f; vol <= 1f; vol += Time.deltaTime)
        {
            musicSource.volume = vol;
            yield return null;
        }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "StartMenu":
                ChangeMusic(background);
                break;

            case "StartRoom":
                ChangeMusic(StartRoomMusic);
                break;

            case "BossRoom":
            case "BossRoomKingDom1":
                ChangeMusic(bossBattleMusic);
                break;
            case "Town":
                ChangeMusic(TownMusic);
                break;

        }
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
