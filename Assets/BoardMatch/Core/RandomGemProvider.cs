using BoardMatch.Utilities;
using UnityEngine;

namespace BoardMatch.Core
{
    public class RandomGemProvider : MonoBehaviour
    {
     
    }

    public interface IRandomGemProvider
    {
        /// <summary>  Returns a random value from the array </summary>
        public int GetRandomGem(int[] availableGemTypes);
    }

    public sealed class UnityRandomGemProvider : IRandomGemProvider
    {
        //Allows for deterministic generation, e.g, for testing purposes
        public UnityRandomGemProvider(int? seed = null)
        {
            if(seed.HasValue) Random.InitState(seed.Value);
        }
        
        public int GetRandomGem(int[] availableGemTypes)
        {
            int index = Random.Range(0, availableGemTypes.Length);
            MatchLog.Log($"Getting {index} which is {availableGemTypes[index]} from {availableGemTypes.Length}");
            return availableGemTypes[index];
        }
    }
}

