using FGWorms.Terrain;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainConfig", menuName = "Terrain/Terrain Config")]
public class TerrainConfigSO : ScriptableObject
{
    public string Name;
    public Noise.NoiseOptions NoiseOptions;
    public float HeightMultiplier;
    public Texture2D GroundTexture;
}
