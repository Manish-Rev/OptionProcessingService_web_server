using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using OptionProcessingService.Enums;
using OptionProcessingService.Types;
using CommonObjects.Logger;

namespace OptionProcessingService
{
    public static class DB
    {
        private static string _connectionString;
        private static IList<Symbol> _dbSymbols;
        private const int SECOND = 1000;
        private const int MINUTE = 1000 * 60;
        private const int FIVEMINUTE = 1000 * 60 * 5;
        private const int QUARTERHOUR = 1000 * 60 * 15;
        private const int HALFHOUR = 1000 * 60 * 30;
        private const int HOUR = 1000 * 60 * 60;

        static DB()
        {
            string dataSource = System.Configuration.ConfigurationSettings.AppSettings["db_data_source"];
            string initCat = System.Configuration.ConfigurationSettings.AppSettings["db_init_catalog"];
            string userName = System.Configuration.ConfigurationSettings.AppSettings["db_UserName"];
            string password = System.Configuration.ConfigurationSettings.AppSettings["db_Password"];
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                _connectionString = @"Data Source=" + dataSource + ";Initial Catalog=" + initCat + ";User Id = "+userName+"; Password = '"+ password+"';";
            }
            else
            {
                _connectionString = "Data Source=" + dataSource + ";Initial Catalog=" + initCat + ";Integrated Security=True;";
            }
            _dbSymbols = GetSymbols();
        }

        //private static IList<Symbol> GetSymbols()
        //{
        //    var symbols = new List<Symbol>();
        //    using (var aConnection = new SqlConnection(_connectionString))
        //    {
        //        SqlTransaction transaction = null;
        //        try
        //        {
        //            aConnection.Open();
        //            transaction = aConnection.BeginTransaction();

        //            var cmd = new SqlCommand("SELECT * FROM SysmbolsNew",
        //                                       aConnection, transaction);

        //            using (var reader = cmd.ExecuteReader())
        //            {

        //                if (!reader.HasRows)
        //                    return symbols;

        //                while (reader.Read())
        //                {
        //                    symbols.Add(new Symbol
        //                    {
        //                        ID = ((int)reader["ID"]).ToString(CultureInfo.InvariantCulture).Trim(),
        //                        Name = ((string)reader["ShortName"]).Trim(),
        //                        Type = (SecurityType)(int)reader["InstrumentTypeID"]
        //                    });
        //                }
        //            }

        //            transaction.Commit();
        //        }
        //        catch (Exception e)
        //        {
        //            if (transaction != null) transaction.Rollback();
        //            Logger.Error(e);
        //        }
        //    }
        //    return symbols;
        //}
        public static IList<Symbol> GetSymbols()
        {
            var symbols = new List<Symbol>();
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    aConnection.Open();

                    transaction = aConnection.BeginTransaction();

                    var cmd = new SqlCommand("p_GetSymbols",
                                               aConnection, transaction);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (!reader.HasRows)
                        {
                            return symbols;
                        }
                        while (reader.Read())
                        {
                            symbols.Add(new Symbol
                            {
                                ID = ((int)reader["ID"]).ToString(CultureInfo.InvariantCulture).Trim(),
                                Name = ((string)reader["Name"]).Trim(),
                                ShortName= ((string)reader["ShortName"]).Trim(),
                                Type = (SecurityType)(int)reader["InstrumentTypeID"],
                                Feed1ProviderName = reader["Feed1ProviderName"] is DBNull ? string.Empty : ((string)reader["Feed1ProviderName"]).Trim(),
                                Feed1Name = reader["Feed1Name"] is DBNull ? string.Empty : ((string)reader["Feed1Name"]).Trim(),
                                Feed1ShortName = reader["Feed1ShortName"] is DBNull ? string.Empty : ((string)reader["Feed1ShortName"]).Trim(),
                                Feed2ProviderName = reader["Feed2ProviderName"] is DBNull?string.Empty:((string)reader["Feed2ProviderName"]).Trim(),
                                Feed2Name = reader["Feed2Name"] is DBNull ? string.Empty : ((string)reader["Feed2Name"]).Trim(),
                                Feed2ShortName = reader["Feed2ShortName"] is DBNull ? string.Empty : ((string)reader["Feed2ShortName"]).Trim(),
                            });
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    Logger.Error(e);
                }
            }
            return symbols;
        }

        public static Account GetAccountByUserID(string id)
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();
                    var cmd = new SqlCommand("SELECT u.ID, u.Login, a.Balance, a.Email FROM Users u INNER JOIN AccountInfo a on u.ID = a.UserID " +
                                             "WHERE u.ID = '" + id + "'", aConnection, transaction);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        while (reader.Read())
                        {
                            return new Account
                            {
                                ID = ((string)reader["ID"]).Trim(),
                                Username = ((string)reader["Login"]).Trim(),
                                Balance = (double)reader["Balance"],
                                Email = ((string)reader["Email"]).Trim()
                            };
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    Logger.Error(e);
                }
            }
            return null;
        }

        public static Account GetAccountByUserName(string userName)
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();

                    transaction = aConnection.BeginTransaction();

                    var cmd = new SqlCommand("SELECT u.ID, u.Login, a.Balance, a.Email FROM Users u INNER JOIN AccountInfo a on u.ID = a.UserID " +
                                             "WHERE u.Login = '"+userName+"'",
                                               aConnection, transaction);

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (!reader.HasRows)
                            return null;

                        while (reader.Read())
                        {
                            return new Account
                            {
                                ID = (reader["ID"].ToString()).Trim(),
                                Username = ((string)reader["Login"]).Trim(),
                                Balance = (double)reader["Balance"],
                                Email = ((string)reader["Email"]).Trim()
                            };
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                }
            }
            return null;
        }

        public static bool UpdateAccount(Account account)
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("UPDATE [AccountInfo] SET [AccountInfo].[Balance] = @balance WHERE [AccountInfo].[UserID] = @id", aConnection);

                cmd.Parameters.AddWithValue("id", account.ID);
                cmd.Parameters.AddWithValue("balance", account.Balance);

                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();
                    cmd.Transaction = transaction;

                    var value = cmd.ExecuteNonQuery();

                    transaction.Commit();

                    if (value > 0)
                        return true;
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    Logger.Error(e);
                }
            }

            return false;
        }

        public static bool AddUser(string login, string password, string email, CreditCard creditCard)
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;
                var userID = Guid.NewGuid().ToString();
                var accountID = Guid.NewGuid().ToString();
                var creditCardID = Guid.NewGuid().ToString();

                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();

                    var cmd = new SqlCommand("INSERT INTO Users ([ID],[Login],[Password]) VALUES(@id, @login, @password)", aConnection);
                    cmd.Parameters.AddWithValue("id", userID);
                    cmd.Parameters.AddWithValue("login", login);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Transaction = transaction;
                    var value = cmd.ExecuteNonQuery();

                    if (value > 0)
                    {
                        cmd = new SqlCommand("INSERT INTO AccountInfo ([ID],[UserID],[Balance],[Email]) VALUES(@id, @userId, @balance, @email)", aConnection);
                        cmd.Parameters.AddWithValue("id", accountID);
                        cmd.Parameters.AddWithValue("userId", userID);
                        cmd.Parameters.AddWithValue("balance", 0);
                        cmd.Parameters.AddWithValue("email", email);
                        cmd.Transaction = transaction;
                        value = cmd.ExecuteNonQuery();

                        if (value > 0)
                        {
                            cmd = new SqlCommand("INSERT INTO CreditCards ([ID],[AccountInfoID],[OwnerName],[CardNumber],[ExpiryMonth],[ExpiryYear],[CVV]) " +
                                                 "VALUES(@id, @accountInfoId, @ownerName, @cardNumber, @expiryMonth, @expiryYear, @cvv)", aConnection);
                            cmd.Parameters.AddWithValue("id", creditCardID);
                            cmd.Parameters.AddWithValue("accountInfoId", accountID);
                            cmd.Parameters.AddWithValue("ownerName", creditCard.OwnerName);
                            cmd.Parameters.AddWithValue("cardNumber", creditCard.Number);
                            cmd.Parameters.AddWithValue("expiryMonth", creditCard.ExpiryMonth);
                            cmd.Parameters.AddWithValue("expiryYear", creditCard.ExpiryYear);
                            cmd.Parameters.AddWithValue("cvv", creditCard.CVV);
                            cmd.Transaction = transaction;
                            value = cmd.ExecuteNonQuery();

                            if (value > 0)
                            {
                                transaction.Commit();
                                return true;
                            }
                        }
                    }

                    transaction.Rollback();
                    return false;

                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    Logger.Error(e);
                }
            }
            return false;
        }

        public static User GetUserByName(string name)
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();

                    transaction = aConnection.BeginTransaction();

                    var cmd = new SqlCommand("SELECT * FROM [Users] WHERE Login = '"+name+"'",
                                               aConnection, transaction);

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (!reader.HasRows)
                            return null;

                        while (reader.Read())
                        {
                            return new User
                            {
                                ID = (reader["ID"].ToString()).Trim(),
                                Login = ((string)reader["Login"]).Trim(),
                                Password = ((string)reader["Password"]).Trim() 
                            };
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    Logger.Error(e);
                }
            }
            return null;
        }

        public static IList<Asset> GetAssets()
        {
            var assets = new List<Asset>();
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();
                    var cmd = new SqlCommand("SELECT a.ID, a.ReturnCoefficient, a.IsClosable, s.ShortName, s.InstrumentTypeID as SecType " +
                                             "FROM Assets a INNER JOIN SysmbolsNew s on a.Symbol = s.ID", aConnection, transaction);

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (!reader.HasRows)
                            return assets;

                        while (reader.Read())
                        {
                            assets.Add(new Asset
                            {
                                ID = ((int)reader["ID"]).ToString(CultureInfo.InvariantCulture).Trim(),
                                Symbol = new Symbol
                                {
                                    Name = ((string)reader["ShortName"]).Trim(),
                                    Type = (SecurityType)(int)reader["SecType"]
                                },
                                ReturnCoefficient = (double)reader["ReturnCoefficient"],
                                IsClosable = (byte)reader["IsClosable"] == 1
                            });
                        }
                    }

                    foreach (var asset in assets)
                    {
                        cmd = new SqlCommand("SELECT Timestamp FROM ExpiryTimestamps WHERE AssetID=" + asset.ID + " AND Timestamp > " + DataProvider.GetCurrentTimestamp(),
                                               aConnection, transaction);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                                while (reader.Read())
                                {
                                    //asset.ExpiryTimestamps.Add((long)reader["Timestamp"]);
                                    //asset.ExpiryDatetime.Add(DataProvider.TimestampToDateTime((long)reader["Timestamp"]).ToString());
                                    asset.ExpiryTimeList.Add(new ExpiryTime()
                                    {
                                        ExpiryDatetime = DataProvider.TimestampToDateTime((long)reader["Timestamp"]).ToString(),
                                        ExpiryTimestamps = (long)reader["Timestamp"]
                                    });
                                }
                        }
                    }

                    transaction.Commit();

                    for (var i = assets.Count - 1; i >= 0; i--)
                    {
                        if (assets[i].ExpiryTimeList.Count == 0)
                        {
                            assets.RemoveAt(i);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    Logger.Error(e);
                }
            }
            return assets;
        }

        public static IList<Order> GetAllOrders()
        {
            var orders = new List<Order>();
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();

                    transaction = aConnection.BeginTransaction();

                    var cmd = new SqlCommand("SELECT o.ID, o.UserID, o.OptionType, o.OrderType, o.ExpressExpiryInSeconds, o.IsClosable, o.Investment, o.ReturnCoefficient, o.MarketPrice, o.State, " +
                                             "o.CreationTimestamp, o.ExpiryTimestamp, o.ExecuteTimestamp, o.Successful, s.ShortName as SymName, s.InstrumentTypeID as SecType, ISNULL(o.ProfitLoss,0) AS ProfitLoss, ISNULL(o.ExpiryPrice,0) AS ExpiryPrice " +
                                             "FROM Orders o INNER JOIN SysmbolsNew s on o.Symbol = s.ID",
                                               aConnection, transaction);

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (!reader.HasRows)
                            return orders;

                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                ID = ((string)reader["ID"]).Trim(),
                                UserID = ((string)reader["UserID"]).Trim(),
                                OptionType = (OptionType)(byte)reader["OptionType"],
                                OrderType = (OrderType)(byte)reader["OrderType"],
                                ExpressExpiryInSeconds = (short)reader["ExpressExpiryInSeconds"],
                                Symbol = new Symbol
                                {
                                    Name = ((string)reader["SymName"]).Trim(),
                                    Type = (SecurityType)(int)reader["SecType"]
                                },
                                IsClosable = (byte)reader["IsClosable"] == 1,
                                Investment = (double)reader["Investment"],
                                ReturnCoefficient = (double)reader["ReturnCoefficient"],
                                MarketPrice = (double)reader["MarketPrice"],
                                State = (OrderState)(int)reader["State"],
                                CreationTimestamp = (long)reader["CreationTimestamp"],
                                ExpiryTimestamp = (long)reader["ExpiryTimestamp"],
                                ExecutionTimestamp = (long)reader["ExecuteTimestamp"],
                                Successful = (byte)reader["Successful"] == 1,
                                ProfitLoss=(double)reader["ProfitLoss"],
                                ExpiryPrice=(double)reader["ExpiryPrice"]
                            });
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                }
            }
            return orders;
        }

        public static bool AddOrder(Order order)
        {
            var symbolId = GetSymbolIdByName(order.Symbol.Name);
            if (symbolId == null) return false;

            using (var aConnection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("INSERT INTO Orders ([ID],[UserID],[Symbol],[OptionType],[OrderType],ExpressExpiryInSeconds,[IsClosable],[Investment],[ReturnCoefficient]," +
                                         "[MarketPrice],[State],[CreationTimestamp],[ExpiryTimestamp],[ExecuteTimestamp],[Successful])" +
                                         "VALUES(@ID, @UserId, @Symbol, @OptionType, @OrderType, @ExpressExpiryInSeconds, @IsClosable, @Investment, @ReturnCoefficient, @MarketPrice, " +
                                         "@State, @CreationTs, @ExpiryTs, @ExecuteTs, @Successful)", aConnection);
                cmd.Parameters.AddWithValue("ID", order.ID);
                cmd.Parameters.AddWithValue("UserId", order.UserID);
                cmd.Parameters.AddWithValue("Symbol", Int32.Parse(symbolId));
                cmd.Parameters.AddWithValue("OptionType", order.OptionType);
                cmd.Parameters.AddWithValue("OrderType", order.OrderType);
                cmd.Parameters.AddWithValue("ExpressExpiryInSeconds", order.ExpressExpiryInSeconds);
                cmd.Parameters.AddWithValue("IsClosable", order.IsClosable ? 1 : 0);
                cmd.Parameters.AddWithValue("Investment", order.Investment);
                cmd.Parameters.AddWithValue("ReturnCoefficient", order.ReturnCoefficient);
                cmd.Parameters.AddWithValue("MarketPrice", order.MarketPrice);
                cmd.Parameters.AddWithValue("State", order.State);
                cmd.Parameters.AddWithValue("CreationTs", order.CreationTimestamp);
                cmd.Parameters.AddWithValue("ExpiryTs", order.ExpiryTimestamp);
                cmd.Parameters.AddWithValue("ExecuteTs", order.ExecuteTimestamp);
                cmd.Parameters.AddWithValue("Successful", order.Successful ? 1 : 0);
				cmd.Parameters.AddWithValue("profitLoss", order.ProfitLoss);
                SqlTransaction transaction = null;
                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();
                    cmd.Transaction = transaction;
                    var value = cmd.ExecuteNonQuery();
                    transaction.Commit();
                    if (value > 0)
{
                        return true;
}

                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    Logger.Error(e);
                }
            }

            return false;
        }

        public static bool UpdateOrder(Order order)
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("UPDATE Orders Set [State] = @state, [ExecuteTimestamp] = @executeTs, [Successful] = @successful WHERE [ID] = @id", aConnection);
                cmd.Parameters.AddWithValue("id", order.ID);
                cmd.Parameters.AddWithValue("state", order.State);
                cmd.Parameters.AddWithValue("executeTs", order.ExecuteTimestamp);
                cmd.Parameters.AddWithValue("successful", order.Successful ? 1 : 0);
                cmd.Parameters.AddWithValue("profitLoss", order.ProfitLoss);
                cmd.Parameters.AddWithValue("expiryPrice", order.ExpiryPrice);
                SqlTransaction transaction = null;
                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();
                    cmd.Transaction = transaction;
                    var value = cmd.ExecuteNonQuery();
                    transaction.Commit();
                    if (value > 0)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    if (transaction != null) transaction.Rollback();
                }
            }

            return false;
        }
public static List<ExpiryTime> GetExpiryAllDayTimestamp()
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();

                    transaction = aConnection.BeginTransaction();
                    var list = new List<ExpiryTime>();

                    var cmd = new SqlCommand("select UTCTimestamp,UTCTime from ExpiryAllTimestamps",
                                               aConnection, transaction);

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (!reader.HasRows)
                            return null;

                        while (reader.Read())
                        {
                            list.Add(new ExpiryTime
                            {
                                ExpiryTimestamps = ((long)reader["UTCTimestamp"]),
                                ExpiryDatetime = ((DateTime)reader["UTCTime"]).ToString()
                            }
                            );
                        }
                    }

                    transaction.Commit();
                    return list;
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                    Logger.Error(e);
                }
            }
            return null;

        }
        public static void UpdateAssets()
        {
            using (var aConnection = new SqlConnection(_connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    aConnection.Open();
                    transaction = aConnection.BeginTransaction();

                    List<int> assetIDs = new List<int>();
                    long startOfDayTimestamp = DataProvider.GetStartOfDayTimestamp();

                    var cmd = new SqlCommand("SELECT [ID] FROM Assets", aConnection, transaction);

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                            while (reader.Read())
                                assetIDs.Add((int)reader["ID"]);
                    }


                    foreach (var assetID in assetIDs)
                    {
                        for (var i = 1; i <= 288; i++)
                        {
                            cmd = new SqlCommand("INSERT INTO ExpiryTimestamps ([AssetID],[Timestamp])" +
                                         "VALUES(@assetId, @timestamp)", aConnection);

                            cmd.Parameters.AddWithValue("assetId", assetID);
                            cmd.Parameters.AddWithValue("timestamp", startOfDayTimestamp + FIVEMINUTE * i);

                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    if (transaction != null) transaction.Rollback();
                }
            }
        }

        private static string GetSymbolIdByName(string name)
        {
            foreach (var symbol in _dbSymbols)
            {
                if (symbol.Name == name || symbol.Feed1ShortName == name || symbol.Feed2ShortName == name)
                {
                    return symbol.ID;
                }
            }
            return null;
        }

    }
}
