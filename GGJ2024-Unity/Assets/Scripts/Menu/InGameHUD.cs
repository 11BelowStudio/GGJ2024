using System;
using Scripts.Game;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts.Menu
{
    public class InGameHUD: MonoBehaviour
    {

        
        
		public TextMeshProUGUI waveCounter;
		

        public Slider ticklishSlider;

        public CanvasGroup ticklishOverlay;
        

        private void Awake()
        {
            
			


			waveCounter.text = "";

            //manager.OnCameraPowerLevelChanged += OnCameraPowerLevelChanged;
            //cameraPowerSlider.value = manager.CameraPowerLevel;
            //manager.OnDisturbanceLevelChanged01 += OnDisturbanceLevelChanged01;
            //disturbanceLevelSlider.value = manager.DisturbanceLevel01;
        }
		
		public void OnNewWaveStart(int newWave){

            if (newWave >= 10) {
                waveCounter.text = $"<b>{newWave}</b>";
            }
            else
            {
                waveCounter.text = $"<b>Wave {newWave}</b>";
            }
			
		}

        

        public void OnTicklishLevelChanged01(float newTicklish01)
        {

            float newVal = 1f - newTicklish01;

            ticklishSlider.value = newVal;
            ticklishOverlay.alpha = newVal;
        }
    }
}