using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.System;
using Ums.Core.Entities.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class SettingService : Service<SystemSetting>, ISettingService
    {
        private DbContext _dbContext;
        public SettingService(DbContext context, IContextService contextService) : base(context, contextService)
        {
            _dbContext = context;
        }

        public string GetValue(string key)
        {
            return Get(key)?.Value ?? string.Empty;
        }

        public SystemSetting Get(string key)
        {
            return Gets().FirstOrDefault(i => i.Key == key);
        }

        public List<DbStat> GetDbStat()
        {
            var query = @"SELECT 
                            t.NAME AS TableName,
                            s.Name AS SchemaName,
                            p.rows,
                            SUM(a.total_pages) * 8 AS TotalSpaceKB, 
                            CAST(ROUND(((SUM(a.total_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS TotalSpaceMB,
                            SUM(a.used_pages) * 8 AS UsedSpaceKB, 
                            CAST(ROUND(((SUM(a.used_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS UsedSpaceMB, 
                            (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB,
                            CAST(ROUND(((SUM(a.total_pages) - SUM(a.used_pages)) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS UnusedSpaceMB
                        FROM 
                            sys.tables t
                        INNER JOIN      
                            sys.indexes i ON t.OBJECT_ID = i.object_id
                        INNER JOIN 
                            sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                        INNER JOIN 
                            sys.allocation_units a ON p.partition_id = a.container_id
                        LEFT OUTER JOIN 
                            sys.schemas s ON t.schema_id = s.schema_id
                        WHERE 
                            t.NAME NOT LIKE 'dt%' 
                            AND t.is_ms_shipped = 0
                            AND i.OBJECT_ID > 255 
                        GROUP BY 
                            t.Name, s.Name, p.Rows
                        ORDER BY 
                            TotalSpaceMB DESC, t.Name";
            return _dbContext.Database.SqlQuery<DbStat>(query).ToList();
        }
    }
}
