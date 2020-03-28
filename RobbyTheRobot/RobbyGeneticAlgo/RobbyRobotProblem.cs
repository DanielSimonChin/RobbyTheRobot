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

        private Generation[] generations;
        private Contents[][,] content;

        public RobbyRobotProblem(int numGenerations,int popSize,AlleleMoveandFitness f,int numActions,int numTestGrids,int gridSize,int numGenes,double eliteRate,double mutationRate)
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
        }
    }
}
