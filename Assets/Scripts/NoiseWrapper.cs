using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlTypes;
using UnityEngine;

namespace Assets
{
    public class NoiseWrapper 
    {
        private FastNoise noise;
        private int Seed;
        public float Frequency { get; private set; }

        public NoiseWrapper(int seed)
        {
            noise = new FastNoise(seed);
            Seed = seed;
        }

        public void SetFrequency(float frequency)
        {
            Frequency = frequency;
            noise.SetFrequency(Frequency);
        }

        public float GetValue(float x, float y) => noise.GetValue(x, y);

        public void SetNoiseType(FastNoise.NoiseType noiseType) => noise.SetNoiseType(noiseType);
    }
}
