using Scripts.Game.Pirates;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils.Annotations;
using UnityEngine;
using System;
using Scripts.Utils;

namespace Scripts.Game.Manager
{
    public class WaveManager : MonoBehaviour
    {

        [SerializeField]
        private GameObject _enemyPiratePrefab;


        [SerializeField] private int _minPirates = 3;

        [SerializeField] private int _maxPirates = 25;

        [SerializeField][ReadOnly] private List<EnemyPirate> enemies;

        private HashSet<EnemyPirate> deadEnemies = new HashSet<EnemyPirate>();

        [SerializeField][ReadOnly] private int _enemiesLeft = 0;

        [SerializeField][ReadOnly] private List<Spawnpoint> _spawnpoints;

        [SerializeField][ReadOnly] private int waveCount = 1;


        private const float _minAggro = 0.05f;
        [SerializeField][ReadOnly] private float _currentMinAggro = _minAggro;
        private const float _minAggroIncreasePerWave = 0.0375f;

        private const float _initialMaxAggro = 0.25f;
        [SerializeField][ReadOnly] private float _currentMaxAggro = _initialMaxAggro;
        private const float _maxAggroIncreasePerWave = 0.05f;

        

        /// <summary>
        /// invoked upon completing a wave (next wave number int invoked)
        /// </summary>
        public Action<int> OnWaveClear;


        public void OnEnemyDed(EnemyPirate IAmDed)
        {
            if (deadEnemies.Contains(IAmDed))
            {
                return;
            }

            deadEnemies.Add(IAmDed);
            _enemiesLeft -= 1;

            if (_enemiesLeft <= 0)
            {
                WaveComplete();
            }
        }

        public void WaveComplete()
        {

            Debug.Log($"Survived wave {waveCount}!");
            waveCount++;


            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
            deadEnemies.Clear();

            OnWaveClear?.Invoke(waveCount);

            

            _currentMaxAggro = Mathf.Clamp(_currentMaxAggro + _maxAggroIncreasePerWave, 0f, 1f);
            _currentMinAggro = Mathf.Clamp(_currentMinAggro + _minAggroIncreasePerWave, 0f, 1f);

            NewWave();
        }


        public void NewWave()
        {

            int enemiesThisWave = Mathf.Max(Mathf.Min(_maxPirates, _spawnpoints.Count, waveCount), _minPirates);

            _enemiesLeft = enemiesThisWave;

            ListShuffler.ShuffleUnityRNG(_spawnpoints);

            for (int i = 0; i < enemiesThisWave; i++)
            {
                GameObject newPirateObj = Instantiate(
                    _enemyPiratePrefab, _spawnpoints[i].GetRandomlyOffsetPosition(), Quaternion.identity
                );

                EnemyPirate newPirate = newPirateObj.GetComponent<EnemyPirate>();
                enemies.Add(newPirate);

                newPirate.OnDedReference += OnEnemyDed;

                newPirate.AI.SetAIParams(
                    EnemyAgentAI.RandomMoveBehaviour,
                    true,
                    EnemyAgentAI.RandomAttackBehaviour,
                    UnityEngine.Random.Range(_currentMinAggro, _currentMaxAggro),
                    UnityEngine.Random.Range(_currentMinAggro, _currentMaxAggro),
                    UnityEngine.Random.Range(_currentMinAggro, _currentMaxAggro),
                    UnityEngine.Random.Range(_currentMinAggro, _currentMaxAggro)
                );
            }

        }

        private void OnValidate()
        {
            _spawnpoints = new List<Spawnpoint>(FindObjectsOfType<Spawnpoint>());
        }


        // Use this for initialization
        void Start()
        {
            NewWave();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}