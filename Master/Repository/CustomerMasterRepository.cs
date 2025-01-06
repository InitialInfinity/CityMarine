using Context;
using Dapper;
using Master.Entity;

using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;
using System.Data.SqlClient;


namespace Master.Repository
{
    public class CustomerMasterRepository:ICustomerMasterRepository
    {
        private readonly DapperContext _context;

        public CustomerMasterRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Customer(CustomerMaster model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();
                    var queryResult = await connection.QueryMultipleAsync("proc_CustomerMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(CustomerMaster model)
        {
            using (var connection = _context.CreateConnection())
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_CustomerMaster", SetParameter(model), commandType: CommandType.StoredProcedure);
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
                    
        public DynamicParameters SetParameter(CustomerMaster user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@c_id", user.c_id, DbType.String);
            parameters.Add("@c_ccode", user.c_ccode, DbType.String);
            parameters.Add("@c_com_id",user.c_com_id, DbType.String);
            parameters.Add("@UserId", user.UserId, DbType.Guid);
            parameters.Add("@c_name", user.c_name, DbType.String);
            parameters.Add("@c_address", user.c_address, DbType.String);
            parameters.Add("@c_contact", user.c_contact, DbType.String);
            parameters.Add("@c_contact2", user.c_contact2, DbType.String);
            parameters.Add("@c_code", user.co_country_code, DbType.String);
           
            parameters.Add("@c_email", user.c_email, DbType.String);
            parameters.Add("@c_dob", user.c_dob, DbType.String);
            parameters.Add("@c_anidate", user.c_anidate, DbType.Date);
          
            parameters.Add("@c_note", user.c_note, DbType.String);
          
            parameters.Add("@c_isactive", user.c_isactive, DbType.String);
            parameters.Add("@c_createddate", user.c_createddate, DbType.DateTime);
            parameters.Add("@c_updateddate", user.c_updateddate, DbType.DateTime);
          
            if (user.DataTable != null && user.DataTable.Rows.Count > 0)
            {
                parameters.Add("@CustomerAttachment", user.DataTable.AsTableValuedParameter("[dbo].[Tbl_CustomerAttachment]"));
            }

            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);


            return parameters;

        }
    }
}
