using UnityEngine;
public enum WaveType
{
    Sin,
    Triangle,
    Square,
    Saw,
    Inverted,
    Noise,
}

public class LightFlicker : MonoBehaviour
{
    //Components
    private Light _light;
    private Color _originalColor;
    [Header("WAVE PARAMETERS")]
    [SerializeField] private WaveType _waveFunction = WaveType.Sin;
    [SerializeField] private float _startValue      = 0.0f; // start
    [SerializeField] private float _amplitude       = 1.0f; // amplitude of the wave
    [SerializeField] private float _phase           = 0.0f; // start point inside on wave cycle
    [SerializeField] private float _frequency       = 0.5f; // cycle frequency per second


    private void Awake()
    {
        if (_light == null)
        {
            if (this.TryGetComponent(out Light light))
                _light = light;
        }

        _originalColor = _light.color;
    }
    private void Update()
    {
        
        _light.color = _originalColor * (SetWave());
    }

    private float SetWave()
    {
        float x = (Time.time + _phase) * _frequency;
        float y;

        x = x - Mathf.Floor(x); // normalized value

        switch (_waveFunction)
        {
            case WaveType.Sin:
                y = Mathf.Sin(x * 2 * Mathf.PI);
                break;
            case WaveType.Triangle:
                if (x < 0.5f)
                    y = 4.0f * x - 1.0f;
                else
                    y = -4.0f * x + 3.0f;
                break;
            case WaveType.Square:
                if (x < 0.5f)
                    y = 1.0f;
                else
                    y = -1.0f;
                break;
            case WaveType.Saw:
                y = x;
                break;
            case WaveType.Inverted:
                y = 1.0f - x;
                break;
            case WaveType.Noise:
                y = 1 - (Random.value * 2);
                break;
            default:
                y = 1.0f;
                break;
        }
        return (y * _amplitude) + _startValue;
    }
}
