using System;
using RobbyGeneticAlgo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobbyGeneticAlgoUnitTests
{
    [TestClass]
    public class ChromosomeTests
    {
        [TestMethod]
        public void TestConstructor1Length()
        {

            Chromosome constructor1 = new Chromosome(5);
            Assert.AreEqual(5, constructor1.arrayLength);
        }
        [TestMethod]
        public void TestConstructor1Contents()
        {
            
            //Test the contents of the array using an indexer. Seed the random field in Helpers.cs with value 0.
            Chromosome constructor1 = new Chromosome(5);
            Allele[] originalArray = constructor1.AlleleArray;

            Random newRandom = new Random(0);
            Allele[] comparingAllele = new Allele[5];
            for (int i = 0; i < comparingAllele.Length; i++)
            {
                comparingAllele[i] = (Allele)newRandom.Next();
            }
            


            CollectionAssert.AreEqual(originalArray, comparingAllele);

        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void TestConstructor1Exceptions()
        {
            //Throws exception in first constructor if input length is less than or equal to 0.
            Chromosome constructor1 = new Chromosome(-5);
        }

        [TestMethod]
        public void TestConstructor2DeepCopy()
        {
            Chromosome constructor2 = new Chromosome(5);
            //give the new Chromosome the genes of the first. Should now contain identical alleles 
            Chromosome newChromosome = new Chromosome(constructor2.AlleleArray);
            CollectionAssert.AreEqual(constructor2.AlleleArray, newChromosome.AlleleArray);
        }
    }
}
