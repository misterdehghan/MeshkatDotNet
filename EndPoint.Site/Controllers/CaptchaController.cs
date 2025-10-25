using EndPoint.Site.Useful.Ultimite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

namespace EndPoint.Site.Controllers
{
    public class CaptchaController : Controller
    {

        public ActionResult CaptchaImage(string prefix, bool noisy = false)
        {
            var rand = new Random((int)DateTime.UtcNow.Ticks);
            //generate new question
            int a = rand.Next(10, 30);
            int b = rand.Next(1, 9);

            var captcha = string.Format("{0} + {1} = ?", a, b);
            string PassHashMd5 = Ultimite.EncodePasswordMd5((a + b).ToString());
            //store answer
           // Session["Captcha" + prefix] = PassHashMd5;
            HttpContext.Session.SetString("Captcha" + prefix, PassHashMd5);
            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((System.Drawing.Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 5; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new System.Drawing.Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }
    }
}
