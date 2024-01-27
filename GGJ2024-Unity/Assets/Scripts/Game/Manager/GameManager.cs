using Scripts.Game.Pirates;
using System.Collections;
using UnityEngine;

namespace Scripts.Game.Manager
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField]
        public CaptainFeathersword theCaptain;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


            // escape button to quit
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }


        public void OnCaptainFeatherswordDefeated()
        {
            // TODO: lose the game
        }
    }
}