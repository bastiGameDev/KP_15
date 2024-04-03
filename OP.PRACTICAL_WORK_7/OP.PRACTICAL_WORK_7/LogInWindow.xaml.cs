using System;
using System.Collections.Generic;
using System.Data;
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
using System.Security.Cryptography;
//using System.Text;

namespace OP.PRACTICAL_WORK_7
{
    /// <summary>
    /// Логика взаимодействия для LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                byte[] hashedBytes = sha256.ComputeHash(bytes);

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             string hashedPassword = HashPassword(tbPassword.Text); 

        // Ваш код проверки авторизации

        // Строка подключения к базе данных
        string connectionString = "Data Source=DESKTOP-5ECDJ4N\\SQLEXPRESS;Initial Catalog=DB_CateringEstablishment;Integrated Security=True;"; ;

            // Логин и пароль, которые вы ввели в интерфейсе
            string enteredLogin = tbLogin.Text; // замените на реальный введенный логин
            string enteredPassword = hashedPassword; // замените на реально введенный пароль

            // Запрос SQL
            string sqlQuery = $"SELECT * FROM [dbo].[Employee] WHERE [Login_Employee] = '{enteredLogin}'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Открываем подключение к базе данных
                connection.Open();

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    try
                    {
                        // Выполняем запрос
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                string storedLogin = reader["Login_Employee"].ToString();
                                string storedPassword = reader["Password_Employee"].ToString();
                                string storedRole = reader["Role_ID"].ToString();
                                
                                if (enteredLogin == storedLogin && enteredPassword == storedPassword)
                                {
                                    Logger logger = new Logger();

                                    switch (storedRole)
                                    {
                                        case "1":
                                            logger.Log("ВХОД В СИСТЕМУ", storedLogin + " вошёл в систему!");
                                            SupplySpecialist_Window window = new SupplySpecialist_Window();
                                            window.Show();
                                            
                                            break;

                                        case "2":
                                            logger.Log("ВХОД В СИСТЕМУ", storedLogin + " вошёл в систему!");
                                            AccountantWindow window2 = new AccountantWindow();
                                            window2.Show();
                                            break;

                                        case "3":
                                            logger.Log("ВХОД В СИСТЕМУ", storedLogin + " вошёл в систему!");
                                            ManagerWindow window3 = new ManagerWindow();
                                            window3.Show();
                                            break;
                                        case "4":
                                            logger.Log("ВХОД В СИСТЕМУ", storedLogin + " вошёл в систему!");
                                            AdminWindow winAdm = new AdminWindow();
                                            winAdm.Show();
                                            break;
                                        default:
                                            MessageBox.Show("Неизвествная ошибка!");
                                            break;
                                    }
                                    

                                    this.Close();


                                }
                                else
                                {
                                    MessageBox.Show("Неверный логин или пароль.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Пользователь с таким логином не найден.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка ошибок при выполнении запроса
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
