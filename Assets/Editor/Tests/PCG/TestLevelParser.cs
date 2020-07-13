﻿using System.Collections.Generic;
using NUnit.Framework;
using Tools.AI.NGram;
using UnityEngine;
using PCG;

namespace Editor.Tests.PCG
{
    public class TestLevelParser
    {
        [Test]
        public void TestParseCustomLevelLinear()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseCustomLevelLinearEnemy()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseCustomLevelPlatform()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseCustomLevelPlatformEnemy()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseCustomLevelPlatformOptional()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseCustomLevelPlatformOptionalEnemy()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseCustomTogether()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseVGLCLevelLinear()
        {
            List<string> columns = new List<string>() { "b-----------" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.Linear, result[0]);

            columns.Add("b-");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.Linear, result[0]);
            Assert.AreEqual(SimplifiedColumns.Linear, result[1]);
        }

        [Test]
        public void TestParseVGLCLevelLinearEnemy()
        {
            List<string> columns = new List<string>() { "b------A----" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);

            columns.Add("bB----------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);

            columns.Add("b--------C");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[2]);

            columns.Add("b----D----");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[2]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[3]);

            columns.Add("b--A--BC-D----");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[2]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[3]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[4]);
        }

        [Test]
        public void TestParseVGLCLevelPlatform()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseVGLCLevelPlatformEnemy()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseVGLCLevelPlatformOptional()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseVGLCLevelPlatformOptionalEnemy()
        {
            Assert.Fail();
        }

        [Test]
        public void TestParseVGLCLevelTogether()
        {
            Assert.Fail();
        }
    }
}
