using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.GoalBounding;
using Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics;
using RAIN.Navigation.NavMesh;
using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures;
using RAIN.Navigation.Graph;
using UnityEngine;
using System;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.GoalBounding
{
    public class GoalBoundingPathfinding : NodeArrayAStarPathfinding
    {
        public GoalBoundingTable GoalBoundingTable { get; protected set; }
        public int DiscardedEdges { get; protected set; }
		public int TotalEdges { get; protected set; }

        public GoalBoundingPathfinding(NavMeshPathGraph graph, IHeuristic heuristic, GoalBoundingTable goalBoundsTable) : base(graph, heuristic)
        {
            this.GoalBoundingTable = goalBoundsTable;
        }

        public override void InitializePathfindingSearch(Vector3 startPosition, Vector3 goalPosition)
        {
            this.DiscardedEdges = 0;
			this.TotalEdges = 0;
            base.InitializePathfindingSearch(startPosition, goalPosition);
        }

        protected override void ProcessChildNode(NodeRecord parentNode, NavigationGraphEdge connectionEdge, int edgeIndex)
        {
            this.TotalEdges++;

            var nodeGoalBounds = parentNode.node == this.StartNode ? null : this.GoalBoundingTable.table[parentNode.node.NodeIndex];
            var edgeGoalBounds = nodeGoalBounds != null && nodeGoalBounds.connectionBounds.Length > edgeIndex ? nodeGoalBounds.connectionBounds[edgeIndex] : null;

            //discard any edges that don't lead to goal node, 
            //except for the initial node (goal bounds is null).
            if (nodeGoalBounds == null || edgeGoalBounds == null || edgeGoalBounds.PositionInsideBounds(this.GoalPosition))
            {
                base.ProcessChildNode(parentNode, connectionEdge, edgeIndex);
            }
            else
            {
                this.DiscardedEdges++;
            }
        }

        public override string DiscardedEdgesText()
        {
            return base.DiscardedEdgesText() + "\nTotal Explored Edges: " + this.TotalEdges
                    + "\nDiscarded Edges: " + this.DiscardedEdges;
        }
    }
}
