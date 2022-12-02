using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashImage : MonoBehaviour
{
    private Image _image;
    [SerializeField] private float _flashTime = 0.2f;
    [Range(0,1)]
    [SerializeField] private float _maxAlpha = 1;
    private Coroutine _currentFlashCO = null;

    private void Awake()
    {
        if (_image == null)
            _image = GetComponent<Image>();
    }

    public void DoFlash(Color newColor)
    {
        _image.color = newColor;       

        if (_currentFlashCO != null)
            StopCoroutine(_currentFlashCO);

        _currentFlashCO = StartCoroutine(FlashCO(_flashTime, _maxAlpha));
    }

    private IEnumerator FlashCO(float flashTime, float maxAlpha)
    {
        // 1) ANIMATE FLASH IN

        float flashTimeHalf = flashTime * 0.5f;

        for (float t = 0; t <= flashTimeHalf; t += Time.deltaTime)
        {
            //GET CURRENT COLOR
            Color currentColor = _image.color;
            //GET NEW ALPHA
            currentColor.a = Mathf.Lerp(0, maxAlpha, t / flashTimeHalf);
            //APPLY NEW ALPHA TO CURRENT COLOR
            _image.color = currentColor;
            // WAIT UNTIL NEXT FRAME
            yield return null;
        }

        // 2) ANIMATE FLASH OUT      

        for (float t = 0; t < flashTimeHalf; t += Time.deltaTime)
        {
            //GET CURRENT COLOR
            Color currentColor = _image.color;
            //GET NEW ALPHA
            currentColor.a = Mathf.Lerp(maxAlpha, 0, t / flashTimeHalf);
            //APPLY NEW ALPHA TO CURRENT COLOR
            _image.color = currentColor;
            // WAIT UNTIL NEXT FRAME
            yield return null;
        }
    }

}
