using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SocketHttpListener.Net
{
    public class HttpListenerPrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
    {
        List<string> prefixes = new List<string>();
        HttpListener listener;

        private ILogger _logger;

        internal HttpListenerPrefixCollection(ILogger logger, HttpListener listener)
        {
            _logger = logger;
            this.listener = listener;
        }

        public int Count => prefixes.Count;

        public bool IsReadOnly => false;

        public bool IsSynchronized => false;

        public void Add(string uriPrefix)
        {
            listener.CheckDisposed();
            //ListenerPrefix.CheckUri(uriPrefix);
            if (prefixes.Contains(uriPrefix))
                return;

            prefixes.Add(uriPrefix);
            if (listener.IsListening)
                HttpEndPointManager.AddPrefix(_logger, uriPrefix, listener);
        }

        public void Clear()
        {
            listener.CheckDisposed();
            prefixes.Clear();
            if (listener.IsListening)
                HttpEndPointManager.RemoveListener(_logger, listener);
        }

        public bool Contains(string uriPrefix)
        {
            listener.CheckDisposed();
            return prefixes.Contains(uriPrefix);
        }

        public void CopyTo(string[] array, int offset)
        {
            listener.CheckDisposed();
            prefixes.CopyTo(array, offset);
        }

        public void CopyTo(Array array, int offset)
        {
            listener.CheckDisposed();
            ((ICollection)prefixes).CopyTo(array, offset);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return prefixes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return prefixes.GetEnumerator();
        }

        public bool Remove(string uriPrefix)
        {
            listener.CheckDisposed();
            if (uriPrefix == null)
                throw new ArgumentNullException(nameof(uriPrefix));

            bool result = prefixes.Remove(uriPrefix);
            if (result && listener.IsListening)
                HttpEndPointManager.RemovePrefix(_logger, uriPrefix, listener);

            return result;
        }
    }
}
