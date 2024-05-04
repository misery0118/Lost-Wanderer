using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="Time Preset", menuName ="Scriptables/Time Preset",order =1)]
public class TimePreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;

    public Material skyDay;
    public Material skyNight;
    public Material skySunset;
    public Material skyOvercast;
    
}
