using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ModalHandler.Tools
{
    /// <summary>
    /// Helper class for working with Windows dialogs through api calls to user32.dll.
    /// </summary>
    internal static class WinApi
    {
        public const string ModalDialogClassName = "#32770";

        /// <summary>
        /// Filter windows by <see cref="ownerTitle"/> and find the first containing a popup.
        /// </summary>
        /// <param name="ownerTitle">Title of the window owning the popup.</param>
        /// <param name="timeout">Maximum amount of time for element search</param>
        /// <returns>Popup window handle.</returns>
        public static IntPtr GetEnabledPopup(string ownerTitle, TimeSpan timeout)
        {
            //See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633515(v=vs.85).aspx
            const int GW_ENABLEDPOPUP = 6;

            var ownerHandle = GetDesktopWindow()
                .GetElement(timeout,
                    e =>
                    {
                        // Owner should have a matching title with a possible postfix.
                        if (!e.GetText().StartsWith(ownerTitle))
                            return false;
                        var modal = GetWindow(e, GW_ENABLEDPOPUP);
                        // Its modal should be of type modal dialog.
                        return !modal.Equals(IntPtr.Zero) && modal.GetClassName().Equals(ModalDialogClassName);
                    });
            return GetWindow(ownerHandle, GW_ENABLEDPOPUP);
        }

        /// <summary>
        /// Get the first visible top level modal dialog
        /// </summary>
        /// <param name="timeout">Maximum amount of time for element search</param>
        /// <returns>Popup window handle.</returns>
        public static IntPtr GetTopLevelDialog(TimeSpan timeout) => GetElement(GetDesktopWindow(), timeout, e => e.GetClassName().Equals(ModalDialogClassName));

        /// <summary>
        /// Get an element by a <see cref="filter"/>.
        /// </summary>
        /// <param name="ownerHandle">Handle of the window owning the element.</param>
        /// <param name="timeout">Maximum amount of time for element search</param>
        /// <param name="filter">Search filter.</param>
        /// <returns>Element's handle.</returns>
        public static IntPtr GetElement(this IntPtr ownerHandle, TimeSpan timeout, Predicate<IntPtr> filter) =>
            GetElements(ownerHandle, timeout, filter).FirstOrDefault();

        /// <summary>
        /// Get a collection of elements by a <see cref="filter"/>.
        /// </summary>
        /// <param name="ownerHandle">Handle of the window owning the element.</param>
        /// <param name="filter">Search filter.</param>
        /// <returns>Collection of found element handles.</returns>
        public static IEnumerable<IntPtr> GetElements(this IntPtr ownerHandle, TimeSpan timeout,
            Predicate<IntPtr> filter)
        {
            var handles = default(IEnumerable<IntPtr>);
            Util.Wait(() => (handles = ownerHandle.GetChildWindows()
                .Where(IsWindowVisible)
                .Where(w => filter(w)))
                .Any(), timeout);
            return handles;
        }

        /// <summary>
        /// Get edit box value.
        /// </summary>
        /// <param name="hwnd">Handle of the owning element.</param>
        /// <returns>Text value</returns>
        public static string GetText(this IntPtr hwnd)
        {
            var sb = new StringBuilder(512);
            SendMessage(hwnd, 0x000D, sb.Capacity, sb);
            return sb.ToString();
        }

        /// <summary>
        /// Get element's class name.
        /// </summary>
        /// <param name="hwnd">Handle of the owning element.</param>
        /// <returns>Class name value.</returns>
        public static string GetClassName(this IntPtr hwnd)
        {
            var className = new StringBuilder(256);
            GetClassName(hwnd, className, className.Capacity);
            return className.ToString();
        }

        /// <summary>
        /// Get all descendant windows of the provided element.
        /// </summary>
        /// <param name="parent">Handle of the owning element.</param>
        /// <returns>Children handles collection.</returns>
        public static IEnumerable<IntPtr> GetChildWindows(this IntPtr parent)
        {
            var result = new List<IntPtr>();
            var listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = EnumWindow;
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static string GetInnerText(this IntPtr hwnd)
        {
            // Get child windows with non-empty caption.
            var list = hwnd.GetChildWindows().Where(w => !string.IsNullOrEmpty(w.GetText())).ToList();
            // Add the parent.
            list.Insert(0, hwnd);
            // Get aggregated text.
            return list.Select(e => e.GetText()).Aggregate('\r', '\n');
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            var gch = GCHandle.FromIntPtr(pointer);
            var list = gch.Target as List<IntPtr>;
            if (list == null)
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            list.Add(handle);
            return true;
        }

        #region Unmanaged

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam,
            [MarshalAs(UnmanagedType.LPStr)] string lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, [Out] StringBuilder lParam);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        #endregion
    }
}