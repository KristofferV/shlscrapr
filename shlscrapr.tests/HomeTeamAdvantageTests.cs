using Microsoft.VisualStudio.TestTools.UnitTesting;
using shlscrapr.Models;
using shlscrapr.Processors;
using shlscrapr.Processors.Goals;

namespace shlscrapr.tests
{
    [TestClass]
    public class HomeTeamAdvantageTests
    {
        private PlayScore _playScore;

        [TestInitialize]
        public void Initialize()
        {
            _playScore = new PlayScore();
        }

        [TestMethod]
        public void OneZeroShouldBeOneUp()
        {
            _playScore.AddGoal(true);

            Assert.AreEqual(TeamAdvantage.OneUp, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void TwoZeroShouldBeTwoUp()
        {
            _playScore.AddGoal(true);
            _playScore.AddGoal(true);

            Assert.AreEqual(TeamAdvantage.TwoUp, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ThreeZeroShouldBeThreeUpPlus()
        {
            _playScore.AddGoal(true);
            _playScore.AddGoal(true);
            _playScore.AddGoal(true);

            Assert.AreEqual(TeamAdvantage.ThreeUpPlus, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ZeroOneShouldBeOneDown()
        {
            _playScore.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.OneDown, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ZeroTwoShouldBeTwoDown()
        {
            _playScore.AddGoal(false);
            _playScore.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.TwoDown, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ZeroThreeShouldBeThreeDownPlus()
        {
            _playScore.AddGoal(false);
            _playScore.AddGoal(false);
            _playScore.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.ThreeDownPlus, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void OneOneShouldBeEven()
        {
            _playScore.AddGoal(true);
            _playScore.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.Even, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void TwoTwoShouldBeEven()
        {
            _playScore.AddGoal(true);
            _playScore.AddGoal(true);
            _playScore.AddGoal(false);
            _playScore.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.Even, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void ZeroZeroShouldBeEven()
        {
            Assert.AreEqual(TeamAdvantage.Even, _playScore.HomeTeamAdvantage);
        }
        [TestMethod]
        public void FiveFiveShouldBeEven()
        {
            _playScore.AddGoal(true);
            _playScore.AddGoal(true);
            _playScore.AddGoal(true);
            _playScore.AddGoal(true);
            _playScore.AddGoal(true);
            _playScore.AddGoal(false);
            _playScore.AddGoal(false);
            _playScore.AddGoal(false);
            _playScore.AddGoal(false);
            _playScore.AddGoal(false);

            Assert.AreEqual(TeamAdvantage.Even, _playScore.HomeTeamAdvantage);
        }
    }
}
