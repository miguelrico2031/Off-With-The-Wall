using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/RouletteEvent")]

public class RouletteEvent : ScriptableObject, IGameEvent
{
    [field: SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public string StartDialogueKey { get; private set; }

    [field: SerializeField] public string ChoiceTextAccept { get; private set; }
    [field: SerializeField] public string ChoiceTextRefuse { get; private set; }

    [field: SerializeField] public string EndDialogueKeyRefuse { get; private set; }
    [field: SerializeField] public string EndDialogueKeyLose { get; private set; }
    [field: SerializeField] public string EndDialogueKeyWin { get; private set; }
    [field: SerializeField] public string EndDialogueKeyCrit { get; private set; }

    [field: SerializeField] public Outcomes OutcomesRefuse { get; private set; }
    [field: SerializeField] public Outcomes OutcomesLose { get; private set; }
    [field: SerializeField] public Outcomes OutcomesWin { get; private set; }
    [field: SerializeField] public Outcomes OutcomesCrit { get; private set; }

    [field: SerializeField][field: Range(0f, 1f)] public float CritChance { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float SucessChance { get; private set; }
    

}
