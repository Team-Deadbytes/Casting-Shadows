/**
This work is licensed under a Creative Commons Attribution 3.0 Unported License.
http://creativecommons.org/licenses/by/3.0/deed.en_GB

You are free:

to copy, distribute, display, and perform the work
to make derivative works
to make commercial use of the work
*/

using UnityEngine;
using UnityStandardAssets.ImageEffects;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/GlitchEffect")]
#endif
public class GlitchEffect : ImageEffectBase
{
    public Texture2D displacementMap;
    float glitchup, glitchdown, flicker,
            glitchupTime = 0.05f, glitchdownTime = 0.05f, flickerTime = 0.5f;

    [Header("Glitch Intensity")]

    [Range(0, 1)]
    public float intensity;

    [Range(0, 1)]
    public float flipIntensity;

    [Range(0, 1)]
    public float colorIntensity;
    [Tooltip("Maximum distance between player and monsters that should generate glitches.")]
    public float maxDistance;
    private float distBetween, _intensity, _colorIntensity, _flipIntensity, minDist;
    [Header("Proximity to monsters")]
    public GameObject[] monsters;
    public GameObject player; // two objects that generate noise if they are close together
    private GameObject minMonster;

    private static int ConvertRange(
        int originalStart, int originalEnd, // original range
        int newStart, int newEnd, // desired range
        int value) // value to convert
    {
        double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
        return (int)(newStart + ((value - originalStart) * scale));
    }
    void start()
    {
        if (monsters.Length == 0 || !player) // if objects are not set disable the effect
            enabled = false;
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        minDist = float.MaxValue; // set min dist to float max value
        if (monsters.Length != 0 || player) // if there are monsters and player
        {
            foreach (GameObject monster in monsters)
            {
                // if there is a monster with shorter distance to player update minDist and minMonster
                float tmpDist = Vector3.Distance(monster.transform.position, player.transform.position);
                if (tmpDist < minDist)
                {
                    minMonster = monster;
                    minDist = tmpDist;
                }
            }
            distBetween = Mathf.Clamp(Vector3.Distance(minMonster.transform.position, player.transform.position), 0.0f, maxDistance) / maxDistance; // get the distance between x and y in 0 to 1 formant
            distBetween = Mathf.Abs(distBetween - 1); // change from 0 to 1, to 1 to 0
        }
        // multiply effects with distance
        _intensity = intensity * distBetween;
        _colorIntensity = colorIntensity * distBetween;
        _flipIntensity = flipIntensity * distBetween; material.SetFloat("_Intensity", _intensity);

        // code from https://github.com/staffantan/unityglitch
        material.SetFloat("_ColorIntensity", _colorIntensity);
        material.SetTexture("_DispTex", displacementMap);

        flicker += Time.deltaTime * _colorIntensity;
        if (flicker > flickerTime)
        {
            material.SetFloat("filterRadius", Random.Range(-3f, 3f) * _colorIntensity);
            material.SetVector("direction", Quaternion.AngleAxis(Random.Range(0, 360) * _colorIntensity, Vector3.forward) * Vector4.one);
            flicker = 0;
            flickerTime = Random.value;
        }

        if (_colorIntensity == 0)
            material.SetFloat("filterRadius", 0);

        glitchup += Time.deltaTime * _flipIntensity;
        if (glitchup > glitchupTime)
        {
            if (Random.value < 0.1f * _flipIntensity)
                material.SetFloat("flip_up", Random.Range(0, 1f) * _flipIntensity);
            else
                material.SetFloat("flip_up", 0);

            glitchup = 0;
            glitchupTime = Random.value / 10f;
        }

        if (_flipIntensity == 0)
            material.SetFloat("flip_up", 0);


        glitchdown += Time.deltaTime * _flipIntensity;
        if (glitchdown > glitchdownTime)
        {
            if (Random.value < 0.1f * _flipIntensity)
                material.SetFloat("flip_down", 1 - Random.Range(0, 1f) * _flipIntensity);
            else
                material.SetFloat("flip_down", 1);

            glitchdown = 0;
            glitchdownTime = Random.value / 10f;
        }

        if (_flipIntensity == 0)
            material.SetFloat("flip_down", 1);

        if (Random.value < 0.05 * _intensity)
        {
            material.SetFloat("displace", Random.value * _intensity);
            material.SetFloat("scale", 1 - Random.value * _intensity);
        }
        else
            material.SetFloat("displace", 0);

        Graphics.Blit(source, destination, material);
    }
}
