using System.Runtime.Serialization;

namespace Route256.Week5.Homework.PriceCalculator.Bll.Exceptions
{
    [Serializable]
    internal class OneOrManyCalculationsNotFoundException : Exception
    {
        public OneOrManyCalculationsNotFoundException()
        {
        }
    }
}