using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStartLoseUIService : IService
{
    public void SetStartScreen(bool show);

    public void SetLoseScreen(bool show);

    public void SetPauseScreen(bool show);


}
