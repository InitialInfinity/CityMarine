using Context;
using Dapper;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;
namespace Master.Repository
{
    public class UnitConfigurationRepository : IUnitConfigurationRepository
    {
        private readonly DapperContext _context;
        public UnitConfigurationRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> UnitConfiguration(UnitConfiguration model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_UnitConfiguration", SetParameter(model), commandType: CommandType.StoredProcedure);
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
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {                       
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
        public async Task<IActionResult> Get(UnitConfiguration model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_UnitConfiguration", SetParameter(model), commandType: CommandType.StoredProcedure);
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
                        return new ObjectResult(result)
                        {
                            StatusCode = 200
                        };
                    }
                    else
                    {                       
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
        public DynamicParameters SetParameter(UnitConfiguration user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@userId", user.UserId, DbType.Guid);
            parameters.Add("@u_id", user.u_id, DbType.Guid);
            parameters.Add("@u_unit", user.u_unit, DbType.String);
            parameters.Add("@u_height", user.u_height, DbType.String);
            parameters.Add("@u_amount", user.u_amount, DbType.String);
            parameters.Add("@u_size", user.u_size, DbType.String);
            parameters.Add("@u_width", user.u_width, DbType.String);
            parameters.Add("@u_updateddate", user.u_updateddate, DbType.DateTime);
            parameters.Add("@u_createddate", user.u_createddate, DbType.DateTime);
            parameters.Add("@u_isactive", user.u_isactive, DbType.String);        
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
           return parameters;
        }
    }
}
