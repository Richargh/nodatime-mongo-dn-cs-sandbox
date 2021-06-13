using FluentAssertions;
using FluentAssertions.Execution;
using NodaTime;
using NodaTime.Text;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest.Serialization
{
    public class InstantFormatTest
    {
        private readonly IClock _clock;

        /// <summary>
        /// The general format pattern is down to the nanosecond
        /// while the 'G'eneral invariant pattern is only down to the second.
        /// Supported are the <see href="https://nodatime.org/3.0.x/userguide/instant-patterns">Patterns for Instant values</see>
        /// but also all the <see href="https://nodatime.org/3.0.x/userguide/localdatetime-patterns">Patterns for LocalDateTime values</see>.
        /// </summary>
        private const string GeneralFormat = "g";

        public InstantFormatTest()
        {
            _clock = SystemClock.Instance;
        }

        [Fact (DisplayName = "Should result in truncated Instant when parsing a formatted string because the general format is only down to the second")]
        public void CanFormatAndParseGeneral()
        {
            // given
            var instant = _clock.GetCurrentInstant();
            var truncated = instant.InUtc().LocalDateTime.With(TimeAdjusters.TruncateToSecond).InUtc().ToInstant();
            var pattern = InstantPattern.General;
            // when
            var formatted = pattern.Format(instant);
            // then
            var result = pattern.Parse(formatted).Value;
            result.Should().Be(truncated);
        }
        
        [Fact (DisplayName = "Should result in the same Instant when parsing a formatted string with the Long sortable format")]
        public void CanFormatAndParseLongSortable()
        {
            // given
            var instant = _clock.GetCurrentInstant();
            var pattern = InstantPattern.ExtendedIso;
            // when
            var formatted = pattern.Format(instant);
            // then
            var result = pattern.Parse(formatted).Value;
            result.Should().Be(instant);
        }
        
        [Fact (DisplayName = "Should create a nicely formatted ISO 8601 instant")]
        public void UtcFormat()
        {
            // given
            var instant = Instant.FromUtc(2021, 10, 5,   20, 15, 0);
            var pattern = InstantPattern.CreateWithInvariantCulture(GeneralFormat);
            // when
            var result = pattern.Format(instant);
            // then
            using (new AssertionScope())
            {
                result.Should().Contain("2021-10-05T20:15:00Z");
            }
        }
    }
}