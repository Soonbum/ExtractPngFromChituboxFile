QT       += core concurrent

CONFIG += c++17 console

# The following define makes your compiler emit warnings if you use
# any Qt feature that has been marked deprecated (the exact warnings
# depend on your compiler). Please consult the documentation of the
# deprecated API in order to know how to port your code away from it.
DEFINES += QT_DEPRECATED_WARNINGS

# You can also make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
# You can also select to disable deprecated APIs only up to a certain version of Qt.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += main.cpp

HEADERS += NativeBridge.h

# [기존 설정 생략]
INCLUDEPATH += $$PWD
LIBS += -L"$$PWD" -lNativeBridge

# 빌드 시 필요한 DLL들을 실행 폴더로 자동 복사하는 명령 (선택 사항)
# 빌드 환경에 맞게 경로를 수정하여 사용하세요.
# win32: CONFIG(release, debug|release): {
#    QMAKE_POST_LINK += $$quote(xcopy /y /d "path\to\NativeBridge.dll" "$$out_pwd\release")
#    QMAKE_POST_LINK += $$quote(xcopy /y /d "path\to\CtbExtractor.dll" "$$out_pwd\release")
# }
