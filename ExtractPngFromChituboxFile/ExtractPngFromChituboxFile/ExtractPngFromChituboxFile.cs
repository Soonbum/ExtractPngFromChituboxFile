using UVtools.Core.FileFormats;
using UVtools.Core.Operations;
using Emgu.CV;
using System.Drawing;
using System.Drawing.Imaging;

namespace ExtractPngFromChituboxFile;

public class LayerData
{
    public uint Index { get; set; }
    public uint ResolutionX { get; set; }
    public uint ResolutionY { get; set; }
    public float LayerHeight { get; set; }
    public float PositionZ { get; set; }
    public bool CanExpose { get; set; }
    public bool ShouldExpose { get; set; }
    public bool ChangeResin { get; set; }
    public bool IsEmpty { get; set; }
    public bool IsBottomLayer { get; set; }
    public float ExposureTime { get; set; }
    public float LiftHeight { get; set; }
    public float LiftSpeed { get; set; }
    public float LiftAcceleration { get; set; }
    public float LiftHeight2 { get; set; }
    public float LiftSpeed2 { get; set; }
    public float LiftAcceleration2 { get; set; }
    public float RetractHeight { get; set; }
    public float RetractSpeed { get; set; }
    public float RetractAcceleration { get; set; }
    public float RetractHeight2 { get; set; }
    public float RetractSpeed2 { get; set; }
    public float RetractAcceleration2 { get; set; }
    public float LightOffDelay { get; set; }
    public byte LightPWM { get; set; }
    public float MaterialMilliliters { get; set; }
    public float MaterialMillilitersPercent { get; set; }
    public float MinimumSpeed { get; set; }
    public float MaximumSpeed { get; set; }
    public uint NonZeroPixelCount { get; set; }
    public double NonZeroPixelPercentage { get; set; }
    public double NonZeroPixelRatio { get; set; }
    public float Area { get; set; }
    public float Volume { get; set; }
    public float PrintTime { get; set; }
    public float StartTime { get; set; }
    public float EndTime { get; set; }
    public float WaitTimeBeforeCure { get; set; }
    public float WaitTimeAfterCure { get; set; }
    public float WaitTimeAfterLift { get; set; }
}

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

                // 썸네일(Thumbnail) 추출 및 저장
                int thumbnailCount = ctbFile.ThumbnailsCount;
                for (int i = 0; i < thumbnailCount; i++)
                {
                    var thumbnail = ctbFile.GetThumbnail(i);
                    if (thumbnail != null && !thumbnail.IsEmpty)
                    {
                        string fileName = $"Thumbnail_{i + 1}.png";
                        string filePath = Path.Combine(outputFolder, fileName);
                        thumbnail.Save(filePath);
                    }
                }

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

                    // XML 데이터 객체 생성 및 값 복사
                    var data = new LayerData
                    {
                        Index = layer.Index,
                        ResolutionX = layer.ResolutionX,
                        ResolutionY = layer.ResolutionY,
                        LayerHeight = layer.LayerHeight,
                        PositionZ = layer.PositionZ,
                        CanExpose = layer.CanExpose,
                        ShouldExpose = layer.ShouldExpose,
                        ChangeResin = layer.ChangeResin,
                        IsEmpty = layer.IsEmpty,
                        IsBottomLayer = layer.IsBottomLayer,
                        ExposureTime = layer.ExposureTime,
                        LiftHeight = layer.LiftHeight,
                        LiftSpeed = layer.LiftSpeed,
                        LiftAcceleration = layer.LiftAcceleration,
                        LiftHeight2 = layer.LiftHeight2,
                        LiftSpeed2 = layer.LiftSpeed2,
                        LiftAcceleration2 = layer.LiftAcceleration2,
                        RetractHeight = layer.RetractHeight,
                        RetractSpeed = layer.RetractSpeed,
                        RetractAcceleration = layer.RetractAcceleration,
                        RetractHeight2 = layer.RetractHeight2,
                        RetractSpeed2 = layer.RetractSpeed2,
                        RetractAcceleration2 = layer.RetractAcceleration2,
                        LightOffDelay = layer.LightOffDelay,
                        LightPWM = layer.LightPWM,
                        MaterialMilliliters = layer.MaterialMilliliters,
                        MaterialMillilitersPercent = layer.MaterialMillilitersPercent,
                        MinimumSpeed = layer.MinimumSpeed,
                        MaximumSpeed = layer.MaximumSpeed,
                        NonZeroPixelCount = layer.NonZeroPixelCount,
                        NonZeroPixelPercentage = layer.NonZeroPixelPercentage,
                        NonZeroPixelRatio = layer.NonZeroPixelRatio,
                        Area = layer.Area,
                        Volume = layer.Volume,
                        PrintTime = layer.PrintTime,
                        StartTime = layer.StartTime,
                        EndTime = layer.EndTime,
                        WaitTimeBeforeCure = layer.WaitTimeBeforeCure,
                        WaitTimeAfterCure = layer.WaitTimeAfterCure,
                        WaitTimeAfterLift = layer.WaitTimeAfterLift
                    };

                    // XML 파일로 저장
                    string xmlFilePath = Path.Combine(outputFolder, $"SEC_{i:D4}.xml");
                    using (var writer = new StreamWriter(xmlFilePath))
                    {
                        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(LayerData));
                        serializer.Serialize(writer, data);
                    }

                    //layer.Index: Gets the layer number, 1 started
                    //layer.ResolutionX
                    //layer.ResolutionY
                    //layer.LayerHeight: Gets the layer height in millimeters of this layer
                    //layer.PositionZ: Gets or sets the absolute layer position on Z in mm
                    //layer.CanExpose: Gets if this layer can be exposed to UV light
                    //layer.ShouldExpose: Gets if this layer should be exposed to UV light, ie: if layer is empty or no exposure time then it useless to expose it
                    //layer.ChangeResin: Gets or sets if printer should be paused to change resin before printing this layer
                    //layer.IsEmpty: Gets if this layer is empty/all black pixels
                    //layer.IsBottomLayer
                    //layer.ExposureTime: Gets or sets the exposure time in seconds
                    //layer.LiftHeight: Gets or sets the lift height in mm
                    //layer.LiftSpeed: Gets or sets the speed in mm/min
                    //layer.LiftAcceleration: Gets or sets the lift acceleration in mm/s²
                    //layer.LiftHeight2: Gets or sets the second lift height in mm
                    //layer.LiftSpeed2: Gets or sets the second lift speed in mm/min
                    //layer.LiftAcceleration2: Gets or sets the second lift acceleration in mm/s²
                    //layer.RetractHeight: Gets the retract height in mm
                    //layer.RetractSpeed: Gets the speed in mm/min for the retracts
                    //layer.RetractAcceleration: Gets or sets the retract acceleration in mm/s²
                    //layer.RetractHeight2: Gets or sets the second retract height in mm
                    //layer.RetractSpeed2: Gets the speed in mm/min for the retracts
                    //layer.RetractAcceleration2: Gets or sets the second retract acceleration in mm/s²
                    //layer.LightOffDelay: Gets or sets the layer off time in seconds
                    //layer.LightPWM: Gets or sets the pwm value from 0 to 255
                    //layer.MaterialMilliliters: Gets the computed material milliliters spent on this layer
                    //layer.MaterialMillilitersPercent: Gets the computed material milliliters percentage compared to the rest of the model
                    //layer.MinimumSpeed: Gets the minimum used speed in mm/min
                    //layer.MaximumSpeed: Gets the maximum used speed in mm/min
                    //layer.NonZeroPixelCount: Gets the number of non zero pixels on this layer image
                    //layer.NonZeroPixelPercentage: Gets the percentage of non zero pixels relative to the display number of pixels
                    //layer.NonZeroPixelRatio: Gets the ratio between non zero pixels and display number of pixels
                    //layer.Area: Gets the layer area (XY)  in mm^2 (Pixel size * number of pixels)
                    //layer.Volume: Gets the layer volume (XYZ) in mm^3 (Pixel size * number of pixels * layer height)
                    //layer.PrintTime: Gets the time estimate in seconds it takes for this layer to be printed
                    //layer.StartTime: Get the start time estimate in seconds when this layer should start at
                    //layer.EndTime: Get the end time estimate in seconds when this layer should end at
                    //layer.WaitTimeBeforeCure: Gets or sets the wait time in seconds before cure the layer
                    //layer.WaitTimeAfterCure: Gets or sets the wait time in seconds after cure the layer
                    //layer.WaitTimeAfterLift

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
