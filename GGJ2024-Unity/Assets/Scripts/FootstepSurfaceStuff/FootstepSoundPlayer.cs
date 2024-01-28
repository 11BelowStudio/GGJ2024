using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.FootstepSurfaceStuff
{
    /// <summary>
    /// bastardized version of https://github.com/llamacademy/get-terrain-texture/blob/main/Assets/Scripts/FootstepSoundPlayer.cs
    /// </summary>
    [RequireComponent(typeof(Collider), typeof(AudioSource))]
    public class FootstepSoundPlayer : MonoBehaviour
    {


        [SerializeField] TextureSoundData soundData;

        
        private LayerMask FloorLayer => soundData.FloorLayer;
        
        private TextureSound[] TextureSounds => soundData.TextureSounds;
        
        private bool BlendTerrainSounds => soundData.BlendTerrainSounds;

        private AudioSource AudioSource;

        private void Awake()
        {

            AudioSource = GetComponent<AudioSource>();
        }

        

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                StartCoroutine(PlayFootstepSoundFromTerrain(terrain, collision.GetContact(0).point));
            }
            /*
            else if (collision.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                StartCoroutine(PlayFootstepSoundFromRenderer(renderer));
            }
            */
        }

        

        private IEnumerator PlayFootstepSoundFromTerrain(Terrain Terrain, Vector3 HitPoint)
        {
            Vector3 terrainPosition = HitPoint - Terrain.transform.position;
            Vector3 splatMapPosition = new Vector3(
                terrainPosition.x / Terrain.terrainData.size.x,
                0,
                terrainPosition.z / Terrain.terrainData.size.z
            );

            int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
            int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);

            float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

            if (!BlendTerrainSounds)
            {
                int primaryIndex = 0;
                for (int i = 1; i < alphaMap.Length; i++)
                {
                    if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])
                    {
                        primaryIndex = i;
                    }
                }

                foreach (TextureSound textureSound in TextureSounds)
                {
                    if (textureSound.Albedo == Terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
                    {
                        AudioClip clip = GetClipFromTextureSound(textureSound);
                        AudioSource.PlayOneShot(clip);
                        yield return new WaitForSeconds(clip.length);
                        break;
                    }
                }
            }
            else
            {
                List<AudioClip> clips = new List<AudioClip>();
                int clipIndex = 0;
                for (int i = 0; i < alphaMap.Length; i++)
                {
                    if (alphaMap[0, 0, i] > 0)
                    {
                        foreach (TextureSound textureSound in TextureSounds)
                        {
                            if (textureSound.Albedo == Terrain.terrainData.terrainLayers[i].diffuseTexture)
                            {
                                AudioClip clip = GetClipFromTextureSound(textureSound);
                                AudioSource.PlayOneShot(clip, alphaMap[0, 0, i]);
                                clips.Add(clip);
                                clipIndex++;
                                break;
                            }
                        }
                    }
                }

                float longestClip = clips.Max(clip => clip.length);

                yield return new WaitForSeconds(longestClip);
            }
        }

        private IEnumerator PlayFootstepSoundFromRenderer(Renderer Renderer)
        {
            foreach (TextureSound textureSound in TextureSounds)
            {
                if (textureSound.Albedo == Renderer.material.GetTexture("_MainTex"))
                {
                    AudioClip clip = GetClipFromTextureSound(textureSound);

                    AudioSource.PlayOneShot(clip);
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }
        }

        private AudioClip GetClipFromTextureSound(TextureSound TextureSound)
        {
            int clipIndex = Random.Range(0, TextureSound.Clips.Length);
            return TextureSound.Clips[clipIndex];
        }

        
    }
}