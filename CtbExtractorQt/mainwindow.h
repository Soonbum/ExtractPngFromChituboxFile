#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QtConcurrent>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

    // C#에서 호출될 전역/정적 함수 (브릿지 역할을 위해 static 필요)
    static void updateProgress(int value);
    static MainWindow* instance; // 정적 함수에서 접근하기 위한 인스턴스 포인터

private slots:
    void on_pushButtonSelectCTB_clicked();
    void on_pushButtonExtract_clicked();

private:
    Ui::MainWindow *ui;
    QString filename;
    QString directoryPath;
};
#endif // MAINWINDOW_H
