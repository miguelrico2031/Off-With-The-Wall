using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "ScriptableObjects/Dialogues")]
public class Dialogues : ScriptableObject
{
    public enum Speaker
    {
        Juan, Pedro, Podemita1, Podemita2
    }

    [Serializable]
    public class SpeakerData
    {
        [SerializeField] private Speaker _speaker;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;

        public Speaker Speaker { get => _speaker; }
        public string Name { get => _name; }
        public Sprite Sprite { get => _sprite; }
    }


    [SerializeField] private SpeakerData[] _speakers;
    [SerializeField] private Dialogue[] _dialogues;

    private Dictionary<string, Dialogue> _dialoguesDict;
    private Dictionary<Speaker, SpeakerData> _speakersDict;

    public Dialogue GetDialogue(string key)
    {
        if (_dialoguesDict == null)
        {
            _dialoguesDict = new();
            foreach(var d in _dialogues) _dialoguesDict.Add(d.Key, d);
        }

        return _dialoguesDict.GetValueOrDefault(key); //devuelve null si no encuentra la key en el dict
    }
        
    public SpeakerData GetSpeakerData(Speaker speaker)
    {
        if (_speakersDict == null)
        {
            _speakersDict = new();
            foreach(var spk in _speakers) _speakersDict.Add(spk.Speaker, spk);
        }

        return _speakersDict.GetValueOrDefault(speaker);
    }
}