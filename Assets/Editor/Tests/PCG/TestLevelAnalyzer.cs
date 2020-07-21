using System.Collections.Generic;
using NUnit.Framework;
using PCG;

namespace Editor.Tests.PCG
{
    public class TestLevelAnalyzer
    {
        [Test]
        public void TestLeniency()
        {
            List<string> columns = new List<string>();
            columns.Add(SimplifiedColumns.Linear);
            Assert.AreEqual(0, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.LinearEnemy);
            Assert.AreEqual(0.5, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.PlatformOptional);
            Assert.AreEqual(0.6, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.PlatformOptionalEnemy);
            Assert.AreEqual(1.2, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.PlatformForced);
            Assert.AreEqual(1.7, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.PlatformForcedEnemy);
            Assert.AreEqual(2.7, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.LinearEnemy);
            Assert.AreEqual(3.2, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.Linear);
            Assert.AreEqual(3.2, LevelAnalyzer.Leniency(columns.ToArray()));
        }

        [Test]
        public void TestLinearity()
        {
            List<string> columns = new List<string>
            {
                "----b",
                "----b",
                "----b",
                "----b"
            };
            List<int> result = LevelAnalyzer.Positions(columns.ToArray());
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(1, result[2]);
            Assert.AreEqual(1, result[3]);

            columns = new List<string>()
            {
                "----b",
                "-----",
                "---b-",
                "-----",
                "--b--",
                "-----",
                "-b---",
                "b----"
            };

            result = LevelAnalyzer.Positions(columns.ToArray());
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(4, result[3]);

            columns = new List<string>()
            {
                "----b",
                "-b--b",
                "-----",
                "----b",
                "--b--",
            };

            result = LevelAnalyzer.Positions(columns.ToArray());
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(4, result[1]);
            Assert.AreEqual(1, result[2]);
            Assert.AreEqual(3, result[3]);
        }
    }
}