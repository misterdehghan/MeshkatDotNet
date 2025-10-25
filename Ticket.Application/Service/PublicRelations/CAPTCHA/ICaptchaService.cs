using ImageProcessor.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;


using Microsoft.Extensions.Primitives;
using System.Drawing.Imaging;


namespace Azmoon.Application.Service.PublicRelations.CAPTCHA
{
    public interface ICaptchaService
    {
        string GenerateCaptchaCode();
        void GenerateCaptchaImage(string captchaCode, MemoryStream memoryStream);
    }

    public class CaptchaService : ICaptchaService
    {
        private static readonly Random Random = new Random();

        public string GenerateCaptchaCode()
        {
            // تولید کد تصادفی CAPTCHA
            var captchaCode = new string(Enumerable.Range(0, 5)
                .Select(_ => (char)Random.Next('A', 'Z'))
                .ToArray());
            return captchaCode;
        }

        public void GenerateCaptchaImage(string captchaCode, MemoryStream memoryStream)
        {
            int width = 150;
            int height = 45;

            // ایجاد تصویر و تنظیم پس‌زمینه
            using (var bitmap = new Bitmap(width, height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.LightGray);

                    // تنظیم فونت و رنگ متن
                    var font = new Font("Arial", 24);
                    var brush = new SolidBrush(Color.Black);

                    // رسم کد CAPTCHA
                    graphics.DrawString(captchaCode, font, brush, new PointF(20, 15));

                    // افزودن نویز (اختیاری)
                    AddNoise(graphics, width, height);

                    // ذخیره تصویر به حافظه
                    bitmap.Save(memoryStream, ImageFormat.Png);
                }
            }
        }

        private void AddNoise(Graphics graphics, int width, int height)
        {
            var random = new Random();
            for (int i = 0; i < 1000; i++) // تعداد نویز
            {
                var x = random.Next(0, width);
                var y = random.Next(0, height);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256))), x, y, 1, 1);
            }
        }



    }
}
