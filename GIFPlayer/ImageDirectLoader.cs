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
    /// Load the Image
    /// </summary>
    /// <remarks>
    /// Require less memory, but more access time
    /// </remarks>
    class ImageDirectLoader : ImageLoader
    {
        private Image image;
        private FrameDimension dimension;
        public override Image Set(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            MemoryStream ms = new MemoryStream(bytes);
            image = Image.FromStream(ms);
            dimension = new FrameDimension(image.FrameDimensionsList[0]);
            return image;
        }
        public override Image Get(int index)
        {
            image.SelectActiveFrame(dimension, index);
            return image;
        }
        public override int Delay
        {
            get
            {
                PropertyItem item = image.GetPropertyItem(0x5100);
                return (item.Value[0] + item.Value[1] * 256) * 10;
            }
        }
    }
}
