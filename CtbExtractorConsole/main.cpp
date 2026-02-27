#include <iostream>
#include <string>
#include <filesystem> // C++17 이상 권장
#include <QCoreApplication>
#include <QFileInfo>
#include <QDir>
#include "NativeBridge.h"

namespace fs = std::filesystem;

// 진행률을 출력할 콜백 함수
void updateProgress(int value) {
    std::cout << "\rExtraction Progress: " << value << "%" << std::flush;
    if (value >= 100) std::cout << std::endl;
}

int main(int argc, char *argv[])
{
    // 인수 개수 체크 (프로그램명, arg1, arg2 총 3개 필요)
    if (argc < 3) {
        std::cout << "Usage: ./CtbExtractor <input_file_path> <output_base_dir>" << std::endl;
        std::cout << "Example: ./CtbExtractor /home/pi/datas/example.ctb /home/pi/buffer/" << std::endl;
        return -1;
    }

    std::string inputPath = argv[1];
    std::string outputBaseDir = argv[2];

    // 입력 파일 존재 여부 확인
    QFileInfo inputFile(QString::fromLocal8Bit(inputPath.c_str()));
    if (!inputFile.exists()) {
        std::cerr << "Error: Input file does not exist: " << inputPath << std::endl;
        return -1;
    }

    // arg2 안에 파일명을 딴 디렉토리 경로 생성
    // 예: example.ctb -> example 디렉토리
    QString baseName = inputFile.completeBaseName();
    QDir dir(QString::fromLocal8Bit(outputBaseDir.c_str()));
    QString finalOutputDir = dir.absoluteFilePath(baseName);

    // 디렉토리 생성 (이미 있으면 생성하지 않음)
    if (!dir.mkpath(baseName)) {
        std::cerr << "Error: Failed to create directory: " << finalOutputDir.toStdString() << std::endl;
        return -1;
    }

    std::string stdInput = inputFile.absoluteFilePath().toLocal8Bit().constData();
    std::string stdOutput = QDir::toNativeSeparators(finalOutputDir).toLocal8Bit().constData();

    std::cout << "Input:  " << stdInput << std::endl;
    std::cout << "Output: " << stdOutput << std::endl;
    std::cout << "Starting extraction..." << std::endl;

    // 변환 작업 수행 (NativeBridge 호출)
    bool result = CallChituboxExtract(stdInput.c_str(), stdOutput.c_str(), &updateProgress);

    if (result) {
        std::cout << "Extraction Finished Successfully!" << std::endl;
        return 0;
    } else {
        std::cerr << "Extraction Failed." << std::endl;
        return -1;
    }
}
