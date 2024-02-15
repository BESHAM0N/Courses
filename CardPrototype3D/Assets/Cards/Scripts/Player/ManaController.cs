using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ManaController : MonoBehaviour
{
    // [SerializeField] private GameObject[] _manaOnePlayer = new GameObject[] { };
    // [SerializeField] private GameObject[] _manaTwoPlayer = new GameObject[] { };

    [SerializeField] private Player _playerOne;
    [SerializeField] private Player _playerTwo;
    
    
    [SerializeField] private int _manaCountOne = 1;
    //[SerializeField] private int _currentManaCountOne;
    [SerializeField] private int _manaCountTwo = 1;
    //[SerializeField] private int _currentManaCountTwo;
    [SerializeField] private TMP_Text _textOne;
    [SerializeField] private TMP_Text _textTwo;

    private const int MAX_MANA_COUNT = 10;

    private void Start()
    {
        EventManager.DecreasePlayerMana.AddListener(DecreaseMana);
    }

    public void IncreaseMana(bool player)
    {
        if (_manaCountOne <= MAX_MANA_COUNT || _manaCountTwo <= MAX_MANA_COUNT)
        {
            if (player)
            {
                for (int i = 0; i <= _manaCountOne; i++)
                {
                    _playerOne.mana[i].gameObject.SetActive(true);
                }
                _manaCountOne += 1;
                _textOne.text = _manaCountOne.ToString();
                _playerOne.currentManaCount = _manaCountOne;
            }
            else
            {
                for (int i = 0; i <= _manaCountOne; i++)
                {
                    _playerTwo.mana[i].gameObject.SetActive(true);
                }
                _manaCountTwo += 1;
                _textTwo.text = _manaCountTwo.ToString();
                _playerTwo.currentManaCount = _manaCountTwo;
            }
        }
    }

    public void DecreaseMana(bool playerOne, int manaCost)
    {
        if (_playerOne.currentManaCount - manaCost >= 0 || _playerTwo.currentManaCount - manaCost >= 0 )
        {
            if (playerOne)
            {
                for (int i = 0; i <= manaCost; i++)
                {
                    _playerOne.mana[i].gameObject.SetActive(false);
                }
                _playerOne.currentManaCount -= manaCost;
                _textOne.text = _playerOne.currentManaCount.ToString();
            }
            else
            {
                for (int i = 0; i <= _manaCountOne; i++)
                {
                    _playerTwo.mana[i].gameObject.SetActive(false);
                }
                _playerTwo.currentManaCount -= manaCost;
                _textTwo.text =  _playerTwo.currentManaCount.ToString();
            }
        }
        else
        {
            Debug.Log("Не хватает маны");
        }
    }
}