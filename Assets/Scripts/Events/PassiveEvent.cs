using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Events/PassiveEvent")]

public class PassiveEvent : Event
{
    [field: SerializeField] public EventReward _reward { get; private set; }
}
