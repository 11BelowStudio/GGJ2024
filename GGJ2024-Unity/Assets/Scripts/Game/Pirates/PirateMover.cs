using Scripts.Utils.Annotations;
using Scripts.Utils.Extensions.Vectors;
using UnityEngine;

namespace Scripts.Game.Pirates
{
    /// <summary>
    /// Class that recieves movement inputs to get the pirate moving
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PirateMover : MonoBehaviour
    {
        public float speed = 10;
        public float rotationSpeed = 60;
        public float blendTreeDamping = 0.1f;

        public float _movePower = 1f;

        [System.NonSerialized] public float verticalInput;
        [System.NonSerialized] public float horizontalInput;

        [System.NonSerialized] public Vector3 _destination;


        // these are all the names of the parameters in the animator
        const string RUNNING = "Running";
        const string ATTACK_1 = "attack_1";
        const string ATTACK_2 = "attack_2";
        const string DED = "ded";
        const string NOT_DED = "not_ded";
        const string T_POSE = "t_pose";

        [SerializeField] Rigidbody rb;
        [SerializeField] Animator animator;

        /// <summary>
        /// if true, we're using x/y inputs like player inputs
        /// if false, we're using a desired destination to work shit out
        /// </summary>
        [ReadOnly] private bool use_inputs = true;

        [ReadOnly] private bool move_to_dest = true;

        private bool is_ded = false;


        /// <summary>
        /// because the moving uses physics stuff, it looks like this needs to be done in fixedupdate
        /// </summary>
        private void FixedUpdate()
        {
            Vector3 currentVelocity = rb.velocity;
            
            if (is_ded)
            {

                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                rb.rotation = rb.rotation;
                return;
            }
            
            if (use_inputs) {
                Vector3 velocity = verticalInput * new Vector3(0, 0, speed);
                velocity.y = currentVelocity.y;
                rb.velocity = rb.rotation * velocity;


                Quaternion deltaRotation = Quaternion.Euler(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
                rb.MoveRotation(deltaRotation * rb.rotation);

                animator?.SetFloat(RUNNING, verticalInput, blendTreeDamping, Time.deltaTime);
            }
            else
            {
                Vector3 posToDestination = (_destination - rb.position).GetFlattened(); // we only care about XZ



                float tempVerticalInput = Mathf.Min((speed * _movePower), posToDestination.magnitude);

                Vector3 velocity = new Vector3(0, 0, tempVerticalInput * _movePower);

                if (move_to_dest)
                {

                    velocity.y = currentVelocity.y;

                    rb.velocity = rb.rotation * velocity;
                    
                    
                    Vector3 myForward = transform.forward;

                    Quaternion forwardToDest = Quaternion.RotateTowards(
                        Quaternion.Euler(myForward), Quaternion.Euler((posToDestination.normalized)), (rotationSpeed * Time.fixedDeltaTime));

                    //Vector3 eulerRot = forwardToDest.eulerAngles;
                    //eulerRot.x = 0;
                    //eulerRot.z = 0;
                    //forwardToDest.eulerAngles = eulerRot;

                    /*
                    rb.MoveRotation(
                        //Quaternion.FromToRotation(rb.rotation.eulerAngles, posToDestination.normalized)
                        //Quaternion.Lerp(rb.rotation, Quaternion.Euler(posToDestination.normalized), 0.5f)
                        //Quaternion.LookRotation(posToDestination.normalized)
                        );
                    */

                    //rb.rotation.SetLookRotation(_destination)

                    rb.MoveRotation(Quaternion.LookRotation(_destination - transform.position));

                    //rb.MoveRotation(forwardToDest);

                    //rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(posToDestination.normalized), Time.fixedDeltaTime));


                } else
                {
                    velocity = currentVelocity.MultXYZ(0, 1, 0);
                    rb.velocity = velocity;

                    rb.MoveRotation(
                        Quaternion.LookRotation(
                            (_destination - rb.position)));
                }

                
                animator?.SetFloat(RUNNING, Mathf.Min(_movePower, velocity.z), blendTreeDamping, Time.deltaTime);
                
                

            }
            
        }

        public void GiveMovePower(float movePower)
        {
            _movePower = movePower;
        }

        /// <summary>
        /// provides move input
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        public void GiveMoveInput(float horizontal, float vertical)
        {
            horizontalInput = horizontal;
            verticalInput = vertical;
            use_inputs = true;
        }

        public void GiveDestination(Vector3 destination)
        {
            _destination = destination;
            move_to_dest = true;
            use_inputs = false;
        }

        public void GiveLookTarget(Vector3 lookTarget)
        {
            _destination = lookTarget;
            use_inputs = false;
            move_to_dest = false;
        }

        /// <summary>
        /// provides move input as vector
        /// </summary>
        /// <param name="xy"></param>
        public void GiveMoveInput(Vector2 xy)
        {
            horizontalInput = xy.x;
            verticalInput = xy.y;
            use_inputs = true;
        }

        /// <summary>
        /// tells pirate to attempt attack 1
        /// </summary>
        public void DoAttack1()
        {
            if (is_ded) { return; }
            animator.SetTrigger(ATTACK_1);
        }

        /// <summary>
        /// tells pirate to attempt attack 2
        /// </summary>
        public void DoAttack2()
        {
            if (is_ded) { return; }
            animator.SetTrigger(ATTACK_2);
        }

        /// <summary>
        /// tells pirate to assert dominance
        /// </summary>
        public void DoTPose()
        {
            if (is_ded) { return;  }
            animator.SetTrigger(T_POSE);
        }

        /// <summary>
        /// tells pirate to be ded
        /// </summary>
        public void DoDed()
        {
            animator.SetTrigger(DED);
            rb.velocity = Vector3.zero;
            is_ded = true;
        }

        /// <summary>
        /// tells pirate to be not ded
        /// </summary>
        public void DoNotDed()
        {
            animator.SetTrigger(NOT_DED);
            is_ded = false;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();

            ConfigureRigidbody();
        }

        private void ConfigureRigidbody()
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
    }
}