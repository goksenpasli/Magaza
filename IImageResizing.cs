using System.IO;

namespace Magaza
{

    public interface IImageResizing
    {
        ImageResizing Resize(int width, int height);
        ImageResizing Quality(int quality);
        void Save(string path, bool dispose);
        MemoryStream ToStream();
    }
}