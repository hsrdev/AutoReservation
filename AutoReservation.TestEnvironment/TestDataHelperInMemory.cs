using AutoReservation.Dal;

namespace AutoReservation.TestEnvironment
{
    public class TestDataHelperInMemory
        : TestDataHelper
    {
        public TestDataHelperInMemory(AutoReservationContext context)
            : base(context) { }

        /// <summary>
        /// Simple drop and re-create which is relatively fast.
        /// </summary>
        protected override void PrepareDatabase()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }
    }
}