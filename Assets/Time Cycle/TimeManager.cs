using UnityEngine;

[ExecuteAlways]
public class TimeManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private TimePreset Preset;
    [SerializeField] private CloudsVolume cloudsVolume;

    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private Material[] cloudsMaterials;

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {

        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            float angle = (timePercent * 360f) - 90f;
if (angle < 0) angle += 360f; // Ensure the angle is positive
float horizonAngle = 90f; // Adjust as needed for your scene
if (timePercent < 0.25f || timePercent > 0.75f) // Nighttime or early morning
{
    horizonAngle = 0f; // Sun is below the horizon
}
DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3(angle, horizonAngle, 0));


        }

        if (cloudsVolume != null && cloudsMaterials != null && cloudsMaterials.Length > 0)
        {
            int materialIndex = Mathf.FloorToInt(timePercent * (cloudsMaterials.Length - 1));
            Material selectedMaterial = cloudsMaterials[materialIndex];
            cloudsVolume.SetCloudMaterial(selectedMaterial);
        }

        if (RenderSettings.skybox != null)
        {
            Material skyMaterial = GetSkyMaterialForTime(timePercent);
            RenderSettings.skybox = skyMaterial;
        }
    }

    private Material GetSkyMaterialForTime(float timePercent)
    {
        if (timePercent < 0.25f) // Assuming 6 AM to be the start of the day (0.00)
        {
            return Preset.skyNight;
        }
        else if (timePercent < 0.5f) // Assuming 6 PM to be the start of the night (0.25)
        {
            return Preset.skyOvercast;
        }
        else if (timePercent < 0.75f) // Assuming 6 PM to be the start of the night (0.25)
        {
            return Preset.skyDay;
        }
        else if (timePercent < 0.85f)
        {
            return Preset.skySunset;
        }
        else
        {
            return Preset.skyNight;
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = Resources.FindObjectsOfTypeAll<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
