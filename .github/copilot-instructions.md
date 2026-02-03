
# Copilot repository instructions

## Conventional Commits (required)

When suggesting commit messages, PR titles, or release notes, always use **Conventional Commits** format:

- Format: `<type>(<scope>): <subject>`
- Allowed `type`: `build`, `chore`, `ci`, `docs`, `feat`, `fix`, `perf`, `refactor`, `revert`, `style`, `test`
- `scope` is optional but recommended for CI/release changes (e.g. `ci`, `release`, `deps`, `docs`).
- `subject` must be lower-case and must not end with a period.

If asked for a commit message, respond with a single Conventional Commit subject line only (no multi-line narrative), unless the user explicitly asks for a longer body.

Do not propose non-Conventional subjects like "Disable …" or "Update …" without a `type:` prefix.

Examples:
- `fix(ci): configure release-please`
- `chore(release): align release-please manifest to v1.2.1`
- `chore(ci): disable commitlint body max line length`
- `feat(noise): add turbulence helper`

## Release Please

- Tags must be prefixed with `v` (e.g. `v1.2.3`).
- Keep Release Please configuration in `.release-please-config.json` and manifest in `.release-please-manifest.json`.
- For this repo, the package path is `.` and the project file is `CrawfisSoftware.Noise.csproj`.
