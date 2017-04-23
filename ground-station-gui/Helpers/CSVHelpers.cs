using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ESFT.GroundStation.Helpers {
    internal static class CSVHelpers {
        public static string ToCSV<T>(this IEnumerable<T> objList, string separator = ",") {
            var t = typeof(T);
            var fields = t.GetFields();

            var header = ToCSVHeader<T>(separator, fields);

            var csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var obj in objList)
                csvdata.AppendLine(obj.ToCSV<T>(separator, fields));

            return csvdata.ToString();
        }

        public static string ToCSV<T>(this object obj, string separator = ",", FieldInfo[] fields = null) {
            if (fields == null) fields = typeof(T).GetFields();

            var line = new StringBuilder();

            foreach (var f in fields) {
                if (line.Length > 0) line.Append(separator);

                var x = f.GetValue(obj);

                if (x != null) line.Append(x);
            }

            return line.ToString();
        }

        public static string ToCSVHeader<T>(string separator = ",", FieldInfo[] fields = null) {
            if (fields == null) fields = typeof(T).GetFields();
            return string.Join(separator, fields.Select(f => f.Name).ToArray());
        }
    }
}
