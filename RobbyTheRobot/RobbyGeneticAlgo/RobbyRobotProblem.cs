using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    public delegate int AllelMoveAndFitness(Chromosome c, Contents[,] grid, ref int x, ref int y);
    public class RobbyRobotProblem
    {
        private Generation[] generations;
        private Contents[,] content;
        private int numGenerations;
        private int popSize;
        private AllelMoveAndFitness f;
       /* private int numActions;
        private int numTestGrids;
        private int gridSize;
        private int numGenes;
        private double eliteRate;
        private double mutationRate; */
        public RobbyRobotProblem(int numGenerations, int popSize, AllelMoveAndFitness f, int numActions=200, int numTestGrids=100, int gridSize=10, int numGenes=243, double eliteRate=0.05, double mutationRate=0.05)
        {
            this.numGenerations = numGenerations;
            this.popSize = popSize;
            this.f = f;

        }
    }
}
