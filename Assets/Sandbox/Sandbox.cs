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
	public class Sandbox : MonoBehaviour
	{
        public EXAMPLE example;

        void Start()
        {
            StartCoroutine(Procedure());

            Invoke(nameof(DestroyExample), 1f);
        }

        IEnumerator Procedure()
        {
            yield return StartCoroutine(example.Procedure());

            Debug.Log("Ended: " + Time.timeSinceLevelLoad);
        }

        void DestroyExample()
        {
            Destroy(example.gameObject);
        }
	}
}