using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace School.Management.Rest.API.Features.Administrator.Commands
{
    public record AddAdministratorRequest(string FullName, string Mail, string IdNumber, string Address, DateTime Birthday) : IRequest<Unit>;

    public class AddAdministratorHandler(IConfiguration configuration) : IRequestHandler<AddAdministratorRequest, Unit>
    {
        public async Task<Unit> Handle(AddAdministratorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(configuration.GetConnectionString("Connection"));

                string username = string.Empty;
                string password = Guid.NewGuid().ToString();

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                string[] names = request.FullName.Split(' ');
                username = string.Concat(names[2], names[0].AsSpan(0)[0], names[1].AsSpan(0)[0]);

                DynamicParameters parameters = new();
                parameters.Add("@fullname", request.FullName);
                parameters.Add("@password", password);
                parameters.Add("@username", username);
                parameters.Add("@idnumber", request.IdNumber);
                parameters.Add("@birthday", request.Birthday, dbType: DbType.Date);
                parameters.Add("@address", request.Address);
                parameters.Add("@mail", request.Mail);

                string query = @"
                    INSERT INTO [Security].[User] VALUES (
	                    NEWID()
	                    ,@username
	                    ,ENCRYPTBYPASSPHRASE(@password, @password)
	                    ,1
	                    ,@fullname
	                    ,@idnumber
	                    ,GETDATE()
	                    ,@address
	                    ,@mail
	                    ,'7680904A-289A-4CAD-881E-D9CED0321C44'
                    );
                ";

                await connection.ExecuteAsync(query, parameters);

                connection.Close();

                return Unit.Value;
            }
            catch
            {
                throw;
            }
        }
    }
}