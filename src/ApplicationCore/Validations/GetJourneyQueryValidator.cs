using Boyner.CaseStudy.ApplicationCore.Queries;
using FluentValidation; 

namespace Boyner.CaseStudy.ApplicationCore.Validations
{
    public class GetJourneyQueryValidator : AbstractValidator<GetPostPagedListQuery>
    {
        public GetJourneyQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThan(0).WithMessage("Sayfa numarası minimum 1 olabilir."); 
        }
    }
}
