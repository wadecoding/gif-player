using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GIFPlayer
{
    public class ImageManager
    {
        public string Path { get; protected set; }
        private ImageLoader loader = null;
        public int ActiveFrameIndex { get; set; }
        public int TotalFramesCount => loader.TotalFramesCount;
        public ImageManager(ImageLoader loader)
        {
            this.loader = loader;
        }
        public Image Get(int index)
        {
            ActiveFrameIndex = index;
            return loader.Get(index);
        }
        public Image Set(string path)
        {
            Path = path;
            ActiveFrameIndex = 0;
            return loader.Set(path);
        }
        public int Delay => loader.Delay;
    }
}
