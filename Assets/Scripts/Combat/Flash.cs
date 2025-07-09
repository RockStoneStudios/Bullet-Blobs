using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    [SerializeField] private float _flashTime = .12f;


    private SpriteRenderer[] _spriteRenderers;
    private ColorChanger _colorChanger;

    void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void StartFlash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.material = _whiteFlashMaterial;
            if (_colorChanger)
            {
                _colorChanger.SetColor(Color.white);
            }
        }
        yield return new WaitForSeconds(_flashTime);
        SetDefaultMaterial();
    }

    private void SetDefaultMaterial()
    {
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.material = _defaultMaterial;
            if (_colorChanger)
            {
                _colorChanger.SetColor(_colorChanger.DefaultColor);
            }
        }
    }

}
