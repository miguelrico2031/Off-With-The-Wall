using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/RouletteEvent")]

public class RouletteEvent : ScriptableObject, IGameEvent
{
    [field: SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public string StartDialogueKey { get; private set; }

    [field: SerializeField] public string ChoiceTextA { get; private set; }
    [field: SerializeField] public string ChoiceTextB { get; private set; }

    [field: SerializeField] public string EndDialogueWinKey { get; private set; }
    [field: SerializeField] public string EndDialogueLoseKey { get; private set; }
    [field: SerializeField] public string EndDialogueCritKey { get; private set; }
    [field: SerializeField] public string RefuseDialogueKey { get; private set; }

    [field: SerializeField] public Outcomes OutcomesWin { get; private set; }
    [field: SerializeField] public Outcomes OutcomesLose { get; private set; }
    [field: SerializeField] public Outcomes OutcomesCrit { get; private set; }

    [field: SerializeField] public uint WinChance { get; private set; }
    [field: SerializeField] public uint LoseChance { get; private set; }
    [field: SerializeField] public uint CritChance { get; private set; }

}
