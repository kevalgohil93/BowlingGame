using BowlingBall.Common;
using System.Collections.Generic;
using System.Linq;

namespace BowlingBall.Domains
{
    public class Frame
    {
        public Frame(int knockedPin, FrameState frameState)
        {
            Bowls.Add(knockedPin);
            State = frameState;
        }

        public List<int> Bowls { get; set; } = new List<int>();

        public int Score => Bowls.Take(BowlingConstants.MaxRoundsInFrame).Sum();

        public FrameState State { get; set; }
    }
}

