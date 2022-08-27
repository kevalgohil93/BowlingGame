using BowlingBall.Common;
using BowlingBall.Domains;

namespace BowlingBall.Factory
{
    public static class FrameFactory
    {
        public static Frame CreateFrame(int pins, FrameState frameState)
        {
            return new Frame(pins, frameState);
        }
    }
}
