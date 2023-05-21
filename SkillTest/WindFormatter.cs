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

            // Check for calm
            if (windData.AverageWindSpeed == null || windData.AverageWindSpeed < 1)
            {
                return result.Append("00000KT").ToString();
            }

            // Wind Direction
            if (windData.AverageWindDirection == null)
            {
                if (IsVariableWind(windData))
                {
                    result.Append("VRB");
                }
                else
                {
                    result.Append("///");
                }
            }
            else
            {
                if (IsVariableWind(windData))
                {
                    result.Append("VRB");
                }
                else
                {
                    var roundedAverageWindDirection = RoundWindDirection(windData.AverageWindDirection.Value);
                    result.Append($"{roundedAverageWindDirection:000}");
                }
            }

            // Wind Speed
            result.Append($"{CheckWindSpeed(windData)}");

            // Wind Direction Variation
            if (IsDirectionVariation(windData))
            {
                //var variation = GetDirectionVariation(windData);
                var roundedMinDirection = RoundWindDirection(windData.MinimumWindDirection.Value);
                var roundedMaxDirection = RoundWindDirection(windData.MaximumWindDirection.Value);
                result.Append($" {roundedMinDirection:000}V{roundedMaxDirection:000}");
            }

            return result.ToString();
        }

        private bool IsVariableWind(WindData windData)
        {
            if (windData.MinimumWindDirection == null || windData.MaximumWindDirection == null)
            {
                return false;
            }

            var directionVariation = Math.Abs(windData.MaximumWindDirection.Value - windData.MinimumWindDirection.Value);

            // Only a variation if between 60<>180 and wind speed is 3 or less
            if (directionVariation >= 60 && directionVariation < 180 && windData.AverageWindSpeed <= 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsDirectionVariation(WindData windData)
        {
            // Check to see if the direction variation is greater than 60 but less than 180
            var directionVariation = Math.Abs(windData.MaximumWindDirection.Value - windData.MinimumWindDirection.Value);

            if (directionVariation >= 60 && directionVariation < 180 && windData.AverageWindSpeed > 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int RoundWindDirection(double windDirection)
        {
            // Always round down
            return (int)(Math.Floor(windDirection / 10.0) * 10.0);
        }

        private string CheckWindSpeed(WindData windData)
        {
            if (windData == null)
            {
                return "/";
            }

            // Only report max wind if its exceeded average by 10 knots
            if ((windData.MaximumWindSpeed - windData.AverageWindSpeed) > 10)
            {
                // Wind speed is only reported if between 1 and 99 knots
                if (windData.AverageWindSpeed < 100 && windData.MaximumWindSpeed < 100)
                {
                    // Round to the nearest knot
                    return $"{Math.Round(windData.AverageWindSpeed.Value):00}G{Math.Round(windData.MaximumWindSpeed.Value):00}KT"; 
                }
                // Else report using P99 instead
                // Both over 100
                else if (windData.AverageWindSpeed >= 100 && windData.MaximumWindSpeed >= 100)
                {
                    return $"P99GP99KT";
                }
                // Max over 100
                else
                {
                    return $"{Math.Round(windData.AverageWindSpeed.Value):00}GP99KT";
                }
            }
            else
            {
                if (windData.AverageWindSpeed < 100)
                {
                    return $"{Math.Round(windData.AverageWindSpeed.Value):00}KT";
                }
                else
                {
                    return $"P99KT";
                }
            }
        }
    }
}
