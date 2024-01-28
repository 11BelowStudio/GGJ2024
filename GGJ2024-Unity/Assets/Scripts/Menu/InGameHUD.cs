using System;
using Scripts.Game;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts.Menu
{
    public class InGameHUD: MonoBehaviour
    {

        public GameManager manager;

        
		public TextMeshProUGUI waveCounter;
		

        public Slider disturbanceLevelSlider;

        public Slider cameraPowerSlider;

        private void Awake()
        {
            manager = GameManager.Instance;
			
			//waveCounter.text = "";

            //manager.OnCameraPowerLevelChanged += OnCameraPowerLevelChanged;
            //cameraPowerSlider.value = manager.CameraPowerLevel;
            //manager.OnDisturbanceLevelChanged01 += OnDisturbanceLevelChanged01;
            //disturbanceLevelSlider.value = manager.DisturbanceLevel01;
        }
		
		private void OnNewWaveStart(int newWave){
			waveCounter.text = $"{newWave}";
		}

        private void OnCameraPowerLevelChanged(float newPower)
        {
            cameraPowerSlider.value = newPower;
        }

        private void OnDisturbanceLevelChanged01(float newDisturbance01)
        {
            disturbanceLevelSlider.value = newDisturbance01;
        }
    }
}