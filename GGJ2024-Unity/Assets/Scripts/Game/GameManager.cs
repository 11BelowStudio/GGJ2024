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
using Scripts.Utils.Placeholders;

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

        public GameState gameState => _gameState;

        [SerializeField]
        private AudioHolder _titleThemeHolder;

        [SerializeField]
        private AudioHolder _gameThemeHolder;

        [SerializeField]
        private AudioHolder _gameOverThemeHolder;

        [SerializeField]
        private AudioHolder _gameOverNoiseHolder;

        public const string K_PLAYERPREFS_HIGH_SCORE = "K_PLAYERPREFS_HIGH_SCORE";

        private void Awake()
        {
            theCaptain.OnHealthChanged01 += _gameHUD.OnTicklishLevelChanged01;

            theCaptain.OnTakenDamage += OnCaptainHurt;
            theCaptain.OnDed += OnCaptainFeatherswordDefeated;

            _waveManager.OnWaveClear += _gameHUD.OnNewWaveStart;

            if (_titleThemeHolder.TryGetRandomAudioClip(out AudioClip titleTheme))
            {
                theCaptain.GameCameraAudioSource.clip = titleTheme;
                theCaptain.GameCameraAudioSource.loop = true;
            }
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

            if (_gameThemeHolder.TryGetRandomAudioClip(out AudioClip gameTheme))
            {
                theCaptain.GameCameraAudioSource.clip = gameTheme;
                theCaptain.GameCameraAudioSource.loop = true;
            }
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

            if (_gameOverNoiseHolder.TryGetRandomAudioClip(out AudioClip gameOverNoise))
            {
                theCaptain.GameCameraAudioSource.PlayOneShot(gameOverNoise);
            }

            if (_gameOverThemeHolder.TryGetRandomAudioClip(out AudioClip gameOverTheme))
            {
                theCaptain.GameCameraAudioSource.clip = gameOverTheme;
                theCaptain.GameCameraAudioSource.loop = true;
            }

            int wavesSurvived = _waveManager.WavesSurvived;

            int highestScore = PlayerPrefs.GetInt(K_PLAYERPREFS_HIGH_SCORE);
            if (wavesSurvived > highestScore)
            {
                PlayerPrefs.SetInt(K_PLAYERPREFS_HIGH_SCORE, wavesSurvived);
            }

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