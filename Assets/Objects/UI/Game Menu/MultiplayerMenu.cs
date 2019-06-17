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

using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Game
{
	public class MultiplayerMenu : UIElement
    {
        public Core Core { get { return Core.Instance; } }

        public NetworkManager Network { get { return Core.Network; } }

		[SerializeField]
        protected PlayerNameInputField playerName;
        public PlayerNameInputField PlayerName { get { return playerName; } }

        [SerializeField]
        protected Button play;
        public Button Play { get { return play; } }

        [SerializeField]
        protected Button back;
        public Button Back { get { return back; } }

        void OnEnable()
        {
            Core.PlayerName.OnChange += OnPlayerNameChanged;

            OnPlayerNameChanged(Core.PlayerName.Value);
        }

        void Start()
        {
            play.onClick.AddListener(OnPlay);
        }

        void OnPlayerNameChanged(string newValue)
        {
            play.interactable = Core.PlayerName.Value.Length > 0;
        }

        void OnPlay()
        {
            back.interactable = false;

            Core.Mode.Multiplayer.Begin();
        }

        public void Reset()
        {
            back.interactable = true;

            Core.Popup.Hide();
        }

        void OnDisable()
        {
            Core.PlayerName.OnChange -= OnPlayerNameChanged;
        }
    }
}