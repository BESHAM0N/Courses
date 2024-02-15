using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

public class BattleCry : StatsEffect
{
    [SerializeField] private int _damage = 0;
    [SerializeField] private int _health = 0;
    [SerializeField] private bool _permanent;
    [SerializeField] private string _name = "BattleCry";
    
    public BattleCry(int damage, int health, Card parent, bool permanent, string name = "") : base(damage, health, parent, permanent, name)
    {
        damage = _damage;
        health = _health;
        permanent = _permanent;
        name = _name;
    }
}
