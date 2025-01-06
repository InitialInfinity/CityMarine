using Context;
using Dapper;
using Master.API.Entity;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Tokens;

namespace Master.Repository
{
    public class ServerAllocationRepository: IServerAllocation
    {
        private readonly DapperContext _context;
        public ServerAllocationRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetAll(CompanyDetails user)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_ServerDetails", SetParameter(user), commandType: CommandType.StoredProcedure);
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
        public async Task<IActionResult> Allocation(CompanyDetails user)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {
                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_ServerDetails", SetParameter(user), commandType: CommandType.StoredProcedure);
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
        public async Task<IActionResult> AllocationCompany(LoginDetails user)
        {
            using (IDbConnection connection = new SqlConnection(user.BaseModel.Server_Value))
            //using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_LoginDetails", SetLogin(user), commandType: CommandType.StoredProcedure);
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
        public DynamicParameters SetParameter(CompanyDetails user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@com_id", user.com_id, DbType.Guid);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@com_name", user.com_company_name, DbType.String);
            parameters.Add("@sub_id", user.sub_id, DbType.String);
            parameters.Add("@py_id", user.py_id, DbType.String);
            parameters.Add("@ip_address", user.com_company_name, DbType.String);
            parameters.Add("@browser_name", user.com_company_name, DbType.String);
            parameters.Add("@browser_version", user.com_company_name, DbType.String);
            parameters.Add("@com_code", user.com_code, DbType.String);
            parameters.Add("@is_signIn", user.com_company_name, DbType.String);
            parameters.Add("@Contact_no", user.com_contact, DbType.String);
            parameters.Add("@Server_Key", user.Server_Key, DbType.String);
            parameters.Add("@Server_Value", user.Server_Value, DbType.String);
            parameters.Add("@ser_updateddate", user.ser_updateddate, DbType.DateTime);
            parameters.Add("@ser_createddate", user.ser_createddate, DbType.DateTime);
            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);


            return parameters;

        }
        public DynamicParameters SetLogin(LoginDetails user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);

            parameters.Add("@Id", user.Id, DbType.Guid);
             parameters.Add("@com_id", user.com_id, DbType.String);
             parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@Contact_no", user.Contact_no, DbType.String);
            parameters.Add("@com_code", user.com_code, DbType.String);
            parameters.Add("@com_password", user.com_password, DbType.String);
            parameters.Add("@EmailId", user.EmailId, DbType.String);
            parameters.Add("@CountryId", user.CountryId, DbType.String);
            parameters.Add("@com_company_name", user.com_company_name, DbType.String);
            parameters.Add("@CreatedDate", user.CreatedDate, DbType.DateTime);
            //parameters.Add("@NewPassword", user.NewPassword, DbType.String);
            //parameters.Add("@IMEI_No", user.IMEI_No, DbType.String);
            //parameters.Add("@DeviceId", user.DeviceId, DbType.String);
            //parameters.Add("@PlayerId", user.PlayerId, DbType.String);

            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);
            return parameters;

        }


    }
}
