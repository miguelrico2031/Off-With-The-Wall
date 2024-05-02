using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Event : ScriptableObject
{
    [Serializable]
    public class EventReward
    {
        [field: SerializeField] public RewardType _type { get; private set; }

        //[Header("Personas o Multiplicador")]
        [field: SerializeField] public float Valor{ get; private set; }

        [field: SerializeField] public float Time { get; private set; }
        [field: SerializeField] public bool Infinite { get; private set; }


    }

    public enum RewardType
    {
        AddPeople,
        PopUpMultiplier,
        RewardMultiplier,
        CritMultiplier,
    }
    [field:SerializeField] public IBuilding.BuildingType _buildingtype { get; private set; }

    [field: SerializeField] public string _startDialogueKey { get; private set; }
}
