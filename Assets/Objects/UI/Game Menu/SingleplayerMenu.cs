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

using Photon.Pun;

namespace Game
{
	public class SingleplayerMenu : UIElement
	{
		void OnEnable()
        {
            PhotonNetwork.OfflineMode = true;

            Core.Instance.Mode.Singleplayer.Begin();

            Hide();
        }
	}
}