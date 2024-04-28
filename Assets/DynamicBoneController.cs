using UnityEngine;

public class DynamicBoneWindController : MonoBehaviour
{
    [Header("Wind Settings")]
    [Tooltip("Strength of the wind affecting dynamic bones.")]
    [Range(0f, 10f)]
    public float windStrength = 1f;

    [Tooltip("Frequency of the wind oscillation.")]
    public float windFrequency = 1f;

    [Tooltip("Amplitude of the wind oscillation.")]
    public float windAmplitude = 0.1f;

    [Header("Dynamic Bone References")]
    [Tooltip("References to Dynamic Bone components to be affected by wind.")]
    public DynamicBone[] dynamicBones;

    private Vector3[] originalPositions; // Store original bone positions

    void Start()
    {
        // Store original positions of all dynamic bones
        originalPositions = new Vector3[dynamicBones.Length];
        for (int i = 0; i < dynamicBones.Length; i++)
        {
            originalPositions[i] = dynamicBones[i].transform.localPosition;
        }
    }

    void LateUpdate()
    {
        // Iterate through each Dynamic Bone component and apply wind effects
        for (int i = 0; i < dynamicBones.Length; i++)
        {
            // Calculate wind force for this bone based on wind strength and direction
            float windFactor = Mathf.Sin(Time.time * windFrequency) * windAmplitude * windStrength;
            Vector3 windForce = transform.right * windFactor; 

            // Apply wind force to the bone's position
            dynamicBones[i].transform.localPosition = originalPositions[i] + windForce;
        }
    }
}
