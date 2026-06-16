# Publishing Brew.Wgpu to NuGet

Publishing is **manual** (`workflow_dispatch`) and targets **nuget.org**.

## One-time setup

1. Create an API key at https://www.nuget.org/account/apikeys
   (scope it to push for `Brew.Wgpu`, or "Push new packages and package versions").
2. Add it as a repository secret:
   **Settings → Secrets and variables → Actions → New repository secret**
   - Name: `NUGET_API_KEY`
   - Value: *(the key)*

## Publish a version

**Actions → `release` → Run workflow**, then fill in:

| Input       | Meaning |
|-------------|---------|
| `version`   | Package version, e.g. `29.0.0` or `29.0.0-rc.1`. Drives the `.nupkg` version (the csproj `<Version>` is only a dev placeholder). |
| `wgpu_repo` | wgpu-native repo to build. Use **your fork** to get the memory-hints build; leave default for stock upstream. |
| `wgpu_ref`  | git ref to build (tag / branch / sha). |
| `dry_run`   | If checked, packs but does **not** push — use this to validate packing first. |

The workflow then:

1. builds wgpu-native for every RID (`build-wgpu-native.yml`, reused),
2. assembles `runtimes/<rid>/native/*`,
3. `dotnet pack` → `Brew.Wgpu.<version>.nupkg` (+ `.snupkg` symbols), natives included,
4. pushes both to nuget.org (`--skip-duplicate`).

## Validate before publishing

`dotnet pack` compiles the assembly first. The project currently builds clean (**0 errors**) on
`net6.0` and `net8.0` and packs locally, so the **Pack** step is good to go. Recommended workflow:

- run once with **`dry_run` checked** — this packs (and validates the native layout) without pushing,
- then uncheck it for the real publish.

If you ever want a **bindings-only** package (e.g. to ship the raw layer ahead of the wrapper),
exclude the wrapper from compilation (add `<Compile Remove="src/*.cs" />`, keeping `src/Native/**`
and `src/Handles/**`).

## Versioning

Use SemVer. A common convention for wgpu bindings is to track the upstream wgpu-native
version (e.g. `29.0.0`) and add a 4th component or `-rc`/`-preview` suffix for binding revisions.

## Adding 32-bit Windows (`win-x86`)

Enable the `win-x86` row (commented) in `.github/workflows/build-wgpu-native.yml`:

```yaml
- { rid: win-x86, os: windows-latest, target: i686-pc-windows-msvc, lib: wgpu_native.dll }
```

No code changes are needed — the bindings are generated with the Windows type model, so
`uint64_t → ulong` and `size_t → nuint` are already correct on 32-bit.
