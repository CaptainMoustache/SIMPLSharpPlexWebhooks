using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Crestron.SimplSharp;                          				// For Basic SIMPL# Classes
using Crestron.SimplSharp.Net.Http;


namespace PlexWebhooks
{

    public class WebHookListener
    {
        private HttpServer WebhookServer;
        private PlexEventArgs PlexPayload;

        //Regex match string to find JSON payload
        private string RegexString = @"\{(.|\s)*\}";

        /// <summary>
        /// Starts a HTTP server to listen for PLEX Webhooks
        /// </summary>
        /// <param name="port">Port to listen on</param>
        public WebHookListener(int port)
        {
            WebhookServer = new HttpServer();
            WebhookServer.Port = port;
            WebhookServer.CloseConnectionsOnShutdown = true;
            WebhookServer.Active = true;
            WebhookServer.OnHttpRequest += new OnHttpRequestHandler(HookServer_OnHttpRequest);
        }

        #region Webhook EventHandler

        public event EventHandler<PlexEventArgs> PlexMediaEvent;

        protected virtual void OnPlexMediaEvent(PlexEventArgs args)
        {
            EventHandler<PlexEventArgs> plexMediaEvent = PlexMediaEvent;
            if (PlexMediaEvent != null)
                PlexMediaEvent(this, args);
        }

        #endregion

        void HookServer_OnHttpRequest(object sender, OnHttpRequestArgs e)
        {
            if (e.Request.Header.RequestType == "POST")
            {
                //Match valid JSON data in HTTP response
                Match JsonMatch = Regex.Match(e.Request.ContentString, RegexString);
                //Instantiate new PlexEventArgs 
                PlexPayload = new PlexEventArgs();
                //Parse JSON data and populate PlexPayload
                ParseJson(JsonMatch.ToString());
                //Trigger PlexEvent
                OnPlexMediaEvent(PlexPayload);
            }
        }

        /// <summary>
        /// Sets properties of the PlexEventArgs object from a JSON string provided by a Plex Webhooks request
        /// </summary>
        /// <param name="data">JSON string</param>
        private void ParseJson(string data)
        {
            JObject JsonData = JObject.Parse(data);

            try
            {
                //Top level info
                PlexPayload.MediaEvent = ConvertMediaEvent(ProcessStringToken(JsonData["event"]));
                PlexPayload.User = ProcessBoolToken(JsonData["user"]);
                PlexPayload.Owner = ProcessBoolToken(JsonData["owner"]);

                //PlexAccount Info
                PlexPayload.PlexAccount.Id = ProcessIntToken(JsonData["Account"]["id"]);
                PlexPayload.PlexAccount.ThumbUrl = ProcessStringToken(JsonData["Account"]["thumb"]);
                PlexPayload.PlexAccount.Title = ProcessStringToken(JsonData["Account"]["title"]);

                //PlexServer Info
                PlexPayload.PlexServer.Title = ProcessStringToken(JsonData["Server"]["title"]);
                PlexPayload.PlexServer.Uuid = ProcessStringToken(JsonData["Server"]["uuid"]);

                //PlexPlayer Info
                PlexPayload.PlexPlayer.Local = ProcessBoolToken(JsonData["Player"]["local"]);
                PlexPayload.PlexPlayer.PublicAddress = ProcessStringToken(JsonData["Player"]["publicAddress"]);
                PlexPayload.PlexPlayer.Title = ProcessStringToken(JsonData["Player"]["title"]);
                PlexPayload.PlexPlayer.Uuid = ProcessStringToken(JsonData["Player"]["uuid"]);

                //PlexPayload.PlexMetadata
                PlexPayload.PlexMetadata.LibrarySectionType = ProcessStringToken(JsonData["Metadata"]["librarySectionType"]);
                PlexPayload.PlexMetadata.RatingKey = ProcessStringToken(JsonData["Metadata"]["ratingKey"]);
                PlexPayload.PlexMetadata.Key = ProcessStringToken(JsonData["Metadata"]["key"]);
                PlexPayload.PlexMetadata.PartentRatingKey = ProcessStringToken(JsonData["Metadata"]["parentRatingKey"]);
                PlexPayload.PlexMetadata.GrandParentRatingKey = ProcessStringToken(JsonData["Metadata"]["grandparentRatingKey"]);
                PlexPayload.PlexMetadata.Guid = ProcessStringToken(JsonData["Metadata"]["guid"]);
                PlexPayload.PlexMetadata.LibrarySectionId = ProcessIntToken(JsonData["Metadata"]["librarySectionID"]);
                PlexPayload.PlexMetadata.LibrarySectionKey = ProcessStringToken(JsonData["Metadata"]["librarySectionKey"]);
                PlexPayload.PlexMetadata.Type = ProcessStringToken(JsonData["Metadata"]["type"]);
                PlexPayload.PlexMetadata.Title = ProcessStringToken(JsonData["Metadata"]["title"]);
                PlexPayload.PlexMetadata.GrandparentKey = ProcessStringToken(JsonData["Metadata"]["grandparentKey"]);
                PlexPayload.PlexMetadata.ParentKey = ProcessStringToken(JsonData["Metadata"]["parentKey"]);
                PlexPayload.PlexMetadata.GrandparentTitle = ProcessStringToken(JsonData["Metadata"]["grandparentTitle"]);
                PlexPayload.PlexMetadata.ParentTitle = ProcessStringToken(JsonData["Metadata"]["parentTitle"]);
                PlexPayload.PlexMetadata.ContentRating = ProcessStringToken(JsonData["Metadata"]["contentRating"]);
                PlexPayload.PlexMetadata.Summary = ProcessStringToken(JsonData["Metadata"]["summary"]);
                PlexPayload.PlexMetadata.Index = ProcessIntToken(JsonData["Metadata"]["index"]);
                PlexPayload.PlexMetadata.ParentIndex = ProcessIntToken(JsonData["Metadata"]["parentIndex"]);
                PlexPayload.PlexMetadata.Rating = ProcessFloatToken(JsonData["Metadata"]["rating"]);
                PlexPayload.PlexMetadata.ViewOffset = ProcessIntToken(JsonData["Metadata"]["viewOffset"]);
                PlexPayload.PlexMetadata.LastViewedAt = ProcessIntToken(JsonData["Metadata"]["lastViewedAt"]);
                PlexPayload.PlexMetadata.Year = ProcessIntToken(JsonData["Metadata"]["year"]);
                PlexPayload.PlexMetadata.Thumb = ProcessStringToken(JsonData["Metadata"]["thumb"]);
                PlexPayload.PlexMetadata.Art = ProcessStringToken(JsonData["Metadata"]["art"]);
                PlexPayload.PlexMetadata.ParentThumb = ProcessStringToken(JsonData["Metadata"]["parentThumb"]);
                PlexPayload.PlexMetadata.GrandparentThumb = ProcessStringToken(JsonData["Metadata"]["grandparentThumb"]);
                PlexPayload.PlexMetadata.GrandparentArt = ProcessStringToken(JsonData["Metadata"]["grandparentArt"]);
                PlexPayload.PlexMetadata.GrandparentTheme = ProcessStringToken(JsonData["Metadata"]["grandparentTheme"]);
                PlexPayload.PlexMetadata.OriginallyAvailableAt = ProcessStringToken(JsonData["Metadata"]["originallyAvailableAt"]);
                PlexPayload.PlexMetadata.AddedAt = ProcessIntToken(JsonData["Metadata"]["addedAt"]);
                PlexPayload.PlexMetadata.UpdatedAt = ProcessIntToken(JsonData["Metadata"]["updatedAt"]);
                PlexPayload.PlexMetadata.ChapterSource = ProcessStringToken(JsonData["Metadata"]["chapterSource"]);
                //Find all listed directors
                JToken DirectorToken = JsonData["Metadata"]["Director"];
                if (!IsNull(DirectorToken) && DirectorToken.HasValues)
                {
                    foreach (var director in DirectorToken.ToList())
                    {
                        PlexPayload.PlexMetadata.DirectorList.Add(new Metadata.WriterDirectorInfo
                        {
                            Id = ProcessIntToken(director["id"]),
                            Filter = ProcessStringToken(director["filter"]),
                            Tag = ProcessStringToken(director["tag"])
                        });
                    }
                }
                //Find all listed wirters
                JToken WriterToken = JsonData["Metadata"]["Writer"];
                if (!IsNull(WriterToken) && WriterToken.HasValues)
                {
                    foreach (var writer in WriterToken.ToList())
                    {
                        PlexPayload.PlexMetadata.WriterList.Add(new Metadata.WriterDirectorInfo
                        {
                            Id = ProcessIntToken(writer["id"]),
                            Filter = ProcessStringToken(writer["filter"]),
                            Tag = ProcessStringToken(writer["tag"])
                        });
                    }
                }
            }
            catch (NullReferenceException nulle)
            {
                ErrorLog.Exception("Null reference exception reading JSON data: {0}", nulle);
            }
            catch (Exception e)
            {
                ErrorLog.Exception("Exception reading JSON data: {0}", e);
            }
        }

        #region JToken Processing Methods

        private bool ProcessBoolToken(JToken token)
        {
            if (IsNull(token))
                return false;
            else
                return token.ToObject<bool>();
        }

        private int ProcessIntToken(JToken token)
        {
            if (IsNull(token))
                return 0;
            else
                return token.ToObject<int>();
        }

        private string ProcessStringToken(JToken token)
        {
            if (IsNull(token))
                return "N/A";
            else
                return token.ToObject<string>();
        }

        private float ProcessFloatToken(JToken token)
        {
            if (IsNull(token))
                return 0;
            else
                return token.ToObject<float>();
        }

        /// <summary>
        /// Check for null JToken type
        /// </summary>
        /// <param name="jtoken">JToken to check for JTokenType.Null</param>
        /// <returns>true when toek is null</returns>
        private bool IsNull(JToken jtoken)
        {
            if (jtoken == null || jtoken.Type == JTokenType.Null)
                return true;
            else
                return false;
        }

        #endregion

        /// <summary>
        /// Returns equivalent MediaEvents from the string value of the JSON data
        /// </summary>
        /// <param name="plexEvent">String</param>
        /// <returns>MediaEvents</returns>
        private MediaEvents ConvertMediaEvent(string plexEvent)
        {
            //Media begin playback
            if (plexEvent == "media.play")
                return MediaEvents.Play;
            //Media paused
            else if (plexEvent == "media.pause")
                return MediaEvents.Pause;
            //Media resumed
            else if (plexEvent == "media.resume")
                return MediaEvents.Resume;
            //Media stopped
            else if (plexEvent == "media.stop")
                return MediaEvents.Stop;
            //Media passed 90% complete
            else if (plexEvent == "media.scrobble")
                return MediaEvents.Scrobble;
            //Media rated
            else if (plexEvent == "media.rate")
                return MediaEvents.Rate;
            else
                return MediaEvents.Unknown;
        }

        /// <summary>
        /// Returns equivalent LibrarySectionType from the string value of the JSON data
        /// </summary>
        /// <param name="sectionType">String</param>
        /// <returns>LibrarySectionType</returns>
        private LibrarySectionType ConvertLibrarySectionType(string sectionType)
        {
            if (sectionType == "movie")
                return LibrarySectionType.Movie;
            else if (sectionType == "show")
                return LibrarySectionType.Show;
            else if (sectionType == "season")
                return LibrarySectionType.Season;
            else if (sectionType == "eipsode")
                return LibrarySectionType.Episode;
            else if (sectionType == "trailer")
                return LibrarySectionType.Trailer;
            else if (sectionType == "comic")
                return LibrarySectionType.Comic;
            else if (sectionType == "person")
                return LibrarySectionType.Person;
            else if (sectionType == "artist")
                return LibrarySectionType.Artist;
            else if (sectionType == "album")
                return LibrarySectionType.Album;
            else if (sectionType == "track")
                return LibrarySectionType.Track;
            else if (sectionType == "photoAlbum")
                return LibrarySectionType.PhotoAlbum;
            else if (sectionType == "picture")
                return LibrarySectionType.Picture;
            else if (sectionType == "photo")
                return LibrarySectionType.Photo;
            else if (sectionType == "clip")
                return LibrarySectionType.Clip;
            else if (sectionType == "playlistItem")
                return LibrarySectionType.Playlist;
            else
                return LibrarySectionType.Unknown;
        }
    }

    /// <summary>
    /// Possible Media Events
    /// 
    /// Scrobble = Media playback > 90% complete
    /// </summary>
    public enum MediaEvents
    {
        Play,
        Pause,
        Resume,
        Stop,
        Scrobble,
        Rate,
        Unknown
    }

    /// <summary>
    /// Possible LibrarySection Types
    /// </summary>
    public enum LibrarySectionType
    {
        Movie,
        Show,
        Season,
        Episode,
        Trailer,
        Comic,
        Person,
        Artist,
        Album,
        Track,
        PhotoAlbum,
        Picture,
        Photo,
        Clip,
        Playlist,
        Unknown
    }
}

