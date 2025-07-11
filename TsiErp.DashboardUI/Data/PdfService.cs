﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Drawing;

namespace TsiErp.DashboardUI.Data
{
    public class PdfService
    {
        public MemoryStream CreatePdf()
        {
            //Creating new PDF document instance
            PdfDocument document = new PdfDocument();
            //Setting margin
            document.PageSettings.Margins.All = 0;
            //Adding a new page
            PdfPage page = document.Pages.Add();
            PdfGraphics g = page.Graphics;

            //Creating font instances
            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 35);
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 16);

            //Drawing content onto the PDF
            g.DrawRectangle(new PdfSolidBrush(Color.Gray), new Syncfusion.Drawing.RectangleF(0, 0, page.Graphics.ClientSize.Width, page.Graphics.ClientSize.Height));
            g.DrawRectangle(new PdfSolidBrush(Color.Black), new Syncfusion.Drawing.RectangleF(0, 0, page.Graphics.ClientSize.Width, 130));
            g.DrawRectangle(new PdfSolidBrush(Color.White), new Syncfusion.Drawing.RectangleF(0, 400, page.Graphics.ClientSize.Width, page.Graphics.ClientSize.Height - 450));
            g.DrawString("Enterprise", headerFont, new PdfSolidBrush(Color.Violet), new Syncfusion.Drawing.PointF(10, 20));
            g.DrawRectangle(new PdfSolidBrush(Color.Violet), new Syncfusion.Drawing.RectangleF(10, 63, 140, 35));
            g.DrawString("Reporting Solutions", subHeadingFont, new PdfSolidBrush(Color.Black), new Syncfusion.Drawing.PointF(15, 70));

            PdfLayoutResult result = HeaderPoints("Develop cloud-ready reporting applications in as little as 20% of the time.", 15, page);
            result = HeaderPoints("Proven, reliable platform thousands of users over the past 10 years.", result.Bounds.Bottom + 15, page);
            result = HeaderPoints("Microsoft Excel, Word, Adobe PDF, RDL display and editing.", result.Bounds.Bottom + 15, page);
            result = HeaderPoints("Why start from scratch? Rely on our dependable solution frameworks", result.Bounds.Bottom + 15, page);

            result = BodyContent("Deployment-ready framework tailored to your needs.", result.Bounds.Bottom + 45, page);
            result = BodyContent("Our architects and developers have years of reporting experience.", result.Bounds.Bottom + 25, page);
            result = BodyContent("Solutions available for web, desktop, and mobile applications.", result.Bounds.Bottom + 25, page);
            result = BodyContent("Backed by our end-to-end product maintenance infrastructure.", result.Bounds.Bottom + 25, page);
            result = BodyContent("The quickest path from concept to delivery.", result.Bounds.Bottom + 25, page);

            PdfPen redPen = new PdfPen(PdfBrushes.Red, 2);
            g.DrawLine(redPen, new Syncfusion.Drawing.PointF(40, result.Bounds.Bottom + 92), new Syncfusion.Drawing.PointF(40, result.Bounds.Bottom + 145));
            float headerBulletsXposition = 40;
            PdfTextElement txtElement = new PdfTextElement("The Experts");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
            txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 5, result.Bounds.Bottom + 90, 450, 200));

            PdfPen violetPen = new PdfPen(PdfBrushes.Violet, 2);
            g.DrawLine(violetPen, new Syncfusion.Drawing.PointF(headerBulletsXposition + 280, result.Bounds.Bottom + 92), new Syncfusion.Drawing.PointF(headerBulletsXposition + 280, result.Bounds.Bottom + 145));
            txtElement = new PdfTextElement("Accurate Estimates");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
            result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 290, result.Bounds.Bottom + 90, 450, 200));

            txtElement = new PdfTextElement("A substantial number of .NET reporting applications use our frameworks");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
            result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 5, result.Bounds.Bottom + 5, 250, 200));


            txtElement = new PdfTextElement("Given our expertise, you can expect estimates to be accurate.");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
            result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 290, result.Bounds.Y, 250, 200));


            PdfPen greenPen = new PdfPen(PdfBrushes.Green, 2);
            g.DrawLine(greenPen, new Syncfusion.Drawing.PointF(40, result.Bounds.Bottom + 32), new Syncfusion.Drawing.PointF(40, result.Bounds.Bottom + 85));

            txtElement = new PdfTextElement("Product Licensing");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
            txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 5, result.Bounds.Bottom + 30, 450, 200));

            PdfPen bluePen = new PdfPen(PdfBrushes.Blue, 2);
            g.DrawLine(bluePen, new Syncfusion.Drawing.PointF(headerBulletsXposition + 280, result.Bounds.Bottom + 32), new Syncfusion.Drawing.PointF(headerBulletsXposition + 280, result.Bounds.Bottom + 85));
            txtElement = new PdfTextElement("About Syncfusion");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
            result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 290, result.Bounds.Bottom + 30, 450, 200));

            txtElement = new PdfTextElement("Solution packages can be combined with product licensing for great cost savings.");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
            result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 5, result.Bounds.Bottom + 5, 250, 200));


            txtElement = new PdfTextElement("Syncfusion has more than 7,000 customers including large financial institutions and Fortune 100 companies.");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
            result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 290, result.Bounds.Y, 250, 200));

            g.DrawString("All trademarks mentioned belong to their owners.", new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Italic), PdfBrushes.White, new Syncfusion.Drawing.PointF(10, g.ClientSize.Height - 30));
            PdfTextWebLink linkAnnot = new PdfTextWebLink();
            linkAnnot.Url = "//www.syncfusion.com";
            linkAnnot.Text = "www.syncfusion.com";
            linkAnnot.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Italic);
            linkAnnot.Brush = PdfBrushes.White;
            linkAnnot.DrawTextWebLink(page, new Syncfusion.Drawing.PointF(g.ClientSize.Width - 100, g.ClientSize.Height - 30));

            //Saving the PDF to the MemoryStream
            MemoryStream ms = new MemoryStream();
            document.Save(ms);
            //If the position is not set to '0' then the PDF will be empty.
            ms.Position = 0;
            return ms;
        }
        private PdfLayoutResult BodyContent(string text, float yPosition, PdfPage page)
        {
            float headerBulletsXposition = 35;
            PdfTextElement txtElement = new PdfTextElement("3");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.ZapfDingbats, 16);
            txtElement.Brush = new PdfSolidBrush(Color.Violet);
            txtElement.StringFormat = new PdfStringFormat();
            txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
            txtElement.StringFormat.LineLimit = true;
            txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition, yPosition, 320, 100));

            txtElement = new PdfTextElement(text);
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 17);
            txtElement.Brush = new PdfSolidBrush(Color.White);
            txtElement.StringFormat = new PdfStringFormat();
            txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
            txtElement.StringFormat.LineLimit = true;
            PdfLayoutResult result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 25, yPosition, 450, 130));
            return result;
        }
        private PdfLayoutResult HeaderPoints(string text, float yPosition, PdfPage page)
        {
            float headerBulletsXposition = 220;
            PdfTextElement txtElement = new PdfTextElement("l");
            txtElement.Font = new PdfStandardFont(PdfFontFamily.ZapfDingbats, 10);
            txtElement.Brush = new PdfSolidBrush(Color.Violet);
            txtElement.StringFormat = new PdfStringFormat();
            txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
            txtElement.StringFormat.LineLimit = true;
            txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition, yPosition, 300, 100));

            txtElement = new PdfTextElement(text);
            txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11);
            txtElement.Brush = new PdfSolidBrush(Color.White);
            txtElement.StringFormat = new PdfStringFormat();
            txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
            txtElement.StringFormat.LineLimit = true;
            PdfLayoutResult result = txtElement.Draw(page, new Syncfusion.Drawing.RectangleF(headerBulletsXposition + 20, yPosition, 320, 100));
            return result;
        }
    }
}
