using System.Collections.Generic;
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
            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            string path = Path.Combine(basePath, $"{extension}_{gram.GetN()}.txt");
            StreamWriter writer = File.CreateText(path);
            writer.WriteLine("Sequence_Probability,Perplexity,Linearity_JSON_Positions,Leniency");

            ICompiledGram compiled = gram.Compile();
            ICompiledGram simpleCompiled = simplifiedGram?.Compile();

            for (int i = 0; i < numSimulations; ++i)
            {
                UtilityRandom.SetSeed(new DateTime().Millisecond);
                List<string> columns;
                List<string> simplified;

                if (simplifiedGram == null)
                {
                    //columns = NGramGenerator.GenerateBestAttempt(compiled, startInput, size, maxAttempts);
                    //simplified = LevelParser.BreakColumnsIntoSimplifiedTokens(
                    //    columns,
                    //    game == Games.Custom);
                    continue;
                }
                else
                {
                    simplified = NGramGenerator.GenerateBestAttempt(
                        simpleCompiled,
                        LevelParser.BreakColumnsIntoSimplifiedTokens(startInput, game == Games.Custom),
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

                string[] columnsArray = columns.ToArray();
                List<double> positions = LevelAnalyzer.Positions(columnsArray);
                JsonArray jsonPositions = new JsonArray();
                foreach (double pos in positions)
                {
                    jsonPositions.Add(pos);
                }

                writer.Write($"{compiled.SequenceProbability(columnsArray)},");
                writer.Write($"{compiled.Perplexity(columnsArray)},");
                writer.Write($"{jsonPositions},");
                writer.Write($"{LevelAnalyzer.Leniency(simplified.ToArray())}\n");

                if (i % 200 == 0)
                { 
                    writer.Flush();
                }
            }

            writer.Flush();
            writer.Close();
            //watch.Stop();
            //Debug.Log($"Execution Time for {extension}_{gram.GetN()}: {watch.ElapsedMilliseconds / 1000d} seconds");
        }
    }
}