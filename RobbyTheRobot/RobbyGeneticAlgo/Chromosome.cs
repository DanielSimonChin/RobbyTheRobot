using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgo
{
    public delegate Chromosome[] Crossover(Chromosome a, Chromosome b);
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
        /// <param name="length"></param>
        public Chromosome(int length)
        {
            if(length <= 0)
            {
                throw new ArgumentException("The input length must be greater than or equal to one.");
            }

            this.alleleArray = new Allele[length];
            for(int i = 0; i<this.alleleArray.Length;i++)
            {
                //COMMENT THIS WHEN TESTING CONSTRUCTORS
                //this.alleleArray[i] = (Allele)Helpers.rand.Next(Enum.GetNames(typeof(Allele)).Length);

                //For testing purposes
                //UNCOMMENT THIS WHEN TESTING CONSTRUCTORS
                this.alleleArray[i] = (Allele)Helpers.rand.Next(0);
            }
        }
        
        /// <summary>
        /// Constructor performs a deep copy of an input gene array of alleles.
        /// </summary>
        /// <param name="gene"></param>
        public Chromosome(Allele[] gene)
        {
            this.alleleArray = new Allele[gene.Length];
            for(int i = 0; i < alleleArray.Length;i++)
            {
                this.alleleArray[i] = gene[i];
            }
        }

        /*public Chromosome[] Reproduce(Chromosome spouse, Crossover f, double mutationRate)
        {

        }
        
        public void EvalFitness(Fitness f)
        {

        }*/

        /// <summary>
        /// Indexer which returns a specific Allele at alleleArray[index]
        /// </summary>
        /// <param name="index"></param>
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
            private set { this.fitness = value;}
        }


        /// <summary>
        /// Returns an int which helps us compare two Chromosome based on their Fitness.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Returns a number greater than 0 if the instance Chromosome's fitness is bigger than the input Chromosome's fitness</returns>
        public int CompareTo(Chromosome other)
        {
            throw new NotImplementedException();
        }

        

        public Allele[] AlleleArray
        {
            get { return this.alleleArray;}
        }

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
        public Chromosome[] SingleCrossover(Chromosome a, Chromosome b)
        {
            Chromosome[] newChildren = new Chromosome[2];
            //finds a random point from 0 to length of enum type Allele
            int singleCrossoverPoint = Helpers.rand.Next(Enum.GetNames(typeof(Allele)).Length);

            Allele[] parent1 = a.AlleleArray;
            Allele[] parent2 = b.AlleleArray;

            Allele[] child1 = new Allele[parent1.Length];
            Allele[] child2 = new Allele[parent1.Length];

            for(int i = 0; i < singleCrossoverPoint;i++)
            {
                child1[i] = parent1[i];
                child2[i] = parent2[i];
            }
            for(int i = singleCrossoverPoint; i < parent1.Length;i++)
            {
                child1[i] = parent2[i];
                child2[i] = parent1[i];
            }

            newChildren[0] = new Chromosome(child1);
            newChildren[1] = new Chromosome(child2);
            return newChildren;

        }

        /*public Chromosome[] DoubleCrossover(Chromosome a, Chromosome b)
        {

        }*/
    }
}
