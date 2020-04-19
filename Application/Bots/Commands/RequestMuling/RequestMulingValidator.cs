using FluentValidation;

namespace FORFarm.Application.Bots.Commands.RequestMuling
{
    public class RequestMulingValidator : AbstractValidator<RequestMuling>
    {
        public RequestMulingValidator()
        {
            RuleFor(r => r.Tag).NotEmpty();
        }
    }
}