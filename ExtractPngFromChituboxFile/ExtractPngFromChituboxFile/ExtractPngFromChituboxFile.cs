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

        // ... DecodeInternally, DecodeCtbImage

        // ... 디코딩할 파일 ctb 경로를 알고 있고
        // ... ctb 파일을 구조체로 읽어들인 후
        // ... ctb 파일 구조체를 통해 레이어 개수를 알고, 모든 레이어를 추출한다. --> Mat 타입 값은 png 파일로 저장한다.

        // ... 1. FileStream을 ChituboxFile로 저장할 것
        // ... 2. DecodeCtbImage 함수를 이용해서 ChituboxFile로부터 PNG 이미지들을 추출한다.
    }
}
