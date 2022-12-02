using UnityEngine;

public class ColorSwitch : Switch, IColorSwitcheable
{                     
    [SerializeField] private SwitchColor _color;
    [SerializeField] private Renderer    _button;
                     private Material    _buttonMat;

    private void Awake()
    {
        _buttonMat = _button.material;
    }

    private void Start()
    {        
        SetColor();         
    }

    public SwitchColor GetColor()
    {
        return _color;
    }

    public void SetColor()
    {
        switch (_color)
        {
            case (SwitchColor.RED):
                _buttonMat.color = Color.red;
                break;
            case (SwitchColor.BLUE):
                _buttonMat.color = Color.blue;
                break;
            case (SwitchColor.YELLOW):
                _buttonMat.color = Color.yellow;
                break;
            default:
                _buttonMat.color = Color.white;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (CheckIfItsTheRightStatue(other.gameObject))
        {
            Toogle(true);                
            e_switchUpdate?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckIfItsTheRightStatue(other.gameObject))
        {
            Toogle(false);
            e_switchUpdate?.Invoke();
        }
    }

    private bool CheckIfItsTheRightStatue(GameObject thing)
    {
        bool coincidence;
        if (thing.TryGetComponent(out IColorSwitcheable statue))
        {
            SwitchColor statueColor = statue.GetColor();
            if(statueColor == _color)
               coincidence = true;
            
            else
               coincidence = false;            
        }
        else
           coincidence = false;        
        return coincidence;
        
    }

}
