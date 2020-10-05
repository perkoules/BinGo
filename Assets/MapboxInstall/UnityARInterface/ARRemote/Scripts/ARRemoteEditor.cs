using UnityEngine;

#if UNITY_EDITOR

using UnityEngine.Networking.PlayerConnection;
using UnityEditor.Networking.PlayerConnection;

#endif

namespace UnityARInterface
{
    public class ARRemoteEditor : ARController
    {
        [SerializeField]
        private bool m_SendVideo;

#if UNITY_EDITOR

        public bool sendVideo
        {
            get { return m_SendVideo; }
            set
            {
                m_SendVideo = value;

                if (m_RemoteInterface != null)
                    m_RemoteInterface.sendVideo = value;
            }
        }

        public override bool BackgroundRendering
        {
            get { return base.BackgroundRendering; }

            set
            {
                if (m_RemoteInterface != null)
                {
                    m_RemoteInterface.BackgroundRendering = base.BackgroundRendering = value;
                }
            }
        }

        private ARRemoteEditorInterface m_RemoteInterface;
        private EditorConnection m_EditorConnection;

        private void SetConnection(EditorConnection editorConnection)
        {
            m_EditorConnection = editorConnection;
            if (m_RemoteInterface != null)
                m_RemoteInterface.editorConnection = editorConnection;
        }

        private void DestroyConnection()
        {
            // Disconnect all players and destroy the connection object
            var connection = EditorConnection.instance;
            connection.DisconnectAll();
            connection.Unregister(ARMessageIds.frame, FrameMessageHandler);
            connection.Unregister(ARMessageIds.addPlane, PlaneAddedMessageHandler);
            connection.Unregister(ARMessageIds.updatePlane, PlaneUpdatedMessageHandler);
            connection.Unregister(ARMessageIds.removePlane, PlaneRemovedMessageHandler);
            connection.Unregister(ARMessageIds.screenCaptureY, ScreenCaptureYMessageHandler);
            connection.Unregister(ARMessageIds.screenCaptureUV, ScreenCaptureUVMessageHandler);
            connection.Unregister(ARMessageIds.screenCaptureParams, ScreenCaptureParamsMessageHandler);
            connection.Unregister(ARMessageIds.pointCloud, PointCloudMessageHandler);
            connection.Unregister(ARMessageIds.lightEstimate, LightEstimateMessageHandler);
            DestroyImmediate(connection);
            SetConnection(null);
        }

        private void SetupConnection()
        {
            // This forces a the connection to be reconstructed
            SetConnection(EditorConnection.instance);
            m_EditorConnection.Initialize();
            m_EditorConnection.RegisterConnection(PlayerConnectedEventHandler);
            m_EditorConnection.RegisterDisconnection(PlayerDisconnectedEventHandler);

            // These EditorConnection callbacks can only be on UnityEngine.Objects
            // so register them here and just forward all messages to the ARRemoteEditorInterface
            m_EditorConnection.Register(ARMessageIds.frame, FrameMessageHandler);
            m_EditorConnection.Register(ARMessageIds.addPlane, PlaneAddedMessageHandler);
            m_EditorConnection.Register(ARMessageIds.updatePlane, PlaneUpdatedMessageHandler);
            m_EditorConnection.Register(ARMessageIds.removePlane, PlaneRemovedMessageHandler);
            m_EditorConnection.Register(ARMessageIds.screenCaptureY, ScreenCaptureYMessageHandler);
            m_EditorConnection.Register(ARMessageIds.screenCaptureUV, ScreenCaptureUVMessageHandler);
            m_EditorConnection.Register(ARMessageIds.screenCaptureParams, ScreenCaptureParamsMessageHandler);
            m_EditorConnection.Register(ARMessageIds.pointCloud, PointCloudMessageHandler);
            m_EditorConnection.Register(ARMessageIds.lightEstimate, LightEstimateMessageHandler);
        }

        private void Start()
        {
            SetupConnection();
        }

        private void ScreenCaptureParamsMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.ScreenCaptureParamsMessageHandler(message);
        }

        private void ScreenCaptureUVMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.ScreenCaptureUVMessageHandler(message);
        }

        private void ScreenCaptureYMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.ScreenCaptureYMessageHandler(message);
        }

        private void PlaneAddedMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.PlaneAddedMessageHandler(message);
        }

        private void PlaneUpdatedMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.PlaneUpdatedMessageHandler(message);
        }

        private void PlaneRemovedMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.PlaneRemovedMessageHandler(message);
        }

        private void FrameMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.FrameMessageHandler(message);
        }

        private void PointCloudMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.PointCloudMessageHandler(message);
        }

        private void LightEstimateMessageHandler(MessageEventArgs message)
        {
            m_RemoteInterface.LightEstimateMessageHandler(message);
        }

        private void PlayerConnectedEventHandler(int playerId)
        {
            m_RemoteInterface.PlayerConnectedMessageHandler(m_EditorConnection, playerId);
        }

        private void PlayerDisconnectedEventHandler(int playerId)
        {
            m_RemoteInterface.PlayerDisconnectedMessageHandler(playerId);
        }

        private void OnGUI()
        {
            string message;
            if (m_RemoteInterface.connected)
            {
                message = string.Format("Connected to remote AR device: {0}", m_RemoteInterface.playerId);
                var buttonRect = new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 200, 400, 100);

                if (m_RemoteInterface.IsRemoteServiceRunning)
                {
                    if (GUI.Button(buttonRect, "Stop Remote AR Session"))
                        m_RemoteInterface.StopRemoteService();
                }
                else
                {
                    if (GUI.Button(buttonRect, "Start Remote AR Session"))
                        m_RemoteInterface.StartRemoteService(GetSettings());
                }
            }
            else
            {
                message = "Please connect to remote Player in the console menu.";
            }

            var boxRect = new Rect((Screen.width / 2) - 200, (Screen.height / 2) + 100, 400, 50);
            GUI.Box(boxRect, message);

            if (GUI.Button(new Rect((Screen.width / 2) - 200, Screen.height - 50, 100, 50), "Disconnect"))
            {
                m_RemoteInterface.StopRemoteService();
                m_EditorConnection.DisconnectAll();
            }
        }

        protected override void SetupARInterface()
        {
            m_RemoteInterface = new ARRemoteEditorInterface();
            m_RemoteInterface.editorConnection = m_EditorConnection;
            m_ARInterface = m_RemoteInterface;
            ARInterface.SetInterface(m_RemoteInterface);
            m_RemoteInterface.sendVideo = sendVideo;
            m_RemoteInterface.BackgroundRendering = BackgroundRendering;
        }

        private void OnDisable()
        {
            m_EditorConnection.DisconnectAll();
        }

#endif
    }
}