namespace CrawfisSoftware.Noise
{
    public class WhiteNoise
    {
        private readonly System.Random _random;
        private const int _width = 1024;
        const int _heightOffset = 33;
        const int _depthOffset = 17;
        private int[] _randomValues = new int[_width];
        private int _minRandomValue;
        private int _maxRandomValue;
        private float _scaleFactor;

        public WhiteNoise(System.Random random, int minRandomValue = 0, int maxRandomValue = int.MaxValue-1)
        {
            _random = random;
            _minRandomValue = minRandomValue;
            _maxRandomValue = maxRandomValue;
            _scaleFactor = 1.0f / (_maxRandomValue - _minRandomValue);
            _randomValues = new int[_width];
            for(int i = 0; i < _width; i++)
            {
                int randomValue = _random.Next();
                if(randomValue < _minRandomValue) randomValue = _minRandomValue;
                if(randomValue > _maxRandomValue) randomValue = _maxRandomValue;
                _randomValues[i] = randomValue;
            }
        }

        public WhiteNoise(int minRandomValue = 0, int maxRandomValue = int.MaxValue) : this(new System.Random(), minRandomValue, maxRandomValue) { }


        /// <summary>
        /// Sample 1D noise and return an array of floats.
        /// </summary>
        /// <param name="width">The width of the desired array.</param>
        /// <param name="scale">A scale factor to use in the noise generation.</param>
        /// <returns>A 1D array of floats.</returns>
        public float[] Calculate1DArray(int width, float scale)
        {
            var values = new float[width];
            for (var i = 0; i < width; i++)
                values[i] = SampleLattice(i, scale);
            return values;
        }

        /// <summary>
        /// Sample 2D noise and return an array of floats.
        /// </summary>
        /// <param name="width">The width of the desired array.</param>
        /// <param name="height">The height of the desired array.</param>
        /// <param name="scale">A scale factor to use in the noise generation.</param>
        /// <returns>A 1D array of floats.</returns>
        public float[,] Calculate2DArray(int width, int height, float scale)
        {
            var values = new float[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    values[i, j] = SampleLattice(i, j, scale);
            return values;
        }

        /// <summary>
        /// Sample 3D noise and return an array of floats.
        /// </summary>
        /// <param name="width">The width of the desired array.</param>
        /// <param name="height">The height of the desired array.</param>
        /// <param name="depth">The depth of the desired array.</param>
        /// <param name="scale">A scale factor to use in the noise generation.</param>
        /// <returns>A 1D array of floats.</returns>
        public float[,,] Calculate3DArray(int width, int height, int depth, float scale)
        {
            var values = new float[width, height, depth];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                    for (var k = 0; k < depth; k++)
                        values[i, j, k] = SampleLattice(i, j, k, scale);
            return values;
        }

        /// <summary>
        /// Sample a 1D White Noise
        /// </summary>
        /// <param name="x">An integer coordinate</param>
        /// <param name="scale">A float to scale the integer coordinate with.</param>
        /// <returns>A float.</returns>
        /// <remarks>Calls Sample with scaled float(s).</remarks>
        public float SampleLattice(int x, float scale = 1)
        {
            return Sample(x * scale);
        }

        /// <summary>
        /// Sample a 2D White Noise
        /// </summary>
        /// <param name="x">An integer coordinate</param>
        /// <param name="y">An integer coordinate</param>
        /// <param name="scale">A float to scale the integer coordinates with.</param>
        /// <returns>A float.</returns>
        /// <remarks>Calls Sample with scaled float(s).</remarks>
        public float SampleLattice(int x, int y, float scale = 1)
        {
            return Sample(x * scale, y * scale);
        }

        /// <summary>
        /// Sample a 3D White Noise
        /// </summary>
        /// <param name="x">An integer coordinate</param>
        /// <param name="y">An integer coordinate</param>
        /// <param name="z">An integer coordinate</param>
        /// <param name="scale">A float to scale the integer coordinates with.</param>
        /// <returns>A float.</returns>
        /// <remarks>Calls Sample with scaled float(s).</remarks>
        public float SampleLattice(int x, int y, int z, float scale = 1)
        {
            return Sample(x * scale, y * scale, z * scale);
        }

        /// <summary>
        /// Sample 1D simplex noise
        /// </summary>
        /// <param name="x">The x-coordinate in the infinite sample space.</param>
        /// <returns>A sample of the band-limited noise.</returns>
        public float Sample(float x)
        {
            int ix = (int)(x * _width);
            ix = ix % _width;
            if(ix < 0) ix += _width;
            return _scaleFactor * (_randomValues[ix] - _minRandomValue);
        }

        /// <summary>
        /// Sample 2D simplex noise
        /// </summary>
        /// <param name="x">The x-coordinate in the infinite sample space.</param>
        /// <param name="y">The y-coordinate in the infinite sample space.</param>
        /// <returns>A sample of the band-limited noise.</returns>
        public float Sample(float x, float y)
        {
            int ix = (int)(x * _width);
            int iy = (int)(y * _width * _heightOffset);
            ix += iy;
            ix = ix % _width;
            if (ix < 0) ix += _width;
            return _scaleFactor * (_randomValues[ix] - _minRandomValue);
        }

        /// <summary>
        /// Sample 2D simplex noise
        /// </summary>
        /// <param name="x">The x-coordinate in the infinite sample space.</param>
        /// <param name="y">The y-coordinate in the infinite sample space.</param>
        /// <param name="z">The z-coordinate in the infinite sample space.</param>
        /// <returns>A sample of the band-limited noise.</returns>
        public float Sample(float x, float y, float z)
        {
            int ix = (int)(x * _width);
            int iy = (int)(y * _width * _heightOffset);
            int iz = (int)(z * _width * _depthOffset);
            ix += iy + iz;
            ix = ix % _width;
            if (ix < 0) ix += _width;
            return _scaleFactor * (_randomValues[ix] - _minRandomValue);
        }
    }
}