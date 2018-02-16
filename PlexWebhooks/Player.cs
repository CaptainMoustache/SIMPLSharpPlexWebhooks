using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace PlexWebhooks
{
    /// <summary>
    /// Information about the player that triggered the webhook
    /// </summary>
    public class Player
    {
        public bool Local
        { get; set; }

        public string PublicAddress
        { get; set; }

        public string Title
        { get; set; }

        public string Uuid
        { get; set; }
    }
}