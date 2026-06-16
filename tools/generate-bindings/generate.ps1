#requires -Version 5
<#
  generate.ps1 — regenerate the raw Brew.Wgpu.Native bindings from a wgpu-native
  checkout's C headers, using ClangSharp.

  RUN THIS ON WINDOWS. The default clang target there is x86_64-pc-windows-msvc,
  under which uint64_t -> ulong and size_t -> nuint — which is what the existing
  bindings use and what is correct on every platform. (Generating on Linux instead
  makes uint64_t map to nuint, because uint64_t == `unsigned long` under the Linux
  data model and ClangSharp maps `unsigned long` to the platform word size. See
  generate.sh for the Linux story.)

  Prereq: apply your wgpu.h memory-hints patch to the checkout first, so the
  WGPUNativeMemoryHint enum + WGPUDeviceExtras.memoryHint/suballocatedBlockSize*
  fields are present in the header. (conv.rs is only needed to build the .so, not
  to generate bindings.)
#>
param(
  # Path to your (patched) wgpu-native checkout's ffi directory (has wgpu.h + webgpu-headers/).
  [string]$Ffi = "$PSScriptRoot\..\..\..\wgpu-native\ffi",
  # Where to write the generated .cs files.
  [string]$Out = "$PSScriptRoot\..\..\src\Native"
)
$ErrorActionPreference = 'Stop'

if (-not (Test-Path "$Ffi\wgpu.h")) {
  throw "wgpu.h not found under '$Ffi' — pass -Ffi <path to your wgpu-native\ffi>."
}

# Pin a version that packages correctly as a dotnet tool. (The latest 21.x failed
# to install as a tool at time of writing — "DotnetToolSettings.xml not found".)
if (-not (Get-Command ClangSharpPInvokeGenerator -ErrorAction SilentlyContinue)) {
  dotnet tool install --global ClangSharpPInvokeGenerator --version 18.1.0.4
}

New-Item -ItemType Directory -Force -Path $Out | Out-Null

ClangSharpPInvokeGenerator `
  --file              "$Ffi\wgpu.h" `
  --include-directory "$Ffi" `
  --include-directory "$Ffi\webgpu-headers" `
  --traverse          "$Ffi\wgpu.h" "$Ffi\webgpu-headers\webgpu.h" `
  --namespace         Brew.Wgpu.Native `
  --methodClassName   WGPU `
  --libraryPath       wgpu_native `
  --output            $Out `
  --config            multi-file latest-codegen generate-helper-types generate-macro-bindings generate-disable-runtime-marshalling

Write-Host ""
Write-Host "Generated bindings -> $Out"
Write-Host "Next:"
Write-Host "  * dotnet format   (file-scoped namespaces, usings, whitespace — per .editorconfig)"
Write-Host "  * docs: ClangSharp does NOT carry the header's doxygen into the bindings."
Write-Host "    Document the safe wrapper layer by hand, or post-process to inject XML docs."
