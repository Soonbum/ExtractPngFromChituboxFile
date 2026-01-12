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
        using OpenFileDialog openFileDialog = new OpenFileDialog();
        
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

    private void ButtonSavePngs_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(CtbFilePath) || !File.Exists(CtbFilePath))
        {
            MessageBox.Show("먼저 유효한 .ctb 파일을 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            // ChituboxFile 객체 생성 및 파일 경로 설정
            using ChituboxFile ctbFile = new();

            // Decode 실행 (이 과정에서 헤더와 레이어 정의가 로드됨)
            // FileDecodeType.Full로 설정해야 이미지 데이터까지 읽어옵니다.
            ctbFile.Decode(CtbFilePath, FileFormat.FileDecodeType.Full, new OperationProgress());

            // 저장 폴더 준비
            string outputFolder = Path.Combine(Path.GetDirectoryName(CtbFilePath), Path.GetFileNameWithoutExtension(CtbFilePath) + "_layers");
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

            // 레이어 반복 추출
            for (uint i = 0; i < ctbFile.LayerCount; i++)
            {
                // ChituboxFile 인덱서 또는 GetLayer를 통해 레이어 접근
                var layer = ctbFile[i];

                // LayerMat 프로퍼티를 통해 Mat(이미지 데이터) 가져오기
                // 내부적으로 DecodeCtbImage를 호출하여 RLE를 풉니다.
                using Mat mat = layer.LayerMat;

                if (mat != null && !mat.IsEmpty)
                {
                    // Mat을 PNG 파일로 저장 (Emgu.CV 기능 이용)
                    string fileName = $"Layer_{i:D4}.png";
                    string filePath = Path.Combine(outputFolder, fileName);
                    mat.Save(filePath);
                }
            }

            MessageBox.Show($"완료! {ctbFile.LayerCount}개의 이미지가 저장되었습니다.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"오류 발생: {ex.Message}");
        }
    }
}
