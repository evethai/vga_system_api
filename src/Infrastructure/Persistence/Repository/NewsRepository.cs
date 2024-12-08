using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface;
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

        public Task<Boolean> CreateImageNews(Guid NewsId, List<ImageNewsPostModel> imageNews)
        {
            var _newsId =  _context.News.Where(s=> s.Id.Equals(NewsId)).FirstOrDefault() ?? throw new Exception("Id is not found");           
            foreach (var image in imageNews)
            {
                ImageNews img = new ImageNews
                {
                    NewsId = NewsId,
                    DescriptionTitle = image.DescriptionTitle,
                    ImageUrl = image.ImageUrl,                   
                };
                _context.ImageNews.Add(img);                
            }
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAllImageNews(Guid NewId)
        {
            var exitNewsImageId = _context.ImageNews.Where(s => s.NewsId.Equals(NewId)).FirstOrDefault();
            _context.ImageNews.Remove(exitNewsImageId);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> DeleteOneImageNews(int id)
        {
            var exitNewsImageId = _context.ImageNews.Where(s => s.Id.Equals(id)).FirstOrDefault() ?? throw new Exception("Id is not found");           
            _context.ImageNews.Remove(exitNewsImageId);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

       public NewsModel HashTagNews(Guid NewsId)
        {
            var ExitNews = _context.News.Where(a => a.Id.Equals(NewsId)).Include(a => a.ImageNews)
                .Include(a => a.University).ThenInclude(a => a.Account).FirstOrDefault() ?? throw new Exception("Id is not found");
            List<HashTag> tagsValue = new List<HashTag>();
            if (ExitNews.Hashtag != null)
            {
                List<string> tagsKey = new List<string>(ExitNews.Hashtag.Split(','));
                foreach (var tag in tagsKey)
                {
                    var NameMajor = _context.Major.Where(s => s.Id.Equals(Guid.Parse(tag))).FirstOrDefault() ?? throw new Exception("Major Id is not found");
                    HashTag hashTag = new HashTag
                    {
                        Keys = tag,
                        Values = NameMajor.Name,
                    };
                    tagsValue.Add(hashTag);
                }
            }
            else
            {
                tagsValue = null;
            }
            NewsModel newsModel = new NewsModel
            {
                Id = NewsId,
                UniversityId = ExitNews.UniversityId,
                UniversityName = ExitNews.University.Account.Name,
                UniversityImageUrl = ExitNews.University.Account.Image_Url,
                Title = ExitNews.Title,
                Content = ExitNews.Content,
                CreatedAt = ExitNews.CreatedAt,
                _HashTag = tagsValue,
            };
            return newsModel;
        }

        public Task<bool> UpdateImageNews(ImageNewsPutModel imageNews, int id)
        {
            var _imgNews = _context.ImageNews.Where(s => s.Id == id).FirstOrDefault() ?? throw new Exception("Id is not found");          
            _imgNews.DescriptionTitle = imageNews.DescriptionTitle;
            _imgNews.ImageUrl = imageNews.ImageUrl;
            _context.ImageNews.Update(_imgNews);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
