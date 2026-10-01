namespace Scratch;

public struct Costume
{
    public string Name = string.Empty;
    public string AssetId = string.Empty;
    public string DataFormat { get; set; } = "png";
    public double BitmapResolution;
    public double RotationCenterX;
    public double RotationCenterY;

    public Costume() { }
}