﻿using System.Collections.Generic;
using System.IO;
using System;

using Tools.AI.NGram;
using Tools.Utility;
using LightJson;
using PCG;

namespace Simulator
{
    public class SimulationThread
    {
        private const int maxAttempts = 5;

        private int numSimulations;
        private int size;
        private Games game;
        private string basePath;
        private string extension;
        private IGram gram;
        private IGram simplifiedGram;
        private List<string> startInput;

        public SimulationThread(
            int numSimulations,
            int size,
            Games game,
            string basePath,
            string extension,
            IGram gram,
            IGram simplifiedGram,
            List<string> startInput)
        {
            this.numSimulations = numSimulations;
            this.size = size;
            this.game = game;
            this.basePath = basePath;
            this.extension = extension;
            this.gram = gram;
            this.simplifiedGram = simplifiedGram;
            this.startInput = startInput;
        }

        public void Execute()
        {
            string keyDirectory = Path.Combine(basePath, $"{extension}_{gram.GetN()}");
            if (Directory.Exists(keyDirectory) == false)
            {
                Directory.CreateDirectory(keyDirectory);
            }

            StreamWriter writer = File.CreateText($"{keyDirectory}.txt");
            writer.WriteLine("Sequence_Probability,Perplexity,Linearity_JSON_Positions,Leniency");

            ICompiledGram compiled = gram.Compile();
            ICompiledGram simpleCompiled = simplifiedGram?.Compile();

            for (int i = 0; i < numSimulations; ++i)
            {
                UtilityRandom.SetSeed(new DateTime().Millisecond);

                Tuple<List<string>, List<string>> tuple;

                if (gram as NGram == null)
                {
                    tuple = GetColumnsBestGuess(compiled, simpleCompiled);
                }
                else
                {
                    tuple = GetColumnsSemiGuaranteed(compiled, simpleCompiled);
                }

                List<string> columns = tuple.Item1;
                List<string> simplified = tuple.Item2;
                
                string[] columnsArray = columns.ToArray();
                List<int> positions = LevelAnalyzer.Positions(columnsArray);
                JsonArray jsonPositions = new JsonArray();
                foreach (int pos in positions)
                {
                    jsonPositions.Add(pos);
                }

                double sequenceProbability = compiled.SequenceProbability(columnsArray);
                writer.Write($"{sequenceProbability},");
                if (sequenceProbability == 0)
                {
                    writer.Write($"0,");
                }
                else
                { 
                    writer.Write($"{1d/sequenceProbability},");

                }

                writer.Write($"{jsonPositions},");
                writer.Write($"{LevelAnalyzer.Leniency(simplified.ToArray())}\n");

                StreamWriter levelWriter = File.CreateText(Path.Combine(keyDirectory, $"{i}.txt"));
                levelWriter.Write(string.Join("\n", columnsArray));
                levelWriter.Flush();
                levelWriter.Close();

                if (i % 200 == 0)
                { 
                    writer.Flush();
                }
            }

            writer.Flush();
            writer.Close();
        }

        private Tuple<List<string>, List<string>> GetColumnsBestGuess(ICompiledGram compiled, ICompiledGram simpleCompiled)
        {
            List<string> columns;
            List<string> simplified;

            if (simplifiedGram == null)
            {
                columns = NGramGenerator.GenerateBestAttempt(compiled, startInput, size, maxAttempts);
                simplified = LevelParser.BreakColumnsIntoSimplifiedTokens(
                    columns,
                    game);
            }
            else
            {
                simplified = NGramGenerator.GenerateBestAttempt(
                    simpleCompiled,
                    LevelParser.BreakColumnsIntoSimplifiedTokens(startInput, game),
                    size,
                    maxAttempts);

                Games localGame = game;
                columns = NGramGenerator.GenerateBestRestrictedAttempt(
                    compiled,
                    startInput,
                    simplified,
                    (inColumn) =>
                    {
                        return LevelParser.ClassifyColumn(inColumn, localGame);
                    },
                    maxAttempts);
            }

            return new Tuple<List<string>, List<string>>(columns, simplified);
        }

        private Tuple<List<string>, List<string>> GetColumnsSemiGuaranteed(ICompiledGram compiled, ICompiledGram simpleCompiled)
        {
            List<string> columns;
            List<string> simplified;

            if (simplifiedGram == null)
            {
                columns = NGramGenerator.Generate(compiled, startInput, size, includeStart:false);
                simplified = LevelParser.BreakColumnsIntoSimplifiedTokens(
                    columns,
                    game);
            }
            else
            {
                simplified = NGramGenerator.GenerateBestAttempt(
                    simpleCompiled,
                    LevelParser.BreakColumnsIntoSimplifiedTokens(startInput, game),
                    size,
                    maxAttempts);

                Games localGame = game;
                columns = NGramGenerator.GenerateRestricted(
                    compiled,
                    startInput,
                    simplified,
                    (inColumn) =>
                    {
                        return LevelParser.ClassifyColumn(inColumn, localGame);
                    },
                    includeStart: false);
            }

            return new Tuple<List<string>, List<string>>(columns, simplified);
        }
    }
}