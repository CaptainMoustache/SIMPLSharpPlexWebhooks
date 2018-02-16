using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace PlexWebhooks
{
    /// <summary>
    /// Information about the account that triggered the webhook
    /// </summary>
    public class Account
    {
        public int Id
        { get; set; }

        public string ThumbUrl
        { get; set; }

        public string Title
        { get; set; }
    }
}