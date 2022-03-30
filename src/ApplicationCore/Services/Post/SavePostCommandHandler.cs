using Boyner.CaseStudy.ApplicationCore.Interfaces.Data;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boyner.CaseStudy.ApplicationCore.Entities; 

namespace Boyner.CaseStudy.ApplicationCore.Commands
{
    public class SavePostCommandHandler : IRequestHandler<SavePostCommand, bool>
    {
        private readonly IMongoRepository<Post> _mongoRepository;

        public SavePostCommandHandler(IMongoRepository<Post> mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public async Task<bool> Handle(SavePostCommand command, CancellationToken cancellationToken)
        {
            var entity = await _mongoRepository.GetAsync(command.Id) ?? new Post();

            entity.Text = command.Text;

            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.CreatedOn = System.DateTime.Now;
                await _mongoRepository.CreateAsync(entity);
                return true;
            }

            entity.UpdatedOn = System.DateTime.Now;
            await _mongoRepository.UpdateAsync(entity.Id, entity);

            return true;
        }
 
    }
}
