using CaptchaAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using SkiaSharp;

namespace CaptchaAPI.Services;

public class CaptchaService : ICaptchaService
{
    private readonly IMemoryCache _cache;

    public CaptchaService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public CaptchaResponse GenerateMathCaptcha()
    {
        var ran = new Random();
        int a = ran.Next(1, 20);
        int b = ran.Next(1, 20);
        string question = $"{a} + {b} = ?";
        string answer = (a + b).ToString();

        string id = Guid.NewGuid().ToString();

        _cache.Set(id, answer, TimeSpan.FromMinutes(5));

        return new CaptchaResponse
        {
            CaptchaId = id,
            ImageBase64 = question,
            Type = "math"
        };
    }

    public bool Validate(string captchaId, string answer)
    {
        if (_cache.TryGetValue(captchaId, out string correctAnswer))
        {
            _cache.Remove(captchaId);
            return correctAnswer.Equals(answer.Trim(),
                StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public CaptchaResponse GenerateImageCaptcha()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjklmnpqrstuvwxyz23456789";
        var random = new Random();
        string text = new string(Enumerable.Range(0, 9)
            .Select(_ => chars[random.Next(chars.Length)]).ToArray());

        var imageInfo = new SKImageInfo(200, 80);
        using var surface = SKSurface.Create(imageInfo);
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.White);

        var paint = new SKPaint { StrokeWidth = 1, IsAntialias = true };
        for (int i = 0; i < 8; i++)
        {
            paint.Color = new SKColor(
                (byte)random.Next(150, 255),
                (byte)random.Next(150, 255),
                (byte)random.Next(150, 255));
            canvas.DrawLine(random.Next(200), random.Next(80),
                random.Next(200), random.Next(80), paint);
        }

        paint.Color = SKColors.DarkBlue;
        paint.TextSize = 36;
        paint.IsStroke = false;
        paint.FakeBoldText = true;
        canvas.DrawText(text, 20, 55, paint);

        for (int i = 0; i < 100; i++)
        {
            paint.Color = new SKColor(
                (byte)random.Next(255),
                (byte)random.Next(255),
                (byte)random.Next(255));
            canvas.DrawPoint(random.Next(200), random.Next(80), paint);

        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        string base64 = Convert.ToBase64String(data.ToArray());

        string id = Guid.NewGuid().ToString();
        _cache.Set(id, text, TimeSpan.FromMinutes(5));

        return new CaptchaResponse
        {
            CaptchaId = id,
            ImageBase64 = $"data:image/png;base64,{base64}",
            Type = "image"
        };
    }
}
