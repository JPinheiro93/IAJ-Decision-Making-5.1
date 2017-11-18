using System;
using Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures;
using Assets.Scripts.IAJ.Unity.Utils;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Pathfinding.Path;
using RAIN.Navigation.Graph;
using RAIN.Navigation.NavMesh;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace Assets.Scripts.IAJ.Unity.Pathfinding
{

    /*  ALGORITMO DAS TEORICAS PDF3 SLIDE 34/70
     *  
        given a path P = [p1,p2,...,pn]
        i=0
        repeat until i < n-2
        for every 3 waypoints Pi, Pi+1, Pi+2 in the path
        if walkable(pi, pi+2) then delete pi+1
        else move to next way point -> i=i+1

        walkable(p1,p2) -> raycast rays between p1 and p2, check if it overlaps any obstacle
                            
    */
    public class StringPullingPathSmoothing
    {

        public static bool Walkable(Vector3 p1, Vector3 p2)
        {
            return !Physics.Linecast(p1, p2);
        }

        public static GlobalPath SmoothPath(Vector3 initialPosition, GlobalPath path)
        {
            int i = 0;

            path.PathPositions.Insert(0, initialPosition);

            while(i < path.PathPositions.Count - 2)
            {
                if (Walkable(path.PathPositions[i], path.PathPositions[i + 2]))
                {
                    path.PathPositions.Remove(path.PathPositions[i + 1]);
                }
                else
                {
                    i++;
                }
            }

            return path;
        }
    }
}
