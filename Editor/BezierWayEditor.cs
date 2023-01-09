using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweener;
using UnityEditor;
namespace Assets.Editors
{
    [CustomEditor(typeof(BezierWay)), CanEditMultipleObjects]
    public class BezierWayEditor : Editor
    {
        public BezierWay BezierWay;
        public bool ViewList;

        [MenuItem("Tweener/Create Bezier Way")]
        public static void CreateBezierWay()
        {
            new GameObject("BezierWay", typeof(BezierWay));
        }
        public void OnEnable()
        {
            BezierWay = (BezierWay)target;
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();


            GUILayout.BeginHorizontal();
            GUILayout.Label($"Point {BezierWay.Count}: ");
            if (GUILayout.Button("Add"))
            {
                BezierPoint bezier = new BezierPoint(Vector3.zero, Vector3.up);
                BezierWay.pointsBezier.Add(bezier);
                bezier._Point.SetParent(BezierWay.transform);
            }
            if (GUILayout.Button("Remove"))
            {
                int index = BezierWay.Count - 1;
                GameObject.DestroyImmediate(BezierWay[index]._Point.gameObject);
                BezierWay.pointsBezier.RemoveAt(index);
            }
            GUILayout.EndHorizontal();
            BezierWay.Visible = EditorGUILayout.Toggle("Draw Bezier?", BezierWay.Visible);
            ViewList = EditorGUILayout.Toggle("List Point", ViewList);
            if (BezierWay.Count > 0 && ViewList)
            {
                foreach (BezierPoint point in BezierWay)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(point.ToString());
                    if (GUILayout.Button("Point"))
                    {
                        Selection.activeGameObject = point._Point.gameObject;
                    }
                    if (GUILayout.Button("Enter"))
                    {
                        Selection.activeGameObject = point._Entrance.gameObject;
                    }
                    GUILayout.EndHorizontal();
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
