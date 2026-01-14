#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QMessageBox>
#include <QFileDialog>
#include <QFileInfo>
#include <QtConcurrent>
#include "NativeBridge.h"

MainWindow* MainWindow::instance = nullptr;

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    instance = this; // 전역 인스턴스 설정

    this->filename = "";
    this->directoryPath = "";
}

MainWindow::~MainWindow()
{
    delete ui;
}

// C#에서 호출하는 실제 함수
void MainWindow::updateProgress(int value) {
    if (instance) {
        // UI 스레드가 아니므로 signal/slot이나 QMetaObject 호출을 써야 안전합니다.
        QMetaObject::invokeMethod(instance->ui->progressBar, "setValue", Q_ARG(int, value));
    }
}

void MainWindow::on_pushButtonSelectCTB_clicked()
{
    QString selectedFile = QFileDialog::getOpenFileName(this, tr("CTB File Selection"), "", tr("Chitubox Files (*.ctb);;All Files (*)"), nullptr, QFileDialog::DontUseNativeDialog);

    if (!selectedFile.isEmpty()) {
        this->filename = QDir::toNativeSeparators(selectedFile);
        QFileInfo fileInfo(this->filename);
        this->directoryPath = QDir::toNativeSeparators(fileInfo.absolutePath() + QDir::separator() + "export");
    }
}

void MainWindow::on_pushButtonExtract_clicked()
{
    // 파일이 선택되었는지 확인합니다.
    if (this->filename.isEmpty()) {
        QMessageBox::warning(this, "Error", "Select CTB file first.");
        return;
    }

    std::string stdPath = this->filename.toLocal8Bit().constData();
    std::string stdDir = this->directoryPath.toLocal8Bit().constData();

    ui->pushButtonExtract->setEnabled(false); // 중복 클릭 방지

    // QtConcurrent를 사용하여 백그라운드에서 실행 (UI Freeze 방지)
    QtConcurrent::run([=]() {
        bool result = CallChituboxExtract(stdPath.c_str(), stdDir.c_str(), &MainWindow::updateProgress);

        // 완료 후 UI 처리
        QMetaObject::invokeMethod(this, [=]() {
            ui->pushButtonExtract->setEnabled(true);
            if (result) QMessageBox::information(this, "Complete", "Extraction Finished!");
        });
    });
}
