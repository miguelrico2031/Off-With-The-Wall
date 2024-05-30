using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultUIService : IService
{
    public void AddPeopleMult(bool perm,float value);
    public void AddPopUpMult(bool perm,float value);
    public void RemovePeopleMult(float value);
    public void RemovePopUpMult(float value);
}
