using UnityEngine;

public class AIWaypoint : MonoBehaviour
{
    public bool Valid { get; set; } = true;
    
    [field:SerializeField] public bool IsOnlyWaypoint { get; private set; } //por si queremos que algunos wps no puedan spawnear/despawnear

    [SerializeField] private bool _isEndWaypoint;
    
    private IAIService _aiService;
    
    private void Start()
    {
        if (_isEndWaypoint) return;
        _aiService = GameManager.Instance.Get<IAIService>();
        _aiService.AddWaypoint(this);
    }
    
}
