using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PCR.Models;

namespace PCR.Repository
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IConfiguration _configuration;
        public AuditRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    }
}