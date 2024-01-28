using Scripts.Game.Pirates;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils.Annotations;
using UnityEngine;
using System;
using Scripts.Utils;
using Scripts.Utils.Types;
using Scripts.Utils.Placeholders;

namespace Scripts.Game.Manager
{
    public class WaveManager : Singleton<WaveManager>
    {

        [SerializeField]
        private GameObject _enemyPiratePrefab;


        [SerializeField] private int _minPirates = 3;

        [SerializeField] private int _maxPirates;

        [SerializeField][ReadOnly] private List<EnemyPirate> enemies;

        private HashSet<EnemyPirate> deadEnemies = new HashSet<EnemyPirate>();

        [SerializeField][ReadOnly] private int _enemiesLeft = 0;

        [SerializeField][ReadOnly] private List<Spawnpoint> _spawnpoints;

        [SerializeField][ReadOnly] private int _waveCount = 0;

        public int WavesSurvived => _waveCount - 1;

        private const float _initialMinAggro = 0.05f;
        [SerializeField][ReadOnly] private float _currentMinAggro = _initialMinAggro;
        private const float _minAggroIncreasePerWave = 0.0375f;

        private const float _initialMaxAggro = 0.25f;
        [SerializeField][ReadOnly] private float _currentMaxAggro = _initialMaxAggro;
        private const float _maxAggroIncreasePerWave = 0.05f;

        [SerializeField][ReadOnly] private float _currentMinSpeed = _initialMinAggro;
        [SerializeField][ReadOnly] private float _currentMaxSpeed = _initialMaxAggro;


        /// <summary>
        /// invoked upon completing a wave (next wave number int invoked)
        /// </summary>
        public Action<int> OnWaveClear;


        private Coroutine _betweenWavesCoroutine;


        [SerializeField] private AudioHolder _waveClearAudioHolder;
        [SerializeField] private AudioHolder _waveStartAudioHolder;

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

            if (GameManager.Instance.gameState != GameState.GAMERING)
            {
                // >:(
                return;
            }

            Debug.Log($"Survived wave {_waveCount}!");
            _waveCount++;


            

            OnWaveClear?.Invoke(_waveCount);

            

            _currentMaxAggro = Mathf.Clamp(_currentMaxAggro + _maxAggroIncreasePerWave, 0f, 1f);
            _currentMinAggro = Mathf.Clamp(_currentMinAggro + _minAggroIncreasePerWave, 0f, 1f);

            _currentMaxSpeed = Mathf.Clamp(_currentMaxSpeed + _maxAggroIncreasePerWave, 0f, 2f);
            _currentMinSpeed = Mathf.Clamp(_currentMinSpeed + _minAggroIncreasePerWave, 0f, 2f);

            if (_waveClearAudioHolder.TryGetRandomAudioClip(out AudioClip waveSoundEffect))
            {
                CaptainFeathersword.Instance.GameCameraAudioSource.PlayOneShot(waveSoundEffect);
            }

            _betweenWavesCoroutine = StartCoroutine(StartWaveCoroutine());
        }

        private IEnumerator StartWaveCoroutine()
        {

            

            yield return new WaitForSeconds(5f);

            if (_waveStartAudioHolder.TryGetRandomAudioClip(out AudioClip nextWaveSoundEffect))
            {
                CaptainFeathersword.Instance.GameCameraAudioSource.PlayOneShot(nextWaveSoundEffect);
            }

            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
            deadEnemies.Clear();
            enemies.Clear();

            yield return new WaitForEndOfFrame();

            int enemiesThisWave = Mathf.Max(Mathf.Min(_maxPirates, _spawnpoints.Count, _waveCount), _minPirates);

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
                    UnityEngine.Random.Range(_currentMinSpeed, _currentMaxSpeed),
                    UnityEngine.Random.Range(_currentMinAggro, _currentMaxAggro),
                    UnityEngine.Random.Range(_currentMinAggro, _currentMaxAggro),
                    UnityEngine.Random.Range(_currentMinAggro, _currentMaxAggro),
                    UnityEngine.Random.Range(_currentMinAggro, _currentMaxAggro)
                );
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.2f));
            }

            _betweenWavesCoroutine = null;

            yield break;
        }

        

        private void OnValidate()
        {
            _spawnpoints = new List<Spawnpoint>(FindObjectsOfType<Spawnpoint>());
        }

        private void Awake()
        {
            _spawnpoints = new List<Spawnpoint>(FindObjectsOfType<Spawnpoint>());
        }


        // Use this for initialization
        void Start()
        {
            //NewWave();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}