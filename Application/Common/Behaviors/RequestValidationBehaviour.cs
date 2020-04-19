using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationContext = FluentValidation.ValidationContext;

namespace FORFarm.Application.Common.Behaviors
{
    class RequestValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<IRequest>> _validators;

        public RequestValidationBehaviour(IEnumerable<IValidator<IRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new FluentValidation.ValidationException(failures);
            }

            return next();
        }
    }
}
