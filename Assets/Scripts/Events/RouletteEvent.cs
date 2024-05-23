using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/RouletteEvent")]

public class RouletteEvent : ScriptableObject, IGameEvent
{
    [field: SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public Dialogue StartDialogue { get; private set; }

    [field: SerializeField] public string ChoiceTextAccept { get; private set; }
    [field: SerializeField] public string ChoiceTextRefuse { get; private set; }

    [field: SerializeField] public Dialogue EndDialogueRefuse { get; private set; }
    [field: SerializeField] public Dialogue EndDialogueLose { get; private set; }
    [field: SerializeField] public Dialogue EndDialogueWin { get; private set; }
    [field: SerializeField] public Dialogue EndDialogueCrit { get; private set; }

    [field: SerializeField] public Outcomes OutcomesRefuse { get; private set; }
    [field: SerializeField] public Outcomes OutcomesLose { get; private set; }
    [field: SerializeField] public Outcomes OutcomesWin { get; private set; }
    [field: SerializeField] public Outcomes OutcomesCrit { get; private set; }

    [field: SerializeField][field: Range(0f, 1f)] public float CritChance { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float SucessChance { get; private set; }
    

}
