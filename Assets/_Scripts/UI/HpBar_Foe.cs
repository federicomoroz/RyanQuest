using System.Collections;
using UnityEngine;

public class HpBar_Foe : HealthBar
{
                     private Foe       _foe;
                     private Transform _cam;
    [SerializeField] private float     _timeShown = 1.5f;
                     private Vector3   _currentScale;

    private void Awake()
    {
        _foe          = GetComponentInParent<Foe>();
        _cam          = GameObject.Find("MainCamera").transform;
        _currentScale = this.transform.localScale;

        this.transform.localScale = Vector3.zero;


    }
    private void OnEnable()
    {
        _foe.e_OnHealthUpdate += HpUpdate;
    }

    private void OnDisable()
    {
        _foe.e_OnHealthUpdate -= HpUpdate;
    }

    private void LateUpdate()
    {
        transform.LookAt(_cam);
        transform.Rotate(0, 180, 0);
    }
    private void HpUpdate(float value)
    {
        HandleHpChange(value);
        StartCoroutine(BarShownCO());
    }    

    private IEnumerator BarShownCO()
    {

        this.transform.localScale = _currentScale;
        float elapsed = 0f;

        while (elapsed < _timeShown)
        {
            elapsed += Time.deltaTime;           
            yield return null;
        }
        this.transform.localScale = Vector3.zero;
    }
}
