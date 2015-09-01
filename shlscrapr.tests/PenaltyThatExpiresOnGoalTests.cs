using Microsoft.VisualStudio.TestTools.UnitTesting;
using shlscrapr.Models;
using shlscrapr.Processors;
using shlscrapr.Processors.Penalties;

namespace shlscrapr.tests
{
    [TestClass]
    public class PenaltyThatExpiresOnGoalTests
    {
        private PenaltyBox _penaltyBox;

        [TestInitialize]
        public void Initialize()
        {
            _penaltyBox = new PenaltyBox(1);
        }

        [TestMethod]
        public void FirstInFirstOut()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMajor(false), PenaltyCreator.CreateMinor(false) });

            Assert.AreEqual(PlayersOnIce.FiveOnThree, _penaltyBox.PlayersOnIce);
        }
    }
}
