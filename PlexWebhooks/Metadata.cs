using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace PlexWebhooks
{
    /// <summary>
    /// Metadata information about the media
    /// </summary>
    public class Metadata
    {
        public Metadata()
        {
            DirectorList = new List<WriterDirectorInfo>();
            WriterList = new List<WriterDirectorInfo>();
        }

        public class WriterDirectorInfo
        {
            public int Id
            { get; set; }

            public string Filter
            { get; set; }

            public string Tag
            { get; set; }
        }

        public List<WriterDirectorInfo> DirectorList
        { get;set; }

        public List<WriterDirectorInfo> WriterList
        { get; set; }

        public string LibrarySectionType
        { get; set; }

        public string RatingKey
        { get; set; }

        public string Key
        { get; set; }

        public string PartentRatingKey
        { get; set; }

        public string GrandParentRatingKey
        { get; set; }

        public string Guid
        { get; set; }

        public int LibrarySectionId
        { get; set; }

        public string LibrarySectionKey
        { get; set; }

        public string Type
        { get; set; }

        public string Title
        { get; set; }

        public string GrandparentKey
        { get; set; }

        public string ParentKey
        { get; set; }

        public string GrandparentTitle
        { get; set; }

        public string ParentTitle
        { get; set; }

        public string ContentRating
        { get; set; }

        public string Summary
        { get; set; }

        public int Index
        { get; set; }

        public int ParentIndex
        { get; set; }

        public float Rating
        { get; set; }

        public int ViewOffset
        { get; set; }

        public int LastViewedAt
        { get; set; }

        public int Year
        { get; set; }

        public string Thumb
        { get; set; }

        public string Art
        { get; set; }

        public string ParentThumb
        { get; set; }

        public string GrandparentThumb
        { get; set; }

        public string GrandparentArt
        { get; set; }

        public string GrandparentTheme
        { get; set; }

        public string OriginallyAvailableAt
        { get; set; }

        public int AddedAt
        { get; set; }

        public int UpdatedAt
        { get; set; }

        public string ChapterSource
        { get; set; }
    }
}