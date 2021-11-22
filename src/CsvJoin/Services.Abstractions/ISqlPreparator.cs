using System.Collections.Generic;

namespace CsvJoin.Services.Abstractions
{
    public interface ISqlPreparator
    {
        string PrepareFullJoinSql(IEnumerable<Table> tables);
        string PrepareLeftJoinSql(IEnumerable<Table> tables);
        string PrepareRightAntiJoinSql(IEnumerable<Table> tables);
    }
}
