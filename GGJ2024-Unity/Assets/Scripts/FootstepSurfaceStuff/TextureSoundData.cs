using System.Collections;
using UnityEngine;

namespace Scripts.FootstepSurfaceStuff
{
    [CreateAssetMenu(menuName = "TextureSound/TextureSoundData", fileName = "TextureSoundData")]
    public class TextureSoundData : ScriptableObject
    {

        [SerializeField]
        private LayerMask _floorLayer;
        [SerializeField]
        private TextureSound[] _textureSounds;
        [SerializeField]
        private bool _blendTerrainSounds;

        public LayerMask FloorLayer => _floorLayer;

        public TextureSound[] TextureSounds => _textureSounds;

        public bool BlendTerrainSounds => _blendTerrainSounds;

    }

    [System.Serializable]
    public class TextureSound
    {
        public Texture Albedo;
        public AudioClip[] Clips;
    }
}