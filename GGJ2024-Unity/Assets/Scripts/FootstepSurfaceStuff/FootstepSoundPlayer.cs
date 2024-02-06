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

        private Rigidbody _rb;

        private void Awake()
        {

            AudioSource = GetComponent<AudioSource>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            StartCoroutine(CheckGround());
        }


      
        
        private IEnumerator CheckGround()
        {
            while (true)
            {
                if (Mathf.Abs(_rb.velocity.y) <= 0.1f && _rb.velocity.sqrMagnitude >= 1f &&
                    Physics.Raycast(transform.position,
                        Vector3.down,
                        out RaycastHit hit,
                        0.2f,
                        FloorLayer)
                    )
                {
                    if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
                    {
                        yield return StartCoroutine(PlayFootstepSoundFromTerrain(terrain, hit.point));
                    }
                    /*
                    else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        yield return StartCoroutine(PlayFootstepSoundFromRenderer(renderer));
                    }*/
                }

                yield return null;
            }
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
                
                if (soundData.TryGetClipFromTexture(
                    Terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture,
                    out AudioClip playThisSound
                    )
                )
                {
                    AudioSource.PlayOneShot(playThisSound);
                    yield return new WaitForSeconds(playThisSound.length);
                }
                
                /*
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
                */
            }
            else
            {
                List<AudioClip> clips = new List<AudioClip>();
                int clipIndex = 0;
                for (int i = 0; i < alphaMap.Length; i++)
                {
                    if (alphaMap[0, 0, i] > 0)
                    {
                        if (soundData.TryGetClipFromTexture(
                            Terrain.terrainData.terrainLayers[i].diffuseTexture,
                            out AudioClip playThisSound
                            )
                        )
                        {
                            AudioSource.PlayOneShot(playThisSound, alphaMap[0, 0, i]);
                            clips.Add(playThisSound);
                            clipIndex++;
                        }
                        /*
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
                        */
                    }
                }

                float longestClip = clips.Max(clip => clip.length);

                yield return new WaitForSeconds(longestClip);
            }
        }

        private IEnumerator PlayFootstepSoundFromRenderer(Renderer Renderer)
        {

            if (soundData.TryGetClipFromTexture(
                Renderer.material.GetTexture("_MainTex"),
                out AudioClip playThisSound
                )
            )
            {
                AudioSource.PlayOneShot(playThisSound);
                yield return new WaitForSeconds(playThisSound.length);
            }
            /*
            foreach (TextureSound textureSound in TextureSounds)
            {
                if (textureSound.Albedo == Renderer.material.GetTexture("_MainTex"))
                {
                    AudioClip clip = GetClipFromTextureSound(textureSound);

                    AudioSource.PlayOneShot(clip);
                    yield return new WaitForSeconds(clip.length);
                    break;
                }
            }*/
        }

        private AudioClip GetClipFromTextureSound(TextureSound TextureSound)
        {
            int clipIndex = Random.Range(0, TextureSound.Clips.Length);
            return TextureSound.Clips[clipIndex];
        }

        
    }
}