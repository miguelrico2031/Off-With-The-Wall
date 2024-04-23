using UnityEngine;

public class AIWaypoint : MonoBehaviour
{
    private IAIService _aiService;
    
    private void Start()
    {
        _aiService = GameManager.Instance.Get<IAIService>();
        _aiService.AddWaypoint(this);
    }
}
