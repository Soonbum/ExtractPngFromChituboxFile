# ExtractPngFromChituboxFile

## 치투박스(CTB) 파일로부터 슬라이스 이미지(PNG)를 추출하는 프로그램

# 구성 요소

* 샘플 ctb 파일
  - 파일 추출 테스트를 위한 샘플 CTB 파일이 들어 있음: 251118_DM400_44shoes_270_2EA_200

* ExtractPngFromChituboxFile (C# 솔루션)
  - UVtools.Core: CTB 파일 구조 및 해제를 위한 핵심 알고리즘이 들어 있으며 원 소스는 C#으로 작성되어 있음 (https://github.com/sn4k3/UVtools)
  - ExtractPngFromChituboxFile: UVtools.Core를 사용하여 CTB 파일로부터 레이어별 이미지 및 데이터를 추출하는 C# Windows Form 샘플 프로그램 (윈도우에서만 사용 가능)
  - CtbExtractor: ExtractPngFromChituboxFile 프로젝트에서 CTB 파일 추출 기능만 따로 떼어놓은 클래스로 DLL 파일을 생성하기 위한 프로젝트
  - NativeBridge: Qt C++에서 CtbExtractor.dll을 사용하기 위해 만든 Bridge 클래스

* CtbExtractorQt
  - CtbExtractorQt.pro, CtbExtractorQt.pro.user: Qt 프로젝트 설정 파일
  - mainwindow.cpp, mainwindow.h, mainwindow.ui: 프로그램 GUI
  - NativeBridge.h: NativeBridge.dll을 호출하기 위한 헤더 파일
  - main.cpp: 프로그램 진입점
  - 그 외 여러 가지 파일들이 포함되어 있는데 Dependency가 있기 때문에 다 있어야만 프로그램이 실행됩니다.
  - Qt Creator 4.11.1로 작성하였으며 빌드시 컴파일러는 Desktop Qt 5.14.2 MSVC2015 64bit를 사용하였습니다.
