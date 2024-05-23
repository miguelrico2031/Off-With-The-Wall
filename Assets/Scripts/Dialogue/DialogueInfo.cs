using System;
using System.Collections.Generic;
using UnityEngine;



//[CreateAssetMenu(menuName = "ScriptableObjects/Dialogues")]
public class DialogueInfo : ScriptableObject
{
    public enum Speaker
    {
        Juan,
        Pedro,
        Podemita1,
        Podemita2,
        Info,
        UndercoverCop,
        ToughCop,
        FuckDonaldsGuy,
        CenterRightGuy,
        CuteGirl,
        BandMemberSalvi,
        BandMemberAdri,
        BandMemberEli,
        Newspaper,
        CrazyGuy,
        Student,
        Teacher,
        GoodCop,
        Guillotine,
        Treasurer,
        EatenCop,
        Pana
    }

    [Serializable]
    public class SpeakerData
    {
        [SerializeField] private Speaker _speaker;
        [SerializeField] private Sprite _sprite;
        public Speaker Speaker { get => _speaker;  }
        public Sprite Sprite { get => _sprite; }
    }
    
    [Serializable]
    public struct DialoguePlaceholder
    {
        public string Placeholder;
        public string Replace;
    }

    [Serializable]
    public struct NewspaperCovers
    {
        public Dialogue Dialogue;
        public Sprite Cover;
    }


    [SerializeField] private SpeakerData[] _speakers;
    [SerializeField] private DialoguePlaceholder[] _placeholders;
    [SerializeField] private string _movementNamePlaceholder;
    [SerializeField] private string _movementSloganPlaceholder;
    [SerializeField] private NewspaperCovers[] _newspaperCovers;

    private Dictionary<Speaker, SpeakerData> _speakersDict;
    private Dictionary<Dialogue, Sprite> _coversDict;
    

    public Dialogue.Phrase ProcessPhrase(Dialogue.Phrase phrase)
    {
        var newPhrase = new Dialogue.Phrase() { Speaker = phrase.Speaker, Text = phrase.Text};
            foreach(var placeholder in _placeholders)
                newPhrase.Text = newPhrase.Text.Replace(placeholder.Placeholder, placeholder.Replace);

            newPhrase.Text = newPhrase.Text.Replace(_movementNamePlaceholder, GameManager.Instance.GameInfo.OrgName);
            newPhrase.Text = newPhrase.Text.Replace(_movementSloganPlaceholder, GameManager.Instance.GameInfo.OrgSlogan);

            if (newPhrase.Speaker is not Speaker.Newspaper) return newPhrase;
            
            var split = newPhrase.Text.Split("*");
            if (split.Length == 3) 
                newPhrase.Text = $"<size=125%><allcaps><b><i>{split[1]}</allcaps></b></i><size=100%>\n{split[2]}";
            return newPhrase;
    }

    public SpeakerData GetSpeakerData(Speaker speaker)
    {
        if (_speakersDict is null)
        {
            _speakersDict = new();
            foreach (var spk in _speakers) _speakersDict.Add(spk.Speaker, spk);
        }

        return _speakersDict.GetValueOrDefault(speaker);
    }

    public Sprite GetNewspaperCover(Dialogue dialogue)
    {
        if (_coversDict is null)
        {
            _coversDict = new();
            foreach (var npc in _newspaperCovers)
            {
                if(npc.Dialogue is not null) _coversDict.Add(npc.Dialogue, npc.Cover);
            }
        }

        return _coversDict.GetValueOrDefault(dialogue);
    }
}
