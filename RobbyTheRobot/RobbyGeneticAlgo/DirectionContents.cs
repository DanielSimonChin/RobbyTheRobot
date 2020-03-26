using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    public struct DirectionContents
    {
        Contents N { get; set; }
        Contents S { get; set; }
        Contents E { get; set; }
        Contents W { get; set; }
        Contents current { get; set; }
    }
}
