using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace School.Management.Rest.API.Features.Authentication.Commands
{
    public record AuthenticateRequest(string UserName, string Password) : IRequest<AuthenticateResponse?>;
    public record AuthenticateResponse(Guid Id, string UserName, string FullName, string RolName);

    public class AuthenticateCommand(IConfiguration configuration) : IRequestHandler<AuthenticateRequest, AuthenticateResponse?>
    {
        public async Task<AuthenticateResponse?> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(configuration.GetConnectionString("Connection"));
                
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                
                DynamicParameters parameters = new();
                parameters.Add("@password", request.Password);
                parameters.Add("@username", request.UserName);
                
                string query = @"
                    SELECT
	                    U.Id
	                    ,U.UserName
	                    ,U.FullName
	                    ,R.[Name] AS RolName
                    FROM
	                    [Security].[User] AS U
	                    INNER JOIN [Security].[Role] AS R ON R.Id = U.RoleId
                    WHERE
	                    U.UserName = @username
	                    AND CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE(@password, U.[Password])) = @password
	                    AND U.[Enabled] = 1;
                ";
                
                AuthenticateResponse? response = await connection.QueryFirstOrDefaultAsync<AuthenticateResponse>(query, parameters);
                
                connection.Close();

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
