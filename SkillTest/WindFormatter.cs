namespace Mma.Common
{
    using System;
    using System.Text;
    using Mma.Common.models;

    public interface IWindFormatter
    {
        string FormatWind(WindData windData);
    }

    public class WindFormatter : IWindFormatter
    {
        public string FormatWind(WindData windData)
        {
            var result = new StringBuilder();

            if (windData.AverageWindDirection == null)
            {
                result.Append("///");
            }
            else
            {
                var roundedAvgWindDirection = RoundWindDirectionDown(windData.AverageWindDirection.Value);
                result.Append($"{roundedAvgWindDirection:000}");
            }

            result.Append($"{windData.AverageWindSpeed,00}");
           
            if (windData.AverageWindDirection == null)
            {
                result.Append("KY");
            }
            else
            {
                result.Append("KT");
            }

            return result.ToString();
        }

        private int RoundWindDirectionDown(double windDirection)
        {
            return (int)(Math.Floor(windDirection / 10.0) * 10.0);
        }
    }
}
