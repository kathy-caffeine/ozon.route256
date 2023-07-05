using FluentMigrator;

namespace Route256.Week5.Workshop.PriceCalculator.Dal.Migrations;

[Migration(20231504, TransactionBehavior.None)]

public class AddAnomalies : Migration
{
    public override void Up()
    {
        Create.Table("anomalies")
            .WithColumn("good_id").AsInt64().PrimaryKey("anomalies_pk").Identity()
            .WithColumn("price").AsDecimal().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("anomalies");
    }
}