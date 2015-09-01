using Microsoft.VisualStudio.TestTools.UnitTesting;
using shlscrapr.Models;
using shlscrapr.Processors.Goals;

namespace shlscrapr.tests
{
    [TestClass]
    public class HomeTeamAdvantageTests
    {
        private ScoreBoard _scoreBoard;

        [TestInitialize]
        public void Initialize()
        {
            _scoreBoard = new ScoreBoard();
        }

        [TestMethod]
        public void OneZeroShouldBeOneUp()
        {
            _scoreBoard.AddGoal(true);

            Assert.AreEqual(TeamAdvantage.OneUp, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void TwoZeroShouldBeTwoUp()
        {
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(true);

            Assert.AreEqual(TeamAdvantage.TwoUp, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ThreeZeroShouldBeThreeUpPlus()
        {
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(true);

            Assert.AreEqual(TeamAdvantage.ThreeUpPlus, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ZeroOneShouldBeOneDown()
        {
            _scoreBoard.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.OneDown, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ZeroTwoShouldBeTwoDown()
        {
            _scoreBoard.AddGoal(false);
            _scoreBoard.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.TwoDown, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ZeroThreeShouldBeThreeDownPlus()
        {
            _scoreBoard.AddGoal(false);
            _scoreBoard.AddGoal(false);
            _scoreBoard.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.ThreeDownPlus, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void OneOneShouldBeEven()
        {
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.Even, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void TwoTwoShouldBeEven()
        {
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(false);
            _scoreBoard.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.Even, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ZeroZeroShouldBeEven()
        {
            Assert.AreEqual(TeamAdvantage.Even, _scoreBoard.HomeTeamAdvantage);
        }
        [TestMethod]
        public void FiveFiveShouldBeEven()
        {
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(true);
            _scoreBoard.AddGoal(false);
            _scoreBoard.AddGoal(false);
            _scoreBoard.AddGoal(false);
            _scoreBoard.AddGoal(false);
            _scoreBoard.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.Even, _scoreBoard.HomeTeamAdvantage);
        }
    }
}
