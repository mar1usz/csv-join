namespace CsvJoin.Services.Abstractions
{
    public interface ISqlPreparator
    {
        string PrepareFullJoinSql(string directory, string[] fileNames);
        string PrepareLeftJoinSql(string directory, string[] fileNames);
        string PrepareRightAntiJoinSql(string directory, string[] fileNames);
    }
}
