namespace Mma.Common.models
{
    public class WindData
    {
        const double MinWindDirection = 10;
        const double MaxWindDirection = 360;
        private double? _averageWindDirection;
        private double? _minimumWindDirection;
        private double? _maximumWindDirection;

        public double? AverageWindSpeed { get; set; } // Value in knots
       
        public double? MaximumWindSpeed { get; set; } // Value in knots

        public double? AverageWindDirection // Null if more than 360 deg
        {
            get { return _averageWindDirection; }
            set { _averageWindDirection = EnsureWithinRange(value); }
        }

        public double? MinimumWindDirection // Null if more than 360 deg
        {
            get { return _minimumWindDirection; }
            set { _minimumWindDirection = EnsureWithinRange(value); }
        }

        public double? MaximumWindDirection // Null if more than 360 deg
        {
            get { return _maximumWindDirection; }
            set { _maximumWindDirection = EnsureWithinRange(value); }
        }

        private double? EnsureWithinRange(double? value)
        {
            if (value < MinWindDirection)
            {
                return MinWindDirection;
            }

            if (value > MaxWindDirection)
            {
                return null;
            }

            return value;
        }
    }
}
