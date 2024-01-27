using System;
using System.Collections;
using UnityEngine;
using Scripts.Utils.Annotations;

namespace Scripts.Game.Pirates
{

    public class EnemyAgentAI: MonoBehaviour
    {

        private const float _minMovePower = 0.1f;
        private const float _maxMovePower = 1f;

        private const float _minAttackAttemptDelay = 0.1f;
        private const float _maxAttackAttemptDelay = 20f;


        private const float _minRespectableDistance = 5f;
        private const float _maxRespectableDistance = 15f;

        private const float _minRushDistance = 0.5f;
        private const float _maxRushDistance = 2f;

        private const float _minFleeDist = 10f;
        private const float _maxFleeDist = 20f;
        
        
        [SerializeField]
        private PirateMover _mover;

        [Tooltip("parameters for this bot's logic")]
        [ReadOnly] private EnemyAgentMoveBehaviours _moveBehaviour;

        [ReadOnly] private bool _randomMove;

        [ReadOnly] private EnemyAgentAttackBehaviours _attackBehaviour;

        [ReadOnly] private float _moveSpeed;

        [ReadOnly] private float _attackTimeMod;

        [ReadOnly] private float _attackChance;

        [ReadOnly] private float _distanceMod;

        [ReadOnly] private float _distanceTarget;

        [Tooltip("non-constant AI internal logic")]
        [ReadOnly] private float _attackTimer;


        

        private void OnValidate()
        {
            _mover = GetComponent<PirateMover>();
        }


        public void SetAIParams(
            EnemyAgentMoveBehaviours moveType,
            bool randomMove,
            EnemyAgentAttackBehaviours attackType,
            float moveSpeedMod = 0.5f,
            float attackMod = 0.5f,
            float attackChance = 0.5f,
            float distanceMod = 0.5f
        )
        {
            _moveBehaviour = moveType;
            _randomMove = randomMove;
            _attackBehaviour = attackType;
            _moveSpeed = _minMovePower + ( moveSpeedMod * _maxMovePower );
            _attackTimeMod = _minAttackAttemptDelay + ((_maxAttackAttemptDelay - _minAttackAttemptDelay) * (1 - attackMod));
            _attackChance = attackChance;
            _distanceMod = distanceMod;


            CalculateDistanceTarget();
        }


        private void CalculateDistanceTarget()
        {
            switch (_moveBehaviour)
            {
                case EnemyAgentMoveBehaviours.RESPECTABLE_DISTANCE:
                    _distanceTarget = _minRespectableDistance + ((_maxRespectableDistance - _minRespectableDistance) * (1 - _distanceMod));
                    break;
                case EnemyAgentMoveBehaviours.RUSHER:
                    _distanceTarget = _minRushDistance + ((_maxRushDistance - _minRushDistance) * (1 - _distanceMod));
                    break;
                case EnemyAgentMoveBehaviours.FLEER:
                    _distanceTarget = _minFleeDist + ((_maxFleeDist - _minFleeDist) * (1 - _distanceMod));
                    break;
            }
        }

        private void Update()
        {
            // step 1: work out where we're going
            Vector3 target = Vector3.zero;

            if (CaptainFeathersword.TryGetInstance( out var feathersword))
            {
                target = feathersword.transform.position;
            }
        }

        public static EnemyAgentMoveBehaviours RandomMoveBehaviour
        {
            get {
                var behaviours = Enum.GetValues(typeof(EnemyAgentMoveBehaviours)) as EnemyAgentMoveBehaviours[];
                return behaviours[UnityEngine.Random.Range(0, behaviours.Length)];
            }
        }

        public static EnemyAgentAttackBehaviours RandomAttackBehaviour
        {
            get
            {
                var behaviours = Enum.GetValues(typeof(EnemyAgentAttackBehaviours)) as EnemyAgentAttackBehaviours[];
                return behaviours[UnityEngine.Random.Range(0, behaviours.Length)];
            }
        }

        private void Awake()
        {
            
        }


    }

    [Serializable]
    public enum EnemyAgentMoveBehaviours
    {
        RESPECTABLE_DISTANCE,
        RUSHER,
        FLEER
    }

    [Serializable]
    public enum EnemyAgentAttackBehaviours
    {
        PASSIVE,
        T_POSER,
        ATTACK_1,
        ATTACK_2,
        ATTACK_1_2,
        ATTACK_1_T,
        ATTACK_2_T,
        ATTACK_1_2_T
    }
}