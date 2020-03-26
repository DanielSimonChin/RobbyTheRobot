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
            //Seed the random field in Helpers.cs with value 0.
            Chromosome constructor1 = new Chromosome(5);
            Allele[] originalArray = constructor1.AlleleArray;

            //create a seperate random instance
            Random newRandom = new Random(0);
            Allele[] comparingAllele = new Allele[5];
            for (int i = 0; i < comparingAllele.Length; i++)
            {
                comparingAllele[i] = (Allele)newRandom.Next(Enum.GetNames(typeof(Allele)).Length);
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

        [TestMethod]
        public void TestSingleCrossover()
        {
            Chromosome parent1 = new Chromosome(20);
            Chromosome parent2 = new Chromosome(20);
            Allele[] parent1genes = parent1.AlleleArray;
            Allele[] parent2genes = parent2.AlleleArray;

            Chromosome[] children = Chromosome.SingleCrossover(parent1, parent2);
            Allele[] child1 = children[0].AlleleArray;
            Allele[] child2 = children[1].AlleleArray;

            //make sure that the length of the child allele arrays are same length as parents
            Assert.AreEqual(20, child1.Length);
            Assert.AreEqual(20, child2.Length);

            //These next four asserts will work most of the time, the only exception would be if the random crossover point happens to be index 0 or length-1 (2/20 probability)
            //These asserts check if the crossover worked, assuming the crossover point is not 0 or length-1
            Assert.AreEqual(child1[0],parent1genes[0]);
            Assert.AreEqual(child1[19], parent2genes[19]);

            Assert.AreEqual(child2[0], parent2genes[0]);
            Assert.AreEqual(child2[19], parent1genes[19]);
        }

        [TestMethod]
        public void TestDoubleCrossover()
        {
            Chromosome parent1 = new Chromosome(20);
            Chromosome parent2 = new Chromosome(20);
            Allele[] parent1genes = parent1.AlleleArray;
            Allele[] parent2genes = parent2.AlleleArray;

            Chromosome[] children = Chromosome.DoubleCrossover(parent1, parent2);
            Allele[] child1 = children[0].AlleleArray;
            Allele[] child2 = children[1].AlleleArray;

            //make sure that the length of the child allele arrays are same length as parents
            Assert.AreEqual(20, child1.Length);
            Assert.AreEqual(20, child2.Length);

            //These next four asserts will work most of the time, the only exception would be if the random crossover point happens to be index 0 or length-1 or if both crossover points are same number (3/20 probability)
            //These asserts check if the crossover worked, assuming the crossover point is not 0 or length-1
            Assert.AreEqual(child1[0], parent1genes[0]);
            Assert.AreEqual(child1[19], parent1genes[19]);

            Assert.AreEqual(child2[0], parent2genes[0]);
            Assert.AreEqual(child2[19], parent2genes[19]);

        }

        [TestMethod]
        public void TestReproduceNoMutations()
        {
            //seed the random object in helpers.cs with 0
            Chromosome parent1 = new Chromosome(10);
            Chromosome parent2 = new Chromosome(10);

            //Will compare these children to those created by reproduce method. Should be equal since there are no mutations
            Chromosome[] children = Chromosome.SingleCrossover(parent1, parent2);

            Crossover crossoverFunction = new Crossover(Chromosome.SingleCrossover);
            //since the mutation rate is greater than 1, the offspring will never mutate
            Chromosome[] resultChildren = parent1.Reproduce(parent2, crossoverFunction, 2.0);

            CollectionAssert.AreEqual(children[0].AlleleArray, resultChildren[0].AlleleArray);
            CollectionAssert.AreEqual(children[1].AlleleArray, resultChildren[1].AlleleArray);
        }

        [TestMethod]
        public void TestReproduceWithMutations()
        {
            Chromosome parent1 = new Chromosome(10);
            Chromosome parent2 = new Chromosome(10);

            //Will compare these children to those created by reproduce method. Should be equal since there are no mutations
            Chromosome[] children = Chromosome.SingleCrossover(parent1, parent2);

            Crossover crossoverFunction = new Crossover(Chromosome.SingleCrossover);
            //Since the mutation rate is so low, the odds of the allele mutating is very high since 0.05 will almost always be less than the generated number
            Chromosome[] resultChildren = parent1.Reproduce(parent2, crossoverFunction, 0.05);

            //Checking that the children have mutations in their arrays since the mutation rate was low
            CollectionAssert.AreNotEqual(children[0].AlleleArray, resultChildren[0].AlleleArray);
            CollectionAssert.AreNotEqual(children[1].AlleleArray, resultChildren[1].AlleleArray);
        }

        [TestMethod]
        public void TestAlleleIndexer()
        {
            //seed the random object so the program creates two identical chromosomes
            Chromosome c1= new Chromosome(10);
            Allele[] comparisonGenes = new Allele[c1.arrayLength];
            for(int i = 0; i < comparisonGenes.Length;i++)
            {
                //call indexer
                comparisonGenes[i] = c1[i];
            }
            CollectionAssert.AreEqual(c1.AlleleArray, comparisonGenes);
        }
    
    }
}
