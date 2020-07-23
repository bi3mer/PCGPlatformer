using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Tools.AI.NGram;
using PCG;
using System.Collections;

namespace Simulator
{ 
    public class ProfileTest : MonoBehaviour
    {
        public string basePath = "fake_data";
        public int NumSimulations = 2;
        public int Size = 50;
        private float time = 0f;

        private Stack<SimulationThread> threads;

        private void Start()
        {
            if (Directory.Exists(basePath) == false)
            {
                Directory.CreateDirectory(basePath);
            }

            threads = new Stack<SimulationThread>(GetSimulationThreads(
                "GameFlow/PCGPlatformer", 
                "custom", 
                Games.Custom));
        }

        private void Update()
        {
            if (threads.Count > 0)
            { 
                if (time == 0)
                { 
                    threads.Pop().Execute();
                }
            }

            if (time > 0.1)
            {
                time = 0;
                Debug.Break();
            }
            time += Time.deltaTime;
        }

        private List<SimulationThread> GetSimulationThreads(string levelFlowPath, string name, Games game)
        {
            // We first get all the levels from the flow and then get the start
            // input from the first level in the sequence. Note that the first
            // we use the same start input but we don't want that to scew 
            // results. To get around this, we use a flag in the ngram generator
            // which will not include the start output in the end result. 
            List<List<string>> levels = BuildSimulator.GetLevels(levelFlowPath);
            List<string> startInput = levels[0].GetRange(0, 10);
            List<SimulationThread> threads = new List<SimulationThread>();

            for (int i = 6; i <= 6; ++i)
            {
                IGram gram = NGramFactory.InitGrammar(i);
                foreach (List<string> level in levels)
                {
                    NGramTrainer.Train(gram, level);
                }

                //threads.Add(BuildThread(gram, null, startInput, game, $"{name}_{Size}_ngram"));

                if (i != 1)
                {
                    gram = NGramFactory.InitHierarchicalNGram(i, 0.6f);
                    IGram simpleGram = NGramFactory.InitHierarchicalNGram(i, 0.6f);

                    IGram bgram = NGramFactory.InitBackOffNGram(i, 0.6f);
                    foreach (List<string> level in levels)
                    {
                        NGramTrainer.Train(gram, level);
                        NGramTrainer.Train(bgram, level);
                        NGramTrainer.Train(
                            simpleGram,
                            LevelParser.BreakColumnsIntoSimplifiedTokens(
                                level,
                                game == Games.Custom));
                    }

                    //threads.Add(BuildThread(gram, null, startInput, game, $"{name}_{Size}_heirarchical"));
                    //threads.Add(BuildThread(gram, simpleGram, startInput, game, $"{name}_{Size}_simple_heirarchical"));

                    //threads.Add(BuildThread(bgram, null, startInput, game, $"{name}_{Size}_backoff"));
                    threads.Add(BuildThread(bgram, simpleGram, startInput, game, $"{name}_{Size}_simple_backoff"));
                }
            }

            return threads;
        }

        private SimulationThread BuildThread(
            IGram gram,
            IGram simplifiedGram,
            List<string> startInput,
            Games game,
            string extension)
        {
            return new SimulationThread(
                NumSimulations,
                Size,
                game,
                basePath,
                extension,
                gram,
                simplifiedGram,
                startInput);
        }
    }
}
