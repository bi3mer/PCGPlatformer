#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using UnityEditor;

using System.Linq;
using System.IO;

using Tools.AI.NGram;
using LightJson;
using PCG;
using System.Threading;

namespace CustomUnityWindow
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
            string path = Path.Combine(basePath, $"{extension}_{gram.GetN()}.txt");
            StreamWriter writer = File.CreateText(path);
            writer.WriteLine("Sequence_Probability,Perplexity,Linearity,Leniency");

            ICompiledGram compiled = gram.Compile();
            for (int i = 0; i < numSimulations; ++i)
            {
                List<string> columns;
                List<string> simplified;

                if (simplifiedGram == null)
                {
                    columns = NGramGenerator.Generate(compiled, startInput, size);
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
                        });
                }

                string[] array = columns.ToArray();
                writer.Write($"{compiled.SequenceProbability(array)},");
                writer.Write($"{compiled.Perplexity(array)},");
                writer.Write($"{LevelAnalyzer.Linearity(array)},");
                writer.Write($"{LevelAnalyzer.Leniency(simplified.ToArray())}\n");
                writer.Flush();
            }

            writer.Close();
        }
    }

    [InitializeOnLoad]
    public static class RunSimulation
    {
        private static readonly string basePath = "data";

        private const float hiearchicalWeight = 0.9f;
        private const int numSimulations = 1;
        private const int size = 50;

        [MenuItem("Sim/Run")]
        public static void RunDataGeneration()
        {
            if (Directory.Exists(basePath) == false)
            {
                Directory.CreateDirectory(basePath);
            }

            Debug.Log("starting threads");
            Run("GameFlow/PCGPlatformer", "custom", Games.Custom);
            Run("GameFlow/SuperMarioBros", "smb", Games.SuperMarioBros);
            Run("GameFlow/SuperMarioBros2", "smb2", Games.SuperMarioBros2);
            Run("GameFlow/SuperMarioBros2Japan", "smb2j", Games.SuperMarioBros2Japan);
            Run("GameFlow/SuperMarioLand", "sml", Games.SuperMarioLand);
            Debug.Log("All threads have started.");
        }

        // 1,2,3,4,5,6 gram
        // 2,3,4,5,6 backoff
        // 2,3,4,5,6 hiearchical
        // 2,3,4,5,6 simple backoff
        // 2,3,4,5,6 simple hierarchical
        private static void Run(string levelFlowPath, string name, Games game)
        {
            // We first get all the levels from the flow and then get the start
            // input from the first level in the sequence. Note that the first
            // we use the same start input but we don't want that to scew 
            // results. To get around this, we use a flag in the ngram generator
            // which will not include the start output in the end result. 
            List<List<string>> levels = GetLevels(levelFlowPath);
            List<string> startInput = levels[0].GetRange(0, 10);

            for (int i = 1; i <= 6; ++i)
            {
                IGram gram = NGramFactory.InitGrammar(i);
                foreach (List<string> level in levels)
                {
                    NGramTrainer.Train(gram, level);
                }

                RunSimulations(gram, null, startInput, game, $"{name}_ngram");

                if (i != 1)
                { 
                    gram = NGramFactory.InitHierarchicalNGram(i, hiearchicalWeight);
                    IGram simpleGram = NGramFactory.InitHierarchicalNGram(i, hiearchicalWeight);
                    foreach (List<string> level in levels)
                    {
                        NGramTrainer.Train(gram, level);
                        NGramTrainer.Train(
                            simpleGram,
                            LevelParser.BreakColumnsIntoSimplifiedTokens(
                                level,
                                game == Games.Custom));
                    }

                    RunSimulations(gram, null, startInput, game, $"{name}_heirarchical");
                    RunSimulations(gram, simpleGram, startInput, game, $"{name}_simple_heirarchical");

                    //Debug.LogWarning("backoff n-gram not yet implemented.");
                    //IGram gram = NGramFactory.InitGrammar(i);
                    //foreach (List<string> level in levels)
                    //{
                    //    NGramTrainer.Train(gram, level);
                    //}

                    //RunSimulations(gram, startInput, game, $"{name}_backoff");
                }
            }
        }

        private static List<List<string>> GetLevels(string gameFlowAssetName)
        {
            List<List<string>> levels = new List<List<string>>();

            TextAsset ta = Resources.Load<TextAsset>(gameFlowAssetName);
            Assert.IsNotNull(ta);

            JsonArray flow = LightJson.Serialization.JsonReader.Parse(ta.text);
            foreach (JsonObject jo in flow)
            {
                foreach (string path in jo["resources"].AsJsonArray)
                {
                    levels.Add(LevelLoader.LoadIntoColumns(path));
                }
            }

            return levels;
        }

        private static void RunSimulations(
            IGram gram,
            IGram simplifiedGram,
            List<string> startInput,
            Games game,
            string extension)
        {
            SimulationThread simulation = new SimulationThread(
                numSimulations,
                size,
                game,
                basePath,
                extension,
                gram,
                simplifiedGram,
                startInput);

            Thread thread = new Thread(new ThreadStart(simulation.Execute));
            thread.Start();
        }

            //private static void RunSimulations(
            //    IGram gram,
            //    IGram simplifiedGram,
            //    List<string> startInput, 
            //    Games game,
            //    string extension)
            //{
            //    string path = Path.Combine(basePath, $"{extension}_{gram.GetN()}.txt");
            //    StreamWriter writer = File.CreateText(path);
            //    writer.WriteLine("Sequence_Probability,Perplexity,Linearity,Leniency");

            //    ICompiledGram compiled = gram.Compile();
            //    for (int i = 0; i < numSimulations; ++i)
            //    {
            //        List<string> columns;
            //        List<string> simplified;

            //        if (simplifiedGram == null)
            //        {
            //            columns = NGramGenerator.Generate(compiled, startInput, size);
            //            simplified = LevelParser.BreakColumnsIntoSimplifiedTokens(
            //                columns, 
            //                game == Games.Custom);
            //        }
            //        else
            //        {
            //            ICompiledGram simpleCompiled = simplifiedGram.Compile();
            //            simplified = NGramGenerator.Generate(
            //                simpleCompiled,
            //                LevelParser.BreakColumnsIntoSimplifiedTokens(startInput, game == Games.Custom),
            //                size);

            //            columns = NGramGenerator.GenerateRestricted(
            //                compiled,
            //                startInput,
            //                simplified,
            //                (inColumn) =>
            //                {
            //                    return LevelParser.ClassifyColumn(inColumn, game);
            //                });
            //        }

            //        string[] array = columns.ToArray();
            //        writer.Write($"{compiled.SequenceProbability(array)},");
            //        writer.Write($"{compiled.Perplexity(array)},");
            //        writer.Write($"{Linearity(array)},");
            //        writer.Write($"{Leniency(simplified.ToArray())}\n");
            //        writer.Flush();
            //    }

            //    writer.Close();
            //}
        }
}
#endif