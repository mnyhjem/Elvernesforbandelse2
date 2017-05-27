using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using Elvencurse2.Model;
using Microsoft.AspNet.SignalR.Client;

namespace Elvencurse2.Client.Components
{
    public class SignalRComponent:IDisposable
    {
        private readonly string _realm;
        public ConcurrentQueue<Payload> UnhandledPayload { get; private set; }
        private IHubProxy _hubProxy;
        private HubConnection _connection;

        public event EventHandler LostConnection;

        public IHubProxy HubProxy
        {
            get { return _hubProxy; }
        }

        private void OnLostConnection()
        {
            LostConnection?.Invoke(this, new EventArgs());
        }

        public string ConnectionId { get; private set; }

        public SignalRComponent(string realm)
        {
            _realm = realm;
            UnhandledPayload = new ConcurrentQueue<Payload>();
        }

        public void Connect()
        {
            _connection = new HubConnection(_realm);
            _connection.CookieContainer = new CookieContainer();
            _connection.CookieContainer.Add(Authentication.AuthCookie);
            //_connection.Closed += _connection_Closed;
            //_connection.ConnectionSlow += _connection_ConnectionSlow;
            //_connection.Error += _connection_Error;
            //_connection.Received += _connection_Received;
            //_connection.Reconnected += _connection_Reconnected;
            _connection.StateChanged += _connection_StateChanged;

            _hubProxy = _connection.CreateHubProxy("ElvenHub");
            _hubProxy.On<DateTime>("Pong", (time) =>
            {
                var i = 0;
            });

            _hubProxy.On<dynamic>("PushPayload", (dynpayload) =>
            {
                Payload payload = new Payload(dynpayload);
                UnhandledPayload.Enqueue(payload);
            });

            _connection.Start().Wait();
            ConnectionId = _connection.ConnectionId;

            _hubProxy.Invoke("EnterWorld").Wait();

            //_hubProxy.Invoke("Ping");
        }

        private void _connection_StateChanged(StateChange obj)
        {
            Debug.WriteLine(string.Format("ConnectionStateChanged {0}", obj.NewState));
            if (obj.NewState == ConnectionState.Reconnecting || obj.NewState == ConnectionState.Disconnected)
            {
                OnLostConnection();
            }
        }

        private void _connection_Reconnected()
        {
            Debug.WriteLine("ConnectionReconnected");
        }

        private void _connection_Received(string obj)
        {
            //Debug.WriteLine("ConnectionReceived");
        }

        private void _connection_Error(Exception obj)
        {
            Debug.WriteLine(string.Format("ConnectionError {0}", obj.Message));
        }

        private void _connection_ConnectionSlow()
        {
            Debug.WriteLine("ConnectionSLow");
        }

        private void _connection_Closed()
        {
            Debug.WriteLine("ConnectionClosed");
        }

        public void Dispose()
        {
            Debug.WriteLine("Disposing");
            //if(_connection != null)
            //{
            //    _connection.Stop();

            //    _connection.Closed -= _connection_Closed;
            //    _connection.ConnectionSlow -= _connection_ConnectionSlow;
            //    _connection.Error -= _connection_Error;
            //    _connection.Received -= _connection_Received;
            //    _connection.Reconnected -= _connection_Reconnected;
            //    _connection.StateChanged -= _connection_StateChanged;
            //}
        }
    }
}
