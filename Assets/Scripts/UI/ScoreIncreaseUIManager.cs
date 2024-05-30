using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIncreaseUIManager : MonoBehaviour,IScoreIncreaseUIService
{
    private ObjectPool<ScoreIncrease> _objectPool;

    [SerializeField] ScoreIncrease _increasePrefab;

    public void endIncrease(ScoreIncrease increase)
    {
        _objectPool.Return(increase);
    }

    public void setIncrease(uint value,bool mult)
    {
        var increase = _objectPool.Get();
        increase.transform.position = Input.mousePosition;
        increase.startAnim(value,mult);
    }
    // Start is called before the first frame update
    void Start()
    {
        _objectPool = new(_increasePrefab, (int)GameManager.Instance.GameInfo.PopUpPoolSize, true, this.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
