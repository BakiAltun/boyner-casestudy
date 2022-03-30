using MediatR;
using System;
using System.Collections.Generic;

namespace Boyner.CaseStudy.ApplicationCore.Queries.PostQueries
{
    public class GetPostByIdQuery : IRequest<PostResult.Item>
    {
        public string Id { get; set; }

        public GetPostByIdQuery(string id)
        {
            Id = id;
        }
    } 
}