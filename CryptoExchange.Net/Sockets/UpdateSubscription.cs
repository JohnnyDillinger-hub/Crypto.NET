﻿using CryptoExchange.Net.Objects;
using System;
using System.Threading.Tasks;

namespace CryptoExchange.Net.Sockets
{
    /// <summary>
    /// Subscription to a data stream
    /// </summary>
    public class UpdateSubscription
    {
        private readonly SocketConnection _connection;
        private readonly SocketSubscription _subscription;

        /// <summary>
        /// Event when the connection is lost. The socket will automatically reconnect when possible.
        /// </summary>
        public event Action ConnectionLost
        {
            add => _connection.ConnectionLost += value;
            remove => _connection.ConnectionLost -= value;
        }

        /// <summary>
        /// Event when the connection is closed and will not be reconnected
        /// </summary>
        public event Action ConnectionClosed
        {
            add => _connection.ConnectionClosed += value;
            remove => _connection.ConnectionClosed -= value;
        }

        /// <summary>
        /// Event when the connection is restored. Timespan parameter indicates the time the socket has been offline for before reconnecting. 
        /// Note that when the executing code is suspended and resumed at a later period (for example, a laptop going to sleep) the disconnect time will be incorrect as the diconnect
        /// will only be detected after resuming the code, so the initial disconnect time is lost. Use the timespan only for informational purposes.
        /// </summary>
        public event Action<TimeSpan> ConnectionRestored
        {
            add => _connection.ConnectionRestored += value;
            remove => _connection.ConnectionRestored -= value;
        }

        /// <summary>
        /// Event when the connection to the server is paused based on a server indication. No operations can be performed while paused
        /// </summary>
        public event Action ActivityPaused
        {
            add => _connection.ActivityPaused += value;
            remove => _connection.ActivityPaused -= value;
        }

        /// <summary>
        /// Event when the connection to the server is unpaused after being paused
        /// </summary>
        public event Action ActivityUnpaused
        {
            add => _connection.ActivityUnpaused += value;
            remove => _connection.ActivityUnpaused -= value;
        }

        /// <summary>
        /// Event when an exception happens during the handling of the data
        /// </summary>
        public event Action<Exception> Exception
        {
            add => _subscription.Exception += value;
            remove => _subscription.Exception -= value;
        }

        /// <summary>
        /// The id of the socket
        /// </summary>
        public int SocketId => _connection.SocketId;

        /// <summary>
        /// The id of the subscription
        /// </summary>
        public int Id => _subscription.Id;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="connection">The socket connection the subscription is on</param>
        /// <param name="subscription">The subscription</param>
        public UpdateSubscription(SocketConnection connection, SocketSubscription subscription)
        {
            this._connection = connection;
            this._subscription = subscription;
        }
        
        /// <summary>
        /// Close the subscription
        /// </summary>
        /// <returns></returns>
        public Task CloseAsync()
        {
            return _connection.CloseAsync(_subscription);
        }

        /// <summary>
        /// Close the socket to cause a reconnect
        /// </summary>
        /// <returns></returns>
        public Task ReconnectAsync()
        {
            return _connection.TriggerReconnectAsync();
        }

        /// <summary>
        /// Unsubscribe a subscription
        /// </summary>
        /// <returns></returns>
        internal async Task UnsubscribeAsync()
        {
            await _connection.UnsubscribeAsync(_subscription).ConfigureAwait(false);
        }

        /// <summary>
        /// Resubscribe this subscription
        /// </summary>
        /// <returns></returns>
        internal async Task<CallResult<bool>> ResubscribeAsync()
        {
            return await _connection.ResubscribeAsync(_subscription).ConfigureAwait(false);
        }
    }
}
