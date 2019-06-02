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
	public class SingleplayerMode : GameMode.Module
	{
        public override void Init()
        {
            base.Init();
        }

        public override void Begin()
        {
            base.Begin();

            var player = Players.Spawn(Grid[0]);

            Menu.Dice.Show();
        }
    }
}