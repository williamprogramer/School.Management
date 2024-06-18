using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using System.Data;

namespace School.Management.Rest.API.Features.Administrator.Queries
{
    public record GetAdministratorsRequest(int Page = 1, int Size = 5) : IRequest<IEnumerable<GetAdministratorsResponse>>;
    public record GetAdministratorsResponse(string UserName, string FullName, string IdNumber, string Mail);

    public class GetAdministratorsHandler(IConfiguration configuration) : IRequestHandler<GetAdministratorsRequest, IEnumerable<GetAdministratorsResponse>>
    {
        public async Task<IEnumerable<GetAdministratorsResponse>> Handle(GetAdministratorsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(configuration.GetConnectionString("Connection"));

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                DynamicParameters parameters = new();
                parameters.Add("@page", request.Page);
                parameters.Add("@size", request.Size);

                string query = @"
                    SELECT
	                    [UserName]
	                    ,[FullName]
	                    ,[IdNumber]
	                    ,[Mail]
                    FROM
	                    [Security].[User]
                    ORDER BY
	                    FullName
                    OFFSET (@page - 1) * @size ROWS
                    FETCH NEXT @size ROWS ONLY
                ";
                IEnumerable<GetAdministratorsResponse> responses = await connection.QueryAsync<GetAdministratorsResponse>(query, parameters);
                await connection.CloseAsync();

                return responses;
            }
            catch
            {
                throw;
            }
        }
    }
}