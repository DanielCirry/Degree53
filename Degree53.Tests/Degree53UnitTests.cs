using Degree53.Controllers;
using Degree53.DataLayer.Contracts;
using Degree53.DataLayer.DbContexts;
using Degree53.DataLayer.Entities;
using Degree53.DataLayer.Repositories;
using Degree53.Domain.Contracts;
using Degree53.Domain.Models;
using Degree53.Domain.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Degree53.Tests
{
    public class Degree53UnitTests
    {      
        [Fact]
        public async void GetPostsAsync_HasData_RetunsCorrectData()
        {
            //Given
            var postDetail1 = new PostDetail
            {
                Id = 1,
                NumbersOfViews = 1,
                CreationDate = new DateTimeOffset(),
                PostId = 1
            };

            var post1 = new Post
            {
                Id = 1,
                Title = "This is a nice Title",
                Content = "Some awesome Content here",
                PostDetail = postDetail1
            };

            var postDetail2 = new PostDetail
            {
                Id = 2,
                NumbersOfViews = 2,
                CreationDate = new DateTimeOffset(),
                PostId = 2
            };

            var post2 = new Post
            {
                Id = 2,
                Title = "This is a nice Title",
                Content = "Some awesome Content here",
                PostDetail = postDetail2
            };

            var repository = MakeRepository();
            A.CallTo(() => repository.GetPostsAsync()).Returns(new List<Post> {post1, post2});
            var service = MakeService(repository);
            
            //When
            var result = await service.GetPostsAsync();

            //hen
            Assert.Contains(result, p => p.Id == post1.Id || p.Id == post2.Id);
            Assert.Contains(result, p => p.Title == post1.Title || p.Title == post2.Title);
            Assert.Contains(result, p => p.Content == post1.Content || p.Content == post2.Content);
            Assert.Contains(result, p => p.PostDetail.Id == post1.PostDetail.Id || p.PostDetail.Id == post2.PostDetail.Id);
            Assert.Contains(result, p => p.PostDetail.PostId == post1.PostDetail.PostId || p.PostDetail.PostId == post2.PostDetail.PostId);
            Assert.Contains(result, p => p.PostDetail.CreationDate == post1.PostDetail.CreationDate || p.PostDetail.CreationDate == post2.PostDetail.CreationDate);
            Assert.Contains(result, p => p.PostDetail.NumberOfViews == post1.PostDetail.NumbersOfViews || p.PostDetail.NumberOfViews == post2.PostDetail.NumbersOfViews);
        }

        [Fact]
        public async void GetPostsAsync_NoData_RetunsNull()
        {
            //Given
            List<Post> returnValue = null;

            var repository = MakeRepository();
            A.CallTo(() => repository.GetPostsAsync()).Returns(returnValue);
            var service = MakeService(repository);

            //When
            var result = await service.GetPostsAsync();

            //hen
            Assert.Null(result);
        }

        [Fact]
        public async void GetPostAsync_HasData_RetunsData()
        {
            //Given
            var postDetail1 = new PostDetail
            {
                Id = 1,
                NumbersOfViews = 1,
                CreationDate = new DateTimeOffset(),
                PostId = 1
            };

            var post1 = new Post
            {
                Id = 1,
                Title = "This is a nice Title",
                Content = "Some awesome Content here",
                PostDetail = postDetail1
            };

            var repository = MakeRepository();
            A.CallTo(() => repository.GetPostAsync(1)).Returns(post1);
            var service = MakeService(repository);

            //When
            var result = await service.GetPostAsync(1);

            //hen
            Assert.Equal(post1.Id, result.Id);
            Assert.Equal(post1.Title,result.Title);
            Assert.Equal(post1.Content, result.Content);
            Assert.Equal(post1.PostDetail.Id, result.PostDetail.Id);
            Assert.Equal(post1.PostDetail.PostId, result.PostDetail.PostId);
            Assert.Equal(post1.PostDetail.CreationDate, result.PostDetail.CreationDate);
            Assert.Equal(post1.PostDetail.NumbersOfViews, result.PostDetail.NumberOfViews);
        }

        [Fact]
        public async void GetPostAsync_NoData_RetunsNull()
        {
            //Given
            Post returnValue = null;

            var repository = MakeRepository();
            A.CallTo(() => repository.GetPostAsync(1)).Returns(returnValue);
            var service = MakeService(repository);

            //When
            var result = await service.GetPostAsync(1);

            //hen
            Assert.Null(result);
        }

        [Fact]
        public async void AddPostAsync_ValidData_CreatesData()
        {
            //Given
            var postDetail1 = new PostDetail
            {
                Id = 0,
                NumbersOfViews = 1,
                CreationDate = new DateTimeOffset(),
                PostId = 1
            };

            var post1 = new Post
            {
                Id = 0,
                Title = "This is a nice Title",
                Content = "Some awesome Content here",
                PostDetail = postDetail1
            };

            var model = MakePostModel(post1);

            var repository = MakeRepository();
            A.CallTo(() => repository.AddPostAsync(post1));
            var service = MakeService(repository);

            //When
            await service.AddPostAsync(model);

            //hen
            A.CallTo(() => repository.GetPostAsync(0)).Returns(post1);
            var result = await service.GetPostAsync(0);
            Assert.Equal(post1.Id, result.Id);
            Assert.Equal(post1.Title, result.Title);
            Assert.Equal(post1.Content, result.Content);
            Assert.Equal(post1.PostDetail.Id, result.PostDetail.Id);
            Assert.Equal(post1.PostDetail.PostId, result.PostDetail.PostId);
            Assert.Equal(post1.PostDetail.CreationDate, result.PostDetail.CreationDate);
            Assert.Equal(post1.PostDetail.NumbersOfViews, result.PostDetail.NumberOfViews);
        }

        [Fact]
        public async void AddPostAsync_InvalidData_DoesntCreateData()
        {
            //Given
            List<Post> returnValue = null;
            PostModel postModel = null;

            var repository = MakeRepository();
            A.CallTo(() => repository.GetPostsAsync()).Returns(returnValue);
            var service = MakeService(repository);

            //When
            await service.AddPostAsync(postModel);
            var result = await service.GetPostsAsync();

            //hen
            Assert.Null(result);
        }


        #region Test Helpers

        private static IDegree53Repository MakeRepository()
        {
            return A.Fake<IDegree53Repository>();
        }

        private static Degree53Service MakeService(IDegree53Repository repository)
        {
            return new Degree53Service(repository);
        }

        private static PostModel MakePostModel(Post post)
        {
            return (PostModel)MakePost(post);
        }

        private static Post MakePost(Post post)
        {
            return new Post
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                PostDetail = post.PostDetail
            };
        }

        #endregion
    }
}
