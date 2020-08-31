using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class NoiseController
    {
        //array of noises  from most granular to most rough/ each by a defined factor
        //somehow you cannot get the frequency from the FastNoise Obj directly so we need to link those two together manually
        private static NoiseWrapper[] Noises;
        private const float mostGranularNoiseFrequency = 0.021f;
        private const float noiseLayerFactor = 40;

        public NoiseController(int[] seeds)
        {
            Noises = new NoiseWrapper[seeds.Length];
            for (int i = 0; i < seeds.Length; i++)
            {
                NoiseWrapper noise = new NoiseWrapper(seeds[i]);
                noise.SetNoiseType(i < (seeds.Length / 2) ? FastNoise.NoiseType.Simplex : FastNoise.NoiseType.SimplexFractal);
                noise.SetFrequency(mostGranularNoiseFrequency / (i == 0 ? 1 : (noiseLayerFactor * i)));
                Noises[i] = noise;
            }
        }

        public void MultiplyFrequencyt(float ammount)
        {
            foreach (NoiseWrapper noise in Noises)
            {
                noise.SetFrequency(noise.Frequency * ammount);
            }
        }

        public float GetNormalizedNoise(float x, float y)
        {
            float avgVal = 0;
            foreach (NoiseWrapper noise in Noises)
            {
                avgVal += noise.GetValue(x,y);
            }
            avgVal = avgVal / Noises.Length; //average

            return (avgVal + 1) / 2; //normalize value from 0-1
        }
    }
}
