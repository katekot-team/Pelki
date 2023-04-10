using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToDamage
{
    Player,
    Enemy,
}

[CreateAssetMenu(fileName = "New Fireball Data", menuName = "Fireball Data", order = 51)]
public class FirebollData : ScriptableObject
{
    [field: SerializeField] public ToDamage toDamage { get; private set; }
    [field: SerializeField] public float speed { get; private set;}
    [field: SerializeField] public float lifeTime { get; private set; }
    [field: SerializeField] public int damage { get; private set; }
}
