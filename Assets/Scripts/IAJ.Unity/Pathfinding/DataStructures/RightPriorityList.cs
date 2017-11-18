using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures
{
    public class RightPriorityList : IOpenSet
    {
        private List<NodeRecord> Open { get; set; }

        public RightPriorityList()
        {
            this.Open = new List<NodeRecord>();    
        }

        public void Initialize()
        {
            this.Open.Clear();
        }

        public void Replace(NodeRecord nodeToBeReplaced, NodeRecord nodeToReplace)
        {
            int index = this.Open.BinarySearch(nodeToBeReplaced);

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("Node to be replaced does not exist.");
            }
            else
            {
                this.Open[index] = nodeToReplace;
            }
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
            //a little help here
            //is very nice that the List<T> already implements a binary search method
            int index = this.Open.BinarySearch(nodeRecord);
            if (index < 0)
            {
                var indexToPlace = Open.FindIndex(x => nodeRecord.fValue == x.fValue ? nodeRecord.hValue > x.hValue : nodeRecord.fValue > x.fValue);
                this.Open.Insert(indexToPlace == -1 ? 0 : indexToPlace, nodeRecord);
            }
        }

        public void RemoveFromOpen(NodeRecord nodeRecord)
        {
            this.Open.Remove(nodeRecord);
        }

        public NodeRecord SearchInOpen(NodeRecord nodeRecord)
        {
            int index = this.Open.BinarySearch(nodeRecord);
            return index < 0 ? null : this.Open[index];
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
