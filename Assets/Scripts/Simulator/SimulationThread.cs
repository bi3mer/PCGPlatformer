using System.Collections.Generic;
using System.IO;

using Tools.AI.NGram;
using LightJson;
using PCG;

namespace Simulator
{
    public class SimulationThread
    {
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
            for (int i = 0; i < numSimulations; ++i)
            {
                List<string> columns;
                List<string> simplified;

                if (simplifiedGram == null)
                {
                    columns = NGramGenerator.Generate(compiled, startInput, size, includeStart: false);
                    simplified = LevelParser.BreakColumnsIntoSimplifiedTokens(
                        columns,
                        game == Games.Custom);
                }
                else
                {
                    ICompiledGram simpleCompiled = simplifiedGram.Compile();
                    simplified = NGramGenerator.Generate(
                        simpleCompiled,
                        LevelParser.BreakColumnsIntoSimplifiedTokens(startInput, game == Games.Custom),
                        size);

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

                string[] array = columns.ToArray();
                List<double> positions = LevelAnalyzer.Positions(array);
                JsonArray jsonPositions = new JsonArray();
                foreach (double pos in positions)
                {
                    jsonPositions.Add(pos);
                }

                writer.Write($"{compiled.SequenceProbability(array)},");
                writer.Write($"{compiled.Perplexity(array)},");
                writer.Write($"{jsonPositions},");
                writer.Write($"{LevelAnalyzer.Leniency(simplified.ToArray())}\n");
                writer.Flush();
            }

            writer.Close();
            //watch.Stop();
            //Debug.Log($"Execution Time for {extension}_{gram.GetN()}: {watch.ElapsedMilliseconds / 1000d} seconds");
        }
    }
}