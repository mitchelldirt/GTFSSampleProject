using Microsoft.Data.Sqlite;

const string connectionString = "Data Source=../../../maxTransit.sqlite;";
var dbConnection = new SqliteConnection(connectionString);

string path = Path.Combine(Directory.GetCurrentDirectory(), @"../../../MAXRoutes");
string[] files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);

foreach (var filePath in files)
{
    var lines = File.ReadAllLines(filePath);
    var lineNum = 0;

    // Grab name of file without extension and add it as a table to the maxTransit.sqlite database
    var tableName = Path.GetFileNameWithoutExtension(filePath);
    var columnNames = lines[0].Split(',');
    var columnTypes = new List<string>();

    foreach (var line in lines[1].Split(','))
    {
        columnTypes.Add(int.TryParse(line, out int n) ? "INTEGER" : "TEXT");
    }
    
    var createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} ({
        string.Join(", ", columnNames.Select((columnName, index) => $"{columnName} {columnTypes[index]}"))
    });";
    
    var createTableCommand = new SqliteCommand(createTableQuery, dbConnection);
    dbConnection.Open();
    createTableCommand.ExecuteNonQuery();
    dbConnection.Close();


    foreach (var line in lines)
    {
        var fields = line.Split(',');
    
        if (lineNum == 0)
        {
            lineNum++;
        }
        else
        {
            var insertQuery = $"INSERT INTO {tableName} VALUES ({string.Join(", ", fields.Select(field => $"'{field.Replace("'", "''")}'"))});";
            var insertCommand = new SqliteCommand(insertQuery, dbConnection);
            dbConnection.Open();
            insertCommand.ExecuteNonQuery();
            dbConnection.Close();
        }
    }
}