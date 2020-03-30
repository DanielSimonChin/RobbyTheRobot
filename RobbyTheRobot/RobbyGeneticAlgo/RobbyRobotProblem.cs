using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    public class RobbyRobotProblem
    {
        private int numGenerations;
        private int popSize;
        private AlleleMoveandFitness f;
        private int numActions;
        private int numTestGrids;
        private int gridSize;
        private int numGenes;
        private double eliteRate;
        private double mutationRate;

        private Generation currentGeneration;
        private Contents[][,] contentGrids;

        public RobbyRobotProblem(int numGenerations, int popSize, AlleleMoveandFitness f, int numActions = 200, int numTestGrids = 100, int gridSize = 10, int numGenes = 243, double eliteRate = 0.05, double mutationRate = 0.05)
        {
            this.numGenerations = numGenerations;
            this.popSize = popSize;
            this.f = f;
            this.numActions = numActions;
            this.numTestGrids = numTestGrids;
            this.gridSize = gridSize;
            this.numGenes = numGenes;
            this.eliteRate = eliteRate;
            this.mutationRate = mutationRate;

            this.contentGrids = new Contents[numTestGrids][,];
        }

        public void Start()
        {
            Crossover f = Chromosome.SingleCrossover;
            //initialize first current generation
            this.currentGeneration = new Generation(this.popSize, this.numGenes);
            //adding a method of same signature to Fitness delegate
            Fitness fitness = RobbyFitness;
            for (int i = 0; i < this.numGenerations; i++)
            {
                EvalFitness(fitness);
                GenerationReplaced?.Invoke(i + 1, this.currentGeneration);
                GenerateNextGeneration(f);
            }
        }

        public void EvalFitness(Fitness f)
        {
            //generates a set of test grids using Helper method.
            for (int i = 0; i < contentGrids.Length; i++)
            {
                this.contentGrids[i] = Helpers.GenerateRandomTestGrid(gridSize);
            }
            this.currentGeneration.EvalFitness(f);
        }

        public void GenerateNextGeneration(Crossover f)
        {
            List<Chromosome> newPopulation = new List<Chromosome>();

            int elitePopulationCount = (int)(this.popSize * this.eliteRate);

            //filling up the new population with the elite population from the current generation
            for (int i = 0; i < elitePopulationCount; i++)
            {
                //chromosomes are already sorted by Fitness
                newPopulation.Add(this.currentGeneration[i]);
            }

            while (newPopulation.Count < this.popSize)
            {
                //select 2 parents to reproduce
                Chromosome parent1 = this.currentGeneration.SelectParent();
                Chromosome parent2 = this.currentGeneration.SelectParent();

                Chromosome[] newGenerationChromosomes = parent1.Reproduce(parent2, f, this.mutationRate);

                newPopulation.Add(newGenerationChromosomes[0]);
                newPopulation.Add(newGenerationChromosomes[1]);
            }

            //the next generation's chromosome population
            Chromosome[] chromosomeArray = new Chromosome[this.popSize];
            //copying the list of chromosomes into an array to use as input to the Generation constructor
            for (int i = 0; i < chromosomeArray.Length; i++)
            {
                chromosomeArray[i] = newPopulation[i];
            }
            Generation nextGeneration = new Generation(chromosomeArray);
            this.currentGeneration = nextGeneration;
        }

        public double RobbyFitness(Chromosome c)
        {
            double totalFitness = 0;
            double averageFitness = 0;
            for (int i = 0; i < this.contentGrids.Length; i++)
            {
                totalFitness += Helpers.RunRobbyInGrid(this.contentGrids[i], c, this.numActions, this.f);
            }

            averageFitness = totalFitness / this.contentGrids.Length;
            return averageFitness;
        }

        public event GenerationEventHandler GenerationReplaced;
    }
}
