using Context;
using Dapper;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Repository
{
    public class SubscriptionMasterRepository : ISubscriptionMasterRepository
    {
        private readonly DapperContext _context;
        public SubscriptionMasterRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Subscription(Subscription model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_SubscriptionMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(Subscription model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_SubscriptionMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public DynamicParameters SetParameter(Subscription user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@sm_id", user.sm_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@package_name", user.package_name, DbType.String);
            parameters.Add("@com_isactive", user.com_isactive, DbType.String);
            parameters.Add("@sm_service", user.sm_service, DbType.String);
            parameters.Add("@subAmount", user.subAmount, DbType.String);
            parameters.Add("@subDiscount", user.subDiscount, DbType.String);
            parameters.Add("@final_Amount", user.final_Amount, DbType.String);
            parameters.Add("@Invoice", user.Invoice, DbType.String);
            parameters.Add("@Quotation", user.Quotation, DbType.String);
            parameters.Add("@Expence", user.Expence, DbType.String);
            parameters.Add("@cash_order", user.cash_order, DbType.String);
            parameters.Add("@sub_duration", user.sub_duration, DbType.String);
            parameters.Add("@com_updateddate", user.com_updateddate, DbType.DateTime);
            parameters.Add("@com_createddate", user.com_createddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);


            return parameters;
        }
    }
}
