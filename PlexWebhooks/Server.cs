using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace PlexWebhooks
{
    /// <summary>
    /// Information about the Plex server that tiggered the webhook
    /// </summary>
    public class Server
    {
        public string Title
        { get; set; }

        public string Uuid
        { get; set; }
    }
}