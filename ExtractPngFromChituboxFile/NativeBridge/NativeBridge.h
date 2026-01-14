#pragma once

typedef void (*ProgressCallback)(int);
extern "C" __declspec(dllexport) bool CallChituboxExtract(const char* path, const char* outDir, ProgressCallback callback);