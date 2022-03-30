using MediatR;
using System;
using System.Collections.Generic;

namespace Boyner.CaseStudy.ApplicationCore.Queries.PostQueries
{
    public class GetPostPagedListQuery : IRequest<PostResult>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetPostPagedListQuery(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }

    public class PostResult
    { 
        public IList<Item> Items { get; set; }
        public PostResult(IList<Item> items)
        {
            Items = items;
        }

        public class Item
        {
            public string Id { get; set; }

            /// <summary>
            /// Açıklama
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// Kalkış zamanı
            /// </summary>
            public DateTime CreatedOn { get; set; }

            /// <summary>
            /// Varış zamanı
            /// </summary>
            public DateTime? UpdatedOn { get; set; }
        }
    }
}