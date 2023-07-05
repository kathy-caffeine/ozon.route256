using System.Text;

namespace Route256.Week1.Homework.PriceCalculator.Api.Dal.Entities;
public record RequestLogEntity(
    DateTime Timestamp,
    string Url,
    string Headers,
    string Body
    );

public record ResponseLogEntity(
    string Body
    );

public record LogEntity(
    RequestLogEntity Request,
    ResponseLogEntity Response
    )
{
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Request:");
        sb.AppendLine("Timestamp: " + Request.Timestamp 
            + Environment.NewLine + "Url: " + Request.Url
            + Environment.NewLine + Request.Headers + Environment.NewLine
            + Environment.NewLine + "Body: " +  Request.Body);
        sb.AppendLine("Response:");
        sb.AppendLine("Body:" + Response.Body);

        return sb.ToString();
    }

}
