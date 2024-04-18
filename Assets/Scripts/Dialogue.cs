using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [Serializable]
    public class Phrase
    {
        [SerializeField] private Dialogues.Speaker _speaker;
        [SerializeField, TextArea] private string _text;

        public Dialogues.Speaker Speaker { get => _speaker; }
        public string Text { get => _text; }
    }
    
    [SerializeField] private string _key;
    [SerializeField] private Phrase[] _phrases;

    public string Key { get => _key; }
    public Phrase[] Phrases { get => _phrases; }
    
}
