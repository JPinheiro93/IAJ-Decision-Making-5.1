using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures
{
    public class LeftPriorityList : IOpenSet
    {
        private List<NodeRecord> Open { get; set; }

        public LeftPriorityList()
        {
            this.Open = new List<NodeRecord>();    
        }

        public void Initialize()
        {
            this.Open.Clear();
        }

        public void Replace(NodeRecord nodeToBeReplaced, NodeRecord nodeToReplace)
        {
            this.RemoveFromOpen(nodeToBeReplaced);
            this.AddToOpen(nodeToReplace);
        }

        public NodeRecord GetBestAndRemove()
        {
            var node = this.PeekBest();
            this.RemoveFromOpen(node);
            return node;
        }

        public NodeRecord PeekBest()
        {
            return this.Open.First();
        }

        public void AddToOpen(NodeRecord nodeRecord)
        {
            var index = Open.BinarySearch(nodeRecord);
            this.Open.Insert(index < 0 ? ~index : index, nodeRecord);
        }

        public void RemoveFromOpen(NodeRecord nodeRecord)
        {
            this.Open.Remove(nodeRecord);
        }

        public NodeRecord SearchInOpen(NodeRecord nodeRecord)
        {
            return this.Open.FirstOrDefault(n => n.Equals(nodeRecord));
        }

        public ICollection<NodeRecord> All()
        {
            return this.Open;
        }

        public int CountOpen()
        {
            return this.Open.Count;
        }
    }
}
