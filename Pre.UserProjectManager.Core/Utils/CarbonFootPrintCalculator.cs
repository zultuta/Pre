using Pre.UserProjectManager.Core.Enums;

namespace Pre.UserProjectManager.Core.Utils
{
    public class CarbonFootPrintCalculator
    {
        public static decimal GetCarbonFootPrint(decimal footPrintPerGram, double weight, string unit)
        {
            var totalFootPrint = footPrintPerGram * (decimal)weight;
            if(unit == UNITS.kg.ToString())
            {
                return totalFootPrint * 1000;
            }
            if(unit == UNITS.t.ToString())
            {
                return totalFootPrint * 1000000;
            }
            return totalFootPrint;
        }
    }
}
