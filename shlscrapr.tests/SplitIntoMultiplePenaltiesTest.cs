using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using shlscrapr.Models;
using shlscrapr.Processors;
using shlscrapr.Processors.Penalties;

namespace shlscrapr.tests
{
    [TestClass]
    public class SplitIntoMultiplePenaltiesTest
    {
        private List<PlayEvent> _penaltyList;

        [TestInitialize]
        public void Initialize()
        {
            _penaltyList = new List<PlayEvent>();
        }

        [TestMethod]
        public void ShouldSplitDoubleMinor()
        {
            _penaltyList.Add(PenaltyCreator.CreateDoubleMinor(true));

            var split = _penaltyList.SplitIntoMultiplePenalties().ToList();

            Assert.AreEqual(2, split.Count());
            Assert.AreEqual(true, split.First().PenaltyIsMinor);
            Assert.AreEqual(true, split.Last().PenaltyIsMinor);
        }

        [TestMethod]
        public void ShouldSplitMinorAndMisconduct()
        {
            _penaltyList.Add(PenaltyCreator.CreateMinorAndMisconduct(true));

            var split = _penaltyList.SplitIntoMultiplePenalties().ToList();

            Assert.AreEqual(2, split.Count());
            Assert.AreEqual(true, split.First().PenaltyIsMinor);
            Assert.AreEqual(true, split.Last().PenaltyIsMisconduct);
            Assert.AreEqual(false, split.Last().PenaltyIsMinor);
        }

        [TestMethod]
        public void ShouldSplitMajorAndGame()
        {
            _penaltyList.Add(PenaltyCreator.CreateMajorAndGame(true));

            var split = _penaltyList.SplitIntoMultiplePenalties();

            Assert.AreEqual(2, split.Count());
        }

        [TestMethod]
        public void ShouldNotSplitMajor()
        {
            _penaltyList.Add(PenaltyCreator.CreateMajor(true));

            var split = _penaltyList.SplitIntoMultiplePenalties();

            Assert.AreEqual(1, split.Count());
        }

        [TestMethod]
        public void ShouldNotSplitMinor()
        {
            _penaltyList.Add(PenaltyCreator.CreateMinor(true));

            var split = _penaltyList.SplitIntoMultiplePenalties();

            Assert.AreEqual(1, split.Count());
        }
    }
}
