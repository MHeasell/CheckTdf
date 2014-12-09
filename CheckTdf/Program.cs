namespace CheckTdf
{
    using System;
    using System.IO;

    using TAUtil;
    using TAUtil.Hpi;
    using TAUtil.Tdf;

    public static class Program
    {
        private const int ParseErrorsFoundCode = 2;
        private const int WarningsFoundCode = 4;
        private const int ErrorCode = 1;

        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Must specify file to check.");
                Environment.Exit(ErrorCode);
            }

            if (args.Length > 1)
            {
                Console.Error.WriteLine("Too many args, exiting.");
                Environment.Exit(ErrorCode);
            }

            var inPath = args[0];

            if (!File.Exists(inPath))
            {
                Console.Error.WriteLine("Could not find file: " + inPath);
                Environment.Exit(ErrorCode);
            }

            var ext = Path.GetExtension(inPath);

            int exitCode = 0;

            try
            {
                if (IsTdfExtension(ext))
                {
                    exitCode = ScanTdf(inPath);
                }
                else if (IsHpiExtension(ext))
                {
                    exitCode = ScanHpi(inPath);
                }
                else
                {
                    Console.Error.WriteLine("Unrecognised file extension: " + ext);
                    Environment.Exit(ErrorCode);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failed to scan file: " + e.Message);
                Environment.Exit(ErrorCode);
            }

            Environment.Exit(exitCode);
        }

        private static bool IsTdfExtension(string ext)
        {
            return string.Equals(ext, ".tdf", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".fbi", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".gui", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".ota", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsHpiExtension(string ext)
        {
            return string.Equals(ext, ".hpi", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".ufo", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".ccx", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".gpf", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ext, ".gp3", StringComparison.OrdinalIgnoreCase);
        }

        private static int ScanTdf(string path)
        {
            int exitCode = 0;
            var adapter = new TdfParserNullAdapter(path);

            using (var s = File.OpenRead(path))
            {
                try
                {
                    var parser = new TdfParser(s, adapter);
                    parser.Load();
                }
                catch (TdfParseException e)
                {
                    ParseBad(path, e);
                    exitCode |= ParseErrorsFoundCode;
                }
            }

            if (adapter.WarningsFound)
            {
                exitCode |= WarningsFoundCode;
            }

            return exitCode;
        }

        private static int ScanHpi(string path)
        {
            int exitCode = 0;

            using (var h = new HpiReader(path))
            {
                foreach (var f in h.GetFilesRecursive())
                {
                    var ext = HpiPath.GetExtension(f.Name);
                    if (!IsTdfExtension(ext))
                    {
                        continue;
                    }

                    string fullPath = HpiPath.Combine(path, f.Name);

                    var adapter = new TdfParserNullAdapter(fullPath);

                    using (var s = h.ReadTextFile(f.Name))
                    {
                        try
                        {
                            var parser = new TdfParser(s, adapter);
                            parser.Load();
                        }
                        catch (TdfParseException e)
                        {
                            ParseBad(fullPath, e);
                            exitCode |= ParseErrorsFoundCode;
                        }
                    }

                    if (adapter.WarningsFound)
                    {
                        exitCode |= WarningsFoundCode;
                    }
                }
            }

            return exitCode;
        }

        private static void ParseBad(string file, TdfParseException error)
        {
            Console.WriteLine(
                "{0}:{1}: Error: {2}",
                file,
                error.LineNumber,
                error.Message);
        }
    }
}
