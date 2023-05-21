namespace Test.Mma.Common
{
    using global::Mma.Common;
    using global::Mma.Common.models;
    using NUnit.Framework;

    [TestFixture]
    public class Wind_formatter_tests
    {
        private IWindFormatter formatter;

        [SetUp]
        public void SetUp()
        {
            // Help from - https://e6bx.com/metar-decoder/
            formatter = new WindFormatter();
        }

        [TestCase(null, "///25KT")]
        [TestCase(10, "01025KT")]
        [TestCase(15, "01025KT")]
        [TestCase(350, "35025KT")]
        public void Average_wind_direction_is_correct(double? direction, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = direction,
                AverageWindSpeed = 25,
                MaximumWindSpeed = 28,
                MinimumWindDirection = direction,
                MaximumWindDirection = direction
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(8, 8, 20, 20, 20, "02008KT")]
        [TestCase(0, 2, 50, 10, 90, "00000KT")]
        [TestCase(2, 2, 50, 10, 90, "VRB02KT")]
        [TestCase(22, 34, 330, 330, 330, "33022G34KT")]
        [TestCase(16, 16, 160, 120, 190, "16016KT 120V190")]
        [TestCase(15, 28, 210, 180, 270, "21015G28KT 180V270")]
        [TestCase(70, 101, 270, 270, 270, "27070GP99KT")]
        [TestCase(25, 25, null, 10, 10, "///25KT")]
        public void Spec_examples_is_correct(double? aveSpeed, double? maxSpeed, double? aveDir, double? minDir, double? maxDir, string expected)
        {
            var data = new WindData
            {
                AverageWindSpeed = aveSpeed,
                MaximumWindSpeed = maxSpeed,
                AverageWindDirection = aveDir,
                MinimumWindDirection = minDir,
                MaximumWindDirection = maxDir
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, "00000KT")]
        public void Calm_is_correct(double? speed, string expected)
        {
            var data = new WindData
            {
                AverageWindDirection = 10,
                AverageWindSpeed = speed,
                MaximumWindSpeed = speed,
                MinimumWindDirection = 10,
                MaximumWindDirection = 10
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(70, 101, 270, 270, 270, "27070GP99KT")]
        [TestCase(22, 34, 270, 270, 270, "27022G34KT")]
        [TestCase(100, 100, 270, 270, 270, "270P99KT")]
        [TestCase(110, 130, 270, 270, 270, "270P99GP99KT")]
        [TestCase(90, 99, 270, 270, 270, "27090KT")]
        [TestCase(0, 9, 270, 270, 270, "00000KT")]
        [TestCase(1, 11, 270, 270, 270, "27001KT")]
        public void Wind_speed_is_correct(double? aveSpeed, double? maxSpeed, double? aveDir, double? minDir, double? maxDir, string expected)
        {
            var data = new WindData
            {
                AverageWindSpeed = aveSpeed,
                MaximumWindSpeed = maxSpeed,
                AverageWindDirection = aveDir,
                MinimumWindDirection = minDir,
                MaximumWindDirection = maxDir
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(2, 2, 50, 10, 90, "VRB02KT")]
        [TestCase(3, 3, 50, 10, 90, "VRB03KT")]
        [TestCase(5, 5, 50, 10, 90, "05005KT")]
        [TestCase(100, 100, 50, 10, 90, "050P99KT")]
        [TestCase(2, 2, 50, 40, 60, "05002KT")]
        [TestCase(5, 5, 50, 40, 60, "05005KT")]
        [TestCase(2, 2, 50, 10, 220, "05002KT")]
        [TestCase(5, 5, 50, 10, 220, "05005KT")]
        public void Wind_direction_is_correct(double? aveSpeed, double? maxSpeed, double? aveDir, double? minDir, double? maxDir, string expected)
        {
            var data = new WindData
            {
                AverageWindSpeed = aveSpeed,
                MaximumWindSpeed = maxSpeed,
                AverageWindDirection = aveDir,
                MinimumWindDirection = minDir,
                MaximumWindDirection = maxDir
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(16, 16, 160, 120, 190, "16016KT 120V190")]
        [TestCase(3, 3, 160, 120, 190, "VRB03KT")]
        [TestCase(16, 16, 160, 150, 190, "16016KT")]
        [TestCase(16, 16, 160, 10, 190, "16016KT")]
        [TestCase(16, 16, 160, 125, 195, "16016KT 120V190")]
        [TestCase(16, 16, 160, 121, 199, "16016KT 120V190")]
        [TestCase(15, 28, 210, 180, 270, "21015G28KT 180V270")]
        [TestCase(25, 25, null, 10, 90, "///25KT 010V090")]
        public void Wind_direction_variation_is_correct(double? aveSpeed, double? maxSpeed, double? aveDir, double? minDir, double? maxDir, string expected)
        {
            var data = new WindData
            {
                AverageWindSpeed = aveSpeed,
                MaximumWindSpeed = maxSpeed,
                AverageWindDirection = aveDir,
                MinimumWindDirection = minDir,
                MaximumWindDirection = maxDir
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(null, null, null, null, null, "00000KT")]
        [TestCase(1, 1, null, null, null, "///01KT")]
        [TestCase(10, 10, null, null, null, "///10KT")]
        [TestCase(10, 10, 60, null, null, "06010KT")]
        [TestCase(10, 10, 60, 50, null, "06010KT")]
        [TestCase(10, 10, 60, null , 90, "06010KT")]
        [TestCase(null, null, 60, 30, 90, "00000KT")]
        public void negative_scenarios_is_correct(double? aveSpeed, double? maxSpeed, double? aveDir, double? minDir, double? maxDir, string expected)
        {
            var data = new WindData
            {
                AverageWindSpeed = aveSpeed,
                MaximumWindSpeed = maxSpeed,
                AverageWindDirection = aveDir,
                MinimumWindDirection = minDir,
                MaximumWindDirection = maxDir
            };

            var result = formatter.FormatWind(data);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
