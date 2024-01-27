using UnityEngine;

namespace Scripts.Game.Pirates
{
    /// <summary>
    /// The captain is here!
    /// </summary>
    public class CaptainFeathersword : Pirate
    {

        /// <summary>
        /// the captain is the captain
        /// </summary>
        public override bool IsTheCaptain { get { return true; } }

        

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            float vertical = 0;
            if (Input.GetKey(KeyCode.W)) vertical += 1;
            if (Input.GetKey(KeyCode.S)) vertical -= 1;

            float horizontal = 0;
            if (Input.GetKey(KeyCode.D)) horizontal += 1;
            if (Input.GetKey(KeyCode.A)) horizontal -= 1;

            _mover.verticalInput = vertical;
            _mover.horizontalInput = horizontal;

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            {
                _mover.DoAttack1();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
            {
                _mover.DoAttack2();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
            {
                _mover.DoDed();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
            {
                _mover.DoNotDed();
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                _mover.DoTPose();
            }
        }
    }
}