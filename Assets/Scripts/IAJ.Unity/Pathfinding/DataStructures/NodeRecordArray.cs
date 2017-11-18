using System;
using System.Collections.Generic;
using RAIN.Navigation.Graph;
using System.Linq;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures
{
    public class NodeRecordArray : IOpenSet, IClosedSet
    {
        private NodeRecord[] NodeRecords { get; set; }
        private List<NodeRecord> SpecialCaseNodes { get; set; }
        private NodePriorityHeap Open { get; set; }

        public int Length { get { return this.NodeRecords.Length; } }

        public NodeRecordArray(List<NavigationGraphNode> nodes)
        {
            //this method creates and initializes the NodeRecordArray for all nodes in the Navigation Graph
            this.NodeRecords = new NodeRecord[nodes.Count];

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                node.NodeIndex = i; //we're setting the node Index because RAIN does not do this automatically
                this.NodeRecords[i] =
                    new NodeRecord
                    {
                        node = node,
                        status = NodeStatus.Unvisited
                    };
            }

            this.SpecialCaseNodes = new List<NodeRecord>();

            this.Open = new NodePriorityHeap();
        }

        public NodeRecord GetNodeRecord(NavigationGraphNode node)
        {
            //do not change this method
            //here we have the "special case" node handling
            if (node.NodeIndex == -1)
            {
                return this.SpecialCaseNodes.FirstOrDefault(x => x.node == node);
            }
            else
            {
                return this.NodeRecords[node.NodeIndex];
            }
        }

        public void AddSpecialCaseNode(NodeRecord node)
        {
            this.SpecialCaseNodes.Add(node);
        }

        void IOpenSet.Initialize()
        {
            this.Open.Initialize();
            
            this.NodeRecords = this.NodeRecords.Select(x => { x.status = NodeStatus.Unvisited; return x; }).ToArray();

            this.SpecialCaseNodes.Clear();
        }

        void IClosedSet.Initialize()
        {
        }

        public void AddToOpen(NodeRecord nodeRecord)
        {
            nodeRecord.status = NodeStatus.Open;
            this.Open.AddToOpen(nodeRecord);
        }

        public void AddToClosed(NodeRecord nodeRecord)
        {
            nodeRecord.status = NodeStatus.Closed;
            this.Open.RemoveFromOpen(nodeRecord);
        }

        public NodeRecord SearchInOpen(NodeRecord nodeRecord)
        {
            var node = GetNode(nodeRecord);

            if (node != null && node.status == NodeStatus.Open)//dif
            {
                return node;
            }

            return null;
        }

        public NodeRecord SearchInClosed(NodeRecord nodeRecord)
        {
            var node = GetNode(nodeRecord);

            if (node != null && node.status == NodeStatus.Closed)
            {
                return node;
            }

            return null;
        }

        public NodeRecord GetBestAndRemove()
        {
            NodeRecord best = Open.GetBestAndRemove();
            best.status = NodeStatus.Unvisited;
            return best;
        }

        public NodeRecord PeekBest()
        {
            return this.Open.PeekBest();
        }

        public void Replace(NodeRecord nodeToBeReplaced, NodeRecord nodeToReplace)
        {
            this.RemoveFromOpen(nodeToBeReplaced);
            this.AddToOpen(nodeToReplace);
        }

        public void RemoveFromOpen(NodeRecord nodeRecord)
        {
            this.Open.RemoveFromOpen(nodeRecord);
            nodeRecord.status = NodeStatus.Unvisited;
        }

        public void RemoveFromClosed(NodeRecord nodeRecord)
        {
            nodeRecord.status = NodeStatus.Unvisited;
        }

        ICollection<NodeRecord> IOpenSet.All()
        {
            return this.Open.All();
        }

        ICollection<NodeRecord> IClosedSet.All()
        {
            return this.NodeRecords.Where(x => x.status == NodeStatus.Closed).ToList();
        }

        public int CountOpen()
        {
            return this.Open.CountOpen();
        }

        private NodeRecord GetNode(NodeRecord nodeRecord)
        {
            var index = nodeRecord.node.NodeIndex;
            return this.NodeRecords[index];
        }
    }
}
