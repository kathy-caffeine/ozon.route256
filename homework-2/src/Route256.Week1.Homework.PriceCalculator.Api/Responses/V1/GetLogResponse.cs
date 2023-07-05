using Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;

namespace Route256.Week1.Homework.PriceCalculator.Api.Responses.V1;

public record GetLogResponse(
    LogEntity[] LogEntities
    );
