using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor { get; private set; }
    [SerializeField] private  SpriteRenderer _fillSpriteRenderer;
    [SerializeField] private Color[] _colors;



    public void SetDefaultColor(Color color)
    {
        DefaultColor = color;
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        _fillSpriteRenderer.color = color;
    }

    public void SetRandomColor()
    {
        int randomNum = Random.Range(0, _colors.Length);
        DefaultColor = _colors[randomNum];
        _fillSpriteRenderer.color = DefaultColor;
    }
}
