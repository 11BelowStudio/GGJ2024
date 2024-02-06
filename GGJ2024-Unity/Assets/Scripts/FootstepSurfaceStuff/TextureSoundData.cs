using System.Collections.Generic;
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

        [SerializeField]
        private Dictionary<Texture, AudioClip[]> textureClipsDict;

        private void Awake()
        {
            GenerateTextureClipsDict();
        }

        private void OnValidate()
        {
            GenerateTextureClipsDict();
        }

        private void GenerateTextureClipsDict()
        {
            textureClipsDict = new Dictionary<Texture, AudioClip[]>();
            foreach (var textureSounds in _textureSounds)
            {
                textureClipsDict[textureSounds.Albedo] = textureSounds.Clips;
            }
        }

        public bool TryGetClipFromTexture(Texture texture, out AudioClip _clip)
        {
            _clip = null;
            if (textureClipsDict.TryGetValue(texture, out AudioClip[] clips))
            {
                _clip = clips[Random.Range(0, clips.Length)];
                return true;
            }
            return false;
        }

    }

    [System.Serializable]
    public class TextureSound
    {
        public Texture Albedo;
        public AudioClip[] Clips;
    }
}