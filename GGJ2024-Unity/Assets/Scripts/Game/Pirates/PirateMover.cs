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

        [System.NonSerialized] public float verticalInput;
        [System.NonSerialized] public float horizontalInput;


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
        /// because the moving uses physics stuff, it looks like this needs to be done in fixedupdate
        /// </summary>
        private void FixedUpdate()
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 velocity = verticalInput * new Vector3(0, 0, speed);
            velocity.y = currentVelocity.y;
            rb.velocity = rb.rotation * velocity;

            Quaternion deltaRotation = Quaternion.Euler(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
            rb.MoveRotation(deltaRotation * rb.rotation);

            animator?.SetFloat(RUNNING, verticalInput, blendTreeDamping, Time.deltaTime);
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
        }

        /// <summary>
        /// provides move input as vector
        /// </summary>
        /// <param name="xy"></param>
        public void GiveMoveInput(Vector2 xy)
        {
            horizontalInput = xy.x;
            verticalInput = xy.y;
        }

        /// <summary>
        /// tells pirate to attempt attack 1
        /// </summary>
        public void DoAttack1()
        {
            animator.SetTrigger(ATTACK_1);
        }

        /// <summary>
        /// tells pirate to attempt attack 2
        /// </summary>
        public void DoAttack2()
        {
            animator.SetTrigger(ATTACK_2);
        }

        /// <summary>
        /// tells pirate to assert dominance
        /// </summary>
        public void DoTPose()
        {
            animator.SetTrigger(T_POSE);
        }

        /// <summary>
        /// tells pirate to be ded
        /// </summary>
        public void DoDed()
        {
            animator.SetTrigger(DED);
        }

        /// <summary>
        /// tells pirate to be not ded
        /// </summary>
        public void DoNotDed()
        {
            animator.SetTrigger(NOT_DED);
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