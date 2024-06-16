using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image _healthBarFilling;
    [SerializeField] private HealhController _healthController; 
    private void Awake()
    {
        // _healthController = GetComponentInParent<HealhController>();
        _healthBarFilling = GetComponent<Image>();
        _healthController.HealthChanged += OnHealthChanged;
    }
    private void OnDestroy()
    {
        _healthController.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float value)
    {
        _healthBarFilling.fillAmount = value;
    }
}