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
    public class InboxEmailRepository:IInboxEmailRepository
    {
        private readonly DapperContext _context;
        public InboxEmailRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> InboxEmail(InboxEmailModel model)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_InboxEmail", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(InboxEmailModel model)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_InboxEmail", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public DynamicParameters SetParameter(InboxEmailModel user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@i_id", user.i_id, DbType.String);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@i_from", user.i_from, DbType.String);
            parameters.Add("@i_to", user.i_to, DbType.String);
            parameters.Add("@i_subject", user.i_subject, DbType.String);
            parameters.Add("@i_body", user.i_body, DbType.String);
            parameters.Add("@i_messageid", user.i_messageid, DbType.String);
            parameters.Add("@i_replyto", user.i_replyto, DbType.String);
            parameters.Add("@i_type", user.i_type, DbType.String);
            parameters.Add("@i_receiveddate", user.i_receiveddate, DbType.String);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
