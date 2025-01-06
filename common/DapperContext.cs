
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        public DapperContext(IConfiguration configuration)//,string Connectionstring)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
            //_connectionString = _configuration.GetConnectionString("SqlConnection") == null ? _configuration.GetConnectionString("Server") : _configuration.GetConnectionString("SqlConnection");

        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
        public IDbConnection CreateConnection(string server)
        {
            string connectionString = _configuration.GetConnectionString(server);
            return new SqlConnection(connectionString);
        }
           //=> new SqlConnection(_configuration.GetConnectionString(server));
           //=> new SqlConnection("Server_1");
        //public DapperContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        //{
        //    _configuration = configuration;
        //    _connectionString = GetConnectionStringFromSession(httpContextAccessor.HttpContext) ?? GetServerConnectionString();
        //}

        //public IDbConnection CreateConnection()
        //{
        //    return new SqlConnection(_connectionString);
        //}

        //private string? GetConnectionStringFromSession(HttpContext httpContext)
        //{
        //    // Replace this with your logic to get the session key
        //    string sessionKey = httpContext.Session.GetString("Server");
        //    string? sessionConnectionString = _configuration.GetConnectionString(sessionKey);

        //    return sessionConnectionString;
        //}

        //private string GetServerConnectionString()
        //{
        //    // Replace this with your logic to determine which "Server" to use
        //    string selectedServer = "SqlConnection";  // Example: You can calculate this based on session or other conditions

        //    string serverConnectionString = _configuration.GetConnectionString(selectedServer);
        //    return serverConnectionString;
        //}
    }
}
