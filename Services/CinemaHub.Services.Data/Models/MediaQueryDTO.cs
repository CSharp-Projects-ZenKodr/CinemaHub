﻿namespace CinemaHub.Services.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CinemaHub.Data.Models.Enums;

    public class MediaQueryDTO
    {
        public string MediaType { get; set; }

        public string SearchQuery { get; set; }

        public string Keywords { get; set; }

        public string SortType { get; set; }

        public int Page { get; set; }

        public string Genres { get; set; }

        public int ElementsPerPage { get; set; }

        public bool AreNotWatched { get; set; } = true;

        public string WatchType { get; set; }

        public string UserId { get; set; }
    }
}
