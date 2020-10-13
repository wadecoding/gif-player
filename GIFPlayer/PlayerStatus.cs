using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIFPlayer
{
    public class PlayerStatus
    {
        public enum Status
        {
            Playing, Pausing, SeekMoving
        }
        public Status status { get; set; } = Status.Pausing;
        public bool isAutoplay { get; set; } = true;
        public float AdditionalDelayCoefficient = 1.0f;
    }
}
