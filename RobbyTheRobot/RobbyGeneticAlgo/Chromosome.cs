using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgo
{
    public delegate Chromosome[] Crossover(Chromosome a, Chromosome b);
    public delegate double Fitness(Chromosome c);
    public delegate int AlleleMoveandFitness(Chromosome c, Contents[,] grid, ref int x, ref int y);
    public delegate void GenerationEventHandler(int num, Generation g);
    /// <summary>
    /// This class represents a single Chromosome, or a solution to a GA problem
    /// </summary>
    public class Chromosome : IComparable<Chromosome>
    {
        private Allele[] alleleArray;
        private double fitness;

        /// <summary>
        /// Creates/instantiates an array of random alleles(genes)
        /// </summary>
        /// <param name="length"> The input length of a chromosome</param>
        public Chromosome(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("The input length must be greater than or equal to one.");
            }

            this.alleleArray = new Allele[length];

            for (int i = 0; i < this.alleleArray.Length; i++)
            {
                this.alleleArray[i] = (Allele)Helpers.rand.Next(Enum.GetNames(typeof(Allele)).Length);
            }
        }

        /// <summary>
        /// Constructor performs a deep copy of an input gene array of alleles.
        /// </summary>
        /// <param name="gene"> Gene is an Allele array that will be deep copied</param>
        public Chromosome(Allele[] gene)
        {
            this.alleleArray = new Allele[gene.Length];
            for (int i = 0; i < alleleArray.Length; i++)
            {
                this.alleleArray[i] = gene[i];
            }
        }
        /// <summary>
        /// Creates two offspring using the Crossover delegate object and changes the Chromosome's alleles according to the mutation rate.
        /// </summary>
        /// <param name="spouse">The second parent that will be used to reproduce offspring</param>
        /// <param name="f">A crossover delegate object, either SingleCrossover or Double Crossover</param>
        /// <param name="mutationRate">The mutation rate at which the alleles can change</param>
        /// <returns>Two offspring Chromosomes</returns>
        public Chromosome[] Reproduce(Chromosome spouse, Crossover f, double mutationRate)
        {
            Chromosome[] resultChildren = f(this, spouse);
            Allele[] child1genes = resultChildren[0].AlleleArray;
            Allele[] child2genes = resultChildren[1].AlleleArray;

            double randomNumber;

            for (int i = 0; i < child1genes.Length; i++)
            {
                randomNumber = Helpers.rand.NextDouble();
                if (mutationRate > randomNumber)
                {
                    child1genes[i] = (Allele)Helpers.rand.Next(Enum.GetNames(typeof(Allele)).Length);
                }
            }
            for (int i = 0; i < child2genes.Length; i++)
            {
                randomNumber = Helpers.rand.NextDouble();
                if (mutationRate > randomNumber)
                {
                    child2genes[i] = (Allele)Helpers.rand.Next(Enum.GetNames(typeof(Allele)).Length);
                }
            }
            resultChildren[0] = new Chromosome(child1genes);
            resultChildren[1] = new Chromosome(child2genes);
            return resultChildren;

        }

        /// <summary>
        /// Takes a fitness delegate object, invokes it on this and sets the result as the Fitness property.
        /// </summary>
        /// <param name="f">A Fitness delegate object</param>
        public void EvalFitness(Fitness f)
        {
            this.Fitness = f(this);
        }

        /// <summary>
        /// Indexer which returns a specific Allele at alleleArray[index]
        /// </summary>
        /// <param name="index"> The index at which will be returned a specific allele</param>
        /// <returns>returns a specific Allele at alleleArray[index]</returns>
        public Allele this[int index]
        {
            get { return this.alleleArray[index]; }
        }

        /// <summary>
        /// Simple get/set for fitness property.
        /// </summary>
        public double Fitness
        {
            get { return this.fitness; }
            private set { this.fitness = value; }
        }


        /// <summary>
        /// Returns an int which helps us compare two Chromosome based on their Fitness.
        /// </summary>
        /// <param name="other">A second chromosome which will be compared to "this"</param>
        /// <returns>Returns a number greater than 0 if the instance Chromosome's fitness is bigger than the input Chromosome's fitness</returns>
        public int CompareTo(Chromosome other)
        {
            if (this.Fitness > other.Fitness)
            {
                return 1;
            }
            else if (this.Fitness < other.Fitness)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }


        /// <summary>
        /// Helper method that returns a deep copy of the Allele[] field.
        /// </summary>
        public Allele[] AlleleArray
        {
            get
            {
                Allele[] deepCopy = new Allele[this.alleleArray.Length];
                for (int i = 0; i < deepCopy.Length; i++)
                {
                    deepCopy[i] = this.alleleArray[i];
                }
                return deepCopy;
            }
        }

        /// <summary>
        /// Helper propert for unit testing which returns the length of the allele[]
        /// </summary>
        public int arrayLength
        {
            get { return this.alleleArray.Length; }
        }

        /// <summary>
        /// Finds a random crossover point, creates two new offspring which takes parts of each parent
        /// </summary>
        /// <param name="a"> First Chromosome parent</param>
        /// <param name="b"> Second Chromosome parent</param>
        /// <returns> An array of two result Chromosome offspring </returns>
        public static Chromosome[] SingleCrossover(Chromosome a, Chromosome b)
        {
            Allele[] parent1 = a.AlleleArray;
            Allele[] parent2 = b.AlleleArray;

            Allele[] child1 = new Allele[parent1.Length];
            Allele[] child2 = new Allele[parent1.Length];

            Chromosome[] newChildren = new Chromosome[2];
            //finds a random point from 0 to length of parent's allele array

            //COMMENT THIS WHEN UNIT TESTING
            int singleCrossoverPoint = Helpers.rand.Next(0, parent1.Length - 1);

            for (int i = 0; i < singleCrossoverPoint; i++)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }
            for (int i = singleCrossoverPoint; i < parent1.Length; i++)
            {
                child1[i] = parent2[i];
                child2[i] = parent1[i];
            }

            newChildren[0] = new Chromosome(child1);
            newChildren[1] = new Chromosome(child2);
            return newChildren;

        }

        public static Chromosome[] DoubleCrossover(Chromosome a, Chromosome b)
        {
            Allele[] parent1 = a.AlleleArray;
            Allele[] parent2 = b.AlleleArray;

            Allele[] child1 = new Allele[parent1.Length];
            Allele[] child2 = new Allele[parent1.Length];
            Chromosome[] newChildren = new Chromosome[2];

            //finds a random point in the first half and the second point in the other half
            int firstCrossoverPoint = Helpers.rand.Next(0, parent1.Length / 2);
            int secondCrossoverPoint = Helpers.rand.Next(parent1.Length / 2, parent1.Length - 1);

            for (int i = 0; i < firstCrossoverPoint; i++)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }
            for (int i = firstCrossoverPoint; i < secondCrossoverPoint; i++)
            {
                child1[i] = parent2[i];
                child2[i] = parent1[i];
            }
            for (int i = secondCrossoverPoint; i < parent1.Length; i++)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }
            newChildren[0] = new Chromosome(child1);
            newChildren[1] = new Chromosome(child2);
            return newChildren;
        }

        /// <summary>
        /// Override of the ToString method to return the contents of the Alleles array, joined with a comma
        /// </summary>
        /// <returns>A new string of the contents of the Allele array seperated by comma</returns>
        public override string ToString()
        {
            string contents = String.Join(",", this.alleleArray);
            return contents;
        }
    }
}
