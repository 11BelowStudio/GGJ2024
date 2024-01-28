using Scripts.Game.Pirates;
using Scripts.Game.Manager;
using System.Collections;
using UnityEngine;
using Scripts.Utils.Types;

namespace Scripts.Game
{
    public class GameManager : Singleton<GameManager>
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

        // it is time to commence the gamering.
        public void ItsGamerTime()
        {

        }


        public void OnCaptainFeatherswordDefeated()
        {
            // TODO: lose the game
        }
    }
}