﻿using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Infrastructure
{
    public class RandomService
    {
        public int Seed { get; private set; }

        private Random _random;
        
        public void Init(int randomSeed = 0)
        {
            if (randomSeed > 0)
                Seed = randomSeed;
            else
            {
                _random = new Random();
                Seed = _random.Next();
            }

            _random = new Random(Seed);
            
            var message = $"{this}: current seed is: {Seed}";
            Debug.Log(message);
        }

        /// <summary>
        /// Random digit from min (included) to max (excluded)
        /// </summary>
        public int GetInt(int maxIndex, int minIndex = 0) 
            => _random.Next(minIndex, maxIndex);

        public double GetToMax(float max) 
            => _random.NextDouble() * max;

        public T GetOne<T>(params T[] values)
            => values[GetInt(values.Length)];

        public T GetRandom<T>(T[] collection)
        {
            var index = GetInt(collection.Count());
            return collection[index];
        }
    }
}