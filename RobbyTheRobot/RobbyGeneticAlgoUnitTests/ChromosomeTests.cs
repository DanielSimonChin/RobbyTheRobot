using System;
using RobbyGeneticAlgo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobbyGeneticAlgoUnitTests
{
    [TestClass]
    public class ChromosomeTests
    {
        [TestMethod]
        public void TestChromosomeConstructor1()
        {
            Chromosome constructor1 = new Chromosome(5);
            Assert.AreEqual(5, constructor1.arrayLength);
        }
    }
}
