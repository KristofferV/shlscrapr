using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using shlscrapr.Models;
using shlscrapr.Processors;
using shlscrapr.Processors.Penalties;

namespace shlscrapr.tests
{
    [TestClass]
    public class CalculatePlayersOnIceTests
    {
        private PenaltyBox _penaltyBox;
        const int StartTimeTwoMinutes = 200;

        [TestInitialize]
        public void Initialize()
        {
            _penaltyBox = new PenaltyBox(1);
        }

        [TestMethod]
        public void MinorAndMajorShouldBeFiveOnThree()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMajor(false), PenaltyCreator.CreateMinor(false) });

            Assert.AreEqual(PlayersOnIce.FiveOnThree, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MinorThenMinorAndMinorShouldKvittas()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true) });
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true, StartTimeTwoMinutes), PenaltyCreator.CreateMinor(false, StartTimeTwoMinutes) }); 
            
            Assert.AreEqual(PlayersOnIce.FourOnFive, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MinorThenMinorAndDoubleMinorShouldBeFourOnFour()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true) });
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true, StartTimeTwoMinutes), PenaltyCreator.CreateDoubleMinor(false, StartTimeTwoMinutes) });

            Assert.AreEqual(PlayersOnIce.FourOnFour, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MinorThenDoubleMinorAndDoubleMinorShouldKvittas()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true) });
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateDoubleMinor(true, StartTimeTwoMinutes), PenaltyCreator.CreateDoubleMinor(false, StartTimeTwoMinutes) });

            Assert.AreEqual(PlayersOnIce.FourOnFive, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MinorThenMinorPlusMinorAndMinorShouldKvittas()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true) });
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true, StartTimeTwoMinutes), PenaltyCreator.CreateMinor(true, StartTimeTwoMinutes), PenaltyCreator.CreateMinor(false, StartTimeTwoMinutes) });

            Assert.AreEqual(PlayersOnIce.ThreeOnFive, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MinorAndMinorShouldKvittasWhenNotFiveOnFive()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true), PenaltyCreator.CreateMinor(false) });
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true, StartTimeTwoMinutes), PenaltyCreator.CreateMinor(false, StartTimeTwoMinutes) });

            Assert.AreEqual(PlayersOnIce.FourOnFour, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MinorAndMinorShouldNotKvittasWhenFiveOnFive()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true), PenaltyCreator.CreateMinor(false) });

            Assert.AreEqual(PlayersOnIce.FourOnFour, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MinorPlusMisconductAndMinorShouldNotKvittasWhenFiveOnFive()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinorAndMisconduct(true), PenaltyCreator.CreateMinor(false) });

            Assert.AreEqual(PlayersOnIce.FourOnFour, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void DoubleMinorAndMinorShouldKvittasWhenFiveOnFive()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateDoubleMinor(true), PenaltyCreator.CreateMinor(false) });

            Assert.AreEqual(PlayersOnIce.FourOnFive, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void DoubleMinorPlusMinorAndMinorPlusMinorShouldKvittasWhenFiveOnFive()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateDoubleMinor(true), PenaltyCreator.CreateMinor(true), PenaltyCreator.CreateMinor(false), PenaltyCreator.CreateMinor(false) });

            Assert.AreEqual(PlayersOnIce.FourOnFive, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void DoubleMinorPlusDoubleMinorAndDoubleMinorShouldBeFourOnFive()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateDoubleMinor(true), PenaltyCreator.CreateDoubleMinor(true), PenaltyCreator.CreateDoubleMinor(false) });

            Assert.AreEqual(PlayersOnIce.FourOnFive, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MajorAndMajorShouldKvittasWhenFiveOnFive()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMajorAndGame(true), PenaltyCreator.CreateMajorAndGame(false) });

            Assert.AreEqual(PlayersOnIce.FiveOnFive, _penaltyBox.PlayersOnIce);
        }

        [TestMethod]
        public void MinorPlusDoubleMinorPlusMinorAndMinorPlusMinorShouldBeFourOnFiveForFourMinutes()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateMinor(true), PenaltyCreator.CreateDoubleMinor(true), PenaltyCreator.CreateMinor(true), PenaltyCreator.CreateMinor(false), PenaltyCreator.CreateMinor(false) });

            Assert.AreEqual(PlayersOnIce.FourOnFive, _penaltyBox.PlayersOnIce);
            Assert.AreEqual(2, _penaltyBox.PenaltyScoreBoard.Home.Count());
            Assert.AreEqual(340, _penaltyBox.PenaltyScoreBoard.Home.Last().EndTime);
        }

        [TestMethod]
        public void DoubleMinorPlusDoubleMinorAndMajorPlusDoubleMinorPlusMinorShouldBeFourOnFourForTwoMinutes()
        {
            _penaltyBox.AddPenalties(new[] { PenaltyCreator.CreateDoubleMinor(true), PenaltyCreator.CreateDoubleMinor(true), PenaltyCreator.CreateMajorAndGame(false), PenaltyCreator.CreateDoubleMinor(false), PenaltyCreator.CreateMinor(false) });

            Assert.AreEqual(PlayersOnIce.FourOnFour, _penaltyBox.PlayersOnIce);

            //Expire minors
            var ex = _penaltyBox.PenaltiesThatExpireThisSecond(220).ToList();
            _penaltyBox.ExpirePenalties(ex);

            Assert.AreEqual(PlayersOnIce.FiveOnFour, _penaltyBox.PlayersOnIce);

            //Expire Major
            var ex2 = _penaltyBox.PenaltiesThatExpireThisSecond(400).ToList();
            _penaltyBox.ExpirePenalties(ex2);

            Assert.AreEqual(PlayersOnIce.FiveOnFive, _penaltyBox.PlayersOnIce);

            //Assert.AreEqual(2, _penaltyBox.PenaltyScoreBoard.Home.Count());
            //Assert.AreEqual(340, _penaltyBox.PenaltyScoreBoard.Home.Last().EndTime);
        }
    }
}
