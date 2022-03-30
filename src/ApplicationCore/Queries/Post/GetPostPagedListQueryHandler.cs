using Boyner.CaseStudy.ApplicationCore.Interfaces.Data;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boyner.CaseStudy.ApplicationCore.Entities;
namespace Boyner.CaseStudy.ApplicationCore.Queries.PostQueries
{
    public class GetPostPagedListQueryHandler : IRequestHandler<GetPostPagedListQuery, PostResult>
    {
        private readonly IMongoRepository<Post> _mongoRepository;

        public GetPostPagedListQueryHandler(IMongoRepository<Post> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<PostResult> Handle(GetPostPagedListQuery query, CancellationToken cancellationToken)
        {
            var list = await _mongoRepository.GetPagedListAsync(query.Page, query.PageSize, o => o.Id);

            return new PostResult(list.Select(MapToModel).ToList());
        }

        private PostResult.Item MapToModel(Post requestModel)
        {
            return new()
            {
                Id = requestModel.Id,
                Text = requestModel.Text,
                CreatedOn = requestModel.CreatedOn,
                UpdatedOn = requestModel.UpdatedOn
            };
        }
    }
}
