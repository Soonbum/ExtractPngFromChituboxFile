using UVtools.Core.FileFormats;
using UVtools.Core.Operations;
using Emgu.CV;
using System.Drawing;
using System.Drawing.Imaging;

namespace ExtractPngFromChituboxFile;

public partial class ExtractPngFromChituboxFile : Form
{
    string CtbFilePath = string.Empty;

    public ExtractPngFromChituboxFile()
    {
        InitializeComponent();
    }

    private void ButtonSelectCtb_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new();
        
        // 초기 디렉토리 설정 (내 문서 등)
        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // 파일 필터 설정 (.ctb 파일만 보이도록 설정)
        // 형식: "표시될이름|확장자"
        openFileDialog.Filter = "Chitubox Files (*.ctb)|*.ctb|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1; // 첫 번째 필터(*.ctb)를 기본으로 선택
        openFileDialog.RestoreDirectory = true;

        // 다이얼로그를 띄우고 사용자가 '확인'을 눌렀는지 체크
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            // 선택된 파일의 전체 경로 가져오기
            CtbFilePath = openFileDialog.FileName;
        }
    }

    private async void ButtonSavePngs_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(CtbFilePath) || !File.Exists(CtbFilePath))
        {
            MessageBox.Show("먼저 유효한 .ctb 파일을 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // 버튼 비활성화 (중복 클릭 방지)
        ButtonSavePngs.Enabled = false;
        progressBar1.Value = 0;

        try
        {
            // 백그라운드 스레드에서 무거운 작업 실행
            await Task.Run(() =>
            {
                // ChituboxFile 객체 생성 및 파일 경로 설정
                using ChituboxFile ctbFile = [];

                // Decode 실행 (내부적으로 decodeProgress를 통해 진행률이 업데이트됨)
                ctbFile.Decode(CtbFilePath, FileFormat.FileDecodeType.Full, new OperationProgress());

                // 저장 폴더 준비
                string outputFolder = Path.Combine(Path.GetDirectoryName(CtbFilePath), Path.GetFileNameWithoutExtension(CtbFilePath) + "_layers");
                if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

                // 레이어 추출 및 저장 진행률
                uint layerCount = ctbFile.LayerCount;
                for (uint i = 0; i < layerCount; i++)
                {
                    var layer = ctbFile[i];
                    using Mat mat = layer.LayerMat;

                    if (mat != null && !mat.IsEmpty)
                    {
                        string fileName = $"SEC_{i:D4}.png";
                        string filePath = Path.Combine(outputFolder, fileName);
                        mat.Save(filePath);
                    }

                    // 데이터 계산
                    int currentStep = (int)(i + 1);
                    int currentProgress = (int)(((double)currentStep / layerCount) * 100);
                    string statusText = $"{currentStep} / {layerCount}"; // "현재 개수 / 총 개수"

                    // UI 스레드 업데이트
                    this.BeginInvoke(new Action(() => {
                        progressBar1.Value = currentProgress;
                        LabelProgress.Text = statusText;
                    }));
                }

                MessageBox.Show($"완료! {ctbFile.LayerCount}개의 이미지가 저장되었습니다.");
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"오류 발생: {ex.Message}");
        }
        finally
        {
            ButtonSavePngs.Enabled = true;
        }
    }
}
