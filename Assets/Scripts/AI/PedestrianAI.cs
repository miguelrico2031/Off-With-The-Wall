using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

using System;
using Random = UnityEngine.Random;

public class PedestrianAI : MonoBehaviour, IPoolObject
{
    #region Attributes

    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    public AIWaypoint Target { get; private set; }
    public readonly UnityEvent<PedestrianAI> OnTargetReached = new();

    private GameInfo _gameInfo;
    private bool _pathEnded = true;
    private NavMeshAgent _agent;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private int _spawnAnim, _walkAnim, _despawnAnim, _idleAnim, _speedParam;
    private float _tryIdleTimer, _tryIdleDealy, _tryIdleProb;
    private float _posCheckTimer;
    private Vector3 _lastPos;
    
    #endregion
    
    #region Unity Callbacks
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.avoidancePriority = 99;
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _spawnAnim = Animator.StringToHash("Spawn");
        _walkAnim = Animator.StringToHash("Walk");
        _despawnAnim = Animator.StringToHash("Despawn");
        _idleAnim = Animator.StringToHash("Idle");
        _speedParam = Animator.StringToHash("Speed");
        _gameInfo = GameManager.Instance.GameInfo;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_pathEnded) return;
        
        _tryIdleTimer += Time.deltaTime;
        if (_tryIdleTimer >= _gameInfo.PedTryIdleDelay) TryIdle();

        _posCheckTimer += Time.deltaTime;
        if (_posCheckTimer >= _gameInfo.PedIdleMaxTime * 1.2f) CheckPositionChanged();
        
        _renderer.flipX = _agent.velocity.x < 0;
        _animator.SetFloat(_speedParam, _agent.velocity.magnitude);
        if(Vector2.Distance(transform.position, Target.transform.position) <= .05f)
        {
            _posCheckTimer = 0f;
            _tryIdleTimer = 0f;
            _pathEnded = true;
            OnTargetReached.Invoke(this);
        }
    }

    #endregion

    public IEnumerator Spawn(AIWaypoint spawnPoint, AIWaypoint target)
    {
        transform.position = spawnPoint.transform.position;
        _agent.speed = Random.Range(_gameInfo.MinAgentSpeed, _gameInfo.MaxAgentSpeed);
        _animator.Play(_spawnAnim);
        yield return null;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Target = target;
        _agent.SetDestination(Target.transform.position);
        _pathEnded = false;
        _animator.Play(_walkAnim);
        _tryIdleTimer = 0f;
        _posCheckTimer = 0f;
        _lastPos = transform.position;
    }

    public void SetTarget(AIWaypoint target)
    {
        Target = target;
        _agent.SetDestination(Target.transform.position);
        _pathEnded = false;
    }

    public IEnumerator Despawn(Action callback)
    {
        _animator.Play(_despawnAnim);
        yield return null;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        callback();
    }
    

    public void Clean()
    {
        Target = null;
        _pathEnded = true;
    }

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        var instance = parent is null ? Instantiate(this) : Instantiate(this, parent);
        instance.gameObject.SetActive(active);
        return instance;
    }

    private void TryIdle()
    {
        _tryIdleTimer = 0f;
        if (Random.Range(0f, 1f) <= _gameInfo.PedTryIdleProbability)
            StartCoroutine(Idle());
    }

    private IEnumerator Idle()
    {
        _animator.Play(_idleAnim);
        var speed = _agent.speed;
        _agent.speed = 0f;
        yield return new WaitForSeconds(Random.Range(_gameInfo.PedIdleMinTime, _gameInfo.PedIdleMaxTime));
        _agent.speed = speed;
        _animator.Play(_walkAnim);
    }

    private void CheckPositionChanged()
    {
        _posCheckTimer = 0f;
        
        if (Vector3.Distance(transform.position, _lastPos) > float.Epsilon)
        {
            _lastPos = transform.position;
            return;
        }
        Debug.Log("atascao");
        _tryIdleTimer = 0f;
        _pathEnded = true;
        OnTargetReached.Invoke(this);
        
    }
}
