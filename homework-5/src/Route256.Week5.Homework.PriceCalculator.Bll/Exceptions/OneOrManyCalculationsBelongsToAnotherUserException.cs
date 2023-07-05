using Route256.Week5.Homework.PriceCalculator.Dal.Entities;
using System.Runtime.Serialization;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

internal class OneOrManyCalculationsBelongsToAnotherUserException : Exception
{
    public OneOrManyCalculationsBelongsToAnotherUserException(
        long[] ids)
    {
        
    }

    public OneOrManyCalculationsBelongsToAnotherUserException(string? message) : base(message) { }
}