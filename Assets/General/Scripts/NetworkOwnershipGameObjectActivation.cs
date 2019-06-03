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
	public class NetworkOwnershipGameObjectActivation : MonoBehaviourPunCallbacks
	{
        [SerializeField]
        protected PhotonView view;
        public PhotonView View { get { return view; } }

        [SerializeField]
        protected GameObject local;
        public GameObject Local { get { return local; } }

        [SerializeField]
        protected GameObject remote;
        public GameObject Remote { get { return remote; } }

        void Start()
        {
            local.SetActive(view.IsMine);
            remote.SetActive(!view.IsMine);
        }
    }
}