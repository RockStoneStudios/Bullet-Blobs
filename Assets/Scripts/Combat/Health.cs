 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Health : MonoBehaviour
{
    public GameObject SplatterPrefab => _splatterPrefabs;
    public GameObject DeathVFX => _deathVFX;
    public static Action<Health> onDeath;

    [SerializeField] private int _startingHealth = 3;
    [SerializeField] private GameObject _splatterPrefabs;
    [SerializeField] private GameObject _deathVFX;

    private int _currentHealth;

    private void Start()
    {
        ResetHealth();
    }

   

    public void ResetHealth()
    {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            onDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

   
}






