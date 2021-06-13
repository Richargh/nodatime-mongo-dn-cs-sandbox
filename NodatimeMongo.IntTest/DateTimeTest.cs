using System;
using System.Globalization;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Richargh.Sandbox.NodatimeMongo.IntTest
{
    public class DateTimeTest
    {
        [Fact(DisplayName = "Datetime Should not but does store different kinds in the same type and the rules are quite strange")]
        public void DifferentKindSameType()
        {
            // given
            var easternStandardTime = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");  
            // when
            var nowUtc = DateTime.UtcNow;
            var nowLocal = DateTime.Now;
            
            var convertedUtcToEst = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, easternStandardTime);  
            var convertedLocalToEst = TimeZoneInfo.ConvertTime(nowLocal, easternStandardTime);  
            
            var parseUnspecified = DateTime.Parse("2021-10-01T10:00:00");
            var parseUtc = DateTime.Parse("2021-10-01T10:00:00Z");
            var parseGmt = DateTime.ParseExact(
                "2021-10-01T10:00:00 GMT", "yyyy-MM-dd'T'HH:mm:ss.FFF Z", CultureInfo.InvariantCulture);
            var parseOffset = DateTime.ParseExact(
                "2021-10-01T10:00:00.000+01:00", "yyyy-MM-dd'T'HH:mm:ss.FFFK", CultureInfo.InvariantCulture);
            // then
            using (new AssertionScope())
            {
                nowUtc.Kind.Should().Be(DateTimeKind.Utc);
                nowLocal.Kind.Should().Be(DateTimeKind.Local);
                
                convertedUtcToEst.Kind.Should().Be(DateTimeKind.Unspecified);
                convertedLocalToEst.Kind.Should().Be(DateTimeKind.Unspecified);
                
                parseUnspecified.Kind.Should().Be(DateTimeKind.Unspecified);
                parseUtc.Kind.Should().Be(DateTimeKind.Local);
                parseGmt.Kind.Should().Be(DateTimeKind.Local);
                parseOffset.Kind.Should().Be(DateTimeKind.Local);
            }
        }
    }
}