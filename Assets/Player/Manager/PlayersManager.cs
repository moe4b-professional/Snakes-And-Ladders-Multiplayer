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
	public class PlayersManager : MonoBehaviour
	{
		[SerializeField]
        protected List<Player> list;
        public List<Player> List { get { return list; } }

        public Player this[int index]
        {
            get
            {
                return list[index];
            }
        }

        public virtual void Init()
        {
            list[0].Init();
        }
    }
}