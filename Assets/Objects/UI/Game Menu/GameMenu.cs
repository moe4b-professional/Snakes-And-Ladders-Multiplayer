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
    public class GameMenu : MonoBehaviour
    {
        [SerializeField]
        protected StartMenu start;
        public StartMenu Start { get { return start; } }

        [SerializeField]
        protected SingleplayerMenu singleplayer;
        public SingleplayerMenu Singleplayer { get { return singleplayer; } }

        [SerializeField]
        protected MultiplayerMenu multiplayer;
        public MultiplayerMenu Multiplayer { get { return multiplayer; } }

        [SerializeField]
        protected RoomMenu room;
        public RoomMenu Room { get { return room; } }

        [SerializeField]
        protected Dice dice;
        public Dice Dice { get { return dice; } }

        [SerializeField]
        protected Popup popup;
        public Popup Popup { get { return popup; } }

        public void Init()
        {
            popup.Init();
        }
    }
}