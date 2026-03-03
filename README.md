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

* CtbExtractorQt (Qt C++ 프로젝트)
  - CtbExtractorQt.pro, CtbExtractorQt.pro.user: Qt 프로젝트 설정 파일
  - mainwindow.cpp, mainwindow.h, mainwindow.ui: 프로그램 GUI
  - NativeBridge.h: NativeBridge.dll을 호출하기 위한 헤더 파일
  - main.cpp: 프로그램 진입점
  - 그 외 여러 가지 파일들이 포함되어 있는데 Dependency가 있기 때문에 다 있어야만 프로그램이 실행됩니다.
  - Qt Creator 4.11.1로 작성하였으며 빌드시 컴파일러는 Desktop Qt 5.14.2 MSVC2015 64bit를 사용하였습니다.

* CtbExtractorConsole (Qt C++ 프로젝트)
  - CtbExtractorQt 프로젝트는 GUI 버전인 반면, 이것은 CUI 버전으로 만듦
  - 동일한 플랫폼에서 빌드하였으므로 Intel x64, Windows 환경에서 실행 가능함
  - 나머지 특징은 CtbExtractorQt와 동일

* CtbExtractorConsoleLinux
  - 가상머신으로 debian-12.0.0-amd64 설치해서 닷넷 환경을 직접 구축하고 amd 32비트로 크로스 컴파일
  - 라즈베리파이 보드에서 실행할 수 있도록 하는 것이 주 목적
  - GLIBC 2.29 버전을 요구하므로 라즈베리파이 11 (bullseye) 이상에서 실행 가능함 (아쉽게도 10 buster 버전에서는 작동 안됨, 도커를 사용해야 할 듯)
  - 본 프로젝트의 개발 환경을 구축하기 위해 터미널에서 다음 절차들을 수행해야 함
    * .NET 6.0 설치
      - `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 6.0    # .NET 6.0 스크립트로 설치`
    * .bashrc 끝 부분에 환경변수 설정 append
      ```
      # append in ~/.bashrc
      expert DOTNET_ROOT=$HOME/.dotnet
      export PATH=$PATH:$HOME/.dotnet
      ```
    * 변경된 .bashrc 적용하기
      - `source ~/.bashrc`
    * .NET 설치된 버전 확인하기
      - `dotnet --list-sdks`
    * 새로운 콘솔 프로젝트 생성
      ```
      new project
      mkdir ~/MyProject && cd ~/MyProject
      dotnet new console -f net6.0
      ```
    * 프로젝트 안에 다음 파일 복사
      - CtbExtractorConsoleLinux.csproj
      - Program.cs
    * NuGet 패키지 설치
      - `dotnet add package UVtools.Core --version 3.6.0`
      - `dotnet add package Emgu.CV --version 4.5.5.4823`
      - `dotnet add package Emgu.CV.runtime.debian-arm  --version 4.5.5.4823  # 32bit`
      - `dotnet add package Emgu.CV.Bitmap --version 4.5.5.4823`
      - `dotnet add package System.Drawing.Common --version 6.0.0`
    * 만약 빌드가 잘 안 될 경우 패키지를 지우고 클린 빌드를 할 것
      - `dotnet clean`
      - `dotnet nuget locals all --clear`
      - `dotnet restore`
      - `dotnet publish -r linux-arm --self-contained true -f net6.0 -c Release    # 빌드 명령어`
