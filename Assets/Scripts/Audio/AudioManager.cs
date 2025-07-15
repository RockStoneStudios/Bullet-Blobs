using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Range(0f, 2f)]
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundCollectionsSo _soundsCollections;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private AudioSource _currentMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #region Unity Methods
    void Start()
    {
        FightMusic();
    }

    void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        PlayerController.OnJetpack += PlayerController_OnJetpack;
        Health.onDeath += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent += DiscoBallMusic;
        Gun.OnGranadeShoot += Gun_OnGrenadeShoot;
    }

    void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        PlayerController.OnJetpack -= PlayerController_OnJetpack;
        Health.onDeath -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent -= DiscoBallMusic;
        Gun.OnGranadeShoot -= Gun_OnGrenadeShoot;
    }

    #endregion


    #region Sound Methods

    private void SoundToPlay(AudioSO soundSo)
    {
        AudioClip clip = soundSo.Clip;
        float pitch = soundSo.Pitch;
        float volume = soundSo.Volume * _masterVolume;
        bool loop = soundSo.Loop;

        AudioMixerGroup audioMixerGroup;

        if (soundSo.RandomizePitch)
        {
            float randomizePitch = Random.Range(-soundSo.RandomPitchRangeModifier, soundSo.RandomPitchRangeModifier);
            pitch = soundSo.Pitch + randomizePitch;
        }

        switch (soundSo.AudioType)
        {
            case AudioSO.AudioTypes.SFX:
                audioMixerGroup = _sfxMixerGroup;
                break;
            case AudioSO.AudioTypes.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }

    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();

        if (!loop)
        {
            Destroy(soundObject, clip.length);
        }

        if (audioMixerGroup == _musicMixerGroup)
        {
            if (_currentMusic != null)
            {
                _currentMusic.Stop();
            }
            _currentMusic = audioSource;
        }
    }
    private void PlayRandomSound(AudioSO[] sounds)
    {
        if (sounds != null && sounds.Length > 0)
        {
            AudioSO audioSO = sounds[Random.Range(0, sounds.Length)];

        }
    }
    #endregion


    #region SFX

    private void Gun_OnShoot()
    {
        PlayRandomSound(_soundsCollections.GunShoot);
    }

    private void PlayerController_OnJump()
    {
        PlayRandomSound(_soundsCollections.Jump);
    }

    private void Health_OnDeath(Health health)
    {
        PlayRandomSound(_soundsCollections.Splat);
    }

    private void PlayerController_OnJetpack()
    {
        PlayRandomSound(_soundsCollections.Jetpack);
    }

    public void Grenade_OnBeep()
    {
        PlayRandomSound(_soundsCollections.GrenadeBeep);
    }

    public void Grenade_OnExplode()
    {
        PlayRandomSound(_soundsCollections.GreandeExplode);
    }

     private void Gun_OnGrenadeShoot()
    {
        PlayRandomSound(_soundsCollections.GrenadeShoot);
    }


    

    #endregion

    #region  Music

    private void FightMusic()
    {
        PlayRandomSound(_soundsCollections.FightMusic);
    }

        private void DiscoBallMusic()
        {
            PlayRandomSound(_soundsCollections.DiscoBallParty);
            float soundLength = _soundsCollections.DiscoBallParty[0].Clip.length;
            Utils.RunAfterDelay(this, soundLength,FightMusic);
        }
    
    #endregion
}
