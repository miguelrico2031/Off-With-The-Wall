using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreIncreaseUIService : IService
{
    public void setIncrease(uint value,bool mult);

    public void endIncrease(ScoreIncrease increase);
}
