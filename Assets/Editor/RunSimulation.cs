//using System.Collections.Generic;
//using UnityEngine.Assertions;
//using UnityEngine;
//using UnityEditor;

//using System.Threading;
//using System.IO;
//using System;

//using Tools.AI.NGram;
//using LightJson;
//using PCG;

//namespace CustomUnityWindow
//{
//    public class SimulationThread
//    {
//        private int numSimulations;
//        private int size;
//        private Games game;
//        private string basePath;
//        private string extension;
//        private IGram gram;
//        private IGram simplifiedGram;
//        private List<string> startInput;

//        public SimulationThread(
//            int numSimulations,
//            int size,
//            Games game, 
//            string basePath,
//            string extension,
//            IGram gram,
//            IGram simplifiedGram,
//            List<string> startInput)
//        {
//            this.numSimulations = numSimulations;
//            this.size = size;
//            this.game = game;
//            this.basePath = basePath;
//            this.extension = extension;
//            this.gram = gram;
//            this.simplifiedGram = simplifiedGram;
//            this.startInput = startInput;
//        }

//        public void Execute()
//        {
//            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
//            watch.Start();

//            string path = Path.Combine(basePath, $"{extension}_{gram.GetN()}.txt");
//            StreamWriter writer = File.CreateText(path);
//            writer.WriteLine("Sequence_Probability,Perplexity,Linearity_JSON_Positions,Leniency");

//            ICompiledGram compiled = gram.Compile();
//            for (int i = 0; i < numSimulations; ++i)
//            {
//                List<string> columns;
//                List<string> simplified;

//                if (simplifiedGram == null)
//                {
//                    columns = NGramGenerator.Generate(compiled, startInput, size, includeStart:false);
//                    simplified = LevelParser.BreakColumnsIntoSimplifiedTokens(
//                        columns,
//                        game == Games.Custom);
//                }
//                else
//                {
//                    ICompiledGram simpleCompiled = simplifiedGram.Compile();
//                    simplified = NGramGenerator.Generate(
//                        simpleCompiled,
//                        LevelParser.BreakColumnsIntoSimplifiedTokens(startInput, game == Games.Custom),
//                        size);

//                    Games localGame = game;
//                    columns = NGramGenerator.GenerateRestricted(
//                        compiled,
//                        startInput,
//                        simplified,
//                        (inColumn) =>
//                        {
//                            return LevelParser.ClassifyColumn(inColumn, localGame);
//                        },
//                        includeStart: false);
//                }

//                string[] array = columns.ToArray();
//                List<double> positions = LevelAnalyzer.Positions(array);
//                JsonArray jsonPositions = new JsonArray();
//                foreach (double pos in positions)
//                {
//                    jsonPositions.Add(pos);
//                }

//                writer.Write($"{compiled.SequenceProbability(array)},");
//                writer.Write($"{compiled.Perplexity(array)},");
//                writer.Write($"{jsonPositions},");
//                writer.Write($"{LevelAnalyzer.Leniency(simplified.ToArray())}\n");
//                writer.Flush();
//            }

//            writer.Close();
//            watch.Stop();

//            Debug.Log($"Execution Time for {extension}_{gram.GetN()}: {watch.ElapsedMilliseconds / 1000d} seconds");
//        }
//    }

//    public class ThreadManager
//    {
//        private Stack<Thread> threads;
//        public ThreadManager(Stack<Thread> threads)
//        {
//            this.threads = threads;
//        }

//        public void Execute()
//        {
//            int numProcessors = Environment.ProcessorCount;
//            List<Thread> runningThreads = new List<Thread>();

//            int totalThreads = threads.Count;
//            int threadsRun = 0;

//            while (threads.Count > 0)
//            {
//                if (runningThreads.Count - 1 != numProcessors)
//                {
//                    ++threadsRun;
//                    //Debug.Log($"Starting thread {threadsRun} of {totalThreads}");
//                    Thread t = threads.Pop();
//                    t.Start();
//                    runningThreads.Add(t);
//                }
//                else
//                {
//                    for (int i = 0; i < runningThreads.Count; ++i)
//                    {
//                        if (runningThreads[i].IsAlive == false)
//                        {
//                            runningThreads.RemoveAt(i);
//                            break;
//                        }
//                    }
//                }
//            }
//        }
//    }

//    public static class RunSimulation
//    {
//        private static readonly string basePath = "data";

//        private const float hiearchicalWeight = 0.9f;
//        private const int numSimulations = 10;
//        //private const int numSimulations = 10000;
//        private const int size = 50;

//        [MenuItem("Sim/Run")]
//        public static void RunDataGeneration()
//        {
//            if (Directory.Exists(basePath) == false)
//            {
//                Directory.CreateDirectory(basePath);
//            }

//            Debug.Log("building threads");
//            List<Thread> threads = new List<Thread>();
//            threads.AddRange(Run("GameFlow/PCGPlatformer", "custom", Games.Custom));
//            //threads.AddRange(Run("GameFlow/SuperMarioBros", "smb", Games.SuperMarioBros));
//            //threads.AddRange(Run("GameFlow/SuperMarioBros2", "smb2", Games.SuperMarioBros2));
//            //threads.AddRange(Run("GameFlow/SuperMarioBros2Japan", "smb2j", Games.SuperMarioBros2Japan));
//            //threads.AddRange(Run("GameFlow/SuperMarioLand", "sml", Games.SuperMarioLand));
            
//            ThreadManager tm = new ThreadManager(new Stack<Thread>(threads));
//            Thread thread = new Thread(new ThreadStart(tm.Execute));
//            thread.Start();
//        }

//        // 1,2,3,4,5,6 gram
//        // 2,3,4,5,6 backoff
//        // 2,3,4,5,6 hiearchical
//        // 2,3,4,5,6 simple backoff
//        // 2,3,4,5,6 simple hierarchical
//        private static List<Thread> Run(string levelFlowPath, string name, Games game)
//        {
//            // We first get all the levels from the flow and then get the start
//            // input from the first level in the sequence. Note that the first
//            // we use the same start input but we don't want that to scew 
//            // results. To get around this, we use a flag in the ngram generator
//            // which will not include the start output in the end result. 
//            List<List<string>> levels = GetLevels(levelFlowPath);
//            List<string> startInput = levels[0].GetRange(0, 10);
//            List<Thread> threads = new List<Thread>();

//            for (int i = 1; i <= 6; ++i)
//            {
//                IGram gram = NGramFactory.InitGrammar(i);
//                foreach (List<string> level in levels)
//                {
//                    NGramTrainer.Train(gram, level);
//                }

//                threads.Add(BuildThread(gram, null, startInput, game, $"{name}_{size}_ngram"));

//                if (i != 1)
//                { 
//                    gram = NGramFactory.InitHierarchicalNGram(i, hiearchicalWeight);
//                    IGram simpleGram = NGramFactory.InitHierarchicalNGram(i, hiearchicalWeight);

//                    IGram bgram = NGramFactory.InitBackOffNGram(i, hiearchicalWeight);
//                    foreach (List<string> level in levels)
//                    {
//                        NGramTrainer.Train(gram, level);
//                        NGramTrainer.Train(bgram, level);
//                        NGramTrainer.Train(
//                            simpleGram,
//                            LevelParser.BreakColumnsIntoSimplifiedTokens(
//                                level,
//                                game == Games.Custom));
//                    }

//                    threads.Add(BuildThread(gram, null, startInput, game, $"{name}_{size}_heirarchical"));
//                    threads.Add(BuildThread(gram, simpleGram, startInput, game, $"{name}_{size}_simple_heirarchical"));

//                    threads.Add(BuildThread(bgram, null, startInput, game, $"{name}_{size}_backoff"));
//                    threads.Add(BuildThread(bgram, simpleGram, startInput, game, $"{name}_{size}_simple_backoff"));
//                }
//            }

//            return threads;
//        }

//        private static List<List<string>> GetLevels(string gameFlowAssetName)
//        {
//            List<List<string>> levels = new List<List<string>>();

//            TextAsset ta = Resources.Load<TextAsset>(gameFlowAssetName);
//            Assert.IsNotNull(ta);

//            JsonArray flow = LightJson.Serialization.JsonReader.Parse(ta.text);
//            foreach (JsonObject jo in flow)
//            {
//                foreach (string path in jo["resources"].AsJsonArray)
//                {
//                    levels.Add(LevelLoader.LoadIntoColumns(path));
//                }
//            }

//            return levels;
//        }

//        private static Thread BuildThread(
//            IGram gram,
//            IGram simplifiedGram,
//            List<string> startInput,
//            Games game,
//            string extension)
//        {
//            SimulationThread simulation = new SimulationThread(
//                numSimulations,
//                size,
//                game,
//                basePath,
//                extension,
//                gram,
//                simplifiedGram,
//                startInput);

//            return new Thread(new ThreadStart(simulation.Execute));
//        }
//    }
//}