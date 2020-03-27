using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgoUnitTests
{
    [TestClass]
    public class GenerationTests
    {
        [TestMethod]
        public void TestConstructor1()
        {
            //seed the Helpers.rand object with 0 so that the values will always be the same sequence
            Generation gen = new Generation(5, 10);

            Random newRand = new Random(0);
            Chromosome[] comparisonChromosomes = new Chromosome[5];

            for (int i = 0; i < comparisonChromosomes.Length; i++)
            {
                Allele[] comparisonAlleles = new Allele[10];
                for (int j = 0; j < comparisonAlleles.Length; j++)
                {
                    comparisonAlleles[j] = (Allele)newRand.Next(Enum.GetNames(typeof(Allele)).Length);
                }
                comparisonChromosomes[i] = new Chromosome(comparisonAlleles);
            }

            CollectionAssert.AreEqual(gen[0].AlleleArray, comparisonChromosomes[0].AlleleArray);
            CollectionAssert.AreEqual(gen[1].AlleleArray, comparisonChromosomes[1].AlleleArray);
            CollectionAssert.AreEqual(gen[2].AlleleArray, comparisonChromosomes[2].AlleleArray);
            CollectionAssert.AreEqual(gen[3].AlleleArray, comparisonChromosomes[3].AlleleArray);
            CollectionAssert.AreEqual(gen[4].AlleleArray, comparisonChromosomes[4].AlleleArray);
        }
        [TestMethod]
        public void TestConstructorDeepCopy()
        {
            Chromosome[] comparisonChromosomes = new Chromosome[5];
            for(int i = 0; i < comparisonChromosomes.Length;i++)
            {
                comparisonChromosomes[i] = new Chromosome(10);
            }
            Generation gen = new Generation(comparisonChromosomes);

            CollectionAssert.AreEqual(gen[0].AlleleArray, comparisonChromosomes[0].AlleleArray);
            CollectionAssert.AreEqual(gen[1].AlleleArray, comparisonChromosomes[1].AlleleArray);
            CollectionAssert.AreEqual(gen[2].AlleleArray, comparisonChromosomes[2].AlleleArray);
            CollectionAssert.AreEqual(gen[3].AlleleArray, comparisonChromosomes[3].AlleleArray);
            CollectionAssert.AreEqual(gen[4].AlleleArray, comparisonChromosomes[4].AlleleArray);
        }

        [TestMethod]
        public void TestIndexer()
        {
            Generation gen = new Generation(5, 10);
            Chromosome[] chromArr = new Chromosome[5];

            for(int i = 0; i < chromArr.Length;i++)
            {
                chromArr[i] = gen[i];
            }

            CollectionAssert.AreEqual(gen[0].AlleleArray, chromArr[0].AlleleArray);
            CollectionAssert.AreEqual(gen[1].AlleleArray, chromArr[1].AlleleArray);
            CollectionAssert.AreEqual(gen[2].AlleleArray, chromArr[2].AlleleArray);
            CollectionAssert.AreEqual(gen[3].AlleleArray, chromArr[3].AlleleArray);
            CollectionAssert.AreEqual(gen[4].AlleleArray, chromArr[4].AlleleArray); 
        }
    }
}
