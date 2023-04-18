using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    smallEnergyItem,
    largeEnergyItem,
    smallHealthItem,
    largeHealthItem,
}

[CreateAssetMenu(fileName = "New ItemData", menuName = "Item Data", order = 51)]

public class ItemData : ScriptableObject
{
    [field: SerializeField] public ItemType type { get; private set; }
    [field: SerializeField] public int score { get; private set; }

}
