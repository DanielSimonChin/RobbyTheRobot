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
            Chromosome constructor1 = new Chromosome(3);
            //giving the allele the first enum number 0 which represent North
            //The constructor is given a seed of 0 for testing purposes.
            Allele northEnum = (Allele)0;
            Assert.AreEqual(northEnum, constructor1[0]);
            Assert.AreEqual(northEnum, constructor1[1]);
            Assert.AreEqual(northEnum, constructor1[2]);
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
