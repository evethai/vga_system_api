using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.News;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        private readonly VgaDbContext _context;
        public NewsRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<News, bool>> filter, Func<IQueryable<News>, IOrderedQueryable<News>> orderBy) BuildFilterAndOrderBy(NewsSearchModel searchModel)
        {
            Expression<Func<News, bool>> filter = p => true;
            Func<IQueryable<News>, IOrderedQueryable<News>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.tilte))
            {
                filter = filter.And(p => p.Title.Contains(searchModel.tilte));
            }
            if (searchModel.UniversityId.HasValue)
            {
                filter = filter.And(p => p.UniversityId == searchModel.UniversityId.Value);
            }
            return (filter, orderBy);
        }

        public Task<Boolean> CreateImageNew(Guid NewsId, List<ImageNewsModel> imageNews)
        {
            var _newsId =  _context.News.Where(s=> s.Id.Equals(NewsId)).FirstOrDefault();
            if (_newsId == null)
            {
                return Task.FromResult(false);
            }
            foreach (var image in imageNews)
            {
                ImageNews img = new ImageNews
                {
                    NewsId = NewsId,
                    DescriptionTitle = image.DescriptionTitle,
                    ImageUrl = image.ImageUrl,
                    Status = true
                };
                _context.ImageNews.Add(img);             
            }
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> DeleteImageNew(Guid NewsId)
        {
            var exitNewsId = _context.ImageNews.Where(s => s.NewsId.Equals(NewsId)).FirstOrDefault();
            if (exitNewsId == null)
            {
                return Task.FromResult(false);
            }
            _context.ImageNews.Remove(exitNewsId);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> UpdateImageNew(ImageNewsModel imageNews)
        {
            var _imgNews = _context.ImageNews.Where(s => s.Id == imageNews.Id).FirstOrDefault();
            if (_imgNews == null)
            {
                return Task.FromResult(false);
            }
            _imgNews.DescriptionTitle = imageNews.DescriptionTitle;
            _imgNews.ImageUrl = imageNews.ImageUrl;
            _context.ImageNews.Update(_imgNews);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
