using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IDialogueService :IService
{
    public void SendDialogue(string key, bool hideOnFinish, Action nextAction);
    public void SkipOrContinue();
    public void Hide();
}
