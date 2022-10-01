using System;
using UnityEngine;

namespace FGWorms.Terrain
{
    public static class Noise
    {
        [Serializable]
        public struct NoiseOptions
        {
            [Min(1f)]
            public float Scale;
            public AnimationCurve RemapCurve;
            [Range(1, 5)]
            public int Octaves;
            [Range(0f, 1f)]
            public float Persistence;
            [Range(1f, 4f)]
            public float Lacunarity;
        }
        
        public static float[,] GenerateNoiseMap(int width, int height, int seed, NoiseOptions options)
        {
            float[,] noiseMap = new float[width, height];
            System.Random random = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[options.Octaves];
            for (int i = 0; i < options.Octaves; i++)
            {
                float offsetX = random.Next(-10000, 10000);
                float offsetY = random.Next(-10000, 10000);
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }
            
            options.Scale = Mathf.Clamp(options.Scale, 1e-3f, 1e5f);
            float maxNoiseHeight = float.MinValue, minNoiseHeight = float.MaxValue;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;
                    
                    for (int i = 0; i < options.Octaves; i++)
                    {
                        float sampleX = x / options.Scale * frequency + octaveOffsets[i].x;
                        float sampleY = y / options.Scale * frequency + octaveOffsets[i].y;

                        float noiseValue = 1 - 2 * Mathf.PerlinNoise(sampleX, sampleY);
                        noiseHeight += noiseValue * amplitude;

                        amplitude *= options.Persistence;
                        frequency *= options.Lacunarity;
                        
                    }
                    
                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }
                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                    noiseMap[x, y] = options.RemapCurve.Evaluate(noiseMap[x, y]);
                }
            }

            return noiseMap;
        }
    }
}