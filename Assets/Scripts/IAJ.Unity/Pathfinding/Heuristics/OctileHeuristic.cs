using RAIN.Navigation.Graph;
using System;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class OctileHeuristic : IHeuristic
    {
        private const float SquareRoot2 = 1.4142135623730950488016887242097f; // sqrt(2)

        public float H(NavigationGraphNode node, NavigationGraphNode goalNode)
        {
            var zDistance =  goalNode.Position.z - node.Position.z;
            var xDistance = goalNode.Position.x - node.Position.x;
            return Math.Max(zDistance, xDistance) + (SquareRoot2 - 1) * Math.Min(zDistance, xDistance);
        }
    }
}
