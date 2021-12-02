using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
namespace ConsoleApp1
{
    //GenShinGothic-Monospace-Medium.ttfを埋め込みリソースに追加する必要がある。

    class PDFSharpSample
    {

        static void Main(string[] args)
        {
            PdfSharp.Fonts.GlobalFontSettings.FontResolver = new JapaneseFontResolver();
            using (var document = new PdfDocument()
            {
                 //PageLayout = PdfPageLayout.SinglePage,
                 //Language = "",
            })
            {
                document.Info.Title = "タイトル";
                document.Info.Author = "作者";
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var tf = new PdfSharp.Drawing.Layout.XTextFormatter(gfx)
                {
                    Alignment = PdfSharp.Drawing.Layout.XParagraphAlignment.Center,
                    
                    LayoutRectangle = new XRect(0, 0, page.Width, page.Height),
                };
                // フォント
                var font = new XFont("Gen Shin Gothic", 20, XFontStyle.Regular, XPdfFontOptions.UnicodeDefault);

                // 文字列描画
                tf.DrawString( 
                    "Hello World!\n こんにちは、世界!",
                    font,
                    XBrushes.Black,
                    new XRect(0, 0, page.Width, page.Height),
                    XStringFormats.TopLeft
                    );

                document.Save("HelloWorld.pdf");
            }
    }
}

    // 日本語フォントのためのフォントリゾルバー
    public class JapaneseFontResolver : PdfSharp.Fonts.IFontResolver
    {
        // 源真ゴシック（ http://jikasei.me/font/genshin/）
        private static readonly string GEN_SHIN_GOTHIC_MEDIUM_TTF =
            "ConsoleApp1.fonts.GenShinGothic-Monospace-Medium.ttf";

        public byte[] GetFont(string faceName)
        {
            switch (faceName)
            {
                case "GenShinGothic#Medium":
                    return LoadFontData(GEN_SHIN_GOTHIC_MEDIUM_TTF);
            }
            return null;
        }

        public PdfSharp.Fonts.FontResolverInfo ResolveTypeface(
                    string familyName, bool isBold, bool isItalic)
        {
            var fontName = familyName.ToLower();

            switch (fontName)
            {
                case "gen shin gothic":
                    return new PdfSharp.Fonts.FontResolverInfo("GenShinGothic#Medium");
            }

            // デフォルトのフォント
            return PdfSharp.Fonts.PlatformFontResolver.ResolveTypeface("Arial", isBold, isItalic);
        }

        // 埋め込みリソースからフォントファイルを読み込む
        private byte[] LoadFontData(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + resourceName);

                int count = (int)stream.Length;
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }
    }
}