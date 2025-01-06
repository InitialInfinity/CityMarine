using Context;
using Dapper;
using Master.Entity;
using Master.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Tokens;

namespace Master.Repository
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DapperContext _context;
        public StaffRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Staff(Staff model)
        {
            using (var connection = _context.CreateConnection(model.BaseModel.Server_Value))
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_Staff", SetParameter(model), commandType: CommandType.StoredProcedure);
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

        public async Task<IActionResult> Get(Staff model)
        {
            using (var connection = _context.CreateConnection(model.BaseModel.Server_Value))
            {
                //Guid? userIdValue = (Guid)user.UserId;
                try
                {

                    var sqlConnection = (Microsoft.Data.SqlClient.SqlConnection)connection;
                    await sqlConnection.OpenAsync();

                    var queryResult = await connection.QueryMultipleAsync("proc_Staff", SetParameter(model), commandType: CommandType.StoredProcedure);
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



        public DynamicParameters SetParameter(Staff user)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OperationType", user.BaseModel.OperationType, DbType.String);
            parameters.Add("@userId", user.UserId, DbType.Guid);
            parameters.Add("@st_id", user.st_id, DbType.Guid);
            parameters.Add("@st_staff_name", user.st_staff_name, DbType.String);
            parameters.Add("@st_email", user.st_email, DbType.String);
            parameters.Add("@st_address", user.st_address, DbType.String);
            parameters.Add("@st_contact", user.st_contact, DbType.String);
            parameters.Add("@st_dob", user.st_dob, DbType.DateTime);
            parameters.Add("@st_gender", user.st_gender, DbType.String);
            parameters.Add("@st_salary", user.st_salary, DbType.String);
            parameters.Add("@st_joining_date", user.st_joining_date, DbType.DateTime);
            parameters.Add("@st_com_id", user.st_com_id, DbType.Guid);
            parameters.Add("@st_state_id", user.st_state_id, DbType.String);
            parameters.Add("@st_city_id", user.st_city_id, DbType.String);
            parameters.Add("@st_country_id", user.st_country_id, DbType.String);
            parameters.Add("@st_designation_id", user.st_designation_id, DbType.String);
            parameters.Add("@st_department_id", user.st_department_id, DbType.String);
            parameters.Add("@st_bloodgroup", user.st_bloodgroup, DbType.String);
            parameters.Add("@st_isactive", user.st_isactive, DbType.String);
            parameters.Add("@st_country_code", user.co_country_code, DbType.String);
            parameters.Add("@st_rolename", user.st_rolename, DbType.String);
            parameters.Add("@st_category", user.st_category, DbType.String);
            parameters.Add("@st_username", user.st_username, DbType.String);
            parameters.Add("@st_staff_code", user.st_staff_code, DbType.String);
            parameters.Add("@st_left_date", user.st_left_date, DbType.DateTime);
            parameters.Add("@st_createddate", user.st_createddate, DbType.DateTime);
            parameters.Add("@st_updateddate", user.st_updateddate, DbType.DateTime);

            parameters.Add("@OutcomeId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@OutcomeDetail", dbType: DbType.String, size: 4000, direction: ParameterDirection.Output);


            return parameters;

        }
    }
}
