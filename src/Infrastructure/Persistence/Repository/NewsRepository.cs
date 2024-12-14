using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Interface;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.News;
using Domain.Model.Response;
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
            List<ImageNewsModel> listImageNews = new List<ImageNewsModel>();
            foreach(var newImg in ExitNews.ImageNews)
            {
                ImageNewsModel img = new ImageNewsModel
                {
                    Id = newImg.Id,
                    DescriptionTitle = newImg.DescriptionTitle,
                    ImageUrl = newImg.ImageUrl,
                };
                listImageNews.Add(img);
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
                ImageNews = listImageNews
            };
            return newsModel;
        }
        public async Task<ResponseModel> HashTagNotification(NewsPostModel postModel)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var checkUniversity = _context.University.Where(s => s.Id.Equals(postModel.UniversityId)).FirstOrDefault();
                        if (postModel == null || checkUniversity == null)
                        { throw new Exception("Model is null or University is not found"); }

                        var StudentCare = new List<StudentChoice>();
                        if (!string.IsNullOrWhiteSpace(postModel.Hashtag))
                        {
                            List<string> tagsKey = new List<string>(postModel.Hashtag.Split(','));
                            foreach (string tagKey in tagsKey)
                            {
                                var NameMajor = _context.Major
                                    .FirstOrDefault(s => s.Id.Equals(Guid.Parse(tagKey)));

                                if (NameMajor == null)
                                {
                                    throw new Exception($"Major Id '{tagKey}' is not found.");
                                }
                                StudentCare = _context.StudentChoice
                                   .Where(s => s.isMajor == true
                                               && s.MajorOrOccupationId.Equals(NameMajor.Id)
                                               && s.Type == StudentChoiceType.Care)
                                   .ToList();
                            }
                        }
                        else { postModel.Hashtag = null; }
                        News news = new News
                        {
                            Title = postModel.Title,
                            Content = postModel.Content,
                            CreatedAt = DateTime.UtcNow.AddHours(7),
                            UniversityId = postModel.UniversityId,
                            Hashtag = postModel.Hashtag,
                        };
                        await _context.News.AddAsync(news);
                        await _context.SaveChangesAsync();
                        //----------
                        var _newsId = _context.News.Where(s => s.Id.Equals(news.Id)).FirstOrDefault() ?? throw new Exception("Id is not found");
                        if (postModel.ImageNews != null)
                        {
                            foreach (var image in postModel.ImageNews)
                            {
                                ImageNews img = new ImageNews
                                {
                                    NewsId = _newsId.Id,
                                    DescriptionTitle = image.DescriptionTitle,
                                    ImageUrl = image.ImageUrl,
                                };
                                await _context.ImageNews.AddAsync(img);
                            }
                            await _context.SaveChangesAsync();
                        }
                        //----------
                        if (StudentCare != null)
                        {
                            List<Notification> notifications = new List<Notification>();
                            foreach (var studentChoice in StudentCare)
                            {
                                var checkAccountId = _context.Student.
                                Where(s => s.Id.Equals(studentChoice.StudentId)).
                                FirstOrDefault() ?? throw new Exception("Account id is not found");
                                notifications.Add(new Notification
                                {
                                    AccountId = checkAccountId.AccountId,
                                    CreatedAt = DateTime.UtcNow.AddHours(7),
                                    Status = Domain.Enum.NotiStatus.Unread,
                                    Message = "Tin tức |" + _newsId.Id,
                                    Title = NotificationConstant.Title.NewsNoti
                                });
                            }
                            await _context.Notification.AddRangeAsync(notifications);
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = "News created successfully.",
                            Data = news
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = ex.Message
                        };
                    }
                }
            });     
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
