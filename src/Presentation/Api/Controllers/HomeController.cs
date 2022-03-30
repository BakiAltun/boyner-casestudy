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
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly IRabbitMQ<Post> _mqueue;
        private readonly IMediator _meditor;
        public HomeController(IRabbitMQ<Post> mq, IMediator meditor)
        {
            _mqueue = mq;
            _meditor = meditor;
        }


        public IActionResult Index()
        {
            return Redirect("/index.html");
        }
    }
}
