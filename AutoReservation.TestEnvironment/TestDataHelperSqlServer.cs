using System;
using System.Data;
using System.Data.Common;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutoReservation.TestEnvironment
{
    public class TestDataHelperSqlServer
        : TestDataHelper
    {
        private static bool _firstTestInExecution = true;
        private static readonly object LockObject = new object();

        public TestDataHelperSqlServer(AutoReservationContext context)
            : base(context) { }

        /// <summary>
        /// Delete data, reset identity seed.
        /// Drop and re-create takes too long to do it every time.
        /// </summary>
        protected override void PrepareDatabase()
        {
            // First execution
            lock (LockObject)
            {
                if (_firstTestInExecution)
                {
                    Context.Database.EnsureDeleted();
                    Context.Database.EnsureCreated();
                    _firstTestInExecution = false;
                    return;
                }
            }

            // Subsequent executions
            Context.Database.EnsureCreated();
            CleanupData();
        }

        protected void CleanupData()
        {
            string luxusklasseAutoTableName = GetTableName<LuxusklasseAuto>();
            string mittelklasseAutoTableName = GetTableName<MittelklasseAuto>();
            string standardAutoTableName = GetTableName<StandardAuto>();
            string autoTableName = GetTableName<Auto>();
            string kundeTableName = GetTableName<Kunde>();
            string reservationTableName = GetTableName<Reservation>();

            // Delete all records from tables
            //      > Cleanup for specific subtypes necessary when not using table per hierarchy (TPH)
            //        since entities will be stored in different tables.
            DeleteAllRecords(reservationTableName);
            ResetIdentitySeed(reservationTableName);

            if (luxusklasseAutoTableName != autoTableName)
            {
                DeleteAllRecords(luxusklasseAutoTableName);
                ResetIdentitySeed(luxusklasseAutoTableName);
            }

            if (mittelklasseAutoTableName != autoTableName)
            {
                DeleteAllRecords(mittelklasseAutoTableName);
                ResetIdentitySeed(mittelklasseAutoTableName);
            }

            if (standardAutoTableName != autoTableName)
            {
                DeleteAllRecords(standardAutoTableName);
                ResetIdentitySeed(standardAutoTableName);
            }

            DeleteAllRecords(autoTableName);
            ResetIdentitySeed(autoTableName);

            DeleteAllRecords(kundeTableName);
            ResetIdentitySeed(kundeTableName);

        }

        private string GetTableName<T>()
        {
            IEntityType entityType = Context.Model
                .FindEntityType(typeof(T));

            string schema = entityType.GetSchema();
            string table = entityType.GetTableName();

            return string.IsNullOrWhiteSpace(schema)
                ? $"[{table}]"
                : $"[{schema}].[{table}]";
        }

        private void DeleteAllRecords(string table)
        {
            string statement = $"DELETE FROM {table}"; // Must be a separate variable
            Context.Database.ExecuteSqlRaw(statement);
        }

        private void ResetIdentitySeed(string table)
        {
            if (TableHasIdentityColumn(table))
            {
                string statement = $"DBCC CHECKIDENT('{table}', RESEED, 0)"; // Must be a separate variable
                Context.Database.ExecuteSqlRaw(statement);
            }
        }

        private bool TableHasIdentityColumn(string table)
        {
            bool hasIdentityColumn = false;
            DbCommand command = Context.Database.GetDbConnection().CreateCommand();
            try
            {
                command.CommandText = $"SELECT OBJECTPROPERTY(OBJECT_ID('{table}'), 'TableHasIdentity')";
                command.CommandType = CommandType.Text;

                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                using DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    hasIdentityColumn = reader.GetInt32(0) == 1;
                }
            }
            catch
            {
                hasIdentityColumn = false;
            }
            finally
            {
                command.Dispose();
            }

            return hasIdentityColumn;
        }
    }
}