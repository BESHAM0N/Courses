using System;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class Timer : MonoBehaviour
{
    public float timeStart = 10f;
    
    [SerializeField] private TMP_Text _timerText;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _timerText.text = timeStart.ToString();
    }

    private void Update()
    {
        Calculator();
    }

    private void Calculator()
    {
        timeStart -= Time.deltaTime;
        _timerText.text = Math.Round(timeStart).ToString();

        if (timeStart <= 0)
        {
            _playerHealth.DecreaseHealth();
            Debug.Log("Время закончилось");
            timeStart = 10;
        }
    }
}
