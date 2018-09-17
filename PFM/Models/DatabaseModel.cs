using System.Data.SqlClient;

namespace PFM
{
    public interface IDataBase
    {
        SqlConnection Connection { get; set; }
        void Connect();
        void Open();
        void Close();
    }


    public class LocalDatabase : IDataBase
    {
        public SqlConnection Connection { get; set; }

        public LocalDatabase()
        {
            Connect();
        }

        public void Connect()
        {
            Connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PFMDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }
    };

    public class CloudDatabase : IDataBase
    {
        public SqlConnection Connection { get; set; }

        public CloudDatabase()
        {
            Connect();
        }

        public void Connect()
        {
            var sqlConnection = new SqlConnectionStringBuilder();
            sqlConnection.DataSource = "pfmdb.database.windows.net";
            sqlConnection.UserID = "domerobi";
            sqlConnection.Password = "Dome0322";
            sqlConnection.InitialCatalog = "PFMDB";
            Connection = new SqlConnection(sqlConnection.ConnectionString);
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}
