using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures;
using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.GoalBounding;
using RAIN.Navigation.Graph;
using RAIN.Navigation.NavMesh;
using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.GoalBounding
{
    //The Dijkstra algorithm is similar to the A* but with a couple of differences
    //1) no heuristic function
    //2) it will not stop until the open list is empty
    //3) we dont need to execute the algorithm in multiple steps (because it will be executed offline)
    //4) we don't need to return any path (partial or complete)
    //5) we don't need to do anything when a node is already in closed
    public class GoalBoundsDijkstraMapFlooding
    {
        public NavMeshPathGraph NavMeshGraph { get; protected set; }
        public NavigationGraphNode StartNode { get; protected set; }
        public NodeGoalBounds NodeGoalBounds { get; protected set; }
        public NodeRecordArray NodeRecordArray { get; set; }

        public IOpenSet Open { get; protected set; }
        public IClosedSet Closed { get; protected set; }
        
        public GoalBoundsDijkstraMapFlooding(NavMeshPathGraph graph)
        {
            this.NavMeshGraph = graph;
            //do not change this
            var nodes = this.GetNodesHack(graph);
            this.NodeRecordArray = new NodeRecordArray(nodes);
            this.Open = this.NodeRecordArray;
            this.Closed = this.NodeRecordArray;
        }

        private void Initialize(NavigationGraphNode startNode)
        {
            var startNodeRecord = new NodeRecord
            {
                fValue = 0,
                gValue = 0,
                hValue = 0,
                StartNodeOutConnectionIndex = -1,
                node = startNode,
                status = NodeStatus.Closed
            };

            this.Open.Initialize();
            this.Closed.Initialize();

            this.Closed.AddToClosed(startNodeRecord);

            for (var i = 0; i < startNode.OutEdgeCount; i++)
            {
                var outEdge = startNode.EdgeOut(i);
                var childNodeRecord = this.NodeRecordArray.GetNodeRecord(outEdge.ToNode);

                var gValue = startNodeRecord.gValue + (childNodeRecord.node.LocalPosition - startNodeRecord.node.LocalPosition).magnitude;

                childNodeRecord.hValue = 0;
                childNodeRecord.gValue = gValue;
                childNodeRecord.fValue = gValue;
                childNodeRecord.StartNodeOutConnectionIndex = i;
                childNodeRecord.parent = startNodeRecord;

                this.UpdateBoundingBoxes(childNodeRecord, i);

                this.Open.AddToOpen(childNodeRecord);
            }
        }

        public void Search(NavigationGraphNode startNode, NodeGoalBounds nodeGoalBounds)
        {
            this.StartNode = startNode;
            this.NodeGoalBounds = nodeGoalBounds;

            // we fill the initial childNodes with start edge indexes, 
            // and add them to the open list. the start node is added to closed list
            this.Initialize(startNode);

            var closed = this.Closed.All();

            var openCount = this.Open.CountOpen();

            while (openCount > 0)
            {
                var currentNode = this.Open.GetBestAndRemove();

                this.Closed.AddToClosed(currentNode);

                for (var i = 0; i < currentNode.node.OutEdgeCount; i++)
                {
                    ProcessChildNode(currentNode, currentNode.node.EdgeOut(i), currentNode.StartNodeOutConnectionIndex);
                }

                openCount = this.Open.CountOpen();
            }
        }
       
        protected void ProcessChildNode(NodeRecord parent, NavigationGraphEdge connectionEdge, int edgeIndex)
        {
            var childNode = connectionEdge.ToNode;
            var childNodeRecord = this.NodeRecordArray.GetNodeRecord(childNode);

            this.UpdateBoundingBoxes(childNodeRecord, edgeIndex);

            if (childNodeRecord == null)
            {
                //this piece of code is used just because of the special start nodes and goal nodes added to the RAIN Navigation graph when a new search is performed.
                //Since these special goals were not in the original navigation graph, they will not be stored in the NodeRecordArray and we will have to add them
                //to a special structure
                //it's ok if you don't understand this, this is a hack and not part of the NodeArrayA* algorithm, just do NOT CHANGE THIS, or your algorithm will not work
                childNodeRecord = new NodeRecord
                {
                    node = childNode,
                    parent = parent,
                    status = NodeStatus.Unvisited
                };
                this.NodeRecordArray.AddSpecialCaseNode(childNodeRecord);
            }
            else
            {
                NodeStatus childStatus = childNodeRecord.status;

                float gValue = parent.gValue + (childNode.LocalPosition - parent.node.LocalPosition).magnitude;

                if (childStatus == NodeStatus.Unvisited)
                {
                    this.NodeRecordArray.AddToOpen(UpdateChildNodeRecord(childNodeRecord, parent, gValue));
                }
                else if (gValue < childNodeRecord.gValue && childStatus == NodeStatus.Open)
                {
                    this.NodeRecordArray.Replace(childNodeRecord, UpdateChildNodeRecord(childNodeRecord, parent, gValue));
                }
            }
        }

        private void UpdateBoundingBoxes(NodeRecord childNodeRecord, int edgeIndex)
        {
            this.NodeGoalBounds.connectionBounds[edgeIndex].UpdateBounds(childNodeRecord.node.LocalPosition);
        }

        private NodeRecord UpdateChildNodeRecord(NodeRecord childNodeRecord, NodeRecord parent, float g)
        {
            childNodeRecord.parent = parent;
            childNodeRecord.gValue = g;
            childNodeRecord.fValue = g;
            childNodeRecord.StartNodeOutConnectionIndex = parent.StartNodeOutConnectionIndex;

            return childNodeRecord;
        }

        private List<NavigationGraphNode> GetNodesHack(NavMeshPathGraph graph)
        {
            //this hack is needed because in order to implement NodeArrayA* you need to have full acess to all the nodes in the navigation graph in the beginning of the search
            //unfortunately in RAINNavigationGraph class the field which contains the full List of Nodes is private
            //I cannot change the field to public, however there is a trick in C#. If you know the name of the field, you can access it using reflection (even if it is private)
            //using reflection is not very efficient, but it is ok because this is only called once in the creation of the class
            //by the way, NavMeshPathGraph is a derived class from RAINNavigationGraph class and the _pathNodes field is defined in the base class,
            //that's why we're using the type of the base class in the reflection call
            return (List<NavigationGraphNode>)Utils.Reflection.GetInstanceField(typeof(RAINNavigationGraph), graph, "_pathNodes");
        }
    }
}
