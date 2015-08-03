namespace SkyScanner.Services.Helpers
{
    using System;
    using System.Diagnostics;

    using SkyScanner.Services.Interfaces;

    /// <summary>
    /// The IncreasingIntervalGenerator is an implementation of ITaskDelayGenerator that
    /// tries to mimic the behavior described in http://business.skyscanner.net/portal/en-GB/Documentation/Faq#qf7
    /// in which SkyScanner states that polling intervals should increase from
    /// 0-1 second to 3 seconds
    /// </summary>
    public class IncreasingIntervalGenerator : ITaskDelayGenerator
    {
        /// <summary>
        /// The number of times NextInterval was called
        /// </summary>
        private int nextIntervalCallCount = 0;

        /// <summary>
        /// The maximum delay value ( 3 seconds now ). In case more than 3 intervals were generated
        /// the generated delay will always be MaxDelayValue
        /// </summary>
        private const int MaxDelayValue = 3000;

        /// <summary>
        /// The delay with which te interval increases for every attempt
        /// </summary>
        private const int DelayPerAttempt = 1000;

        /// <summary>
        /// Gets the next interval
        /// </summary>
        public int NextInterval
        {
            get
            {
                var result = this.CalculateDelay();
                this.nextIntervalCallCount++;
                return result;
            }
        }

        /// <summary>
        /// Gets the attempt category.
        /// </summary>
        private AttemptCategory Attempt => this.nextIntervalCallCount > 3 ? AttemptCategory.AfterThird : (AttemptCategory) this.nextIntervalCallCount;

        /// <summary>
        /// Enum categorization of the interval counter
        /// </summary>
        enum AttemptCategory
        {
            First = 0,
            Second = 1,
            Third = 2,
            AfterThird = 3
        }

        /// <summary>
        /// Calculates the delay, which - in this implementation - depends on the number of times
        /// NextInterval has been called
        /// </summary>
        /// <returns>The calculated delay</returns>
        private int CalculateDelay()
        {
            int result;
            switch (this.Attempt)
            {
                case AttemptCategory.First:
                case AttemptCategory.Second:
                case AttemptCategory.Third:
                    result = DelayPerAttempt * this.nextIntervalCallCount;
                    break;
                case AttemptCategory.AfterThird:
                    result = MaxDelayValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}