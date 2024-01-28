using Scripts.Utils.Annotations;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Game.Pirates
{
    public class EnemyPirate : Pirate
    {

        /// <summary>
        /// the enemy is not the captain
        /// </summary>
        public override bool IsTheCaptain { get { return false; } }


        [SerializeField] EnemyAgentAI _ai;

        public EnemyAgentAI AI => _ai;

        public Action<EnemyPirate> OnDedReference;

        private const float _minIdleNoiseDelay = 7.5f;
        private const float _maxIdleNoiseDelay = 30f;

        private float IdleNoiseDelay { get { return UnityEngine.Random.Range(_minIdleNoiseDelay, _maxIdleNoiseDelay); } }

        [SerializeField]
        [ReadOnly]
        private float _idleNoiseTimer = _maxIdleNoiseDelay;



        protected override void OnValidate()
        {
            base.OnValidate();
            _ai = GetComponent<EnemyAgentAI>();
        }

        protected override void Awake()
        {

            OnDed += OnDedInvokeReference;
            base.Awake();
            _ai = GetComponent<EnemyAgentAI>();
            _idleNoiseTimer = IdleNoiseDelay;
        }

        private void OnDedInvokeReference()
        {
            OnDedReference?.Invoke(this);
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            
            if (!IsDed)
            {
                _idleNoiseTimer -= Time.deltaTime;
                if (_idleNoiseTimer <= 0)
                {
                    _idleNoiseTimer = IdleNoiseDelay;
                    if (_idleNoiseHolder.TryGetRandomAudioClip(out AudioClip idleNoise))
                    {
                        _characterAudioSource.PlayOneShot(idleNoise);
                    }
                }
            }
            
            base.Update();

            
            
        }
    }
}