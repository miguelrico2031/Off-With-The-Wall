using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

using System;

public class PedestrianAI : MonoBehaviour, IPoolObject
{
    #region Attributes

    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }
    public AIWaypoint Target { get; private set; }
    public readonly UnityEvent<PedestrianAI> OnTargetReached = new();
    private bool _pathEnded = true;
    private NavMeshAgent _agent;
    private Animator _animator;
    private int _spawnAnim, _walkAnim, _despawnAnim;
    
    #endregion
    
    #region Unity Callbacks
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.avoidancePriority = 99;
        _animator = GetComponentInChildren<Animator>();
        _spawnAnim = Animator.StringToHash("Spawn");
        _walkAnim = Animator.StringToHash("Walk");
        _despawnAnim = Animator.StringToHash("Despawn");
    }

    private void Update()
    {

        if(!_pathEnded && Vector2.Distance(transform.position, Target.transform.position) <= .05f)
        {
            _pathEnded = true;
            OnTargetReached.Invoke(this);
        }
    }

    #endregion

    public IEnumerator Spawn(AIWaypoint spawnPoint, AIWaypoint target, float agentSpeed)
    {
        transform.position = spawnPoint.transform.position;
        _agent.speed = agentSpeed;
        _animator.Play(_spawnAnim);
        yield return null;
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Target = target;
        _agent.SetDestination(Target.transform.position);
        _pathEnded = false;
        _animator.Play(_walkAnim);
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
}
