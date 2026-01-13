#include "pch.h"

#include "NativeBridge.h"
#include <msclr/marshal_cppstd.h> // 마샬링을 위한 도우미 헤더
#using <CtbExtractor.dll> // C# DLL 참조

using namespace System;
using namespace System::Runtime::InteropServices;

extern "C" __declspec(dllexport) bool CallChituboxExtract(const char* path, const char* outDir) {
    if (path == nullptr || outDir == nullptr) return false;

    try {
        // 방법 1: Marshal 클래스를 이용한 안전한 변환
        String^ managedPath = Marshal::PtrToStringAnsi(IntPtr((char*)path));
        String^ managedOut = Marshal::PtrToStringAnsi(IntPtr((char*)outDir));

        // C# 클래스 호출: CtbExtractor 네임스페이스 안의 CtbExtractor 클래스 
        return CtbExtractor::CtbExtractor::ExtractAll(managedPath, managedOut);
    }
    // C++/CLI에서는 관리형 예외를 잡을 때 ^ 기호를 사용해야 합니다.
    catch (Exception^ ex) {
        System::Diagnostics::Debug::WriteLine("C++/CLI Bridge Error: " + ex->Message);
        return false;
    }
    catch (...) {
        // 모든 기타 예외 처리
        return false;
    }
}