using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Data;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Management;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace OP.PRACTICAL_WORK_7
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {

        public AdminWindow()
        {
            InitializeComponent();
            UpdateServerStatus();
        }       

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateServerStatus();
        }

        private void UpdateServerStatus()
        {
            // Обновление данных о сервере
            ServerStatusText.Text = IsServerRunning() ? "Включен" : "Выключен";
            ServerVersionText.Text = GetServerVersion();

            // Обновление данных о нагрузке на сервер
            CpuUsageText.Text = GetCpuUsage().ToString();
            MemoryUsageText.Text = GetMemoryUsage().ToString();
        }

        private bool IsServerRunning()
        {
            // Замените на ваш код для проверки статуса сервера
            return true;
        }

        private string GetServerVersion()
        {
            // Замените на ваш код для получения версии сервера
            return "SQL Server 2019";
        }

        private double GetCpuUsage()
        {
            // Замените на ваш код для получения использования процессора
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return Convert.ToDouble(obj["PercentProcessorTime"]);
                }
            }
            return 0.0;
        }

        private double GetMemoryUsage()
        {
            // Замените на ваш код для получения использования памяти
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    return Convert.ToDouble(obj["TotalVisibleMemorySize"]);
                }
            }
            return 0.0;
        }

        private void ExportLogsToTxt()
        {
            try
            {
                // Открываем диалог сохранения файла
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Открываем поток для записи в выбранный файл
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        // Записываем заголовок
                        writer.WriteLine("ID лога\tТаймстемп\tУровень\tСообщение");

                        // Записываем данные из датагрида
                        foreach (DataRowView row in dgLogs.Items)
                        {
                            writer.WriteLine($"{row[0]}\t{row[1]}\t{row[2]}\t{row[3]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений, если необходимо
            }
        }

        private void SaveToFile(string data)
        {
            // Открываем диалог сохранения файла
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            // Если пользователь выбрал файл и нажал "ОК"
            if (saveFileDialog.ShowDialog() == true)
            {
                // Сохраняем данные в выбранный файл
                File.WriteAllText(saveFileDialog.FileName, data);
                MessageBox.Show("Данные успешно экспортированы.");
            }
        }



        private void DG_Logs()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Log], [Timestamp], [LogLevel], [Message] from [dbo].[Log]",
                        DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_Log;

                    dgLogs.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgLogs.Columns[0].Header = "ID лога";
                    dgLogs.Columns[1].Header = "Таймстемп";
                    dgLogs.Columns[2].Header = "Уровень";
                    dgLogs.Columns[2].Header = "Сообщение";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_Log(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_Logs();
            }
        }

        private void dgContractSupplyProducts_Loaded(object sender, RoutedEventArgs e)
        {
            DG_Logs();
        }

        private void dgLogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExportLogsToTxt();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Получаем выделенные строки из DataGrid
            var selectedLogs = dgLogs.SelectedItems;

            // Проверяем, есть ли что-то для экспорта
            if (selectedLogs.Count > 0)
            {
                // Создаем строку для хранения данных
                StringBuilder exportData = new StringBuilder();

                // Перебираем выделенные строки и добавляем данные в строку
                foreach (DataRowView log in selectedLogs)
                {
                    exportData.AppendLine($"{log[0]}, {log[1]}, {log[2]}, {log[3]}");
                }

                // Вызываем метод для сохранения данных в файл
                SaveToFile(exportData.ToString());
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Строка подключения к вашей базе данных
            string connectionString = "Data Source=DESKTOP-5ECDJ4N\\SQLEXPRESS;Initial Catalog=DB_CateringEstablishment;Integrated Security=True;";

            // Создаем диалоговое окно сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
            saveFileDialog.Title = "Выберите файл для сохранения";

            // Если пользователь выбрал файл и нажал "ОК"
            if (saveFileDialog.ShowDialog() == true)
            {
                string outputFile = saveFileDialog.FileName;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Получаем список всех таблиц в базе данных
                    DataTable tables = connection.GetSchema("Tables");

                    using (StreamWriter writer = new StreamWriter(outputFile, true))
                    {
                        foreach (DataRow table in tables.Rows)
                        {
                            string tableName = table["TABLE_NAME"].ToString();

                            // Выполняем запрос для каждой таблицы
                            string query = $"SELECT * FROM {tableName}";

                            using (SqlCommand command = new SqlCommand(query, connection))
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Записываем название таблицы в файл
                                writer.WriteLine($"Таблица: {tableName}");

                                // Записываем заголовок
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    writer.Write($"{reader.GetName(i)}\t");
                                }
                                writer.WriteLine();

                                // Записываем данные
                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        writer.Write($"{reader[i]}\t");
                                    }
                                    writer.WriteLine();
                                }

                                // Разделяем таблицы пустой строкой
                                writer.WriteLine();
                            }
                        }
                    }

                    connection.Close();
                }

                MessageBox.Show($"Данные успешно сохранены в файл: {outputFile}", "Успех");
            }
            else
            {
                MessageBox.Show("Отменено пользователем.", "Отмена");
            }

        }

        
    }
    }
    

