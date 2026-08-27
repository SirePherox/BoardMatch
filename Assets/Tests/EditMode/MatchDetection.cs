using System.Collections.Generic;
using BoardMatch.Core;
using NUnit.Framework;
using UnityEngine;

namespace BoardMatch.Tests.EditMode
{
    public class MatchDetection
    {
        [Test]
        public void TrySwap_CompletesAMatch_ClearAndRefill()
        {
            var config = ScriptableObject.CreateInstance<MatchConfig>();
            config.height = 3;
            config.width = 3;
            config.availableGemTypes = new[] { 0, 1, 2 };
            config.minMatchCount = 3;

            var gemProvider = new SequenceGemProvider(0, 1, 2);
            var board = new BoardModel(config, gemProvider);
            
            int[,] initialGrid =
            {
                { 1, 0, 2 }, 
                { 1, 2, 0 }, 
                { 2, 1, 0 } 
            };
            board.SetGridForTesting(initialGrid);
            
            //Sanity Check. Confirm there is no initial match
            Assert.IsEmpty(MatchFinder.FindAllMatches(board.GetGridForTesting(), config.width,config.height,config.minMatchCount), "Intial test board doesn't contain any matches");
            //
            
            gemProvider.Reset(); 
            
            //Board Events
            IReadOnlyList<Match> matches = null;
            int fallCount = 0;
            int spawnCount = 0;
            board.OnMatchesCleared += m => matches = m;
            board.OnGemsFell += fall => fallCount = fall.Count;
            board.OnGemsSpawned += spawn => spawnCount = spawn.Count;
            
            //Swap Test
            bool swapped = board.TrySwapCells(new Vector2Int(2, 0), new Vector2Int(2, 1));
            
            //Asserts
            Assert.IsTrue(swapped, "Swap should succeed. It completes a match");
            Assert.IsNotNull(matches, "Matches should have fired, and not be null");
            Assert.AreEqual(1, matches.Count, "Matches should have fired, and exactly 1 match should be found");
            Assert.AreEqual(6, fallCount, "Each of the 3 cleared cells would cause 6 gems to fall ");
            Assert.AreEqual(3, spawnCount, "The 3 cleared cells should have been refilled with 3 new unique gems");

            int[,] finalGrid = board.GetGridForTesting();
            var remainingMatches = MatchFinder.FindAllMatches(finalGrid, config.width,config.height,config.minMatchCount);
            Assert.IsEmpty(remainingMatches, "No matches should remain after the cascade");
        }
    }

    internal sealed class SequenceGemProvider : IRandomGemProvider
    {
        private readonly int[] _sequence;
        private int _index;

        public SequenceGemProvider(params int[] sequence)
        {
            _sequence = sequence;
        }

        public int GetRandomGem(int[] availableGemTypes)
        {
            int value = _sequence[_index % _sequence.Length];
            _index++;
            return value;
        }

        public void Reset() => _index = 0;
    }
}
