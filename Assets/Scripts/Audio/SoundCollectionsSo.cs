using UnityEngine;

[CreateAssetMenu()]
public class SoundCollectionsSo : ScriptableObject
{
    [Header("Music")]
    public AudioSO[] FightMusic;
    public AudioSO[] DiscoBallParty;

    [Header("SFX")]
    public AudioSO[] GunShoot;
    public AudioSO[] Jump;
    public AudioSO[] Splat;
    public AudioSO[] Jetpack;
    public AudioSO[] GrenadeBeep;
    public AudioSO[] GrenadeShoot;
    public AudioSO[] GreandeExplode;

}
