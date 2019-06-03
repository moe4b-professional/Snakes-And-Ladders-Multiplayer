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
	public class EXAMPLE : MonoBehaviour
	{
		public IEnumerator Procedure()
        {
            var timer = 2f;

            while(timer != 0f)
            {
                timer = Mathf.MoveTowards(timer, 0f, Time.deltaTime);

                Debug.Log("timer: " + timer);

                yield return null;
            }
        }
	}
}