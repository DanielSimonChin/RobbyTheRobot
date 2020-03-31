using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    /// <summary>
    /// Gets all the other classes to work together. Calls several methods in the Helpers.cs
    /// </summary>
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

        /// <summary>
        /// Initializes the fields of a Robby and initializes the grids
        /// </summary>
        /// <param name="numGenerations">How many robby's we will create(generations)</param>
        /// <param name="popSize">How many chromosomes per generation</param>
        /// <param name="f">An AlleleMoveandFitness which points to the ScoreForAllele method</param>
        /// <param name="numActions">How many actions per grid</param>
        /// <param name="numTestGrids">Number of test grids to test on one chromosome</param>
        /// <param name="gridSize">Width or height of a grid</param>
        /// <param name="numGenes">Genes per chromosome</param>
        /// <param name="eliteRate">Percentage of elite chromosomes which is retained for future gens</param>
        /// <param name="mutationRate">Mutation rate deciding how often a gene is changed</param>
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

        /// <summary>
        /// Initializes the current gen and runs a loop which repeatedly creates new and improved gens
        /// </summary>
        public void Start()
        {
            Crossover f = Chromosome.DoubleCrossover;
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

        /// <summary>
        /// Creates 100 grids and evaluates the fitness of every chromosome using EvalFitness
        /// </summary>
        /// <param name="f">Fitness delegate which points to RobbyFitness</param>
        public void EvalFitness(Fitness f)
        {
            //generates a set of test grids using Helper method.
            for (int i = 0; i < contentGrids.Length; i++)
            {
                this.contentGrids[i] = Helpers.GenerateRandomTestGrid(gridSize);
            }
            this.currentGeneration.EvalFitness(f);
        }

        /// <summary>
        /// Retains the elite and reproduces and creates a new population of chromosomes
        /// </summary>
        /// <param name="f">A crossover delegate object which is either Single or double crossover</param>
        public void GenerateNextGeneration(Crossover f)
        {
            List<Chromosome> newPopulation = new List<Chromosome>();
            int elitePopulationCount = (int)(this.popSize * this.eliteRate);
            //adding the elites to the next generation
            for(int i=0;i<elitePopulationCount;i++)
            {
                //already sorted by fitness with best at smaller indexes
                newPopulation.Add(this.currentGeneration[i]);
            }
            //while less than 200
            while(newPopulation.Count < this.popSize)
            {
                //select 2 parents to reproduce from the current population
                Chromosome parent1 = this.currentGeneration.SelectParent();
                Chromosome parent2 = this.currentGeneration.SelectParent();

                Chromosome[] children = parent1.Reproduce(parent2, f, this.mutationRate);
                newPopulation.Add(children[0]);
                newPopulation.Add(children[1]);
            }
            Chromosome[] nextGenPop = new Chromosome[newPopulation.Count];
            for(int i=0;i<nextGenPop.Length;i++)
            {
                nextGenPop[i] = newPopulation[i];
            }
            //new generation
            Generation nextGen = new Generation(nextGenPop);
            //reference the current gen to point to new gen
            this.currentGeneration = nextGen;

            /*int elitePopulationCount = (int)(this.popSize * this.eliteRate);
            Chromosome[] elitePopulation = new Chromosome[elitePopulationCount];
            for (int i = 0; i < elitePopulation.Length; i++)
            {
                //the elites are already sorted in the current gen by fitness
                elitePopulation[i] = this.currentGeneration[i];
            }
            Generation newGen = new Generation(elitePopulation);

            //the children that will be added by reproducing the elite
            List<Chromosome> newChildren = new List<Chromosome>();

            while (newChildren.Count < this.popSize - elitePopulation.Length)
            {
                //select 2 parents to reproduce using SelectParent() in generation class
                Chromosome parent1 = newGen.SelectParent();
                Chromosome parent2 = newGen.SelectParent();

                Chromosome[] children = parent1.Reproduce(parent2, f, this.mutationRate);
                newChildren.Add(children[0]);
                newChildren.Add(children[1]);
            }
            List<Chromosome> nextGenPopList = new List<Chromosome>();
            for (int i = 0; i < elitePopulation.Length; i++)
            {
                nextGenPopList.Add(elitePopulation[i]);
            }
            for (int i = 0; i < newChildren.Count; i++)
            {
                nextGenPopList.Add(newChildren[i]);
            }
            Chromosome[] nextGenPop = new Chromosome[nextGenPopList.Count];
            for (int i = 0; i < nextGenPop.Length; i++)
            {
                nextGenPop[i] = nextGenPopList[i];
            }
            //creating the new generation with a new population
            newGen = new Generation(nextGenPop);
            //referencing the current gen to point to the newly created one.
            this.currentGeneration = newGen;*/
        }

        /// <summary>
        /// Runs a single chromosome across all the grids and returns the average fitness
        /// </summary>
        /// <param name="c">A single chromosome to find fitness of</param>
        /// <returns>Average fitness of a chromosome</returns>
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