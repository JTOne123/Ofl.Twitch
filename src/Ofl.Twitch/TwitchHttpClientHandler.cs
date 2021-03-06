﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Net.Http;

namespace Ofl.Twitch
{
    public class TwitchHttpClientHandler : HttpClientHandler
    {
        #region Constructor

        public TwitchHttpClientHandler(IClientIdProvider clientIdProvider)
        {
            // Validate parameters.
            _clientIdProvider = clientIdProvider ?? throw new ArgumentNullException(nameof(clientIdProvider));

            // Set decompression.
            this.SetCompression();
        }

        #endregion

        #region Instance, read-only state

        private readonly IClientIdProvider _clientIdProvider;

        #endregion

        #region Overrides of HttpClientHandler

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken
        )
        {
            // Validate parameters.
            if (request == null) throw new ArgumentNullException(nameof(request));

            // The client ID header.
            const string clientIdHeaderKey = "Client-ID";

            // If the client ID is not set.
            if (!request.Headers.Contains(clientIdHeaderKey))
                // Add.
                request.Headers.Add(
                    clientIdHeaderKey,
                    _clientIdProvider.ClientId
                );

            // Call the base.
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}
