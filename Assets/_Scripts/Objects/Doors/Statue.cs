using UnityEngine;
public enum SwitchColor
{
    RED,
    BLUE,
    YELLOW,
    WHITE,
}
public class Statue : PickableObject, IColorSwitcheable
{
    [Header("Statue Color")] 
    [SerializeField] private SwitchColor currentColor;    
                     private Renderer    _renderer;

    protected override void Awake()
    {
        base.Awake();       
        _renderer = this.GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        SetColor();
    }

    private void OnDestroy()
    {
        Destroy(_renderer.material);
    }

    public void SetColor()
    {
        switch (currentColor)
        {
            case (SwitchColor.RED):
                _renderer.material.color = Color.red;
                break;
            case (SwitchColor.BLUE):
                _renderer.material.color = Color.blue;
                break;
            case (SwitchColor.YELLOW):
                _renderer.material.color = Color.yellow;
                break;
            default:
                _renderer.material.color = Color.white;
                break;
        }
    }

    public SwitchColor GetColor()
    {
        return currentColor;
    }

}
