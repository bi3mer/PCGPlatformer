using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

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
            Assert.AreEqual(1, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.LinearEnemy);
            Assert.AreEqual(1.5, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.PlatformOptional);
            Assert.AreEqual(2.4, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.PlatformOptionalEnemy);
            Assert.AreEqual(2.8, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.PlatformForced);
            Assert.AreEqual(3.3, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.PlatformForcedEnemy);
            Assert.AreEqual(3.3, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.LinearEnemy);
            Assert.AreEqual(3.8, LevelAnalyzer.Leniency(columns.ToArray()));

            columns.Add(SimplifiedColumns.Linear);
            Assert.AreEqual(4.8, LevelAnalyzer.Leniency(columns.ToArray()));
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
            List<double> result = LevelAnalyzer.Positions(columns.ToArray());
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1 / 5d, result[0]);
            Assert.AreEqual(1 / 5d, result[1]);
            Assert.AreEqual(1 / 5d, result[2]);
            Assert.AreEqual(1 / 5d, result[3]);

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
            Assert.AreEqual(1 / 5d, result[0]);
            Assert.AreEqual(2 / 5d, result[1]);
            Assert.AreEqual(3 / 5d, result[2]);
            Assert.AreEqual(4 / 5d, result[3]);

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
            Assert.AreEqual(1 / 5d, result[0]);
            Assert.AreEqual(4 / 5d, result[1]);
            Assert.AreEqual(1 / 5d, result[2]);
            Assert.AreEqual(3 / 5d, result[3]);
        }
    }
}