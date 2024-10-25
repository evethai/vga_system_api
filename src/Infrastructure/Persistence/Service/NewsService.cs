﻿using System;
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
            var news = _mapper.Map<News>(postModel);
            var result = await _unitOfWork.NewsRepository.AddAsync(news);
            await _unitOfWork.SaveChangesAsync();
            var img = await _unitOfWork.NewsRepository.CreateImageNews(news.Id, postModel.ImageNews);
            if(img == false)
            {
                return new ResponseModel
                {
                    Message = "Create News is fail",
                    IsSuccess = false
                };
            } 
            return new ResponseModel
            {
                Message = "Create News is Successfully",
                IsSuccess = true,
                Data = result
            };
        }

        public async Task<ResponseModel> DeleteNewsAsync(Guid Id)
        {
           var exitNews = await _unitOfWork.NewsRepository.GetByIdGuidAsync(Id);
            if (exitNews == null)
            {
                return new ResponseModel
                {
                    Message = "New Id is not found",
                    IsSuccess = false,
                };
            }
            await _unitOfWork.NewsRepository.DeleteAllImageNews(exitNews.Id);
            await _unitOfWork.NewsRepository.DeleteAsync(exitNews);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Delete News is Successfully",
                IsSuccess = true,
                Data = exitNews
            };
        }

        public async Task<ResponseNewsModel> GetListNewsAsync(NewsSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.NewsRepository.BuildFilterAndOrderBy(searchModel);
            var news = await _unitOfWork.NewsRepository
                .GetBySearchAsync(filter, orderBy,
                q => q.Include(s => s.ImageNews),
                pageIndex: searchModel.currentPage,
                pageSize: searchModel.pageSize);
            var total = await _unitOfWork.NewsRepository.CountAsync(filter);
            var listNews = _mapper.Map<List<NewsModel>>(news);
            return new ResponseNewsModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                _news = listNews,
            };
        }

        public async Task<NewsModel> GetNewsByIdAsync(Guid NewsId)
        {

            var news = await _unitOfWork.NewsRepository.
                SingleOrDefaultAsync(predicate: c => c.Id.Equals(NewsId), include: c => c.Include(c => c.ImageNews))
                ?? throw new NotExistsException();
            return _mapper.Map<NewsModel>(news);
        }

        public async Task<ResponseModel> UpdateNewsAsync(NewsPutModel putModel, Guid Id)
        {
            var exit = await _unitOfWork.NewsRepository.SingleOrDefaultAsync(predicate: c => c.Id.Equals(Id), include: c => c.Include(c => c.ImageNews));
            if (exit ==null)
            {
                return new ResponseModel
                {
                    Message = "NewsId is not found",
                    IsSuccess = false
                };
            }
            exit.Title = putModel.Title;
            exit.Content = putModel.Content;
            await _unitOfWork.NewsRepository.UpdateAsync(exit);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Update news is Successfully",
                IsSuccess = true,
                Data = exit
            }; 
        }

        public async Task<ResponseModel> CreateImageNewsAsync(Guid NewsId, List<ImageNewsPostModel> imageNews)
        {
            var result = await _unitOfWork.NewsRepository.CreateImageNews(NewsId, imageNews);
            if (result == false)
            {
                return new ResponseModel
                {
                    Message = "News Id is not found",
                    IsSuccess = false
                };
            }
            return new ResponseModel
            {
                Message = "Create Image news is Successfully",
                IsSuccess = true,
            };
        }

        public async Task<ResponseModel> DeleteImageNewsAsync(int Id)
        {
            var result = await _unitOfWork.NewsRepository.DeleteOneImageNews(Id);
            if (result == false)
            {
                return new ResponseModel
                {
                    Message = "ImageNews Id is not found",
                    IsSuccess = false
                };
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
                return new ResponseModel
                {
                    Message = "ImageNews Id is not found",
                    IsSuccess = false
                };
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