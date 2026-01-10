namespace CheckInApp.Data;

public static class Constants
{
    public const string DatabaseFilename = "CheckInDB.db3";
    public static string? DatabaseDirPath;
    public static string? DatabasePath;

    public static void InitDatabasePath()
    {
        DatabaseDirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#if ANDROID
        if (Directory.Exists("/sdcard/Documents"))
        {
            DatabaseDirPath = "/sdcard/Documents";
        }
#endif
        DatabasePath = $"Data Source={Path.Combine(DatabaseDirPath, DatabaseFilename)}";
    }
}
