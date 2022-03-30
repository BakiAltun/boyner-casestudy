using Boyner.CaseStudy.ApplicationCore.Interfaces.Data;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boyner.CaseStudy.ApplicationCore.Entities;
namespace Boyner.CaseStudy.ApplicationCore.Queries.PostQueries
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostResult.Item>
    {
        private readonly IMongoRepository<Post> _mongoRepository;

        public GetPostByIdQueryHandler(IMongoRepository<Post> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<PostResult.Item> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
        {
            var entity = await _mongoRepository.GetAsync(query.Id);

            return MapToModel(entity);
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
