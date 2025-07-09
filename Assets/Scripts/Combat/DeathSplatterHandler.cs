using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{

    void OnEnable()
    {
        Health.onDeath += SpawnDeathSplatterPrefab;
        Health.onDeath += SpawnDeathVFX;
    }

    void OnDisable()
    {
        Health.onDeath -= SpawnDeathSplatterPrefab;
        Health.onDeath -= SpawnDeathVFX;
    }

    private void SpawnDeathSplatterPrefab(Health sender)
    {
        GameObject newSplaterrPrefab = Instantiate(sender.SplatterPrefab, sender.transform.position, transform.rotation);
        SpriteRenderer deathSplatterSpriteRenderer = newSplaterrPrefab.GetComponent<SpriteRenderer>();
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        Color currentColor = colorChanger.DefaultColor;
        deathSplatterSpriteRenderer.color = currentColor;
        newSplaterrPrefab.transform.SetParent(this.transform);
    }

   private void SpawnDeathVFX(Health sender)
    {
        GameObject deathVFX = Instantiate(sender.DeathVFX, sender.transform.position, transform.rotation);
        ParticleSystem.MainModule ps = deathVFX.GetComponent<ParticleSystem>().main;
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        Color currentColor = colorChanger.DefaultColor;
        ps.startColor = currentColor;
        deathVFX.transform.SetParent(this.transform);
    }

}