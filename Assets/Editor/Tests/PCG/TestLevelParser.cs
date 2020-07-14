using System.Collections.Generic;
using NUnit.Framework;
using PCG;

namespace Editor.Tests.PCG
{
    public class TestLevelParser
    {
        [Test]
        public void TestParseCustomLevelLinear()
        {
            List<string> columns = new List<string>() { "-----------------b---" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.Linear, result[0]);

            columns.Add("-----------------b-----------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.Linear, result[0]);
            Assert.AreEqual(SimplifiedColumns.Linear, result[1]);
        }

        [Test]
        public void TestParseCustomLevelLinearEnemy()
        {
            List<string> columns = new List<string>() { "-----------------bA--" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);

            columns.Add("-----------------b------------B----");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);

            columns.Add("-----C-----------b-----------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[2]);

            columns.Add("-----------------b----------------D");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[2]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[3]);
        }

        [Test]
        public void TestParseCustomLevelPlatform()
        {
            List<string> columns = new List<string>() { "---------------------" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[0]);

            columns.Add("-----------------bbbbb--------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[1]);

            columns.Add("----------------bbbbbb--------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[1]);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[2]);
        }

        [Test]
        public void TestParseCustomLevelPlatformEnemy()
        {
            List<string> columns = new List<string>() { "---A-----------------" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[0]);

            columns.Add("-----------------bbbbb---------B----");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[1]);

            columns.Add("----------------bbbbbbC-------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[2]);

            columns.Add("----------------bbbbbbD-------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[2]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[3]);
        }

        [Test]
        public void TestParseCustomLevelPlatformOptional()
        {
            List<string> columns = new List<string>() { "-----------------b--b-" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformOptional, result[0]);

            columns.Add("----b------------b-----------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformOptional, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptional, result[1]);
        }

        [Test]
        public void TestParseCustomLevelPlatformOptionalEnemy()
        {
            List<string> columns = new List<string>() { "-----------------b--bA" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[0]);

            columns.Add("----bB-----------b-----------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[1]);

            columns.Add("----b------------bC----------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[2]);

            columns.Add("D---b------------b------------------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[2]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[3]);
        }

        [Test]
        public void TestParseCustomTogether()
        {
            List<string> columns = new List<string>() { 
                "-----------------b---",
                "-----------------bA--",
                "---------------------",
                "-----------------A---",
                "-----------------b-b-",
                "-----------------b-Cb",
            };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, true);
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual(SimplifiedColumns.Linear, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[2]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[3]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptional, result[4]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[5]);
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

            columns.Add("b----$$$$---");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(SimplifiedColumns.Linear, result[0]);
            Assert.AreEqual(SimplifiedColumns.Linear, result[1]);
            Assert.AreEqual(SimplifiedColumns.Linear, result[2]);
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
            List<string> columns = new List<string>() { "bb---------" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[0]);

            columns.Add("--------");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[1]);

            columns.Add("bbbb----");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[1]);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[2]);
        }

        [Test]
        public void TestParseVGLCLevelPlatformEnemy()
        {
            List<string> columns = new List<string>() { "bbA--------" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[0]);

            columns.Add("---C-----");
            result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[0]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[1]);
        }

        [Test]
        public void TestParseVGLCLevelPlatformOptional()
        {
            List<string> columns = new List<string>() { "b---b----" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformOptional, result[0]);
        }

        [Test]
        public void TestParseVGLCLevelPlatformOptionalEnemy()
        {
            List<string> columns = new List<string>() { "b---bA----" };
            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[0]);
        }

        [Test]
        public void TestParseVGLCLevelTogether()
        {
            List<string> columns = new List<string>() 
            { 
               "b---------",
               "b---A-----",
               "----------",
               "---B------",
               "b----b----",
               "b----b---D"
            };

            List<string> result = LevelParser.BreakColumnsIntoSimplifiedTokens(columns, false);
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual(SimplifiedColumns.Linear, result[0]);
            Assert.AreEqual(SimplifiedColumns.LinearEnemy, result[1]);
            Assert.AreEqual(SimplifiedColumns.PlatformForced, result[2]);
            Assert.AreEqual(SimplifiedColumns.PlatformForcedEnemy, result[3]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptional, result[4]);
            Assert.AreEqual(SimplifiedColumns.PlatformOptionalEnemy, result[5]);
        }
    }
}
