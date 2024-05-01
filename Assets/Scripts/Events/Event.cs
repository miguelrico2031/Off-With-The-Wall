using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event")]

public class Event : ScriptableObject
{

    [SerializeField] private Dialogue startDialogue;
    [SerializeField] private Dialogue endDialogue;
    [field:SerializeField] public EventType _eventtype { get; private set; }
    [field:SerializeField] public IBuilding.BuildingType _buildingtype { get; private set; }


    public enum EventType
    {
        ChoiceEvent, //Dos Opciones
        PassiveEvent, //Simplemente te dan algo pasivo (mas personas por x tiempo, mas pop-ups etc..)
        ChanceEvent, //Tirar la ruleta o no
    }
}
