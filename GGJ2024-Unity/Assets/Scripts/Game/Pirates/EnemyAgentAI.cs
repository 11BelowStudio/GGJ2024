using System;
using System.Collections;
using UnityEngine;
using Scripts.Utils.Annotations;
using UnityEngine.AI;

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


        private const float _minRandomChangeTime = 0.01f;
        private const float _maxRandomChangeTime = 5f;
        
        
        [SerializeField]
        private PirateMover _mover;

        [SerializeField]
        private EnemyPirate _pirate;
        
        

        [Tooltip("parameters for this bot's logic")]
        [ReadOnly][SerializeField] private EnemyAgentMoveBehaviours _moveBehaviour;

        [ReadOnly][SerializeField] private bool _randomMove;

        [ReadOnly][SerializeField] private EnemyAgentAttackBehaviours _attackBehaviour;

        [ReadOnly][SerializeField] private float _moveSpeed;

        [ReadOnly][SerializeField] private float _attackTimeMod;

        [ReadOnly][SerializeField] private float _attackChance;

        [ReadOnly][SerializeField] private float _distanceMod;

        [ReadOnly][SerializeField] private float _randomChangeDelay;

        [ReadOnly][SerializeField] private float _distanceTarget;
        [ReadOnly][SerializeField] private float _sqrDistanceTarget;

        

        [Tooltip("non-constant AI internal logic")]
        [ReadOnly][SerializeField] private float _attackTimer;
        [ReadOnly][SerializeField] private float _randomChangeTimer;

        [ReadOnly][SerializeField] private Vector3 destination;
        [ReadOnly][SerializeField] private Vector3 nextCorner;

        [SerializeField] private NavMeshPath _destinationPath;


        private bool _isDead;

        private void Awake()
        {
            _destinationPath = new NavMeshPath();


            _pirate.OnDed += OnPirateKilled;


            SetAIParams(
                RandomMoveBehaviour,
                true, //(UnityEngine.Random.value < 0.5),
                RandomAttackBehaviour,
                (UnityEngine.Random.value),
                (UnityEngine.Random.value),
                (UnityEngine.Random.value),
                (UnityEngine.Random.value),
                (UnityEngine.Random.value)
            );

        }

        /// <summary>
        /// Lobotomize the AI upon pirate death
        /// </summary>
        void OnPirateKilled()
        {
            this.enabled = false;
            
        }


        private void OnValidate()
        {
            _mover = GetComponent<PirateMover>();
            _pirate = GetComponent<EnemyPirate>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.up, destination + Vector3.up);

            Gizmos.DrawLine(transform.position + Vector3.up, nextCorner + Vector3.up);
            Gizmos.DrawCube(nextCorner + Vector3.up, Vector3.one);
        }


        public void SetAIParams(
            EnemyAgentMoveBehaviours moveType,
            bool randomMove,
            EnemyAgentAttackBehaviours attackType,
            float moveSpeedMod = 0.5f,
            float attackMod = 0.5f,
            float attackChance = 0.5f,
            float distanceMod = 0.5f,
            float randomTimeMod = 0.5f
        )
        {
            _moveBehaviour = moveType;
            _randomMove = randomMove;
            _attackBehaviour = attackType;
            _moveSpeed = _minMovePower + ( moveSpeedMod * _maxMovePower );
            _attackTimeMod = _minAttackAttemptDelay + ((_maxAttackAttemptDelay - _minAttackAttemptDelay) * (1 - attackMod));
            _attackChance = attackChance;
            _distanceMod = distanceMod;
            _randomChangeDelay = _minRandomChangeTime + ((_maxRandomChangeTime - _minRandomChangeTime) * (1 - randomTimeMod));

            CalculateDistanceTarget();
            destination = Vector3.zero;

            if (NavMesh.CalculatePath(
                transform.position, destination,
                NavMesh.GetAreaFromName("walkable"),
                _destinationPath
            ))
            {
                nextCorner = _destinationPath.corners[0];
                foreach(Vector3 corner in _destinationPath.corners)
                {
                    if (Vector3.Distance(transform.position, corner) > 3f)
                    {
                        nextCorner = corner;
                        break;
                    }
                }
                
            } else
            {
                nextCorner = destination;
            }

            
            _mover.GiveDestination(nextCorner);

        }


        private void FixedUpdate()
        {
            
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
            _sqrDistanceTarget = (_distanceTarget * _distanceTarget);
        }

        private void Update()
        {
            // step 1: work out where we're going
            Vector3 featherswordTarget = Vector3.zero;

            if (CaptainFeathersword.TryGetInstance( out var feathersword))
            {
                featherswordTarget = feathersword.transform.position;
            }

            
            
            Vector3 fromDestinationToPos = featherswordTarget - transform.position;


            float destToPosMag = Vector3.Distance(featherswordTarget, transform.position);

            Vector3 destToPosNorm = fromDestinationToPos / destToPosMag;

            destination = featherswordTarget + (destToPosNorm * _distanceTarget);


            //Vector3 fromNextCornerToPos = transform.position - nextCorner;

            if ( 
                // if fleeing and we've fleed far enough
                ((_moveBehaviour == EnemyAgentMoveBehaviours.FLEER) && (destToPosMag >= _distanceTarget))
                || // or
                // if not fleeing and we're close enough
                ((_moveBehaviour != EnemyAgentMoveBehaviours.FLEER) && (destToPosMag <= _distanceTarget))
            )
            {
                // we have reached the destination
                _mover.GiveLookTarget(featherswordTarget);

                // if we're doing random movement, we pick a new move behaviour
                if (_randomMove)
                {

                    

                    if (_randomChangeTimer > 0f)
                    {
                        _randomChangeTimer -= Time.deltaTime;
                    } else
                    {

                        _randomChangeTimer = UnityEngine.Random.Range(_minRandomChangeTime, _maxRandomChangeTime);

                        _moveBehaviour = RandomMoveBehaviour;
                        CalculateDistanceTarget();

                        destination = featherswordTarget + (destToPosNorm * _distanceTarget);

                        if (NavMesh.CalculatePath(
                            transform.position, destination,
                            NavMesh.GetAreaFromName("walkable"),
                            _destinationPath
                        ))
                        {
                            nextCorner = _destinationPath.corners[0];
                            foreach (Vector3 corner in _destinationPath.corners)
                            {
                                if (Vector3.Distance(transform.position, corner) > 3f)
                                {
                                    nextCorner = corner;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            nextCorner = destination;
                        }

                        _mover.GiveDestination(nextCorner);
                    }
                    
                    
                }
            }
            else
            {
                // we haven't reached our destination


                if (NavMesh.CalculatePath(
                        transform.position, destination,
                        NavMesh.GetAreaFromName("walkable"),
                        _destinationPath
                    ))
                {
                    nextCorner = _destinationPath.corners[0];
                    foreach (Vector3 corner in _destinationPath.corners)
                    {
                        if (Vector3.Distance(transform.position, corner) > 3f)
                        {
                            nextCorner = corner;
                            break;
                        }
                    }
                }
                else
                {
                    nextCorner = destination;
                }

                _mover.GiveDestination(nextCorner);

            }

            
            if (_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime;
            }
            else
            {
                _attackTimer = _attackTimeMod;

                if (UnityEngine.Random.value < _attackChance)
                {
                    switch (_attackBehaviour)
                    {
                        case EnemyAgentAttackBehaviours.PASSIVE:
                            break;
                        case EnemyAgentAttackBehaviours.T_POSER:
                            _mover.DoTPose();
                            break;
                        case EnemyAgentAttackBehaviours.ATTACK_1:
                            _mover.DoAttack1();
                            break;
                        case EnemyAgentAttackBehaviours.ATTACK_2:
                            _mover.DoAttack2();
                            break;
                        case EnemyAgentAttackBehaviours.ATTACK_1_2:
                            if (UnityEngine.Random.value < 0.5)
                            {
                                _mover.DoAttack1();
                            }
                            else
                            {
                                _mover.DoAttack2();
                            }
                            break;
                        case EnemyAgentAttackBehaviours.ATTACK_1_T:
                            if (UnityEngine.Random.value < 0.5)
                            {
                                _mover.DoAttack1();
                            }
                            else
                            {
                                _mover.DoTPose();
                            }
                            break;
                        case EnemyAgentAttackBehaviours.ATTACK_2_T:
                            if (UnityEngine.Random.value < 0.5)
                            {
                                _mover.DoAttack2();
                            }
                            else
                            {
                                _mover.DoTPose();
                            }
                            break;
                        case EnemyAgentAttackBehaviours.ATTACK_1_2_T:
                            float ayylmao = UnityEngine.Random.Range(0, 3);
                            if (ayylmao == 0)
                            {
                                _mover.DoAttack1();
                            }
                            else if (ayylmao == 1)
                            {
                                _mover.DoAttack2();
                            }
                            else
                            {
                                _mover.DoTPose();
                            }
                            break;
                    }
                }

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