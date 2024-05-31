using UnityEngine;

public class AIWaypoint : MonoBehaviour
{
    public bool Valid { get; set; } = true;
    
    [field:SerializeField] public bool IsOnlyWaypoint { get; private set; } //por si queremos que algunos wps no puedan spawnear/despawnear
    
    private IAIService _aiService;
    
    private void Start()
    {
        _aiService = GameManager.Instance.Get<IAIService>();
        _aiService.AddWaypoint(this);
    }
    
}
