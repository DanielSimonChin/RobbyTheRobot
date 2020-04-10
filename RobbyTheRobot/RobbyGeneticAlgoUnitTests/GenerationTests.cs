using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyGeneticAlgo;

namespace RobbyGeneticAlgoUnitTests
{
    [TestClass]
    public class GenerationTests
    {
        [TestMethod]
        //THIS TEST METHOD FAILS WHEN RUNNING ALL THE TESTS COLLECTIVELY, IF FAILS, RUN THIS TEST INDIVIDUALLY (because of random instances)
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
        public void TestEvalFitness()
        {
            Fitness f = new Fitness(Chromosome.TestEvalFitness);
            Generation gen = new Generation(5, 243);

            //calls EvalFitness, the chromosomes now have a Fitness(non-0 value) and are sorted with best chromosome at the smallest index(decreasing order)
            gen.EvalFitness(f);
            Assert.AreNotEqual(0, gen[0].Fitness);
            Assert.AreNotEqual(0, gen[1].Fitness);
            Assert.AreNotEqual(0, gen[2].Fitness);
            Assert.AreNotEqual(0, gen[3].Fitness);
            Assert.AreNotEqual(0, gen[4].Fitness);
            //check for proper order(decreasing)
            Assert.AreEqual(true, gen[0].Fitness > gen[1].Fitness);
            Assert.AreEqual(true, gen[1].Fitness > gen[2].Fitness);
            Assert.AreEqual(true, gen[2].Fitness > gen[3].Fitness);
            Assert.AreEqual(true, gen[3].Fitness > gen[4].Fitness);


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

        [TestMethod]
        public void TestSelectParent()
        {
            Generation gen = new Generation(5,243);

            Fitness f = new Fitness(Chromosome.TestEvalFitness);
            //calls EvalFitness, the chromosomes now have a Fitness(non-0 value) and are sorted with best chromosome at the smallest index(decreasing order)
            gen.EvalFitness(f);

            Chromosome result = gen.SelectParent();
            //Since the fitnesses have already been calculated, the method must return a chromosome with a non-zero fitness
            Assert.AreNotEqual(0, result.Fitness);
            Assert.AreEqual(243, result.arrayLength);
        }
    }
}
