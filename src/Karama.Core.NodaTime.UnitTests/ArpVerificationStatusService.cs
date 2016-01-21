using NodaTime;

namespace Karama.Core.NodaTime.UnitTests
{
    public class ArpVerificationStatusService
    {
        private readonly IClock _systemClock;

        public ArpVerificationStatusService(IClock systemClock)
        {
            this._systemClock = systemClock;
        }


        public ZonedDateTime SendExpiryNotifications()
        {
            // Get the current time
            ZonedDateTime currentTime = _systemClock.Now.InUtc();

            // Rest of the code
            return currentTime;

        }

        public ArpVerificationStatus GetArpVerificationStatus()
        {
            if (true)
            {
                return ArpVerificationStatus.ArpNotFound;
            }
        }
    }
}
