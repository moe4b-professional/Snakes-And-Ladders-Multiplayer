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
	public class NetworkCallbacks : MonoBehaviour, IConnectionCallbacks
    {
        public void Init()
        {
            
        }

        void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public event Action ConnectedEvent;
        public void OnConnected()
        {
            if (ConnectedEvent != null) ConnectedEvent();
        }

        public event Action ConnectedToMasterEvent;
        public void OnConnectedToMaster()
        {
            if (ConnectedToMasterEvent != null) ConnectedToMasterEvent();
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {

        }
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {

        }

        public event Action<DisconnectCause> DisconnectedEvent;
        public void OnDisconnected(DisconnectCause cause)
        {
            if (DisconnectedEvent != null) DisconnectedEvent(cause);
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {

        }

        void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}