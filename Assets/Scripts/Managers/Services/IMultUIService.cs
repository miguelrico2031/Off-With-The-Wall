using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMultUIService : IService
{
    public void AddPeopleMult(bool perm);
    public void AddPopUpMult(bool perm);
    public void RemovePeopleMult();
    public void RemovePopUpMult();
}
