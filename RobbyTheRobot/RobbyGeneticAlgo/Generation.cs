using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    /// <summary>
    /// The Generation class represents a generation with a population size of Chromosome members. It contains an array of Chromosomes.
    /// </summary>
    public class Generation
    {
        private Chromosome[] chromosomeArray;

        /// <summary>
        /// Initializes a population of random Chromosomes
        /// </summary>
        /// <param name="populationSize">Amount of chromosomes in a single generation</param>
        /// <param name="numGenes">Number of genes that will make up a single chromosome</param>
        public Generation(int populationSize, int numGenes)
        {
            this.chromosomeArray = new Chromosome[populationSize];
            for(int i = 0; i < this.chromosomeArray.Length;i++)
            {
                this.chromosomeArray[i] = new Chromosome(numGenes);
            }
        }

        /// <summary>
        /// makes a deep copy of an array of Chromosomes
        /// </summary>
        /// <param name="members">An array of chromosomes</param>
        public Generation(Chromosome[] members)
        {
            this.chromosomeArray = new Chromosome[members.Length];
            for(int i = 0; i < this.chromosomeArray.Length;i++)
            {
                //uses the helper method in Chromosome.cs that returns the allele array
                this.chromosomeArray[i] = new Chromosome(members[i].AlleleArray);
            }
        }

        /// <summary>
        /// This method takes a Fitness delegate object, invokes it on all the Chromosome members, and then sorts the array of members 
        /// </summary>
        /// <param name="f">A Fitness delegate object</param>
        public void EvalFitness(Fitness f)
        {
            for(int i = 0; i < this.chromosomeArray.Length;i++)
            {
                this.chromosomeArray[i].EvalFitness(f);
            }
            //sorting the chromosomes by their fitness since we implemented IComparable.
            Array.Sort(this.chromosomeArray);
            //reversing the array so the best fitness is the fist index.
            Array.Reverse(this.chromosomeArray);
        }

        /// <summary>
        /// A readonly indexer is used to get a specific Chromosome
        /// </summary>
        /// <param name="index">The index at which will be returned a chromosome</param>
        /// <returns>Returns a specific chromosome</returns>
        public Chromosome this[int index]
        {
            get { return this.chromosomeArray[index]; }
        }

        /// <summary>
        /// The SelectParent method is used to select a parent for the next generation
        /// </summary>
        /// <returns>Returns the chromosome with the smallest random index out of 10 random indexes</returns>
        public Chromosome SelectParent()
        {
            int[] randomIndexes = new int[10];
            for(int i = 0; i < randomIndexes.Length;i++)
            {
                randomIndexes[i] = Helpers.rand.Next(this.chromosomeArray.Length - 1);
            }
            Array.Sort(randomIndexes);

            return this.chromosomeArray[randomIndexes[0]];
        }
    }
}
