using Context;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;
using Master.Entity;
using Master.Repository.Interface;

namespace Master.Repository

{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DapperContext _context;

        public DashboardRepository(DapperContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Get(Dashboard model)
        {
            using (IDbConnection connection = _context.CreateConnection(model.BaseModel.Server_Value))
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_Dashboard", SetParameter(model), commandType: CommandType.StoredProcedure);
                    var Model = queryResult.ReadSingleOrDefault<Object>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;




                    var result = new Result
                    {

                        Outcome = outcome,
                        Data = Model
                        //UserId = userIdValue
                    };

                    if (outcomeId == 1)
                    {
                        // Login successful
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        // Login failed
                        return new ObjectResult(result)
                        {
                            StatusCode = 400
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<IActionResult> GetList(Dashboard model)
        {
            using (IDbConnection connection = _context.CreateConnection(model.BaseModel.Server_Value))
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_Dashboard", SetParameter(model), commandType: CommandType.StoredProcedure);
                    var Model = queryResult.Read<Object>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;




                    var result = new Result
                    {

                        Outcome = outcome,
                        Data = Model
                        //UserId = userIdValue
                    };

                    if (outcomeId == 1)
                    {
                        // Login successful
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        // Login failed
                        return new ObjectResult(result)
                        {
                            StatusCode = 400
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }



       

        public DynamicParameters SetParameter(Dashboard user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);

            parameters.Add("@com_id", user.com_id, DbType.String);
            parameters.Add("@monthlysale", user.monthlysale, DbType.String);
            parameters.Add("@monthlyrollpurchase", user.monthlyrollpurchase, DbType.String);
            parameters.Add("@monthlyexpense", user.monthlyexpense, DbType.String);
            parameters.Add("@monthlyprofit", user.monthlyprofit, DbType.String);
            parameters.Add("@monthlyquotation", user.monthlyquotation, DbType.String);
            parameters.Add("@monthlycashorder", user.monthlycashorder, DbType.String);

            parameters.Add("@dailysale", user.dailysale, DbType.String);
            parameters.Add("@dailyadvance", user.dailyadvance, DbType.String);
            parameters.Add("@dailyexpense", user.dailyexpense, DbType.String);
            parameters.Add("@dailybalance", user.dailybalance, DbType.String);


            parameters.Add("@totalamount", user.totalamount, DbType.String);
            parameters.Add("@totaladvance", user.totaladvance, DbType.String);
            parameters.Add("@totalpurchase", user.totalpurchase, DbType.String);
            parameters.Add("@totalexpense", user.totalexpense, DbType.String);
            parameters.Add("@totalprofit ", user.totalprofit, DbType.String);


            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);


            return parameters;

        }
    }
}
