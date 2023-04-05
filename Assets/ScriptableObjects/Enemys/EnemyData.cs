using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    snake,
    fish,
    wolf,
    bear,
    plant,
    stone,
}

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Enemy Data", order = 51)]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public EnemyType enemyType { get; private set; }
    [field: SerializeField] public int health { get; private set; }
    [field: SerializeField] public int speed { get; private set; }
    [field: SerializeField] public int detectionDistance { get; private set; }
    [field: SerializeField] public float speedAttack { get; private set; }

}
