using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.GoalBounding
{
    [Serializable]
    public class Bounds //: ScriptableObject
    {
        public float minx;
        public float maxx;
        public float minz;
        public float maxz;

        public Bounds()
        {
        }

        public Bounds(Vector3 position)
        {
            this.InitializeBounds(position);
        }

        public void InitializeBounds(Vector3 position)
        {
            this.minx = position.x;
            this.maxx = position.x;
            this.minz = position.z;
            this.maxz = position.z;
        }

        public void UpdateBounds(Vector3 position)
        {
            if (this.minx > position.x)
            {
                this.minx = position.x;
            }

            if (this.maxx < position.x)
            {
                this.maxx = position.x;
            }

            if (this.minz > position.z)
            {
                this.minz = position.z;
            }

            if (this.maxz < position.z)
            {
                this.maxz = position.z;
            }
        }

        public bool PositionInsideBounds(Vector3 position)
        {
            //Optimization 3: eliminate disgusting IF's, and return boolean expressions!
            return !(position.x < minx ||
                     position.x > maxx ||
                     position.z < minz ||
                     position.z > maxz);
        }
    }
}
