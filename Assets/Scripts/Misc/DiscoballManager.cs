using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    public static Action OnDiscoBallHitEvent;

    [SerializeField] private float _discoBallPartyTime = 2f;
    [SerializeField] private float _discoGlobalLightIntensity = .2f;
    [SerializeField] private Light2D _globalLight;

    private float _defaultGlobalLightIntensity;
    private Coroutine _discoCoroutine;
    private ColorSpothLight[] _allSpotlights;

    private void Awake() {
        _defaultGlobalLightIntensity = _globalLight.intensity;
    }

    private void Start() {
        _allSpotlights = FindObjectsByType<ColorSpothLight>(FindObjectsSortMode.None);
    }

    private void OnEnable() {
        OnDiscoBallHitEvent += DimTheLights;
    }

    private void OnDisable() {
        OnDiscoBallHitEvent -= DimTheLights;
    }

    public void DiscoBallParty() {
        Debug.Log("la vaca muuuuu");
        if (_discoCoroutine != null) { return; }

        OnDiscoBallHitEvent?.Invoke();
    }

    private void DimTheLights() {
        foreach (ColorSpothLight spotLight in _allSpotlights)
        {
            StartCoroutine(spotLight.SpothLightDiscoParty(_discoBallPartyTime));
        }

        _discoCoroutine = StartCoroutine(GlobalLightResetRoutine());
    }

    private IEnumerator GlobalLightResetRoutine() {
        _globalLight.intensity = _discoGlobalLightIntensity;
        yield return new WaitForSeconds(_discoBallPartyTime);
        _globalLight.intensity = _defaultGlobalLightIntensity;
        _discoCoroutine = null;
    }
}
