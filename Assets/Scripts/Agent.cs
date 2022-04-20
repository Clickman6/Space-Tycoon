using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour {
    [SerializeField] private Transform _visual;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    private State _state;
    private Coroutine _animationCoroutine;

    private void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        SetRandomPoint(); 
    }

    private void Update() {
        if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance) {
            SetState(State.Idle);
        }
    }

    private void SetState(State state) {
        if (_state == state) return;

        if (state == State.Idle) {
            _navMeshAgent.SetDestination(transform.position);
            StartCoroutine(WaitForNextPoint());
            _animationCoroutine = StartCoroutine(ChangeAnimations());
        } else if (state == State.Walk) {
            if (_animationCoroutine != null) {
                StopCoroutine(_animationCoroutine);
            }

            _animator.SetTrigger(Walk);
        }

        _state = state;
    }

    private IEnumerator WaitForNextPoint() {
        float timer = Random.Range(1f, 5f);

        yield return new WaitForSeconds(timer);

        SetRandomPoint();
    }

    private IEnumerator ChangeAnimations() {

        while (true) {
            int index = Random.Range(0, IdleAnimations.Length);
            float timer = Random.Range(1f, 5f);

            _animator.SetTrigger(IdleAnimations[index]);

            yield return new WaitForSeconds(timer);
        }
    }

    private void SetRandomPoint() {
        var points = AgentManager.Instance.Points;
        int index = Random.Range(0, points.Count);
        Vector3 offset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));

        _navMeshAgent.SetDestination(points[index].position + offset);
        SetState(State.Walk);
    }

    public void SetVisual(GameObject prefab) {
        _animator = Instantiate(prefab, _visual.position, _visual.rotation, _visual).GetComponent<Animator>();
    }

    #region Animation Static Trigger

    private static readonly int Walk = Animator.StringToHash("WALK");
    private static readonly string[] IdleAnimations = { "DANCE", "DANCE2", "IDLE" };

    #endregion

    private enum State {
        Idle,
        Walk
    }
}
