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
    public class InboxClientRepository:IInboxClientRepository
    {
        private readonly DapperContext _context;
        public InboxClientRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> InboxClient(InboxClientModel model)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_InboxEmailNew", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(InboxClientModel model)
        {
            using (var connection = _context.CreateConnection())
            {
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_InboxEmailNew", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public DynamicParameters SetParameter(InboxClientModel user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@i_id", user.ic_id, DbType.String);
            parameters.Add("@ic_count", user.ic_count, DbType.String);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@ic_year", user.ic_year, DbType.String);
            parameters.Add("@ic_to", user.ic_to, DbType.String);
            parameters.Add("@ic_subject", user.ic_subject, DbType.String);
            parameters.Add("@ic_body", user.ic_body, DbType.String);
            parameters.Add("@ic_from", user.ic_from, DbType.String);
            parameters.Add("@clientid", user.clientid, DbType.String);
            parameters.Add("@ic_type", user.ic_type, DbType.String);
            parameters.Add("@ic_attachment", user.ic_attachment, DbType.String);
            parameters.Add("@ic_receiveddate", user.ic_receiveddate, DbType.String);            
            parameters.Add("@ic_claimno", user.ic_claimno, DbType.String);            
            parameters.Add("@ic_enquiryno", user.ic_enquiryno, DbType.String);            
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;
        }
    }
}
