using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using NavMeshPlus.Components;

public class AIManager : MonoBehaviour, IAIService
{
    #region Attributes

    [SerializeField] private PedestrianAI _pedestrianPrefab;
    [SerializeField] private Transform _pedsParent;
    [SerializeField] private float _delaybetweenSpawns;
    [SerializeField] private GameObject _notWalkable, _notWalkableEnd;
    
    private ObjectPool<PedestrianAI> _objectPool;
    private GameInfo _gameInfo;
    private readonly List<AIWaypoint> _waypoints = new();
    private AIWaypoint _lastSpawnPoint, _lastTarget;

    

    #endregion
    
    #region Unity Callbacks
    

    private IEnumerator Start()
    {
        _gameInfo = GameManager.Instance.GameInfo;
        _objectPool = new(_pedestrianPrefab, (int) _gameInfo.InitialPedestrians * 2, true, _pedsParent);

        yield return null;

        StartCoroutine(SpawnPedestrians((int) _gameInfo.InitialPedestrians));
    }

    #endregion

    public void AddWaypoint(AIWaypoint waypoint)
    {
        _waypoints.Add(waypoint);
    }
    

    private IEnumerator SpawnPedestrians(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(_delaybetweenSpawns);
            var (spawnPoint, target) = GetRandomSpawnPointAndTarget();
            SpawnPedestrian(spawnPoint, target);
        }
    }

    private (AIWaypoint spawn, AIWaypoint target) GetRandomSpawnPointAndTarget()
    {
        AIWaypoint spawnPoint, target;
            
        do spawnPoint = GetRandomWaypoint();
        while (spawnPoint == _lastSpawnPoint || spawnPoint == _lastTarget || spawnPoint.IsOnlyWaypoint);
        _lastSpawnPoint = spawnPoint;
            
        do target = GetRandomWaypoint();
        while (target == _lastTarget || target == _lastSpawnPoint);
        _lastTarget = target;
        return (spawnPoint, target);
    }

    private AIWaypoint GetRandomWaypoint()
    {
        AIWaypoint wp = null;
        while (wp is null)
        {
            wp = _waypoints[Random.Range(0, _waypoints.Count)];
            if (!wp.Valid) wp = null;
        }

        return wp;
    }

    private void SpawnPedestrian(AIWaypoint spawnPoint, AIWaypoint target)
    {
        var pedestrian = _objectPool.Get();
        StartCoroutine(pedestrian.Spawn(spawnPoint.transform.position, target));
        pedestrian.OnTargetReached.AddListener(OnTargetReached);
    }

    private void OnTargetReached(PedestrianAI pedestrian)
    {
        _lastTarget = pedestrian.Target;
        if (!_lastTarget.IsOnlyWaypoint && Random.Range(0f, 1f) <= _gameInfo.PedDespawnProbability)
        { //despawn y spawn en otro sitio
            pedestrian.OnTargetReached.RemoveListener(OnTargetReached);
            StartCoroutine(pedestrian.Despawn(() =>
            {
                _objectPool.Return(pedestrian);
                StartCoroutine(SpawnPedestrians(1));
            }));
        }
        else
        { //solo cambiar target sin despawnearlo
           pedestrian.SetTarget(GetRandomSpawnPointAndTarget().target);
        }
    }


    public void TargetWall()
    {
        var pool = _objectPool.Pool.Cast<PedestrianAI>().ToList();
        var wallWp = FindAnyObjectByType<Wall>().GetComponentInChildren<AIWaypoint>();
        foreach (var ped in pool)
        {
            if (!ped.Active)
            {
                ped.Active = true;
            }

            StartCoroutine(ped.Spawn(ped.transform.position, wallWp, PedestrianAI.Mode.ToWall));
        }
    }

    public IEnumerator TargetHouses()
    {
        var nav = GetComponentInChildren<NavMeshSurface>();
        _notWalkable.SetActive(false);
        _notWalkableEnd.SetActive(true);
        yield return nav.BuildNavMeshAsync();

        var endWps = transform.Find("End Waypoints").GetComponentsInChildren<AIWaypoint>();
        int wpI = 0;
        var pool = _objectPool.Pool.Cast<PedestrianAI>().ToList();
        foreach (var ped in pool)
        {
            if (!ped.Active)
            {
                ped.Active = true;
            }

            var wp = endWps[wpI++ % endWps.Length];
            StartCoroutine(ped.Spawn(ped.transform.position, wp, PedestrianAI.Mode.ToRichHouses));
        }
        
    }
    
}
