using Boyner.CaseStudy.ApplicationCore.Commands;
using Boyner.CaseStudy.ApplicationCore.Entities;
using Boyner.CaseStudy.ApplicationCore.Queries.PostQueries;
using Boyner.CaseStudy.Infrastructure.RabbitMQ;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boyner.CaseStudy.Presentation.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IRabbitMQ<Post> _mqueue;
        private readonly IMediator _meditor;
        public PostController(IRabbitMQ<Post> mq, IMediator meditor)
        {
            _mqueue = mq;
            _meditor = meditor;
        }
  
        [HttpGet]
        public async Task<PostResult> Get(int page = 1, int pageSize = 10)
        {   
            return await _meditor.Send(new GetPostPagedListQuery(page, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<PostResult.Item> Get(string id)
        {   
            return await _meditor.Send(new GetPostByIdQuery(id));
        }

        [HttpPost]
        public void Post(PostResult.Item item)
        {
            _mqueue.Publish(new Post(item.Text));

            return;
        }

        [HttpPut("{id}")]
        public void Put(string id, PostResult.Item item)
        {
            _mqueue.Publish(new Post(id, item.Text));

            return;
        }
    }
}
