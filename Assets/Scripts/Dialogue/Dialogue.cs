using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [Serializable]
    public class Phrase
    {
        [FormerlySerializedAs("_speaker")] [SerializeField] public Dialogues.Speaker Speaker;
        [FormerlySerializedAs("_text")] 
        [SerializeField, TextArea(minLines:4, maxLines:6)] public string Text;
    }

    [SerializeField] private string _key;
    [SerializeField] private Phrase[] _phrases;

    public string Key
    {
        get => _key;
    }

    public Phrase[] Phrases
    {
        get => _phrases;
    }

}
