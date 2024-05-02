using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IDialogueService :IService
{
    public void SendDialogue(string _key, Action _nextAction);
    public void SkipOrContinue();
}
