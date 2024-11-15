﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tweener
{
    public class Tween : MonoBehaviour
    {
        internal static Tween instance;
        private static bool Launched;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("ErrorInitializeObjects: " + gameObject.name);
                GameObject.Destroy(gameObject);
            }
            instance = this;
        }
        private void OnDestroy()
        {
            instance = null;
        }
        internal bool isPause => Time.timeScale == 0;
        private IEnumerator MainProcess()
        {
            while (Tweener.BetweenObjects.Count != 0)
            {
                if (isPause)
                    yield return new WaitUntil(() => !isPause);
                yield return null;
                List<string> Delete = new();
                foreach (KeyValuePair<string, ITweenable> keyValues in Tweener.BetweenObjects)
                    if (keyValues.Value.IsUsed())
                        Delete.Add(keyValues.Key);
                Delete.ForEach(key =>
                {
                    ITweenable tweenable = Tweener.BetweenObjects[key];
                    tweenable.OnComplection();
                    if (tweenable.Timer >= 1F)
                        Tweener.BetweenObjects.Remove(key);
                });
            }
            Launched = false;
            yield break;
        }
        #region Color
        public static IExpansionColor SetColor(Transform transform, Color newColor, float time = 1F)
        {
            return new SetColor(transform, newColor, time);
        }
        public static IExpansionColor AddColor(Transform transform, Color newColor, float time = 1F)
        {
            return new AddColor(transform, newColor, time);
        }
        #endregion 
        #region Vector3
        public static IExpansionTween SetScale(Transform Object, Vector3 scale, float time = 1F)
        {
            return new Scale(Object, scale, time, false);
        }
        public static IExpansionTween AddScale(Transform Object, Vector3 scale, float time = 1F)
        {
            return new Scale(Object, scale, time, true);
        }
        public static IExpansionTween SetRotation(Transform Object, Vector3 Euler, float time = 1F)
        {
            return new Rotation(Object, Euler, time, false);
        }
        public static IExpansionTween AddRotation(Transform Object, Vector3 Euler, float time = 1F)
        {
            return new Rotation(Object, Euler, time, true);
        }
        public static IExpansionTween SetPosition(Transform Object, Vector3 position, float time = 1F)
        {
            return new Position(Object, position, time, false);
        }
        public static IExpansionTween AddPosition(Transform Object, Vector3 position, float time = 1F)
        {
            return new Position(Object, position, time, true);
        }

        #endregion 
        #region Bezier

        public static IExpansionBezier GoWay(Transform transform,BezierWay way, float timeOrSpeed, bool isSpeed = false)
        {
            return new Bezier(way, transform , timeOrSpeed, isSpeed);
        }

        #endregion 
        public static IExpansionTween StartTween(IExpansionTween tween)
        {
            Tweener tweener = tween as Tweener;
            if (tweener.transform == null) return null;
            tweener.Restart();
            Tweener.BetweenObjects.Add(tweener.NameOperator,tweener);
            Launch();
            return tween;
        }
        internal static void Launch()
        {
            if (!instance)
            {
                new GameObject("Tweener", typeof(Tween));
            }
            if (Launched) return;
            instance.StartCoroutine(instance.MainProcess());
            Launched = true;
        }
    }
}
