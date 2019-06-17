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
using ExitGames.Client.Photon;

using PunPlayer = Photon.Realtime.Player;

namespace Game
{
	public class NetworkCallbacks : MonoBehaviour
    {
        public void Init()
        {
            
        }

        void Start()
        {
            Connection = Init(new ConnectionCallbacks());

            Room = Init(new RoomCallbacks());

            Matchmaking = Init(new MatchmakingCallbacks());
        }

        public static TCallback Init<TCallback>(TCallback instance)
        {
            PhotonNetwork.AddCallbackTarget(instance);

            return instance;
        }

        public abstract class CallbacksGroup
        {
            public abstract void Clear();
        }

        public ConnectionCallbacks Connection { get; protected set; }
        public class ConnectionCallbacks : CallbacksGroup, IConnectionCallbacks
        {
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

            public override void Clear()
            {
                ConnectedEvent = null;
                ConnectedToMasterEvent = null;
                DisconnectedEvent = null;
            }
        }

        public RoomCallbacks Room { get; protected set; }
        public class RoomCallbacks : CallbacksGroup, IInRoomCallbacks
        {
            public event Action<PunPlayer> MasterClientChangedEvent;
            public void OnMasterClientSwitched(PunPlayer newMasterClient)
            {
                if (MasterClientChangedEvent != null) MasterClientChangedEvent(newMasterClient);
            }

            public event Action<PunPlayer> PlayerEnteredEvent;
            public void OnPlayerEnteredRoom(PunPlayer newPlayer)
            {
                if (PlayerEnteredEvent != null) PlayerEnteredEvent(newPlayer);
            }

            public event Action<PunPlayer> PlayerLeftEvent;
            public void OnPlayerLeftRoom(PunPlayer otherPlayer)
            {
                if (PlayerLeftEvent != null) PlayerLeftEvent(otherPlayer);
            }

            public delegate void PlayerPropertiesUpdate(PunPlayer targetPlayer, ExitGames.Client.Photon.Hashtable changedProps);
            public event PlayerPropertiesUpdate PlayerPropertiesUpdateEvent;
            public void OnPlayerPropertiesUpdate(PunPlayer targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
            {
                if (PlayerPropertiesUpdateEvent != null) PlayerPropertiesUpdateEvent(targetPlayer, changedProps);
            }

            public event Action<ExitGames.Client.Photon.Hashtable> PropertiesUpdateEvent;
            public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
            {
                if (PropertiesUpdateEvent != null) PropertiesUpdateEvent(propertiesThatChanged);
            }

            public override void Clear()
            {
                MasterClientChangedEvent = null;
                PlayerEnteredEvent = null;
                PlayerLeftEvent = null;
                PlayerPropertiesUpdateEvent = null;
                PropertiesUpdateEvent = null;
            }
        }

        public MatchmakingCallbacks Matchmaking { get; protected set; }
        public class MatchmakingCallbacks : CallbacksGroup, IMatchmakingCallbacks
        {
            public delegate void FailDelegate(short returnCode, string message);

            public event Action CreatedRoomEvent;
            public void OnCreatedRoom()
            {
                if (CreatedRoomEvent != null) CreatedRoomEvent();
            }

            public event FailDelegate CreateRoomFailedEvent;
            public void OnCreateRoomFailed(short returnCode, string message)
            {
                if (CreateRoomFailedEvent != null) CreateRoomFailedEvent(returnCode, message);
            }

            public void OnFriendListUpdate(List<FriendInfo> friendList)
            {

            }

            public event Action JoinedRoomEvent;
            public void OnJoinedRoom()
            {
                if (JoinedRoomEvent != null) JoinedRoomEvent();
            }

            public event FailDelegate JoinRandomRoomFailedEvent;
            public void OnJoinRandomFailed(short returnCode, string message)
            {
                if (JoinRandomRoomFailedEvent != null) JoinRandomRoomFailedEvent(returnCode, message);
            }

            public event FailDelegate JoinRoomFailedEvent;
            public void OnJoinRoomFailed(short returnCode, string message)
            {
                Debug.Log("join room failed " + message);
                if (JoinRoomFailedEvent != null) JoinRoomFailedEvent(returnCode, message);
            }

            public event Action LeftRoomEvent;
            public void OnLeftRoom()
            {
                if (LeftRoomEvent != null) LeftRoomEvent();
            }

            public override void Clear()
            {

                CreatedRoomEvent = null;
                CreateRoomFailedEvent = null;
                JoinedRoomEvent = null;
                JoinRandomRoomFailedEvent = null;
                JoinRoomFailedEvent = null;
                LeftRoomEvent = null;
            }
        }

        void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);

            Connection.Clear();
            Room.Clear();
            Matchmaking.Clear();
        }
    }
}