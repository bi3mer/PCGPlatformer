using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using TMPro;

using System.Threading;
using System.IO;
using System;

using Tools.DataStructures;
using Tools.AI.NGram;
using LightJson;
using PCG;

namespace Simulator
{
    public class BuildSimulator : MonoBehaviour
    {
        private static readonly string basePath = "data";

        [SerializeField]
        private TextMeshProUGUI TextField = null;

        [SerializeField]
        private TextMeshProUGUI Ratio = null;

        private const float hiearchicalWeight = 0.9f;

        [SerializeField]
        private int numSimulations = 10000;

        [SerializeField]
        private int size = 50;

        private int totalThreads = 0;
        private int threadsRun = 0;
        private Stack<Thread> threads;
        private List<Tuple<Thread, System.Diagnostics.Stopwatch>> runningThreads;
        private CircularQueue<string> relevantText = new CircularQueue<string>(12);

        private int threadCount;

        public void Start()
        {
            runningThreads = new List<Tuple<Thread, System.Diagnostics.Stopwatch>>();

            if (Directory.Exists(basePath) == false)
            {
                Directory.CreateDirectory(basePath);
            }

            List<Thread> threads = new List<Thread>();
            threads.AddRange(Run("GameFlow/PCGPlatformer", "custom", Games.Custom));
            threads.AddRange(Run("GameFlow/SuperMarioBros", "smb", Games.SuperMarioBros));
            threads.AddRange(Run("GameFlow/SuperMarioBros2", "smb2", Games.SuperMarioBros2));
            threads.AddRange(Run("GameFlow/SuperMarioBros2Japan", "smb2j", Games.SuperMarioBros2Japan));
            threads.AddRange(Run("GameFlow/SuperMarioLand", "sml", Games.SuperMarioLand));

            totalThreads = threads.Count;
            this.threads = new Stack<Thread>(threads);

            threadCount = Environment.ProcessorCount - 1;

            relevantText.Add($"{numSimulations} simulations per a thread.");
            relevantText.Add("Threads built. Starting Process.");
        }

        public void Update()
        {
            if (threads.Count > 0)
            {
                if (runningThreads.Count - 1 != threadCount)
                {
                    ++threadsRun;

                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();

                    Thread t = threads.Pop();
                    t.Start();
                    runningThreads.Add(new Tuple<Thread, System.Diagnostics.Stopwatch>(t, watch));

                    relevantText.Add($"Starting thread {threadsRun}.");
                    TextField.text = string.Join("\n", relevantText.ToArray());
                    Ratio.text = $"{threadsRun} / {totalThreads}";
                }
                else
                {
                    for (int i = 0; i < runningThreads.Count; ++i)
                    {
                        if (runningThreads[i].Item1.IsAlive == false)
                        {
                            System.Diagnostics.Stopwatch watch = runningThreads[i].Item2;
                            watch.Stop();
                            relevantText.Add($"Thread finished after {watch.ElapsedMilliseconds / 60000d} minutes.");
                            runningThreads.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < runningThreads.Count; ++i)
                {
                    if (runningThreads[i].Item1.IsAlive == false)
                    {
                        System.Diagnostics.Stopwatch watch = runningThreads[i].Item2;
                        watch.Stop();
                        relevantText.Add($"Thread finished after {watch.ElapsedMilliseconds / 60000d} minutes.");
                        runningThreads.RemoveAt(i);
                        break;
                    }
                }

                if (runningThreads.Count == 0)
                {
                    relevantText.Add("Done");
                    TextField.text = string.Join("\n", relevantText.ToArray());
                    enabled = false;
                }
            }
        }

        public void OnDestroy()
        {
            if (runningThreads != null)
            { 
                foreach (Tuple<Thread, System.Diagnostics.Stopwatch> tuple in runningThreads)
                {
                    tuple.Item1.Abort();
                }
            }
        }

        // 1,2,3,4,5,6 gram
        // 2,3,4,5,6 backoff
        // 2,3,4,5,6 hiearchical
        // 2,3,4,5,6 simple backoff
        // 2,3,4,5,6 simple hierarchical
        private List<Thread> Run(string levelFlowPath, string name, Games game)
        {
            // We first get all the levels from the flow and then get the start
            // input from the first level in the sequence. Note that the first
            // we use the same start input but we don't want that to scew 
            // results. To get around this, we use a flag in the ngram generator
            // which will not include the start output in the end result. 
            List<List<string>> levels = GetLevels(levelFlowPath);
            List<string> startInput = levels[0].GetRange(0, 10);
            List<Thread> threads = new List<Thread>();

            for (int i = 1; i <= 6; ++i)
            {
                IGram gram = NGramFactory.InitGrammar(i);
                foreach (List<string> level in levels)
                {
                    NGramTrainer.Train(gram, level);
                }

                threads.Add(BuildThread(gram, null, startInput, game, $"{name}_{size}_ngram"));

                if (i != 1)
                {
                    gram = NGramFactory.InitHierarchicalNGram(i, hiearchicalWeight);
                    IGram simpleGram = NGramFactory.InitHierarchicalNGram(i, hiearchicalWeight);

                    IGram bgram = NGramFactory.InitBackOffNGram(i, hiearchicalWeight);
                    foreach (List<string> level in levels)
                    {
                        NGramTrainer.Train(gram, level);
                        NGramTrainer.Train(bgram, level);
                        NGramTrainer.Train(
                            simpleGram,
                            LevelParser.BreakColumnsIntoSimplifiedTokens(
                                level,
                                game));
                    }

                    //threads.Add(BuildThread(gram, null, startInput, game, $"{name}_{size}_heirarchical"));
                    //threads.Add(BuildThread(gram, simpleGram, startInput, game, $"{name}_{size}_simple_heirarchical"));

                    //threads.Add(BuildThread(bgram, null, startInput, game, $"{name}_{size}_backoff"));
                    //threads.Add(BuildThread(bgram, simpleGram, startInput, game, $"{name}_{size}_simple_backoff"));
                }
            }

            return threads;
        }

        public static List<List<string>> GetLevels(string gameFlowAssetName)
        {
            List<List<string>> levels = new List<List<string>>();

            TextAsset ta = Resources.Load<TextAsset>(gameFlowAssetName);
            Assert.IsNotNull(ta);

            JsonArray flow = LightJson.Serialization.JsonReader.Parse(ta.text);
            foreach (JsonObject jo in flow)
            {
                foreach (string path in jo["resources"].AsJsonArray)
                {
                    levels.Add(LevelLoader.LoadIntoColumns(path, removeLastColumn: true));
                }
            }

            return levels;
        }

        private Thread BuildThread(
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

            return new Thread(new ThreadStart(simulation.Execute));
        }
    }
}