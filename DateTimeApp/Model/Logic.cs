using DateTimeApp.Model.Base;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DateTimeApp.Model
{
    public static class Logic
    {
        private static readonly string autorunName = "Alarm Clock";

        internal static void Log(string text, int id) => Trace.WriteLine($"[{DateTime.Now}] " + text, $"{(LogEnum)id}"); 

        internal static string ChangeColor(string act)
        {
            ColorDialog dialog = new ColorDialog() { Color = ColorTranslator.FromHtml(act) };
            if (dialog.ShowDialog() == DialogResult.OK)
                return ColorTranslator.ToHtml(dialog.Color);

            return act;
        }

        internal static void AddAutorun()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            try
            {
                registryKey.SetValue(autorunName, Application.ExecutablePath);

                MessageBox.Show("Программа успешно добавлена в автозапуск.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            registryKey.Close();
        }

        internal static void RemoveAutorun()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            try
            {
                registryKey.DeleteValue(autorunName, false);

                MessageBox.Show("Программа успешно удалена из автозапуска.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            registryKey.Close();
        }
    }
}
