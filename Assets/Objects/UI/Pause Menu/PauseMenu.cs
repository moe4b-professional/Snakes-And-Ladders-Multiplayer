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
	public class PauseMenu : UIElement
	{
        public Core Core { get { return Core.Instance; } }

        public GameMenu Menu { get { return Core.Menu; } }

        public NetworkManager Network { get { return Core.Network; } }

		public void Init()
        {
            
        }
        
        public void Process()
        {
            if(Core.Match.Active)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
                    Visible = !Visible;
            }
        }
    }
}