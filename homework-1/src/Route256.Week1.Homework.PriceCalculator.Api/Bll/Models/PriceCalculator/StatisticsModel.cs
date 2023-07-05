namespace Route256.Week1.Homework.PriceCalculator.Api.Bll.Models.PriceCalculator;

public record Statistics(
    int max_weight, 
    int max_volume,
    int max_distance_for_heaviest_good, 
    int max_distance_for_largest_good,
    decimal wavg_price
    );