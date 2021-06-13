using FluentAssertions;
using FluentAssertions.Execution;
using NodaTime;
using NodaTime.Text;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest.Serialization
{
    public class ZonedDateTimeFormatTest
    {
        private readonly IClock _clock;

        /// <summary>
        /// The  Extended invariant pattern is down to the nanosecond
        /// while the 'G'eneral invariant pattern is only down to the second.
        /// <see href="https://nodatime.org/3.0.x/userguide/zoneddatetime-patterns">Patterns for ZonedDateTime values</see>
        /// </summary>
        private const string ExtendedInvariant = "F";
        private const string GeneralInvariant = "G";

        /// <summary>
        /// The IANA time zone identifier for the India Standard Time (IST).
        /// <see href="https://time.is/IST">Time.is</see>
        /// </summary>
        private const string IndiaStandardTime = "Asia/Kolkata";

        public ZonedDateTimeFormatTest()
        {
            _clock = SystemClock.Instance;
        }
        
        [Fact (DisplayName = "Should result in the truncated ZonedDateTime when parsing a formatted string with the general pattern")]
        public void CanFormatAndParseSystemDefaultGeneralPattern()
        {
            // given
            var zone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            var zonedDateTime = _clock.GetCurrentInstant().InZone(zone);
            var truncated = zonedDateTime.LocalDateTime.With(TimeAdjusters.TruncateToSecond).InZoneStrictly(zone);
            var pattern = ZonedDateTimePattern.CreateWithInvariantCulture(GeneralInvariant, DateTimeZoneProviders.Tzdb);
            // when
            var formatted = pattern.Format(zonedDateTime);
            // then
            var result = pattern.Parse(formatted).Value;
            result.Should().Be(truncated);
        }

        [Fact (DisplayName = "Should result in the same ZonedDateTime when parsing a formatted string")]
        public void CanFormatAndParseSystemDefaultExtendedPattern()
        {
            // given
            var zone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            var zonedDateTime = _clock.GetCurrentInstant().InZone(zone);
            var pattern = ZonedDateTimePattern.CreateWithInvariantCulture(ExtendedInvariant, DateTimeZoneProviders.Tzdb);
            // when
            var formatted = pattern.Format(zonedDateTime);
            // then
            var result = pattern.Parse(formatted).Value;
            result.Should().Be(zonedDateTime);
        }
        
        [Fact (DisplayName = "Should create a nicely formatted ISO 8601 UTC datetime")]
        public void UtcFormat()
        {
            // given
            var zonedDateTime = Instant.FromUtc(2021, 10, 5,   20, 15, 0).InZone(DateTimeZone.Utc);
            var pattern = ZonedDateTimePattern.CreateWithInvariantCulture(ExtendedInvariant, DateTimeZoneProviders.Tzdb);
            // when
            var result = pattern.Format(zonedDateTime);
            // then
            using (new AssertionScope())
            {
                result.Should().Contain("2021-10-05T20:15:00 UTC (+00)");
            }
        }
        
        [Fact (DisplayName = "Should create a nicely formatted ISO 8601 IST datetime")]
        public void EasterFormat()
        {
            // given
            var zone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(IndiaStandardTime)!;
            var zonedDateTime = Instant.FromUtc(2021, 10, 5,   10, 15, 0).InZone(zone);
            var pattern = ZonedDateTimePattern.CreateWithInvariantCulture(ExtendedInvariant, DateTimeZoneProviders.Tzdb);
            // when
            var result = pattern.Format(zonedDateTime);
            // then
            using (new AssertionScope())
            {
                result.Should().Contain($"2021-10-05T15:45:00 {IndiaStandardTime} (+05:30)");
            }
        }
    }
}