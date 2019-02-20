using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Magaza
{
    public class ImageResizing : IImageResizing, IDisposable
    {
        #region Fields

        private readonly BitmapFrame _firstImageBitmapFrame;
        private readonly JpegBitmapEncoder _jpegEncoder;
        private readonly Stream _sourceImageStream;

        #endregion Fields

        #region Constructors

        public ImageResizing(string sourceImagePath)
            : this(new MemoryStream(File.ReadAllBytes(sourceImagePath)))
        { }

        public ImageResizing(byte[] sourceImageBytes)
            : this(new MemoryStream(sourceImageBytes))
        { }

        public ImageResizing(Stream sourceImageStream)
        {
            _sourceImageStream = sourceImageStream;
            _firstImageBitmapFrame = GetFirstBitmapFrame(_sourceImageStream);

            _jpegEncoder = new JpegBitmapEncoder();
            _jpegEncoder.Frames.Add(_firstImageBitmapFrame);
        }

        #endregion Constructors

        #region IImageResizer Members

        public ImageResizing Quality(int quality)
        {
            if (quality <= 75)
            {
                _jpegEncoder.QualityLevel = quality;
            }

            return this;
        }

        public ImageResizing Resize(int width, int height)
        {
            var resizedBitmapFrame = Resize(_firstImageBitmapFrame, width, height);

            _jpegEncoder.Frames.Clear();
            _jpegEncoder.Frames.Add(resizedBitmapFrame);

            return this;
        }

        public void Save(string path, bool dispose = true)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                _jpegEncoder.Save(fs);
            }

            if (dispose)
            {
                Dispose();
            }
        }

        public MemoryStream ToStream()
        {
            var memStream = new MemoryStream();
            _jpegEncoder.Save(memStream);
            return memStream;
        }

        #endregion IImageResizer Members

        #region IDisposable Support

        private bool disposedValue;

        public void Dispose() => Dispose(true);

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _sourceImageStream.Dispose();
                }
                disposedValue = true;
            }
        }

        #endregion IDisposable Support

        #region Private Helpers

        private static BitmapFrame GetFirstBitmapFrame(Stream imageStream)
        {
            var bitmapDecoder = BitmapDecoder.Create(imageStream,
                BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);
            return bitmapDecoder.Frames[0];
        }

        private static BitmapFrame Resize(BitmapFrame bitmapFrame, int width, int height)
        {
            double scaleWidth, scaleHeight;

            if (height == 0)
            {
                scaleWidth = width;
                scaleHeight = (((double)width / bitmapFrame.PixelWidth) * bitmapFrame.PixelHeight);
            }
            else if (width == 0)
            {
                scaleHeight = height;
                scaleWidth = (((double)height / bitmapFrame.PixelHeight) * bitmapFrame.PixelWidth);
            }
            else
            {
                scaleWidth = width;
                scaleHeight = height;
            }

            var scaleTransform = new ScaleTransform(scaleWidth / bitmapFrame.PixelWidth, scaleHeight / bitmapFrame.PixelHeight, 0, 0);

            var transformedBitmap = new TransformedBitmap(bitmapFrame, scaleTransform);

            return BitmapFrame.Create(transformedBitmap);
        }

        #endregion Private Helpers
    }
}