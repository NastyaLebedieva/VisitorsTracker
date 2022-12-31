namespace VisitorsTracker.Storage
{
    public static class FileHelper
    {
        public static void AppendLineToFileAsync(string path, string line)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentOutOfRangeException(nameof(path), path, "Null or whitepsace.");

            if (!File.Exists(path))
                throw new FileNotFoundException("File not found.", nameof(path));

            using var file = File.Open(path, FileMode.Append, FileAccess.Write);
            using var writer = new StreamWriter(file);

            writer.WriteLine(line);
            writer.Flush();
        }
    }
}
