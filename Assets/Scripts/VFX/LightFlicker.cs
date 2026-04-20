using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    [SerializeField] private float _flickerIntensity = 0.5f;
    [SerializeField] private float _flickerSpeed = 5f;
    [SerializeField] private float _minFlickerTime = 0.75f;
    [SerializeField] private float _maxFlickerTime = 1.5f;

    private float _originalIntensity;
    private float _intensity;
    private float _targetIntensity;
    private float _flickerTime;
    private float _nextFlicker;

    private void Start()
    {
        _originalIntensity = _light.intensity;
        _intensity = _originalIntensity;
        _targetIntensity = _originalIntensity;
        _flickerTime = Random.Range(_minFlickerTime, _maxFlickerTime);
        _nextFlicker = _flickerTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_nextFlicker <= 0f)
        {
            _intensity = MathFunctions.Flicker(_light.intensity, -_flickerIntensity, _flickerIntensity);
            _light.intensity = _intensity;
            _flickerTime = Random.Range(_minFlickerTime, _maxFlickerTime);
            _nextFlicker = _flickerTime;
        }
        else
        {
            _nextFlicker -= Time.deltaTime;
        }
        _intensity = Mathf.Lerp(_intensity, _originalIntensity, Time.deltaTime * _flickerSpeed);
        _light.intensity = _intensity;
    }
}
