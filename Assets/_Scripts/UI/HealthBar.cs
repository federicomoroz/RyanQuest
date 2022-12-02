using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("View Components")]
    [SerializeField] protected Slider   _slider;
    [SerializeField] protected Gradient _gradient;
    [SerializeField] protected Image    _fill;

    [Header("Bar parameters")]
    [SerializeField] protected float    _updateSpeedSeconds = 0.5f;

    protected virtual void HandleHpChange(params object[] parameters)
    {
        StartCoroutine(SetSlider((float)parameters[0]));
    }

    protected IEnumerator SetSlider(float ratio)
    {
        float preValue = _slider.value;
        float elapsed = 0f;

        while (elapsed < _updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            _slider.value = Mathf.Lerp(preValue, ratio, elapsed / _updateSpeedSeconds);
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
            yield return null;
        }

        _slider.value = ratio;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}

