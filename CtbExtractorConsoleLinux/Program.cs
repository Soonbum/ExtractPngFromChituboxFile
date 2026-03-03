using System;
using System.IO;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using UVtools.Core.FileFormats;
using UVtools.Core.Operations;
using UVtools.Core.Layers;

namespace CtbExtractor
{
    public class LayerData
    {
        public uint Index { get; set; }
        public uint ResolutionX { get; set; }
        public uint ResolutionY { get; set; }
        public float LayerHeight { get; set; }
        public float PositionZ { get; set; }
        //public bool CanExpose { get; set; }
        //public bool ShouldExpose { get; set; }
        //public bool ChangeResin { get; set; }
        public bool IsEmpty { get; set; }
        //public bool IsBottomLayer { get; set; }
        public float ExposureTime { get; set; }
        public float LiftHeight { get; set; }
        public float LiftSpeed { get; set; }
        //public float LiftAcceleration { get; set; }
        //public float LiftHeight2 { get; set; }
        //public float LiftSpeed2 { get; set; }
        //public float LiftAcceleration2 { get; set; }
        public float RetractHeight { get; set; }
        public float RetractSpeed { get; set; }
        //public float RetractAcceleration { get; set; }
        //public float RetractHeight2 { get; set; }
        //public float RetractSpeed2 { get; set; }
        //public float RetractAcceleration2 { get; set; }
        public float LightOffDelay { get; set; }
        public byte LightPWM { get; set; }
        //public float MaterialMilliliters { get; set; }
        //public float MaterialMillilitersPercent { get; set; }
        //public float MinimumSpeed { get; set; }
        //public float MaximumSpeed { get; set; }
        //public uint NonZeroPixelCount { get; set; }
        //public double NonZeroPixelPercentage { get; set; }
        //public double NonZeroPixelRatio { get; set; }
        //public float Area { get; set; }
        //public float Volume { get; set; }
        //public float PrintTime { get; set; }
        //public float StartTime { get; set; }
        //public float EndTime { get; set; }
        //public float WaitTimeBeforeCure { get; set; }
        //public float WaitTimeAfterCure { get; set; }
        //public float WaitTimeAfterLift { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Arguments check
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: ./CtbExtractorConsoleLinux <input_file_path> <output_base_dir>");
                Console.WriteLine("Example: ./CtbExtractorConsoleLinux /home/pi/example.ctb /home/pi/buffer/");
                return;
            }
        
            string filePath = args[0];
            string outputBaseDir = args[1];
        
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found -> {filePath}");
                return;
            }
        
            // Create output directory
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            string finalOutputDir = Path.Combine(outputBaseDir, fileNameWithoutExt);
        
            try
            {
                Console.WriteLine($"Target Directory: {finalOutputDir}");
                ExtractAll(filePath, finalOutputDir, (progress) => {
                    Console.Write($"\rExtraction Progress: {progress}%  ");
                });
                Console.WriteLine("\nExtraction Finished Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError during extraction: {ex.Message}");
            }
        }
    
        public static bool ExtractAll(string filePath, string outputFolder, Action<int> progressCallback)
        {
            using (ChituboxFile ctbFile = new ChituboxFile())
            {
                progressCallback?.Invoke(5);    // 5%
                ctbFile.Decode(filePath, FileFormat.FileDecodeType.Full, new OperationProgress());
                progressCallback?.Invoke(10);   // 10%

                if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

                // Extract thumbnail
                uint thumbnailCount = (uint)ctbFile.ThumbnailsCount;
                for (uint i = 0; i < thumbnailCount; i++)
                {
                    using (Emgu.CV.Mat thumbnail = ctbFile.GetThumbnail(i))
                    {
                        if (thumbnail != null && !thumbnail.IsEmpty)
                        {
                            string fileName = $"Thumbnail_{i + 1}.png";
                            thumbnail.Save(Path.Combine(outputFolder, fileName));
                        }
                    }
                }
                progressCallback?.Invoke(15);   // 15%

                // png, xml for each layer
                uint layerCount = ctbFile.LayerCount;
                for (uint i = 0; i < layerCount; i++)
                {
                    UVtools.Core.Layers.Layer layer = ctbFile.GetLayer(i);
                    using (Emgu.CV.Mat mat = layer.LayerMat)
                    {
                        if (mat != null && !mat.IsEmpty)
                        {
                            string fileName = $"SEC_{i:D4}.png";
                            mat.Save(Path.Combine(outputFolder, fileName));
                        }
                    }

                    // XML serialization
                    LayerData data = new LayerData
                    {
                        Index = (uint)layer.Index,
                        ResolutionX = (uint)ctbFile.ResolutionX,
                        ResolutionY = (uint)ctbFile.ResolutionY,
                        LayerHeight = layer.LayerHeight,
                        PositionZ = layer.PositionZ,
                        //CanExpose = layer.CanExpose,
                        //ShouldExpose = layer.ShouldExpose,
                        //ChangeResin = layer.ChangeResin,
                        IsEmpty = layer.IsEmpty,
                        //IsBottomLayer = layer.IsBottomLayer,
                        ExposureTime = layer.ExposureTime,
                        LiftHeight = layer.LiftHeight,
                        LiftSpeed = layer.LiftSpeed,
                        //LiftAcceleration = layer.LiftAcceleration,
                        //LiftHeight2 = layer.LiftHeight2,
                        //LiftSpeed2 = layer.LiftSpeed2,
                        //LiftAcceleration2 = layer.LiftAcceleration2,
                        RetractHeight = layer.RetractHeight,
                        RetractSpeed = layer.RetractSpeed,
                        //RetractAcceleration = layer.RetractAcceleration,
                        //RetractHeight2 = layer.RetractHeight2,
                        //RetractSpeed2 = layer.RetractSpeed2,
                        //RetractAcceleration2 = layer.RetractAcceleration2,
                        LightOffDelay = layer.LightOffDelay,
                        LightPWM = layer.LightPWM
                        //MaterialMilliliters = layer.MaterialMilliliters,
                        //MaterialMillilitersPercent = layer.MaterialMillilitersPercent,
                        //MinimumSpeed = layer.MinimumSpeed,
                        //MaximumSpeed = layer.MaximumSpeed,
                        //NonZeroPixelCount = layer.NonZeroPixelCount,
                        //NonZeroPixelPercentage = layer.NonZeroPixelPercentage,
                        //NonZeroPixelRatio = layer.NonZeroPixelRatio,
                        //Area = layer.Area,
                        //Volume = layer.Volume,
                        //PrintTime = layer.PrintTime,
                        //StartTime = layer.StartTime,
                        //EndTime = layer.EndTime,
                        //WaitTimeBeforeCure = layer.WaitTimeBeforeCure,
                        //WaitTimeAfterCure = layer.WaitTimeAfterCure,
                        //WaitTimeAfterLift = layer.WaitTimeAfterLift
                    };

                    string xmlFilePath = Path.Combine(outputFolder, $"SEC_{i:D4}.xml");
                    using (StreamWriter writer = new StreamWriter(xmlFilePath))
                    {
                        XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(LayerData));
                        serializer.Serialize(writer, data);
                    }

                    int currentProgress = 15 + (int)(((double)(i + 1) / layerCount) * 85);
                    progressCallback?.Invoke(currentProgress);

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
		}
            }
            return true;
        }
    }
}
