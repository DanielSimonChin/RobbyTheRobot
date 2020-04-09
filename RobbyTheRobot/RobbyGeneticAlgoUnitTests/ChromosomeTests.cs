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
        //THIS TEST METHOD FAILS WHEN RUNNING ALL THE TESTS COLLECTIVELY, IF FAILS, RUN THIS TEST INDIVIDUALLY (because of random instances)
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
            //allele array generated using same random seed value should be equal
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
            //UNCOMMENT THE SECTION FOR UNIT TESTING THE REPRODUCE METHOD IN SINGLECROSSOVER(singlecrossover has a line that must be uncommented when not unit testing)
            Chromosome parent1 = new Chromosome(20);
            Chromosome parent2 = new Chromosome(20);

            //Will compare these children to those created by reproduce method. Should be equal since there are no mutations
            Chromosome[] children = Chromosome.SingleCrossover(parent1, parent2);

            Crossover crossoverFunction = new Crossover(Chromosome.SingleCrossover);
            //since the mutation rate is so low, the offspring will never mutate so the results will be the same as the SingleCrossover
            //The randomly generated number will never be lower than the mutation rate, so no mutation
            Chromosome[] resultChildren = parent1.Reproduce(parent2, crossoverFunction, -1.0);

            CollectionAssert.AreEqual(children[0].AlleleArray, resultChildren[0].AlleleArray);
            CollectionAssert.AreEqual(children[1].AlleleArray, resultChildren[1].AlleleArray);
        }

        [TestMethod]
        public void TestReproduceWithMutations()
        {
            //UNCOMMENT THE SECTION FOR UNIT TESTING THE REPRODUCE METHOD IN SINGLECROSSOVER(singlecrossover has a line that must be uncommented when not unit testing)
            Chromosome parent1 = new Chromosome(20);
            Chromosome parent2 = new Chromosome(20);

            //Will compare these children to those created by reproduce method. Should be equal since there are no mutations
            Chromosome[] children = Chromosome.SingleCrossover(parent1, parent2);

            Crossover crossoverFunction = new Crossover(Chromosome.SingleCrossover);
            //The mutation rate is very high so the odds of it mutating are very high as well
            Chromosome[] resultChildren = parent1.Reproduce(parent2, crossoverFunction, 0.95);

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
        [TestMethod]
        public void TestEvalFitness()
        {
            Fitness f = new Fitness(Chromosome.TestEvalFitness);
            Chromosome[] chromArr = new Chromosome[5];
            for(int i = 0; i < chromArr.Length;i++)
            {
                chromArr[i] = new Chromosome(243);
                
            }
            for(int i = 0; i <chromArr.Length;i++)
            {
                chromArr[i].EvalFitness(f);
            }
            //Since the EvalFitness invokes the Fitness delegate on this --> the fitness of the chromosomes are no longer default 0.0
            Assert.AreNotEqual(0, chromArr[0].Fitness);
            Assert.AreNotEqual(0, chromArr[1].Fitness);
            Assert.AreNotEqual(0, chromArr[2].Fitness);
            Assert.AreNotEqual(0, chromArr[3].Fitness);
            Assert.AreNotEqual(0, chromArr[4].Fitness);
        }

        [TestMethod]
        public void TestCompareTo()
        {
            //using the EvalFitness method to calculate the fitness of every chromosome then using Array.Sort() with the implementation of IComparable<Chromosome>
            Fitness f = new Fitness(Chromosome.TestEvalFitness);
            Chromosome[] chromArr = new Chromosome[5];
            for (int i = 0; i < chromArr.Length; i++)
            {
                chromArr[i] = new Chromosome(243);

            }
            for (int i = 0; i < chromArr.Length; i++)
            {
                chromArr[i].EvalFitness(f);
            }
            //sorting the array by fitness from worst to best fitness
            Array.Sort(chromArr);

            Assert.AreEqual(true, chromArr[0].Fitness < chromArr[1].Fitness);
            Assert.AreEqual(true, chromArr[1].Fitness < chromArr[2].Fitness);
            Assert.AreEqual(true, chromArr[2].Fitness < chromArr[3].Fitness);
            Assert.AreEqual(true, chromArr[3].Fitness < chromArr[4].Fitness);
        }

        [TestMethod]
        public void TestToString()
        {
            Chromosome testToString = new Chromosome(5);
            Allele[] alleleArr = testToString.AlleleArray;
            string[] individualAlleles = new string[alleleArr.Length];
            for(int i = 0; i < individualAlleles.Length;i++)
            {
                individualAlleles[i] = alleleArr[i].ToString();
            }

            string returnValue = testToString.ToString();
            string[] split = returnValue.Split(',');

            CollectionAssert.AreEqual(individualAlleles, split);
        }
        
    }
}
