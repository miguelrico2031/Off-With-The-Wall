using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IDialogueService :IService
{
    public void SendDialogue(Dialogue dialogue, bool hideOnFinish, Action nextAction);
    public void SendInfoText(string text, Action nextAction);
    public void SkipOrContinue();
    public void Hide();
}
