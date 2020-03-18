using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgo
{
    /// <summary>
    /// This class represents a single Chromosome, or a solution to a GA problem
    /// </summary>
    public class Chromosome : IComparable<Chromosome>
    {
        private Allele[] alleleArray;

        /// <summary>
        /// Creates/instantiates an array of random alleles(genes)
        /// </summary>
        /// <param name="length"></param>
        public Chromosome(int length)
        {
            this.alleleArray = new Allele[length];
            for(int i = 0; i<this.alleleArray.Length;i++)
            {
                this.alleleArray[i] = (Allele)Helpers.rand.Next(Enum.GetNames(typeof(Allele)).Length);
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

        }*/

        /// <summary>
        /// Returns an int which helps us compare two Chromosome based on their Fitness.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Returns a number greater than 0 if the instance Chromosome's fitness is bigger than the input Chromosome's fitness</returns>
        public int CompareTo(Chromosome other)
        {
            throw new NotImplementedException();
        }

        public int arrayLength
        {
            get { return this.alleleArray.Length; }
        }
    }
}
