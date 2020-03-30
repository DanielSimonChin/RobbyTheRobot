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
            int elitePopulationCount = (int)(this.popSize * this.eliteRate);
            Chromosome[] elitePopulation = new Chromosome[elitePopulationCount];
            for(int i = 0; i < elitePopulation.Length;i++)
            {
                //the elites are already sorted in the current gen by fitness
                elitePopulation[i] = this.currentGeneration[i];
            }
            Generation newGen = new Generation(elitePopulation);

            //loop until the next generation is full of chromosomes
            while(newGen.ArrayLength < this.popSize)
            {
                //select 2 parents to reproduce using SelectParent() in generation class
                Chromosome parent1 = newGen.SelectParent();
                Chromosome parent2 = newGen.SelectParent();

                Chromosome[] newGenerationChromosomes = parent1.Reproduce(parent2, f, this.mutationRate);

                List<Chromosome> newPopulation = new List<Chromosome>();
                for (int i = 0;i<newGen.ArrayLength;i++)
                {
                    newPopulation.Add(newGen[i]);
                }
                //add the 2 new children
                newPopulation.Add(newGenerationChromosomes[0]);
                newPopulation.Add(newGenerationChromosomes[1]);

                Chromosome[] nextGenPop = new Chromosome[newPopulation.Count];
                for(int i = 0; i < nextGenPop.Length;i++)
                {
                    nextGenPop[i] = newPopulation[i];
                }
                newGen = new Generation(nextGenPop);
                
            }
            //reference the current gen to this newly created generation
            this.currentGeneration = newGen;
        }


        

        public double RobbyFitness(Chromosome c)
        {
            double totalFitness = 0;
            double averageFitness = 0;
            for(int i = 0; i < this.contentGrids.Length;i++)
            {
                 totalFitness += Helpers.RunRobbyInGrid(this.contentGrids[i], c, this.numActions, this.f);
            }

            averageFitness = totalFitness / this.contentGrids.Length;
            return averageFitness; 
        }
        public event GenerationEventHandler GenerationReplaced;
    }
}