#pragma once

using namespace System;

namespace CtbWrapper {
    public ref class CtbWrapper {
    public:
        static bool RunExtraction(String^ path, String^ outDir) {
            // C#의 CtbExtractor::ExtractAll 호출
			return CtbExtractor::CtbExtractor::ExtractAll(path, outDir);
        }
    };
}
