using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
    [RequireComponent(typeof(RectTransform))]
	public class AutoLayoutRebuild : MonoBehaviour
	{
        void OnEnable()
        {
            Execute();

            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            yield return new WaitForEndOfFrame();

            Execute();
        }

        void Execute()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}