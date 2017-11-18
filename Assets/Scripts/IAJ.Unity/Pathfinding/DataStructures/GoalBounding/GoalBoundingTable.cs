﻿using RAIN.Navigation.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.GoalBounding
{
    [Serializable]
    public class GoalBoundingTable //: ScriptableObject
    {
        public NodeGoalBounds[] table;

        //public void SaveToAssetDatabase()
        //{
        //    string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        //    if (path == "")
        //    {
        //        path = "Assets";
        //    }
        //    else if (System.IO.Path.GetExtension(path) != "")
        //    {
        //        path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        //    }

        //    string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(GoalBoundingTable).Name.ToString() + ".asset");

        //    AssetDatabase.CreateAsset(this, assetPathAndName);
        //    EditorUtility.SetDirty(this);

        //    int total = this.table.Length;
        //    int index = 0;
        //    float progress = 0;
        //    //save the GoalBounds Table
        //    foreach (var nodeGoalBounds in this.table)
        //    {
        //        if (index % 10 == 0)
        //        {
        //            progress = (float)index / (float)total;
        //            EditorUtility.DisplayProgressBar("GoalBounding precomputation progress", "Saving GoalBoundsTable to an Asset file", progress);
        //        }

        //        if (nodeGoalBounds != null)
        //        {
        //            AssetDatabase.AddObjectToAsset(nodeGoalBounds, assetPathAndName);

        //            //save goalBounds for each edge of the node
        //            foreach (var goalBounds in nodeGoalBounds.connectionBounds)
        //            {
        //                AssetDatabase.AddObjectToAsset(goalBounds, assetPathAndName);
        //            }
        //        }

        //        index++;
        //    }


        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh();
        //    EditorUtility.FocusProjectWindow();
        //    Selection.activeObject = this;
        //}

        public void Save(string path, string fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(path + fileName, FileMode.Create);

            bf.Serialize(fs, this);
            fs.Close();
        }

        public static GoalBoundingTable Load(string path, string fileName)
        {
            FileStream fs = File.OpenRead(path + fileName);
            BinaryFormatter bf = new BinaryFormatter();
            var result = (GoalBoundingTable)bf.Deserialize(fs);
            fs.Close();

            return result;
        }
    }
}
