using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface ITextDialogService :IService
{
    public void SendDialog(string _key, Action _nextAction);
    public void SkipOrContinue();
}
