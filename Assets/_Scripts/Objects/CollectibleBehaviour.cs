using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
[SerializeField] private float   _lifeTime        = 5f;
                 private float   _lifeTimeCurrent;
[SerializeField] private bool    _isAnimated      = false;

[SerializeField] private bool    _isRotating      = false;
[SerializeField] private bool    _isFloating      = false;
[SerializeField] private bool    _isScaling       = false;

[SerializeField] private Vector3 _rotationAngle;
[SerializeField] private float   _rotationSpeed;

[SerializeField] private float   _floatSpeed;
                 private bool    _isGoingUp         = true;
[SerializeField] private float   _floatRate;
                 private float   _floatTimer;

[SerializeField] private Vector3 _startScale;
[SerializeField] private Vector3 _endScale;

                 private bool    _isScalingUp       = true;
[SerializeField] float           _scaleSpeed;
[SerializeField] float           _scaleRate;
                 private float   _scaleTimer;

    private void Start()
    {
        _lifeTimeCurrent = _lifeTime;
    }

    void Update()
    {
        ItemLifeTime();

        if (_isAnimated)
        {
            if (_isRotating)
            {
                transform.Rotate(_rotationAngle * _rotationSpeed * Time.deltaTime);
            }

            if (_isFloating)
            {
                _floatTimer += Time.deltaTime;
                Vector3 moveDir = new Vector3(0.0f, _floatSpeed, 0.0f);
                transform.Translate(moveDir * Time.deltaTime);

                if (_isGoingUp && _floatTimer >= _floatRate)
                {
                    _isGoingUp = false;
                    _floatTimer = 0;
                    _floatSpeed = -_floatSpeed;
                }

                else if (!_isGoingUp && _floatTimer >= _floatRate)
                {
                    _isGoingUp = true;
                    _floatTimer = 0;
                    _floatSpeed = +_floatSpeed;
                }
            }

            if (_isScaling)
            {
                _scaleTimer += Time.deltaTime;

                if (_isScalingUp)                
                    transform.localScale = Vector3.Lerp(transform.localScale, _endScale, _scaleSpeed * Time.deltaTime);
                
                else if (!_isScalingUp)                
                    transform.localScale = Vector3.Lerp(transform.localScale, _startScale, _scaleSpeed * Time.deltaTime);
                

                if (_scaleTimer >= _scaleRate)
                {
                    if (_isScalingUp) { _isScalingUp = false; }
                    else if (!_isScalingUp) { _isScalingUp = true; }
                    _scaleTimer = 0;
                }
            }
        }
    }

    private void ItemLifeTime()
    {
        _lifeTimeCurrent -= Time.deltaTime;
        if (_lifeTimeCurrent <= 0)        
            Destroy(gameObject);
        
    }
}
