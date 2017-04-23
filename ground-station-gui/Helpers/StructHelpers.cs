using System.Runtime.InteropServices;

namespace ESFT.GroundStation.Helpers {
    internal static class StructHelpers {
        public static byte[] GetBytes<T>(this T str) where T : struct {
            var size = Marshal.SizeOf(str);
            var arr = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static T FromBytes<T>(this byte[] arr) where T : struct {
            var str = default(T);

            var size = Marshal.SizeOf(str);
            var ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            str = (T) Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }
    }
}
