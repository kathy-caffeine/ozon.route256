using FluentValidation;
using Route256.Week6.Homework.PriceCalculator.Bll.Kafka.Models;

namespace Route256.Week6.Homework.PriceCalculator.Api.Validators.V1
{
    public class CalculateMessageValidator : AbstractValidator<RequestMessage>
    {
        public CalculateMessageValidator()
        {
            RuleFor(x => x.GoodId)
                .GreaterThan(0);

            RuleFor(x => x.Width)
                .GreaterThan(0);

            RuleFor(x => x.Height)
                .GreaterThan(0);

            RuleFor(x => x.Length)
                .GreaterThan(0);

            RuleFor(x => x.Weight)
                .GreaterThan(0);
        }
    }
}
