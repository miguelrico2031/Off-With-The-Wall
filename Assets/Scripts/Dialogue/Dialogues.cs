using System;
using System.Collections.Generic;
using UnityEngine;



//[CreateAssetMenu(menuName = "ScriptableObjects/Dialogues")]
public class Dialogues : ScriptableObject
{
    public enum Speaker
    {
        Juan,
        Pedro,
        Podemita1,
        Podemita2,
        Info,
        Clumsycop,
        ToughCop,
        FastFoodWorker,
        CenterRightGuy,
        BandMemberSalvi,
        CuteGirl
    }

    [Serializable]
    public class SpeakerData
    {
        [SerializeField] private Speaker _speaker;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _sprite;

        public Speaker Speaker
        {
            get => _speaker;
        }

        public string Name
        {
            get => _name;
        }

        public Sprite Sprite
        {
            get => _sprite;
        }
    }
    
    [Serializable]
    public struct DialoguePlaceholder
    {
        public string Placeholder;
        public string Replace;
    }


    [SerializeField] private SpeakerData[] _speakers;
    [SerializeField] private Dialogue[] _dialogues;
    [SerializeField] private DialoguePlaceholder[] _placeholders;

    private Dictionary<string, Dialogue> _dialoguesDict;
    private Dictionary<Speaker, SpeakerData> _speakersDict;

    public Dialogue GetDialogue(string key)
    {
        if (_dialoguesDict == null)
        {
            _dialoguesDict = new();
            foreach (var d in _dialogues) _dialoguesDict.Add(d.Key, d);
        }

        var dialogue = _dialoguesDict.GetValueOrDefault(key);
        if (dialogue is null) return null;

        foreach (var phrase in dialogue.Phrases)
            foreach(var placeholder in _placeholders)
                phrase.Text = phrase.Text.Replace(placeholder.Placeholder, placeholder.Replace);
        
        return dialogue;
    }

    public SpeakerData GetSpeakerData(Speaker speaker)
    {
        if (_speakersDict == null)
        {
            _speakersDict = new();
            foreach (var spk in _speakers) _speakersDict.Add(spk.Speaker, spk);
        }

        return _speakersDict.GetValueOrDefault(speaker);
    }
}
