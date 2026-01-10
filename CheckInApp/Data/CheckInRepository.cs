using CheckInApp.Models;
using Microsoft.Extensions.Logging;
using static CheckInApp.Data.Constants;

namespace CheckInApp.Data;

/// <summary>
/// Repository class for managing items in the database.
/// </summary>
public class CheckInRepository
{
    private bool _hasBeenInitialized = false;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckInRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public CheckInRepository(ILogger<CheckInRepository> logger)
    {
        _logger = logger;
    }

    /*

    /// <summary>
    /// Initializes the database connection and creates the CheckIn table if it does not exist.
    /// </summary>
    private async Task Init()
    {
        if (_hasBeenInitialized)
            return;

        await using var connection = new SqliteConnection(Constants.DatabasePath);
        await connection.OpenAsync();

        try
        {
            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS CheckIn (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                CheckInTime TEXT NULL,
                CheckOutTime TEXT NULL
            );";
            await createTableCmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating tables");
            throw;
        }

        _hasBeenInitialized = true;
    }

    /// <summary>
    /// Retrieves a list of all items from the database.
    /// </summary>
    public async Task<List<CheckIn>> ListAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(Constants.DatabasePath);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM CheckIn";
        var items = new List<CheckIn>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            items.Add(new CheckIn
            {
                ID = reader.GetInt32(0),
                Name = reader.GetString(1),
                CheckInTime = reader.IsDBNull(2) ? null : DateTimeOffset.Parse(reader.GetString(2)),
                CheckOutTime = reader.IsDBNull(3) ? null : DateTimeOffset.Parse(reader.GetString(3)),
            });
        }

        return items;
    }

    /// <summary>
    /// Retrieves a list of items for a specified name
    /// </summary>
    public async Task<List<CheckIn>> ListAsync(string name)
    {
        await Init();
        await using var connection = new SqliteConnection(Constants.DatabasePath);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = @"
            SELECT * FROM CheckIn
            WHERE Name = @Name";
        selectCmd.Parameters.AddWithValue("Name", name);

        var items = new List<CheckIn>();

        await using var reader = await selectCmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            items.Add(new CheckIn
            {
                ID = reader.GetInt32(0),
                Name = reader.GetString(1),
                CheckInTime = reader.IsDBNull(2) ? null : DateTimeOffset.Parse(reader.GetString(2)),
                CheckOutTime = reader.IsDBNull(3) ? null : DateTimeOffset.Parse(reader.GetString(3)),
            });
        }

        return items;
    }

    /// <summary>
    /// Retrieves a specific item by its ID.
    /// </summary>
    public async Task<CheckIn?> GetAsync(int id)
    {
        await Init();
        await using var connection = new SqliteConnection(Constants.DatabasePath);
        await connection.OpenAsync();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM CheckIn WHERE ID = @ID";
        selectCmd.Parameters.AddWithValue("@ID", id);

        await using var reader = await selectCmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new CheckIn
            {
                ID = reader.GetInt32(0),
                Name = reader.GetString(1),
                CheckInTime = reader.IsDBNull(2) ? null : DateTimeOffset.Parse(reader.GetString(2)),
                CheckOutTime = reader.IsDBNull(3) ? null : DateTimeOffset.Parse(reader.GetString(3)),
            };
        }

        return null;
    }

    /// <summary>
    /// Saves an item to the database. If the item ID is 0, a new item is created;
    /// otherwise, the existing item is updated.
    /// </summary>
    public async Task<int> SaveItemAsync(CheckIn item)
    {
        await Init();
        await using var connection = new SqliteConnection(Constants.DatabasePath);
        await connection.OpenAsync();

        var saveCmd = connection.CreateCommand();
        if (item.ID == 0)
        {
            saveCmd.CommandText = 
@"
INSERT INTO CheckIn (Name, CheckInTime, CheckOutTime)
VALUES (@Name, @CheckInTime, @CheckOutTime);
--SELECT last_insert_rowid();
";
        }
        else
        {
            saveCmd.CommandText = 
@"
UPDATE CheckIn
SET Name = @Name,
CheckInTime = @CheckInTime,
CheckOutTime = @CheckOutTime
WHERE ID = @ID";
            saveCmd.Parameters.AddWithValue("@ID", item.ID);
        }

        saveCmd.Parameters.AddWithValue("@Name", item.Name);
        saveCmd.Parameters.AddWithValue("@CheckInTime", item.CheckInTime?.ToString("O"));
        saveCmd.Parameters.AddWithValue("@CheckOutTime", item.CheckOutTime?.ToString("O"));

        var result = await saveCmd.ExecuteScalarAsync();
        if (item.ID == 0)
        {
            item.ID = Convert.ToInt32(result);
        }

        connection.Close();
        return item.ID;
    }

    /// <summary>
    /// Deletes an item from the database.
    /// </summary>
    public async Task<int> DeleteItemAsync(CheckIn item)
    {
        await Init();
        await using var connection = new SqliteConnection(Constants.DatabasePath);
        await connection.OpenAsync();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM CheckIn WHERE ID = @ID";
        deleteCmd.Parameters.AddWithValue("@ID", item.ID);

        return await deleteCmd.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Drops the CheckIn table from the database.
    /// </summary>
    public async Task DropTableAsync()
    {
        await Init();
        await using var connection = new SqliteConnection(Constants.DatabasePath);
        await connection.OpenAsync();

        var dropTableCmd = connection.CreateCommand();
        dropTableCmd.CommandText = "DROP TABLE IF EXISTS CheckIn";
        await dropTableCmd.ExecuteNonQueryAsync();

        _hasBeenInitialized = false;
    }

    */

    public async Task<int> SaveItemAsync(CheckIn item)
    {
        if (!_hasBeenInitialized)
        {
            _hasBeenInitialized = true;
        }
        var checkIn = item.CheckInTime == null ? "null" : item.CheckInTime?.ToString("O");
        var checkOut = item.CheckOutTime == null ? "null" : item.CheckOutTime?.ToString("O");
        //var data = $"Name=\"{item.Name}\",CheckIn=\"{checkIn}\",CheckOut=\"{checkOut}\"";
        var data = System.Text.Json.JsonSerializer.Serialize(item) + Environment.NewLine;
        await Task.Run(() => File.AppendAllText(DataLogPath, data));
        return 1;
    }
}
