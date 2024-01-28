using Scripts.Game.Pirates;
using Scripts.Game.Manager;
using System.Collections;
using UnityEngine;
using Scripts.Utils.Types;
using Scripts.Menu;
using System;
using Scripts.Utils.Annotations;
using UnityEngine.SceneManagement;
using Scripts.Game.Menu;

namespace Scripts.Game
{
    public class GameManager : Singleton<GameManager>
    {

        [SerializeField]
        public CaptainFeathersword theCaptain;

        [SerializeField]
        private WaveManager _waveManager;

        [SerializeField]
        private InGameHUD _gameHUD;

        [SerializeField]
        private GameOverHUD _gameOverHUD;

        [SerializeField]
        [ReadOnly]
        private GameState _gameState = GameState.NOT_STARTED;


        private void Awake()
        {
            theCaptain.OnHealthChanged01 += _gameHUD.OnTicklishLevelChanged01;

            theCaptain.OnTakenDamage += OnCaptainHurt;
            theCaptain.OnDed += OnCaptainFeatherswordDefeated;

            _waveManager.OnWaveClear += _gameHUD.OnNewWaveStart;
        }

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

            _gameState = GameState.GAMERING;

            _waveManager.WaveComplete();
            

        }

        private void OnValidate()
        {
            _gameHUD = FindObjectOfType<InGameHUD>(true);
            _waveManager = FindObjectOfType<WaveManager>(true);
            _gameOverHUD = FindObjectOfType<GameOverHUD>(true);

        }

        private void OnCaptainHurt()
        {
            if (_gameState == GameState.NOT_STARTED)
            {
                // wtf you hurting the captain for before the game has even started????
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
        }



        public void OnCaptainFeatherswordDefeated()
        {
            if (_gameState == GameState.NOT_STARTED)
            {
                // wtf you killing the captain for before the game has even started????
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }

            _gameState = GameState.GAME_OVER;

            int wavesSurvived = _waveManager.WavesSurvived;
            _waveManager.enabled = false;
            // TODO: lose the game
            _gameOverHUD.gameObject.SetActive(true);
            _gameOverHUD.BadEnding(wavesSurvived);
        }
    }

    [Serializable]
    public enum GameState
    {
        NOT_STARTED,
        GAMERING,
        GAME_OVER
    }
}