#include "pch.h"

#include "NativeBridge.h"
#using <CtbExtractor.dll> // C# DLL 참조

using namespace System;
using namespace System::Runtime::InteropServices;

extern "C" __declspec(dllexport) bool __stdcall CallChituboxExtract(const char* path, const char* outDir) {
    try {
        // Native char*를 .NET String^으로 마샬링 (C++ -> C#)
        String^ managedPath = Marshal::PtrToStringAnsi((IntPtr)(char*)path);
        String^ managedOut = Marshal::PtrToStringAnsi((IntPtr)(char*)outDir);

        // C# 함수 호출
        return CtbExtractor::CtbExtractor::ExtractAll(managedPath, managedOut);
    }
    catch (...) {
        return false;
    }
}