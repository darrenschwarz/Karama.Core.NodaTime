//using NodaTime;

using NodaTime;
using NodaTime.Testing;
using NUnit.Framework;

namespace Karama.Core.NodaTime.UnitTests
{
    [TestFixture]
    public class ArpVerificationStatusServiceTestFixture
    {
        // Instant represents time from epoch
        Instant _now = SystemClock.Instance.Now;

        [Test]
        public void Test()
        {
            Assert.AreEqual(true, true);
            // Convert an instant to a ZonedDateTime
            ZonedDateTime nowInIsoUtc = _now.InUtc();

            // Create a duration
            Duration duration = Duration.FromMinutes(3);

            // Add it to our ZonedDateTime
            ZonedDateTime thenInIsoUtc = nowInIsoUtc + duration;

            // Time zone support (multiple providers)
            var london = DateTimeZoneProviders.Tzdb["Europe/London"];
            
            // Time zone conversions
            var localDate = new LocalDateTime(2012, 3, 27, 0, 45, 00);

            var r = london.ResolveLocal(localDate, null);

            var before = london.AtStrictly(localDate);
        }

        [Test]
        public void GetArpVerificationStatus_CalledWith_ReturnsArpVerificationStatusVerifyingArp()
        {
            

            var london = TimeZones.London;
            var hongKong = TimeZones.HongKong;

            var londonInstant = Instant.FromUtc(2016, 1, 20, 23, 15, 0);


            //Assume that using Instant.FromUtc() will always return expected value
            //test that dates running on hongkong servers have the correct offsets applied
            //Write simpleapplication and log all values run simultaneously on hongKong and UK servers
            //Ultimately we want to be able store datetimes as a utc value and be able to represent that datime with deisred offset,
            //Such that if a user wave workflow is to perform a date calculation it should do so accounting for the offset, in the current environment
            //Is it important that when chcking on weekends we ensure that the right offset is used basd on the waveuser/colleagues TimeZone
            //i.e. Friday 23:00 for a colleague in London (Not a weekend day) would be Saturday 07:00 in Hong Kong (is a weekend day)
            var utc = _now.InUtc();

            var hongKongOffset = hongKong.GetUtcOffset(londonInstant);
            Duration hongKongDuration = Duration.FromTicks(hongKongOffset.Ticks);

            var londonOffset = london.GetUtcOffset(londonInstant);
            Duration londonDuration = Duration.FromTicks(londonOffset.Ticks);

            ZonedDateTime londonInstantZonedDateTime = londonInstant.InUtc();

            ZonedDateTime hongKongInstantZonedDateTime = londonInstantZonedDateTime + hongKongDuration;



            //var r = hongKong.ResolveLocal(londonInstantZonedDateTime.LocalDateTime);
            var localDate = new LocalDateTime(2016, 1, 20, 22, 29, 0);

            var londonLocalDateTime = london.AtStrictly(localDate);
            
            //var hongKongLocalDateTime = hongKong.GetUtcOffset(aTimeStamp);//hongKong.AtLeniently(localDate) + hongKong.GetUtcOffset();
            var systemClock = new FakeClock(Instant.FromUtc(2016, 1, 20, 23, 15, 0), Duration.FromStandardDays(10));

            var service = new ArpVerificationStatusService(systemClock);

            ZonedDateTime nowInIsoUtc = systemClock.Now.InUtc();
            var nowInIsoUtcPlusTenDays = service.SendExpiryNotifications();

            var period = Period.Between(nowInIsoUtc.LocalDateTime, nowInIsoUtcPlusTenDays.LocalDateTime, PeriodUnits.Days);

            Assert.AreEqual(period.Days, 10);
        }
    }
}
