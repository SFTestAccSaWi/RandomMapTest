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
    public class NoiseWrapper : Singleton<NoiseWrapper>
    {
        public FastNoise noise;
        private static float currentFrequency;

        public NoiseWrapper(int seed)
        {
            noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.Simplex);
            currentFrequency = 0.021f;
            noise.SetFrequency(currentFrequency);
        }

        public void SetSeed(int seed) => noise.SetSeed(seed);

        public void MultiplyFrequencyt(float ammount)
        {
            currentFrequency *= ammount;
            noise.SetFrequency(currentFrequency);
        }

        public float GetNoise(float x, float y) => noise.GetNoise(x, y);
        public float GetNoise(int x, int y) => noise.GetNoise(x, y);
    }
}
