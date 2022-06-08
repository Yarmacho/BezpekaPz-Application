using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Application.Entities
{
    public class CaptchaImage : IDisposable
    {
        private string _text; // текст капчи
        private int _width; // ширина картинки
        private int _height; // высота картинки
        public Bitmap Image { get; set; } // само изображение капчи

        public CaptchaImage(string text, int width, int height)
        {
            _text = text;
            _width = width;
            _height = height;
            generateImage();
        }
        // создаем изображение
        private void generateImage()
        {
            Bitmap bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);

            using Graphics g = Graphics.FromImage(bitmap);
            // отрисовка строки
            g.DrawString(_text, new Font("Arial", _height / 2, FontStyle.Bold),
                                Brushes.Red, new RectangleF(0, 0, _width, _height));

            Image = bitmap;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Image.Dispose();
        }
    }
}
