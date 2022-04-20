using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers {
    public class AgentManager : Singleton<AgentManager> {
        [SerializeField] private Agent _agentPrefab;
        [SerializeField] private GameObject[] _visualPrefab;
        [SerializeField] private int _maxAgents;
        private List<Agent> _agents = new();
        public List<Transform> Points;

        public void InstantiatePeople() {
            if (_agents.Count >= _maxAgents) return;

            int max = _maxAgents - _agents.Count;
            BigFloat people = LevelManager.People;
            BigFloat create = people > max ? max : people;

            for (int i = 0; i < create; i++) {
                int visualIndex = Random.Range(0, _visualPrefab.Length);
                int index = Random.Range(0, Points.Count);
                Agent agent = Instantiate(_agentPrefab, Points[index].position, Quaternion.identity);
                agent.SetVisual(_visualPrefab[visualIndex]);
                _agents.Add(agent);
            }
        }
    }
}
