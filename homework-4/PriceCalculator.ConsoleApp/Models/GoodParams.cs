using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculator.ConsoleApp.Models;

public record GoodParams(
    int id,
    int width,
    int length,
    int height,
    int weight);
