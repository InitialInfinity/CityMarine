using Context;
using Dapper;
using Master.API.Entity;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Repository
{
    public class SentEmailRepository:ISentEmailRepository
    {
        private readonly DapperContext _context;
        public SentEmailRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> SentEmail(SentEmailModel model)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_SentEmail", SetParameter(model), commandType: CommandType.StoredProcedure);
                    var Model = queryResult.Read<Object>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    var result = new Result
                    {
                        Outcome = outcome,
                        Data = Model
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

        public async Task<IActionResult> Get(SentEmailModel model)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_SentEmail", SetParameter(model), commandType: CommandType.StoredProcedure);
                    var Model = queryResult.ReadSingleOrDefault<Object>();
                    var outcome = queryResult.ReadSingleOrDefault<Outcome>();
                    var outcomeId = outcome?.OutcomeId ?? 0;
                    var outcomeDetail = outcome?.OutcomeDetail ?? string.Empty;
                    var result = new Result
                    {
                        Outcome = outcome,
                        Data = Model
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

        public DynamicParameters SetParameter(SentEmailModel user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@s_id", user.s_id, DbType.String);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@s_from", user.s_from, DbType.String);
            parameters.Add("@s_to", user.s_to, DbType.String);
            parameters.Add("@s_subject", user.s_subject, DbType.String);
            parameters.Add("@s_body", user.s_body, DbType.String);
            parameters.Add("@s_messageid", user.s_messageid, DbType.String);
            parameters.Add("@s_replyto", user.s_replyto, DbType.String);
            parameters.Add("@s_type", user.s_type, DbType.String);
            parameters.Add("@s_sentdate", user.s_sentdate, DbType.String);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
