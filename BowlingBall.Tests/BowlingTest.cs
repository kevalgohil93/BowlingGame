using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BowlingBall.Tests
{
    [TestClass]
    public class GameFixture
    {
        private readonly Game bowling;

        public GameFixture()
        {
            bowling = new Game();
        }

        #region Private methods
        private void GenerateRollPins(IEnumerable<int> knockedPins)
        {
            AddRollPins(knockedPins);
        }

        private void GenerateRepeatRollPins(int count, int RepeatKnockedPin)
        {
            IEnumerable<int> knockedPins = Enumerable.Repeat(RepeatKnockedPin, count);
            AddRollPins(knockedPins);
        }

        private void AddRollPins(IEnumerable<int> knockedPins)
        {
            foreach (int knockedPin in knockedPins)
            {
                bowling.Roll(knockedPin);
            }
        }
        #endregion

        [TestMethod]
        public void Bowling_TestAllZeroPins()
        {
            //Arrange
            GenerateRepeatRollPins(20, 0);

            //Act
            int Result = bowling.GetScore();

            //Assert
            Assert.AreEqual(0, Result);
        }

        [TestMethod]
        public void Bowling_TestAllOnePins()
        {
            //Arrange
            GenerateRepeatRollPins(20, 1);

            //Act
            int Result = bowling.GetScore();

            //Assert
            Assert.AreEqual(20, Result);
        }

        [TestMethod]
        public void Bowling_TestOneSpare()
        {
            //Arrange
            bowling.Roll(9);
            bowling.Roll(1);
            GenerateRepeatRollPins(18, 1);

            //Act
            int Result = bowling.GetScore();

            //Assert
            Assert.AreEqual(29, Result);
        }

        [TestMethod]
        public void Bowling_TestOneStrike()
        {
            //Arrange
            bowling.Roll(10);
            GenerateRepeatRollPins(19, 1);

            //Act
            int Result = bowling.GetScore();

            //Assert
            Assert.AreEqual(31, Result);
        }


        [TestMethod]
        public void Bowling_TestAllStrike()
        {
            //Arrange
            GenerateRepeatRollPins(12, 10);

            //Act
            int Result = bowling.GetScore();

            //Assert
            Assert.AreEqual(300, Result);
        }

        [TestMethod]
        public void Bowling_TestLastFramewithSpareAndStrike()
        {
            //Arrange
            GenerateRollPins(new List<int>() { 10, 9, 1, 5, 5, 7, 2, 10, 10, 10, 9, 0, 8, 2, 9, 1, 10 });

            //Act
            int Result = bowling.GetScore();

            //Assert
            Assert.AreEqual(187, Result);
        }

        [TestMethod]
        public void Bowling_TestLastFramewithThreeStrike()
        {
            //Arrange
            GenerateRollPins(new List<int>() { 10, 9, 1, 5, 5, 7, 2, 10, 10, 10, 9, 0, 8, 2, 10, 10, 10 });

            //Act
            int Result = bowling.GetScore();

            //Assert
            Assert.AreEqual(198, Result);
        }
    }
}
