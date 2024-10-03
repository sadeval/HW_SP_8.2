using System;
using Microsoft.Win32;
using System.Text;
using System.Windows.Forms;

namespace SystemFontChanger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnChangeFontSize_Click(object sender, EventArgs e)
        {
            // Получаем размер шрифта из текстового поля
            if (int.TryParse(txtFontSize.Text, out int fontSize))
            {
                SetSystemFontSize(fontSize);
                MessageBox.Show("Изменения шрифта вступят в силу после перезагрузки.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректный размер шрифта.");
            }
        }

        private void SetSystemFontSize(int fontSize)
        {
            try
            {
                // Используем конструкцию using для автоматического закрытия ключа реестра
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop\WindowMetrics", true))
                {
                    if (key != null)
                    {
                        // Устанавливаем значение шрифта в реестре
                        key.SetValue("CaptionFont", GenerateLogFontForSize(fontSize), RegistryValueKind.Binary);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось получить доступ к реестру.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при изменении размера шрифта: " + ex.Message);
            }
        }

        // Метод для генерации структуры LOGFONT в бинарном формате
        private byte[] GenerateLogFontForSize(int fontSize)
        {
            byte[] logFont = new byte[92]; // Примерная длина для структуры LOGFONT

            // Установка размера шрифта
            logFont[0] = (byte)(-fontSize & 0xFF);
            logFont[1] = (byte)((-fontSize >> 8) & 0xFF);

            // Установка имени шрифта
            byte[] fontName = Encoding.Unicode.GetBytes("Tahoma");
            Array.Copy(fontName, 0, logFont, 32, fontName.Length);

            return logFont;
        }
    }
}
