using BowlingBall.Common;
using BowlingBall.Domains;
using BowlingBall.Factory;
using System.Collections.Generic;
using System.Linq;

namespace BowlingBall
{
    public class Game : IBowlingGame
    {
        private bool IsResetFrame { get; set; } = true;

        private readonly List<Frame> frames = new List<Frame>();

        public void Roll(int pins)
        {
            if (IsResetFrame) // first bowl in new frame
            {
                FrameState frameState = GetFrameState(pins);
                frames.Add(FrameFactory.CreateFrame(pins, frameState));
                IsResetFrame = !IsLastFrame() && IsStrike(frameState);
            }
            else // subsequent bowl in last frame
            {
                bool isLastFrame = IsLastFrame();
                if (isLastFrame && frames[BowlingConstants.MaxFrame - 1].Bowls.Count == BowlingConstants.MaxRoundsInFrame + BowlingConstants.BonusRoundsInLastFrame)
                {
                    return;
                }

                Frame recentFrame = frames.Last();
                recentFrame.Bowls.Add(pins);
                recentFrame.State = GetFrameState(recentFrame.Score);
                IsResetFrame = !isLastFrame || IsResetFrame;
            }
        }

        public int GetScore()
        {
            int score = 0;
            Frame frame;

            for (int frameIndex = 0; frameIndex < frames.Count; frameIndex++)
            {
                frame = frames[frameIndex];

                if (!IsLastFrameIndex(frameIndex) && IsStrike(frame.State)) //calculate strike bonus if its not last frame
                {
                    score += frame.Score + CalculateStrikeBouns(frameIndex);
                }
                else if (!IsLastFrameIndex(frameIndex) && IsSpared(frame.State)) //calculate spare bonus if its not last frame
                {
                    score += frame.Score + frames[GetNextFrameIndex(frameIndex)].Bowls.FirstOrDefault();
                }
                else //if its not strike or spare then get score of all rounds in that frame
                {
                    //Score with Bonus
                    score += frame.Bowls.Sum();
                }
            }

            return score;
        }

        #region Private Methods
        private FrameState GetFrameState(int score)
        {
            if (score == 10)
            {
                return IsResetFrame ? FrameState.IsStrike : FrameState.IsSpare;
            }

            return FrameState.Ordinary;
        }

        private static bool IsStrike(FrameState frameState)
        {
            return frameState == FrameState.IsStrike;
        }

        private static bool IsSpared(FrameState frameState)
        {
            return frameState == FrameState.IsSpare;
        }

        private bool IsLastFrame()
        {
            return frames.Count == BowlingConstants.MaxFrame;
        }

        private static bool IsLastFrameIndex(int frameIndex)
        {
            return frameIndex == BowlingConstants.MaxFrame - 1;
        }

        private int CalculateStrikeBouns(int frameIndex)
        {
            int sum = 0;
            int nextFrameIndex = GetNextFrameIndex(frameIndex);
            Frame frame = frames[nextFrameIndex];

            if (IsStrike(frame.State) && !IsLastFrameIndex(nextFrameIndex)) // If the next frame also has strike then add next frame ball
            {
                sum += frame.Score + frames[GetNextFrameIndex(nextFrameIndex)].Bowls.FirstOrDefault();
            }
            else
            {
                sum += frame.Score;
            }

            return sum;
        }

        private static int GetNextFrameIndex(int frameIndex)
        {
            return ++frameIndex;
        }

        #endregion
    }
}
