namespace CaptchaAPI.Models;

public class CaptchaValidateRequest
{
    public string CaptchaId { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}
