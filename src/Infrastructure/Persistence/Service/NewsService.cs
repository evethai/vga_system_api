using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.News;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateNewsAsync(NewsPostModel postModel)
        {
            var news = await _unitOfWork.NewsRepository.HashTagNotification(postModel);
            return news;
        }

        public async Task<ResponseModel> DeleteNewsAsync(Guid Id)
        {
           var exitNews = await _unitOfWork.NewsRepository.GetByIdGuidAsync(Id) ?? throw new Exception("Id is not found");
            await _unitOfWork.NewsRepository.DeleteAllImageNews(exitNews.Id);
            await _unitOfWork.NewsRepository.DeleteAsync(exitNews);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Delete News is Successfully",
                IsSuccess = true
            };
        }

        public async Task<ResponseNewsModel> GetListNewsAsync(NewsSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.NewsRepository.BuildFilterAndOrderBy(searchModel);
            var news = await _unitOfWork.NewsRepository
                .GetBySearchAsync(filter, orderBy,
                q => q.Include(s => s.ImageNews)
                .Include(s=>s.University).
                ThenInclude(s=>s.Account),
                pageIndex: searchModel.currentPage,
                pageSize: searchModel.pageSize);          
            var total = await _unitOfWork.NewsRepository.CountAsync(filter);
            List<NewsModel> listNews = new List<NewsModel>(); 
            foreach (var item in news)
            {
                var HashTagValues = _unitOfWork.NewsRepository.HashTagNews(item.Id);
                listNews.Add(HashTagValues);
            }
            return new ResponseNewsModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                _news = listNews,
            };
        }

        public Task<NewsModel> GetNewsByIdAsync(Guid NewsId)
        {
            var HashTagValues = _unitOfWork.NewsRepository.HashTagNews(NewsId);
            return Task.FromResult(HashTagValues);
        }
        public async Task<ResponseModel> UpdateNewsAsync(NewsPutModel putModel, Guid Id)
        {
            var exit = await _unitOfWork.NewsRepository.SingleOrDefaultAsync(predicate: c => c.Id.Equals(Id))
                ?? throw new Exception("Id is not found");
            exit.Title = putModel.Title;
            exit.Content = putModel.Content;
            exit.Hashtag = putModel.Hashtag;
            await _unitOfWork.NewsRepository.UpdateAsync(exit);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Update news is Successfully",
                IsSuccess = true,
                Data = putModel
            }; 
        }
        public async Task<ResponseModel> CreateImageNewsAsync(Guid NewsId, List<ImageNewsPostModel> imageNews)
        {
            var result = await _unitOfWork.NewsRepository.CreateImageNews(NewsId, imageNews) ;
            if (result == false)
            {
                throw new Exception("Id is not found");
            }
            return new ResponseModel
            {
                Message = "Create Image news is Successfully",
                IsSuccess = true,
                Data = imageNews
            };
        }

        public async Task<ResponseModel> DeleteImageNewsAsync(int Id)
        {
            var result = await _unitOfWork.NewsRepository.DeleteOneImageNews(Id);
            if (result == false)
            {
                throw new Exception("Id is not found");
            }
            return new ResponseModel
            {
                Message = "Delete Image news is Successfully",
                IsSuccess = true,
            };
        }

        public async Task<ResponseModel> UpdateImageNewsAsync(ImageNewsPutModel imageNewsModel, int id)
        {
            var result = await _unitOfWork.NewsRepository.UpdateImageNews(imageNewsModel, id);
            if (result == false)
            {
                throw new Exception("Id is not found");
            }
            return new ResponseModel
            {
                Message = "Update Image news is Successfully",
                IsSuccess = true,
                Data = imageNewsModel
            };
        }
    }
}
