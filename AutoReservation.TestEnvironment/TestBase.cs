using System;
using AutoReservation.Dal;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.TestEnvironment
{
    public abstract class TestBase
    {
        /// <summary>
        /// Constructor, runs before every run of a test method.
        /// Initializes Test data for each test run.
        /// </summary>
        protected TestBase()
        {
            InitializeTestEnvironment();
        }

        /// <summary>
        /// This method initializes the test environment including the database.
        /// </summary>
        private void InitializeTestEnvironment()
        {
            using AutoReservationContext context = new AutoReservationContext();

            TestDataHelper testDataHelper;
            if (context.Database.IsInMemory())
            {
                testDataHelper = new TestDataHelperInMemory(context);
            }
            else if (context.Database.IsSqlServer())
            {
                testDataHelper = new TestDataHelperSqlServer(context);
            }
            else
            {
                throw new NotImplementedException();
            }
            
            testDataHelper.InitializeTestData();
        }
    }
}
