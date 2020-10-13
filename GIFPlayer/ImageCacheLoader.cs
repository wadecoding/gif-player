using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace GIFPlayer
{
    /// <summary>
    /// Loads and stores image.
    /// Instead of accessing the image directly everytime, it caches the image data frame-by-frame, to save the loading time.
    /// </summary>
    /// <remarks>
    /// Require more memory, but less access time
    /// </remarks>
    class ImageCacheLoader : ImageLoader
    {
        private Image source;
        private Image[] frames;
        public override Image Set(string path)
        {
            FileStream fs = File.OpenRead(path);
            source = new Bitmap(fs);
            FrameDimension dimension = new FrameDimension(source.FrameDimensionsList[0]);
            TotalFramesCount = source.GetFrameCount(dimension);
            frames = new Image[TotalFramesCount];
            for (int i = 0; i < TotalFramesCount; i++)
            {
                source.SelectActiveFrame(dimension, i);
                frames[i] = new Bitmap(source);
            }
            return source;
        }
        public override Image Get(int index)
        {
            return frames[index];
        }
        public override int Delay {
            get
            {
                PropertyItem item = source.GetPropertyItem(0x5100);
                return (item.Value[0] + item.Value[1] * 256) * 10;
            }
        }
    }
}
