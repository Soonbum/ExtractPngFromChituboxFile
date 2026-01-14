#include "pch.h"
#include "NativeBridge.h"
#include <msclr/marshal_cppstd.h> // 마샬링을 위한 도우미 헤더
#using <CtbExtractor.dll> // C# DLL 참조

using namespace System;
using namespace System::Runtime::InteropServices;

ref class ProgressWrapper {
private:
    ProgressCallback _nativeCallback;
public:
    ProgressWrapper(ProgressCallback callback) : _nativeCallback(callback) {}

    // Action<int>가 요구하는 시그니처와 일치하는 메서드
    void UpdateProgress(int progress) {
        if (_nativeCallback != nullptr) {
            _nativeCallback(progress);
        }
    }
};

extern "C" __declspec(dllexport) bool CallChituboxExtract(const char* path, const char* outDir, ProgressCallback callback) {
    if (path == nullptr || outDir == nullptr) return false;

    try {
        // Marshal 클래스를 이용한 안전한 변환
        String^ managedPath = Marshal::PtrToStringAnsi(IntPtr((char*)path));
        String^ managedOut = Marshal::PtrToStringAnsi(IntPtr((char*)outDir));

        Action<int>^ progressAction = nullptr;
        if (callback != nullptr) {
            // 래퍼 클래스 객체를 생성하고 메서드를 대리자에 연결
            ProgressWrapper^ wrapper = gcnew ProgressWrapper(callback);
            progressAction = gcnew Action<int>(wrapper, &ProgressWrapper::UpdateProgress);
        }

        // C# 클래스 호출: CtbExtractor 네임스페이스 안의 CtbExtractor 클래스 
        return CtbExtractor::CtbExtractor::ExtractAll(managedPath, managedOut, progressAction);
    }
    // C++/CLI에서는 관리형 예외를 잡을 때 ^ 기호를 사용해야 합니다.
    catch (Exception^ ex) {
        System::Diagnostics::Debug::WriteLine("C++/CLI Bridge Error: " + ex->Message);
        return false;
    }
    catch (...) {
        return false;
    }
}
