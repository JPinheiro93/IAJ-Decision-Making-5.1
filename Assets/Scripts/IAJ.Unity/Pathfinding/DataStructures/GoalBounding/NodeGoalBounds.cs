using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.GoalBounding
{
    [Serializable]
    public class NodeGoalBounds //: ScriptableObject
    {
        public Bounds[] connectionBounds;
    }
}
