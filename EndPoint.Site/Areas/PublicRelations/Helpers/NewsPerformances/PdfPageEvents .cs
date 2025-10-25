using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace EndPoint.Site.Areas.PublicRelations.Helpers.NewsPerformances
{
    public class PdfPageEvents : PdfPageEventHelper
    {
        private readonly Font _footerFont;

        public PdfPageEvents(Font footerFont)
        {
            _footerFont = footerFont;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // ایجاد جدول برای فوتر
            PdfPTable footerTable = new PdfPTable(1);
            footerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTable.DefaultCell.Border = Rectangle.NO_BORDER;
            footerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;


            PdfPCell cell = new PdfPCell(new Phrase($"صفحه {writer.PageNumber}", _footerFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL; // تنظیم جهت نوشتار
            footerTable.AddCell(cell);

            // نوشتن جدول فوتر
            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin + 5, writer.DirectContent);

            //اضافه کردن تصویر به فوتر که کل پایین صفحه را پر کند
            var footerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "NewsPerformances", "iTextSharp", "Footer.jpg");
            if (File.Exists(footerImagePath))
            {
                var footerImage = iTextSharp.text.Image.GetInstance(footerImagePath);

                // تنظیم عرض تصویر به اندازه کل عرض صفحه و ارتفاع آن برای پر کردن کامل پایین
                footerImage.ScaleAbsolute(document.PageSize.Width, 80f); // ارتفاع 100 واحدی برای مثال
                footerImage.SetAbsolutePosition(0, 0); // موقعیت تصویر کاملاً در پایین صفحه
                writer.DirectContent.AddImage(footerImage);
            }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            // ایجاد جدول برای هدر (اختیاری)
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
            headerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;


            // نوشتن جدول هدر
            headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + headerTable.TotalHeight, writer.DirectContent);

            // اضافه کردن تصویر به هدر که کل بالای صفحه را پر کند
            var headerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "NewsPerformances", "iTextSharp", "Header.jpg");
            if (File.Exists(headerImagePath))
            {
                var headerImage = iTextSharp.text.Image.GetInstance(headerImagePath);

                // تنظیم عرض و ارتفاع تصویر به طور دلخواه، مثلاً ارتفاع 100 واحد
                headerImage.ScaleAbsolute(document.PageSize.Width, 80f); // تنظیم عرض به اندازه کل عرض صفحه
                headerImage.SetAbsolutePosition(0, document.PageSize.Height - headerImage.ScaledHeight); // قرار دادن تصویر در بالای صفحه
                writer.DirectContent.AddImage(headerImage);
            }

            PdfPCell cell = new PdfPCell(new Phrase("معاونت تبلیغات و روابط عمومی سازمان عقیدتی سیاسی انتظامی ج.ا ایران", _footerFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL; // تنظیم جهت نوشتار
            headerTable.AddCell(cell);

            //تکرار هدر
            //headerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - document.TopMargin + 20, writer.DirectContent);
        }
    }
}
