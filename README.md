# CrawfisSoftware.Noise

`CrawfisSoftware.Noise` is a small .NET library that provides a couple of common procedural noise generators.

Targets:

- `.NET Standard 2.1`
- `.NET 8`

## Install

If you are consuming this as a NuGet package:

- `dotnet add package CrawfisSoftware.Noise`

## Namespace

All public types are under:

- `CrawfisSoftware.Noise`

## Noise ranges and conventions

Both generators in this library return `float` samples in the range `[0, 1]`.

- The `Sample(...)` overloads take continuous coordinates (float space).
- The `SampleLattice(...)` overloads take integer lattice coordinates and apply an optional `scale`.
- The `Calculate*Array(...)` helpers generate entire 1D/2D/3D grids by repeatedly sampling lattice coordinates.

## Classes

### `SimplexNoise`

Implementation of Perlin Simplex Noise (band-limited gradient noise).

Key points:

- Supports 1D, 2D, and 3D sampling.
- Deterministic when using the default constructor (fixed permutation table).
- Can be seeded by providing a `System.Random`, which generates a new permutation table.
  - Note: the constructor remarks mention that using randomized permutations may introduce low-frequency aliasing.

#### Constructors

- `SimplexNoise()`
  - Uses an internal, fixed permutation table (deterministic output across runs).
- `SimplexNoise(System.Random random)`
  - Fills permutations with random bytes from `random`.

#### Sampling API

- `float Sample(float x)`
- `float Sample(float x, float y)`
- `float Sample(float x, float y, float z)`

Each returns a `float` in `[0, 1]`.

#### Lattice helpers

- `float SampleLattice(int x, float scale = 1)`
- `float SampleLattice(int x, int y, float scale = 1)`
- `float SampleLattice(int x, int y, int z, float scale = 1)`

These call the corresponding `Sample(...)` overload using `x * scale`, `y * scale`, etc.

#### Array generation helpers

- `float[] Calculate1DArray(int width, float scale)`
- `float[,] Calculate2DArray(int width, int height, float scale)`
- `float[,,] Calculate3DArray(int width, int height, int depth, float scale)`

These are convenience methods for generating grids of samples.

#### Example

```csharp
using CrawfisSoftware.Noise;

var noise = new SimplexNoise();

// Single samples
float a = noise.Sample(0.1f);
float b = noise.Sample(12.3f, 4.56f);
float c = noise.Sample(1.0f, 2.0f, 3.0f);

// Generate a 2D heightmap
float[,] height = noise.Calculate2DArray(width: 256, height: 256, scale: 0.02f);
```

---

### `WhiteNoise`

White noise sampler backed by a precomputed table of random integers.

Key points:

- Supports 1D, 2D, and 3D sampling.
- Intended for fast, simple “random-looking” values (no gradient continuity).
- Deterministic if you pass a seeded `System.Random`.

Internals (useful for understanding output):

- A table of size `1024` is filled once in the constructor.
- Samples hash the input coordinates into the table by scaling and combining indices.
- Output is normalized into `[0, 1]` using `minRandomValue` and `maxRandomValue`.

#### Constructors

- `WhiteNoise(System.Random random, int minRandomValue = 0, int maxRandomValue = int.MaxValue - 1)`
  - Uses the provided RNG to populate the table.
  - Generated values are clamped to `[minRandomValue, maxRandomValue]`.
- `WhiteNoise(int minRandomValue = 0, int maxRandomValue = int.MaxValue)`
  - Uses a new `System.Random()`.

#### Sampling API

- `float Sample(float x)`
- `float Sample(float x, float y)`
- `float Sample(float x, float y, float z)`

Each returns a `float` in `[0, 1]`.

#### Lattice helpers

- `float SampleLattice(int x, float scale = 1)`
- `float SampleLattice(int x, int y, float scale = 1)`
- `float SampleLattice(int x, int y, int z, float scale = 1)`

#### Array generation helpers

- `float[] Calculate1DArray(int width, float scale)`
- `float[,] Calculate2DArray(int width, int height, float scale)`
- `float[,,] Calculate3DArray(int width, int height, int depth, float scale)`

#### Example

```csharp
using CrawfisSoftware.Noise;

// Deterministic white noise
var rng = new System.Random(1234);
var noise = new WhiteNoise(rng);

float s = noise.Sample(0.42f, 1.25f);
float[,] grid = noise.Calculate2DArray(128, 128, scale: 0.1f);
```

## Notes

- This library focuses on lightweight sampling primitives. Any higher-level composition (octaves/fBm, domain warping, tiling, etc.) should be built on top of these samplers.
