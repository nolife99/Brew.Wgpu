#!/usr/bin/env bash
# generate.sh — regenerate Brew.Wgpu.Native on Linux (e.g. CI).
#
# PREFER generate.ps1 ON WINDOWS. Two Linux-only complications, both handled below
# except the last:
#   1. The ClangSharp dotnet tool ships only the *Windows* native libs, so we fetch
#      the linux-x64 libClangSharp.so + libclang.so from NuGet and drop them beside
#      the tool, plus the soname symlink libClangSharp needs (libclang.so.18.1).
#   2. Under the Linux data model uint64_t == `unsigned long`, which ClangSharp maps
#      to nuint (platform word size) — WRONG vs. the Windows-generated baseline
#      (ulong). Forcing the Windows model needs `-a --target=x86_64-pc-windows-msvc`
#      plus MSVC/CRT headers on the include path (clang will look for stdint.h,
#      math.h, ...). Setting that up is fiddly, which is why Windows is the
#      recommended host. Without it, this script emits nuint for uint64_t fields.
set -euo pipefail
FFI="${1:?usage: generate.sh <path to wgpu-native/ffi> [out-dir]}"
OUT="${2:-src/Native}"
VER=18.1.0.4
CS_RT=18.1.3.1     # libclangsharp.runtime.linux-x64
LC_RT=18.1.3.2     # libclang.runtime.linux-x64

export PATH="$HOME/.dotnet:$HOME/.dotnet/tools:$PATH"
export DOTNET_ROOT="${DOTNET_ROOT:-$HOME/.dotnet}" DOTNET_ROLL_FORWARD=Major

command -v ClangSharpPInvokeGenerator >/dev/null \
  || dotnet tool install -g ClangSharpPInvokeGenerator --version "$VER"

# Supply the linux natives the tool omits.
TN=$(find "$HOME/.dotnet/tools/.store/clangsharppinvokegenerator" -type d -path '*tools/net8.0/any' | head -1)
if [[ ! -f "$TN/libClangSharp.so" ]]; then
  tmp=$(mktemp -d)
  curl -sL "https://api.nuget.org/v3-flatcontainer/libclangsharp.runtime.linux-x64/$CS_RT/libclangsharp.runtime.linux-x64.$CS_RT.nupkg" -o "$tmp/cs.zip"
  curl -sL "https://api.nuget.org/v3-flatcontainer/libclang.runtime.linux-x64/$LC_RT/libclang.runtime.linux-x64.$LC_RT.nupkg" -o "$tmp/lc.zip"
  unzip -o -j "$tmp/cs.zip" 'runtimes/linux-x64/native/*' -d "$TN" >/dev/null
  unzip -o -j "$tmp/lc.zip" 'runtimes/linux-x64/native/*' -d "$TN" >/dev/null
  ln -sf "$TN/libclang.so" "$TN/libclang.so.18.1"   # libClangSharp's DT_NEEDED soname
fi
export LD_LIBRARY_PATH="$TN"

mkdir -p "$OUT"
ClangSharpPInvokeGenerator \
  --file              "$FFI/wgpu.h" \
  --include-directory "$FFI" \
  --include-directory "$FFI/webgpu-headers" \
  --traverse          "$FFI/wgpu.h" "$FFI/webgpu-headers/webgpu.h" \
  --namespace         Brew.Wgpu.Native \
  --methodClassName   WGPU \
  --libraryPath       wgpu_native \
  --output            "$OUT" \
  --config            multi-file latest-codegen generate-helper-types generate-macro-bindings generate-disable-runtime-marshalling

echo
echo "NOTE: on Linux, uint64_t fields come out as nuint. Diff against a Windows-"
echo "generated baseline before committing, or generate on Windows (recommended)."
