using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Health : MonoBehaviour, IHealth
{
    public int HealthAmount { get; }

    [SerializeField, Tooltip("Количество жизней на уровне"), Range(1, 20)] 
    private int _health = 6;
    
    [SerializeField, Tooltip("Количество очков, отнимаемое после пропуска шара"), Range(1, 5)] 
    private int _damage = 1;
    [SerializeField] private Text _currentCounterOne;
    [SerializeField] private Text _currentCounterTwo;


    private void Awake()
    {
        EventManager.ShortenLife.AddListener(SkipBall);
        _currentCounterOne.text = _health.ToString();
        _currentCounterTwo.text = _health.ToString();
    }

    public Health()
    {
        HealthAmount = _health;
    }

    private void SkipBall()
    {
        _health -= _damage;
        Debug.Log($"У тебя осталось: {_health} жизней");
        _currentCounterOne.text = _health.ToString(); 
        _currentCounterTwo.text = _health.ToString();


        if (_health <= 0)
        {
            Debug.Log("Игра закончена");
            Time.timeScale = 0;
        }
    }
    

    private void OnDisable()
    {
        EventManager.ShortenLife.RemoveListener(SkipBall);
    }
}