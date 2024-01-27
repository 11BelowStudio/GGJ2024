using Scripts.Game.Pirates;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils.Annotations;
using UnityEngine;

namespace Scripts.Game.Manager
{
    public class WaveManager : MonoBehaviour
    {

        [SerializeField]
        private GameObject _enemyPiratePrefab;


        [SerializeField] private int _minPirates = 3;

        [SerializeField] private int _maxPirates = 12;

        [ReadOnly] private List<EnemyPirate> enemies = new List<EnemyPirate>();

        private HashSet<EnemyPirate> deadEnemies = new HashSet<EnemyPirate>();

        [ReadOnly] private int _enemiesLeft = 0;

        [ReadOnly] private List<Spawnpoint> _spawnpoints;

        [ReadOnly] private int waveCount = 0;


        public void OnEnemyDed(EnemyPirate IAmDed)
        {
            if (deadEnemies.Contains(IAmDed))
            {
                return;
            }
        }


        public void NewWave()
        {
            foreach (var enemy in enemies) {
                Destroy(gameObject);
            }
        }

        private void OnValidate()
        {
            _spawnpoints = new List<Spawnpoint>(FindObjectsOfType<Spawnpoint>());
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}