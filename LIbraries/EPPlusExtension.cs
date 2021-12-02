
    internal static class EPPlusExtension
    {
        public static void SetValue(this ExcelRange range, Object value)
        {
            range.Value = value;
        }
        public static void SetValue(this ExcelRange range, string str, string format = "@")
        {
            range.Value = str;
            range.Style.Numberformat.Format = format;
        }
        public static void SetValue(this ExcelRange range, DateTime dateTime, string format = "yyyy/m/d hh:mm:ss.000")
        {
            range.Value = dateTime;
            range.Style.Numberformat.Format = format;
        }
        public static void SetValue(this ExcelRange range, TimeSpan timeSpan, string format = "hh:mm:ss.000")
        {
            range.Value = timeSpan;
            range.Style.Numberformat.Format = format;
        }
        public static ExcelWorksheet AddIfAbsent(this ExcelWorksheets worksheets, string name)
        {
            var found = worksheets.SingleOrDefault(sheet => sheet.Name.Equals(name));
            return found ?? worksheets.Add(name);
        }

        public static void For<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (var item in source)
            {
                action(item, i);
                i++;
            }
        }
        public static void For<T>(this IEnumerable<T> source, Func<T, int, bool> action)
        {
            int i = 0;
            foreach (var item in source)
            {
                if (!action(item, i))
                    break;
                i++;
            }
        }

        public static void For<T>(this IEnumerable<T> source, Action<T, long> action)
        {
            long i = 0;
            foreach (var item in source)
            {
                action(item, i);
                i++;
            }
        }

        public static void For<T>(this IEnumerable<T> source, Func<T, long, bool> action)
        {
            long i = 0;
            foreach (var item in source)
            {
                if (!action(item, i))
                    break;
                i++;
            }
        }

    }