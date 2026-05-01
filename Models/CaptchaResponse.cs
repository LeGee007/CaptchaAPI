namespace CaptchaAPI.Models;

public class CaptchaResponse
{
    public string CaptchaId { get; set; } = string.Empty;
    public string ImageBase64 { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
