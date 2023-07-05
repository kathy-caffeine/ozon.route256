using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCalculator.IntegrationTests
{
    public class AppFixture : WebApplicationFactory<Route256.PriceCalculator.Api.Program>
    {
    }
}
