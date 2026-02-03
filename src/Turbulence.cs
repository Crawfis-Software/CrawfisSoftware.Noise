using System;

namespace CrawfisSoftware.Noise
{
    /// <summary>
    /// Helper methods for generating turbulence from <see cref="SimplexNoise"/> by summing multiple octaves.
    /// </summary>
    /// <remarks>
    /// Turbulence is typically computed by summing the absolute value of a signed noise signal across octaves.
    /// Since <see cref="SimplexNoise"/> returns samples in <c>[0, 1]</c>, this implementation first remaps
    /// to <c>[-1, 1]</c> via <c>2*n - 1</c> and then takes the absolute value.
    /// </remarks>
    public static class Turbulence
    {
        /// <summary>
        /// Sample 1D turbulence.
        /// </summary>
        /// <param name="noise">The underlying simplex noise generator.</param>
        /// <param name="x">The x-coordinate in the infinite sample space.</param>
        /// <param name="octaves">Number of octaves to sum. Must be positive.</param>
        /// <param name="lacunarity">Frequency multiplier per octave. Must be positive.</param>
        /// <param name="gain">Amplitude multiplier per octave. Must be positive.</param>
        /// <returns>
        /// A non-negative value representing turbulence. Use <see cref="SampleNormalized(SimplexNoise, float, int, float, float)"/>
        /// to obtain a value roughly in <c>[0, 1]</c>.
        /// </returns>
        public static float Sample(SimplexNoise noise, float x, int octaves = 4, float lacunarity = 2.0f, float gain = 0.5f)
        {
            if (noise is null) throw new ArgumentNullException(nameof(noise));
            if (octaves <= 0) throw new ArgumentOutOfRangeException(nameof(octaves));
            if (lacunarity <= 0) throw new ArgumentOutOfRangeException(nameof(lacunarity));
            if (gain <= 0) throw new ArgumentOutOfRangeException(nameof(gain));

            float sum = 0.0f;
            float frequency = 1.0f;
            float amplitude = 1.0f;

            for (int i = 0; i < octaves; i++)
            {
                var n = noise.Sample(x * frequency);
                sum += MathF.Abs(2.0f * n - 1.0f) * amplitude;
                frequency *= lacunarity;
                amplitude *= gain;
            }

            // Not strictly normalized; caller can scale if desired.
            return sum;
        }

        /// <summary>
        /// Sample 2D turbulence.
        /// </summary>
        /// <inheritdoc cref="Sample(SimplexNoise, float, int, float, float)"/>
        /// <param name="y">The y-coordinate in the infinite sample space.</param>
        public static float Sample(SimplexNoise noise, float x, float y, int octaves = 4, float lacunarity = 2.0f, float gain = 0.5f)
        {
            if (noise is null) throw new ArgumentNullException(nameof(noise));
            if (octaves <= 0) throw new ArgumentOutOfRangeException(nameof(octaves));
            if (lacunarity <= 0) throw new ArgumentOutOfRangeException(nameof(lacunarity));
            if (gain <= 0) throw new ArgumentOutOfRangeException(nameof(gain));

            float sum = 0.0f;
            float frequency = 1.0f;
            float amplitude = 1.0f;

            for (int i = 0; i < octaves; i++)
            {
                var n = noise.Sample(x * frequency, y * frequency);
                sum += MathF.Abs(2.0f * n - 1.0f) * amplitude;
                frequency *= lacunarity;
                amplitude *= gain;
            }

            return sum;
        }

        /// <summary>
        /// Sample 3D turbulence.
        /// </summary>
        /// <inheritdoc cref="Sample(SimplexNoise, float, int, float, float)"/>
        /// <param name="y">The y-coordinate in the infinite sample space.</param>
        /// <param name="z">The z-coordinate in the infinite sample space.</param>
        public static float Sample(SimplexNoise noise, float x, float y, float z, int octaves = 4, float lacunarity = 2.0f, float gain = 0.5f)
        {
            if (noise is null) throw new ArgumentNullException(nameof(noise));
            if (octaves <= 0) throw new ArgumentOutOfRangeException(nameof(octaves));
            if (lacunarity <= 0) throw new ArgumentOutOfRangeException(nameof(lacunarity));
            if (gain <= 0) throw new ArgumentOutOfRangeException(nameof(gain));

            float sum = 0.0f;
            float frequency = 1.0f;
            float amplitude = 1.0f;

            for (int i = 0; i < octaves; i++)
            {
                var n = noise.Sample(x * frequency, y * frequency, z * frequency);
                sum += MathF.Abs(2.0f * n - 1.0f) * amplitude;
                frequency *= lacunarity;
                amplitude *= gain;
            }

            return sum;
        }

        /// <summary>
        /// Sample 1D turbulence and normalize by the theoretical maximum amplitude.
        /// </summary>
        /// <returns>A value roughly in <c>[0, 1]</c>.</returns>
        public static float SampleNormalized(SimplexNoise noise, float x, int octaves = 4, float lacunarity = 2.0f, float gain = 0.5f)
        {
            return Normalize(Sample(noise, x, octaves, lacunarity, gain), octaves, gain);
        }

        /// <summary>
        /// Sample 2D turbulence and normalize by the theoretical maximum amplitude.
        /// </summary>
        /// <returns>A value roughly in <c>[0, 1]</c>.</returns>
        public static float SampleNormalized(SimplexNoise noise, float x, float y, int octaves = 4, float lacunarity = 2.0f, float gain = 0.5f)
        {
            return Normalize(Sample(noise, x, y, octaves, lacunarity, gain), octaves, gain);
        }

        /// <summary>
        /// Sample 3D turbulence and normalize by the theoretical maximum amplitude.
        /// </summary>
        /// <returns>A value roughly in <c>[0, 1]</c>.</returns>
        public static float SampleNormalized(SimplexNoise noise, float x, float y, float z, int octaves = 4, float lacunarity = 2.0f, float gain = 0.5f)
        {
            return Normalize(Sample(noise, x, y, z, octaves, lacunarity, gain), octaves, gain);
        }

        private static float Normalize(float value, int octaves, float gain)
        {
            float max = 0.0f;
            float amp = 1.0f;
            for (int i = 0; i < octaves; i++)
            {
                max += amp;
                amp *= gain;
            }

            return max > 0 ? value / max : 0;
        }
    }
}
