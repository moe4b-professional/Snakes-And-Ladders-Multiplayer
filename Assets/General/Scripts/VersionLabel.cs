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
    [RequireComponent(typeof(Text))]
	public class VersionLabel : MonoBehaviour
	{
        [SerializeField]
        protected string prefix = "Version ";
        public string Prefix { get { return prefix; } }

        void Start()
        {
            GetComponent<Text>().text = prefix + Application.version;
        }
	}
}