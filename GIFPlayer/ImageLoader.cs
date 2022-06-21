using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace GIFPlayer
{
    abstract public class ImageLoader
    {
        public abstract Image Get(int index);
        public abstract Image Set(string path);
        public abstract int Delay { get; }
        public int TotalFramesCount { get; protected set; }
    }
}
