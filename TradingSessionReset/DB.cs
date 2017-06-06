using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptionProcessingService.Enums;
using OptionProcessingService.Types;
using System.Data.SqlClient;
using System.Globalization;
using CommonObjects.Unitity;
using CommonObjects.Logger;

namespace TradingSessionReset
{
    public static class DB
    {
        private static string _connectionString;
        private const int SECOND = 1000;
        private const int MINUTE = 1000 * 60;
        private const int FIVEMINUTE = 1000 * 60 * 5;
        private const int QUARTERHOUR = 1000 * 60 * 15;
        private const int HALFHOUR = 1000 * 60 * 30;
        private const int HOUR = 1000 * 60 * 60;
        private static int expiryduration;
        private static int expiryMultiplier;

        static DB()
        {
            string dataSource = System.Configuration.ConfigurationSettings.AppSettings["db_data_source"];
            string initCat = System.Configuration.ConfigurationSettings.AppSettings["db_init_catalog"];
            string userName = System.Configuration.ConfigurationSettings.AppSettings["db_UserName"];
            string password = System.Configuration.ConfigurationSettings.AppSettings["db_Password"];
            expiryduration= Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["ExpiryDurationInMinutes"]);
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                _connectionString =
                    @"Data Source=" + dataSource + ";Initial Catalog=" + initCat + ";User Id = " + userName + "; Password = '" + password + "';";
            }
            else
            {
                _connectionString =
                    "Data Source=" + dataSource + ";Initial Catalog=" + initCat + ";Integrated Security=True;";
            }

            switch(expiryduration)
            {
                case 5:
                    expiryMultiplier = FIVEMINUTE;
                    break;
                case 15:
                    expiryMultiplier = QUARTERHOUR;
                    break;
                case 30:
                    expiryMultiplier = HALFHOUR;
                    break;
                case 60:
                    expiryMultiplier = HOUR;
                    break;
                default:
                    throw new Exception("Not a valid ExpiryDurationInMinutes,Possible values:5,15,30,60 ");
            }
        }        
        private static List<AssetNew> GetAssetsDetailNew(SqlConnection aConnection, SqlTransaction transaction)
        {
            List<AssetNew> assets = new List<AssetNew>();
            var cmd = new SqlCommand("p_GetAssetsNew ",
                                               aConnection, transaction);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using (var reader = cmd.ExecuteReader())
            {

                if (!reader.HasRows)
                    return assets;

                while (reader.Read())
                {
                    assets.Add(new AssetNew
                    {
                        ID = ((int)reader["ID"]).ToString(CultureInfo.InvariantCulture).Trim(),
                        Type = (AssetType)(int)reader["Type"],
                        Symbol = new Symbol
                        {
                            Name = ((string)reader["ShortName"]).Trim(),
                            Type = (SecurityType)(int)reader["SecType"]
                        },
                        ReturnCoefficient = (double)reader["ReturnCoefficient"],
                        IsClosable = (byte)reader["IsClosable"] == 1,
                        UTCStartDateTime = (DateTime?)reader["UTCStartDateTime"],
                        UTCEndDateTime = (DateTime?)reader["UTCEndDateTime"]
                    });
                }
            }
            return assets;
        }

        public static void UpdateAssetsNew()
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();

                    IList<AssetNew> assets = new List<AssetNew>();
                    long startOfDayTimestamp = TimestampUtility.GetStartOfDayTimestamp();
                    long endOfDayTimestamp = TimestampUtility.GetStartOfDayTimestamp();
                    var cmd = new SqlCommand("p_ResetData", aConnection, transaction);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    assets = GetAssetsDetailNew(aConnection, transaction);
                    foreach (var asset in assets)
                    {
                        startOfDayTimestamp = TimestampUtility.GetCurrentTimestamp((DateTime)asset.UTCStartDateTime);
                        endOfDayTimestamp = TimestampUtility.GetCurrentTimestamp((DateTime)asset.UTCEndDateTime);
                        for (var i = 1; i <= 288; i++)
                        {
                            var timestamp = startOfDayTimestamp + expiryMultiplier * i;
                            if (timestamp > endOfDayTimestamp)
                                break;
                            cmd = new SqlCommand("INSERT INTO ExpiryTimestamps ([AssetID],[Timestamp])" +
                                         "VALUES(@assetId, @timestamp)", aConnection);

                            cmd.Parameters.AddWithValue("assetId", asset.ID);
                            cmd.Parameters.AddWithValue("timestamp", timestamp);

                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    throw e;

                }
            }
        }
        public static void UpdateExpiryAllDayTimestamp()
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();

                    IList<AssetNew> assets = new List<AssetNew>();
                    long startOfDayTimestamp = TimestampUtility.GetStartOfDayTimestamp();
                    long endOfDayTimestamp = TimestampUtility.GetEndOfDayTimestamp();
                    for (var i = 1; i <= 288; i++)
                    {
                        var timestamp = startOfDayTimestamp + expiryMultiplier * i;
                        if (timestamp > endOfDayTimestamp)
                            break;
                        var cmd = new SqlCommand("p_UpdateExpiryAllDayTimestamps", aConnection, transaction);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("timestamp", timestamp);
                        cmd.Parameters.AddWithValue("time", TimestampUtility.TimestampToDateTime(timestamp));
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    throw e;
                    
                }
            }
        }
    }
}
