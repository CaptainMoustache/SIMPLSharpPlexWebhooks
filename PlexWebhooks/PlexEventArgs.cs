using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharp;

namespace PlexWebhooks
{
    public class PlexEventArgs : EventArgs
    {
        public PlexEventArgs()
        {
            PlexAccount = new Account();
            PlexServer = new Server();
            PlexPlayer = new Player();
            PlexMetadata = new Metadata();
        }

        public MediaEvents MediaEvent
        { get; set; }

        public LibrarySectionType LibrarySection
        { get; set; }

        public bool User
        { get; set; }

        public bool Owner
        { get; set; }

        public Account PlexAccount
        { get; set; }

        public Server PlexServer
        { get; set; }

        public Player PlexPlayer
        { get; set; }

        public Metadata PlexMetadata
        { get; set; }
    }
}