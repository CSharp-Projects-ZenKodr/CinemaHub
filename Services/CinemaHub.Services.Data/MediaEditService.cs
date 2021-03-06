﻿namespace CinemaHub.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CinemaHub.Common;
    using CinemaHub.Data.Common.Repositories;
    using CinemaHub.Data.Models;
    using CinemaHub.Services.Mapping;
    using CinemaHub.Web.ViewModels.Media;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class MediaEditService : IMediaEditService
    {
        private readonly IDeletableEntityRepository<MediaEdit> mediaEditRepo;

        public MediaEditService(IDeletableEntityRepository<MediaEdit> mediaEditRepo)
        {
            this.mediaEditRepo = mediaEditRepo;
        }

        public async Task ApplyEditForApproval(MediaDetailsInputModel edit, string userId, string rootPath)
        {
            var mediaEdit = new MediaEdit()
            {
                Title = edit.Title,
                MediaId = edit.Id,
                Overview = edit.Overview,
                Language = edit.Language,
                IsDetailFull = true,
                ReleaseDate = edit.ReleaseDate,
                Runtime = edit.Runtime,
                Budget = edit.Budget,
                YoutubeTrailerUrl = edit.YoutubeTrailerUrl,
                KeywordsJson = edit.Keywords,
                Genres = edit.Genres,
                CreatorId = userId,
            };

            var image = edit.PosterImageFile;
            if (image != null)
            {
                var tempPath = "\\images\\temporary\\";
                var path = rootPath = tempPath;

                var name = "tempPoster-" + mediaEdit.Id;
                var imageExtension = await FileDownloader.DownloadImage(image, path, name);
                mediaEdit.PosterPath = path + $"{name}.{imageExtension}";
            }
            else
            {
                mediaEdit.PosterPath = edit.PosterPath;
            }

            await this.mediaEditRepo.AddAsync(mediaEdit);
            await this.mediaEditRepo.SaveChangesAsync();
        }

        public async Task<T> GetEditForApproval<T>(string editId)
        {
            var result = await this.mediaEditRepo.AllAsNoTracking()
                .Where(x => x.Id == editId)
                .To<T>()
                .FirstOrDefaultAsync();

            if (result is null)
            {
                throw new Exception($"Edit \"{editId}\" does not exist");
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetEditsForApproval<T>(int page, int count)
        {
            int pagination = (page - 1) * count;
            var results = await this.mediaEditRepo.AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Skip(pagination)
                .Take(count)
                .To<T>()
                .ToListAsync();

            return results;
        }

        public async Task<int> GetEditsForApprovalCount()
        {
            return await this.mediaEditRepo.AllAsNoTracking().CountAsync();
        }

        public async Task DeleteEdit(string editId)
        {
            return;
        }
    }
}
