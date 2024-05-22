using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [Serializable]
    public class Phrase
    {
        [FormerlySerializedAs("_speaker")] [SerializeField] public DialogueInfo.Speaker Speaker;
        [FormerlySerializedAs("_text")] 
        [SerializeField, TextArea(minLines:4, maxLines:6)] public string Text;
    }
    public Phrase[] Phrases { get => _phrases; }
    [SerializeField] private Phrase[] _phrases;
    

}
