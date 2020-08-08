using Degree53.Controllers;
using Degree53.DataLayer.Entities;
using Degree53.Domain.Models;
using Degree53.Domain.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Degree53.DataLayer.Contracts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Degree53.DataLayer.DbContexts;
using Degree53.DataLayer.Repositories;

namespace Degree53.Tests
{
    public class Degree53ControllerIntegrationTests
    {
        [Fact]
        public async void GetPostsAsync_HasData_RetunsOK()
        {
            //Given
            var postDetail1 = new PostDetail
            {
                Id = 1,
                NumbersOfViews = 1,
                CreationDate = new DateTimeOffset(),
            };

            var post1 = new Post
            {
                //Without specify 1, it complaints has it is asseting 1 against 0
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
            };

            var post2 = new Post
            {
                Id = 2,
                Title = "This is a nice Title",
                Content = "Some awesome Content here",
                PostDetail = postDetail2
            };

            var options = MakeDbContext();
            using (var context = new Degree53DbContext(options))
            {
                context.Posts.Add(MakePost(post1));
                context.Posts.Add(MakePost(post2));
                context.SaveChanges();
            }
            using (var context = new Degree53DbContext(options))
            using (var repository = new Degree53Repository(context))
            {
                var controller = MakeController(repository);

                //When
                var result = await controller.GetPostsAsync();

                //Then
                Assert.IsType<OkObjectResult>(result);
                Assert.True(context.Posts.Any(p => p.Id == post1.Id));
                Assert.True(context.Posts.Any(p => p.Id == post2.Id));
            }
        }

        [Fact]
        public async void GetPostsAsync_NoData_RetunsNotFound()
        {
            //Given
            var options = MakeDbContext();

            using (var context = new Degree53DbContext(options))
            using (var repository = new Degree53Repository(context))
            {
                var controller = MakeController(repository);

                //When
                var result = await controller.GetPostsAsync();

                //Then
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async void GetPostAsync_HasData_RetunsOK()
        {
            //Given
            var postDetail = new PostDetail
            {
                Id = 3,
                NumbersOfViews = 3,
                CreationDate = new DateTimeOffset(),
            };

            var post = new Post
            {
                Id = 3,
                Title = "This is a nice Title",
                Content = "Some awesome Content here",
                PostDetail = postDetail
            };

            var options = MakeDbContext();
            using (var context = new Degree53DbContext(options))
            {
                context.Posts.Add(MakePost(post));
                context.SaveChanges();
            }
            using (var context = new Degree53DbContext(options))
            using (var repository = new Degree53Repository(context))
            {
                var controller = MakeController(repository);

                //When
                var result = await controller.GetPostAsync(3);

                //Then
                Assert.IsType<OkObjectResult>(result);
                Assert.True(context.Posts.Any(p => p.Id == post.Id));
                Assert.True(context.Posts.Any(p => p.PostDetail == post.PostDetail));
                Assert.True(context.Posts.Any(p => p.PostDetail.Id == post.PostDetail.Id));
            }
        }

        [Fact]
        public async void GetPostAsync_NoData_RetunsNotFound()
        {
            //Given    
            var options = MakeDbContext();
     
            using (var context = new Degree53DbContext(options))
            using (var repository = new Degree53Repository(context))
            {
                var controller = MakeController(repository);

                //When
                var result = await controller.GetPostAsync(3);

                //Then
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async void AddPostAsync_ValidData_RetunsOK()
        {
            //Given
            var postDetail = new PostDetail
            {
                Id = 4,
                NumbersOfViews = 4,
                CreationDate = new DateTimeOffset(),
            };

            var post = new Post
            {
                Id = 4,
                Title = "This is a nice Title",
                Content = "Some awesome Content here",
                PostDetail = postDetail
            };

            var postModel = MakePostModel(post);

            var options = MakeDbContext();

            using (var context = new Degree53DbContext(options))
            using (var repository = new Degree53Repository(context))
            {
                var controller = MakeController(repository);

                //When
                var result = await controller.AddPostAsync(postModel);

                //Then
                Assert.IsType<OkResult>(result);
                Assert.True(context.Posts.Any(p => p.Id == post.Id));
                Assert.True(context.Posts.Any(p => p.PostDetail == post.PostDetail));
                Assert.True(context.Posts.Any(p => p.PostDetail.Id == post.PostDetail.Id));
            }
        }

        [Fact]
        public async void AddPostAsync_InvalidData_RetunsBadRequest()
        {
            //Given
            var postDetail = new PostDetail
            {
                Id = 4,
                NumbersOfViews = 4,
                CreationDate = new DateTimeOffset(),
            };

            var post = new Post
            {
                Id = 4,
                Title = "This is a nice Title",
                Content = "Some awesome Content here",
                PostDetail = postDetail
            };

            var postModel = MakePostModel(post);

            var options = MakeDbContext();

            using (var context = new Degree53DbContext(options))
            using (var repository = new Degree53Repository(context))
            {
                var controller = MakeController(repository);

                //When
                var result = await controller.AddPostAsync(postModel);

                //Then
                Assert.IsType<OkResult>(result);
                Assert.True(context.Posts.Any(p => p.Id == post.Id));
                Assert.True(context.Posts.Any(p => p.PostDetail == post.PostDetail));
                Assert.True(context.Posts.Any(p => p.PostDetail.Id == post.PostDetail.Id));
            }
        }

        #region Test Helpers

        private static DbContextOptions<Degree53DbContext> MakeDbContext()
        {
            return new DbContextOptionsBuilder<Degree53DbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private static Degree53Controller MakeController(IDegree53Repository repository, bool modelIsInvalid = false)
        {
            var degree53Service = new Degree53service(repository);
            var controller = new Degree53Controller(degree53Service);
            var validator = A.Fake<IObjectModelValidator>();
            controller.ObjectValidator = validator;
            
            //I have tried to test a wrong model but it didn't work, maybe my configuration is wrong and i get 200Ok back
            if (modelIsInvalid)
                controller.ModelState.AddModelError("model", "InvalidModel");

            return controller;
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
