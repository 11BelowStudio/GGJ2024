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
            base.Update();
        }
    }
}