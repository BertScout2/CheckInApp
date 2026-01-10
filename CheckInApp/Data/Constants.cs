namespace CheckInApp.Data;

public static class Constants
{
    //public const string DatabaseFilename = "CheckInDB.db3";
    public const string DataLogFilename = "CheckIn.log";
    public static string Basepath = "";
    public static string DataLogPath = "";

    public static void InitDatabasePath()
    {
        Basepath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
#if ANDROID
        if (Directory.Exists("/sdcard/Documents"))
        {
            Basepath = "/sdcard/Documents";
        }
#endif
        //DatabasePath = $"Data Source={Path.Combine(DatabaseDirPath, DatabaseFilename)}";
        DataLogPath = Path.Combine(Basepath, DataLogFilename);
    }
}
