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
        private FastNoise GranularNoise;
        private FastNoise RoughNoise;
        private static float currentGranularFrequency;
        private static float currentRoughFrequency;

        public NoiseWrapper(int seedGranular, int seedRough)
        {
            GranularNoise = new FastNoise(seedGranular);
            GranularNoise.SetNoiseType(FastNoise.NoiseType.Simplex);
            currentGranularFrequency = 0.021f;
            GranularNoise.SetFrequency(currentGranularFrequency);

            RoughNoise = new FastNoise(seedRough);
            RoughNoise.SetNoiseType(FastNoise.NoiseType.Simplex);
            currentRoughFrequency = 0.081f;
            RoughNoise.SetFrequency(currentRoughFrequency);
        }

        public void MultiplyFrequencyt(float ammount)
        {
            currentGranularFrequency *= ammount;
            GranularNoise.SetFrequency(currentGranularFrequency);

            currentRoughFrequency *= ammount;
            RoughNoise.SetFrequency(currentRoughFrequency);
        }

        public float GetNormalizedNoise(float x, float y)
        {
           return (GranularNoise.GetNoise(x, y) + 1)/2;
        }


    }
}
