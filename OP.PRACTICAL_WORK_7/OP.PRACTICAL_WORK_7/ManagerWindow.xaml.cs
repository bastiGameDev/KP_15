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
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void WriteLogADD(string nameTable)
        {
            try
            {
                Logger logger = new Logger();

                logger.Log("ДОБАВЛЕНА ЗАПИСЬ", "Добавлена новая запись в таблицу " + nameTable);
            }
            catch (Exception ex)
            {

            }
        }

        private void WriteLogEdit(string nameTable)
        {
            try
            {
                Logger logger = new Logger();

                logger.Log("ИЗМЕНЕНА ЗАПИСЬ", "Изменена запись в таблице " + nameTable);
            }
            catch (Exception ex)
            {

            }
        }

        private void WriteLogDelete(string nameTable)
        {
            try
            {
                Logger logger = new Logger();

                logger.Log("УДАЛЕНА ЗАПИСЬ", "Удалена запись в таблице " + nameTable);
            }
            catch (Exception ex)
            {

            }
        }

        private void DG_EmployeeFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Employee], [First_Name_Employee], [Second_Name_Employee], " +
                        "[Middle_Name_Employee], [Birth_Day_Employee], [Citizenship_Employee], " +
                        "[Education_Employee], [Login_Employee], [Password_Employee], [Name_Role] from [dbo].[Employee] " +
                        "inner join [dbo].[Role]  on [Role_ID] = [ID_Role]",
                        DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_Employee;

                    dgEmployee.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgEmployee.Columns[0].Visibility = Visibility.Hidden;
                    dgEmployee.Columns[1].Header = "Имя сотрудника";
                    dgEmployee.Columns[2].Header = "Фамилия сотрудника";
                    dgEmployee.Columns[3].Header = "Отчество сотрудника";
                    dgEmployee.Columns[4].Header = "Дата рождения сотрудника";
                    dgEmployee.Columns[5].Header = "Гражданство сотрудника";
                    dgEmployee.Columns[6].Header = "Образование сотрудника";
                    dgEmployee.Columns[7].Header = "Логин сотрудника";
                    dgEmployee.Columns[8].Visibility = Visibility.Hidden;
                    dgEmployee.Columns[9].Header = "Должность сотрудника";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_Employee(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_EmployeeFill();
            }
        }

        private void dgEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            DG_EmployeeFill();
        }

        private void dgEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgEmployee.SelectedItems[0] as DataRowView;

                tbFirstName.Text = dataRowView[1].ToString();
                tbSecondName.Text = dataRowView[2].ToString();
                tbMiddleName.Text = dataRowView[3].ToString();
                tbBirthDay.Text = dataRowView[4].ToString();
                tbCitizenchip.Text = dataRowView[5].ToString();
                tbEducation.Text = dataRowView[6].ToString();
                tbLoginEmp.Text = dataRowView[7].ToString();
                //tbPasswordEmp.Text = dataRowView[8].ToString();

                // Устанавливаем выбранный элемент в комбобокс
                cbRoleEmp.Text = dataRowView[9].ToString();
                
            }
            catch { }
        }


        private void fillcbRoleEmp()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Role], [Name_Role] from [dbo].[Role]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbRoleEmp_OnChange;
                cbRoleEmp.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbRoleEmp.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbRoleEmp.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbRoleEmp_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbRoleEmp();
        }

        private void cbRoleEmp_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbRoleEmp();

            // Убедимся, что устанавливаем выбранный элемент, если он есть
            if (dgEmployee.SelectedItem != null)
            {
                DataRowView dataRowView = dgEmployee.SelectedItem as DataRowView;
                cbRoleEmp.SelectedValue = dataRowView[9].ToString();
            }
        }


        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployee.Items.Count != 0 & dgEmployee.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgEmployee.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Employee] " +
                    "where [ID_Employee] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogDelete("Сотрудники");
            }
        }

        private void btnUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgEmployee.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Employee] set " +
                    "[First_Name_Employee] = '{0}'," +
                    "[Second_Name_Employee] = '{1}'," +
                    "[Middle_Name_Employee] = '{2}'," +
                    "[Birth_Day_Employee] = '{3}'," +
                    "[Citizenship_Employee] = '{4}'," +
                    "[Education_Employee] = '{5}'," +
                    "[Login_Employee] = '{6}'," +
                    "[Password_Employee] = '{7}'," +
                    "[Role_ID] = '{8}' " +
                    "where [ID_Employee] = {9}",
                     tbFirstName.Text, tbSecondName.Text,
                        tbMiddleName.Text,
                        tbBirthDay.Text,
                        tbCitizenchip.Text,
                        tbEducation.Text,
                        tbLoginEmp.Text,
                        tbPasswordEmp.Text,
                        cbRoleEmp.SelectedValue,
                dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogEdit("Сорудники");

            }
            catch { /*MessageBox.Show("Ошибка");*/ }
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Преобразуем строку в байты
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                // Хэшируем байты
                byte[] hashedBytes = sha256.ComputeHash(bytes);

                // Преобразуем байты в строку и возвращаем результат
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }


        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            string plainPass = tbPasswordEmp.Text;
            string hashedPassword = HashPassword(plainPass);
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Employee] ([First_Name_Employee], " +
                "[Second_Name_Employee], [Middle_Name_Employee], [Birth_Day_Employee], " +
                "[Citizenship_Employee], [Education_Employee], " +
                "[Login_Employee], [Password_Employee], [Role_ID])" +
                "values ( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                tbFirstName.Text, 
                tbSecondName.Text,
                        tbMiddleName.Text,
                        tbBirthDay.Text,
                        tbCitizenchip.Text,
                        tbEducation.Text,
                        tbLoginEmp.Text,
                        hashedPassword,
                        cbRoleEmp.SelectedValue), DataBaseClass.act.manipulation);

            WriteLogADD("Сотрудники");
        }

        private void dgProductsInStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgProductsInStock.SelectedItems[0] as DataRowView;
                tbNamePos.Text = dataRowView[1].ToString();
                tbAmounStock.Text = dataRowView[2].ToString();
                cbNumberSupply.Text = dataRowView[3].ToString();
            }
            catch { }
        }

        private void DG_ProductsInStockeFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Products_In_Stock], [Name_Position_Products_In_Stock], " +
                        "[Quantity_In_Stock_Products_In_Stock], [Number_Supply_Products] from [dbo].[Products_In_Stock] " +
                        "inner join [dbo].[Supply_Products]  on [Supply_Products_ID] = [ID_Supply_Products]",
                        DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_ProductsInStock;

                    dgProductsInStock.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgProductsInStock.Columns[0].Visibility = Visibility.Hidden;
                    dgProductsInStock.Columns[1].Header = "Наименование позиции";
                    dgProductsInStock.Columns[2].Header = "Количество в наличии";
                    dgProductsInStock.Columns[3].Header = "Номер поставки";;
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_ProductsInStock(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_ProductsInStockeFill();
            }
        }

        private void dgProductsInStock_Loaded(object sender, RoutedEventArgs e)
        {
            DG_ProductsInStockeFill();
        }

        private void fillcbNumberSupply()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Supply_Products], [Number_Supply_Products] " +
                    "from [dbo].[Supply_Products]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbNumberSupply_OnChange;
                cbNumberSupply.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbNumberSupply.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbNumberSupply.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbNumberSupply_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbNumberSupply();
        }
        private void cbNumberSupply_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbNumberSupply();
        }

        private void btnDeleteProductsInStock_Click(object sender, RoutedEventArgs e)
        {
            if (dgProductsInStock.Items.Count != 0 & dgProductsInStock.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgProductsInStock.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Products_In_Stock] " +
                    "where [ID_Products_In_Stock] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogDelete("Продукция в наличии");
            }
        }

        private void btnUpdateProductsInStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgProductsInStock.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Products_In_Stock] set " +
                    "[Name_Position_Products_In_Stock] = '{0}'," +
                    "[Quantity_In_Stock_Products_In_Stock] = '{1}', " +
                    "[Supply_Products_ID] = '{2}' " +
                    "where [ID_Products_In_Stock] = {3}",
                    tbNamePos.Text, tbAmounStock.Text, cbNumberSupply.SelectedValue, 
                    dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogEdit("Продукция в наличии");
            }
            catch { /*MessageBox.Show("Ошибка");*/ }
        }

        private void btnAddProductsInStock_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Products_In_Stock] " +
                "([Name_Position_Products_In_Stock], [Quantity_In_Stock_Products_In_Stock], [Supply_Products_ID])" +
                "values ( '{0}', '{1}', '{2}')",
                tbNamePos.Text, tbAmounStock.Text, cbNumberSupply.SelectedValue),
                DataBaseClass.act.manipulation);

            WriteLogADD("Продукция в наличии");
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            // Получаем значение из текстового поля
            string searchText = txtSearch.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchText))
                    {
                        // Если текстовое поле пусто, отображаем все записи
                        query = "select [ID_Products_In_Stock], [Name_Position_Products_In_Stock], " +
                                "[Quantity_In_Stock_Products_In_Stock], [Number_Supply_Products] " +
                                "from [dbo].[Products_In_Stock] " +
                                "inner join [dbo].[Supply_Products] on [Supply_Products_ID] = [ID_Supply_Products]";
                    }
                    else
                    {
                        // Если есть значение, выполняем поиск
                        query = $"select [ID_Products_In_Stock], [Name_Position_Products_In_Stock], " +
                                $"[Quantity_In_Stock_Products_In_Stock], [Number_Supply_Products] " +
                                $"from [dbo].[Products_In_Stock] " +
                                $"inner join [dbo].[Supply_Products] on [Supply_Products_ID] = [ID_Supply_Products] " +
                                $"where [Name_Position_Products_In_Stock] like '%{searchText}%'";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_ProductsInStock;

                    dgProductsInStock.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgProductsInStock.Columns[0].Visibility = Visibility.Hidden;
                    dgProductsInStock.Columns[1].Header = "Наименование позиции";
                    dgProductsInStock.Columns[2].Header = "Количество в наличии";
                    dgProductsInStock.Columns[3].Header = "Номер поставки";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }

        

        private void txtSerEmployee_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            // Получаем значение из текстового поля
            string searchLastName = txtSerEmployee.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchLastName))
                    {
                        // Если текстовое поле пусто, отображаем все записи
                        query = "select [ID_Employee], [First_Name_Employee], [Second_Name_Employee], " +
                                "[Middle_Name_Employee], [Birth_Day_Employee], [Citizenship_Employee], " +
                                "[Education_Employee], [Login_Employee], [Password_Employee], [Name_Role] " +
                                "from [dbo].[Employee] " +
                                "inner join [dbo].[Role] on [Role_ID] = [ID_Role]";
                    }
                    else
                    {
                        // Если есть значение, выполняем поиск
                        query = $"select [ID_Employee], [First_Name_Employee], [Second_Name_Employee], " +
                                "[Middle_Name_Employee], [Birth_Day_Employee], [Citizenship_Employee], " +
                                "[Education_Employee], [Login_Employee], [Password_Employee], [Name_Role] " +
                                "from [dbo].[Employee] " +
                                "inner join [dbo].[Role] on [Role_ID] = [ID_Role] " +
                                $"where [Second_Name_Employee] like '%{searchLastName}%'";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_Employee;

                    dgEmployee.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgEmployee.Columns[0].Visibility = Visibility.Hidden;
                    dgEmployee.Columns[1].Header = "Имя сотрудника";
                    dgEmployee.Columns[2].Header = "Фамилия сотрудника";
                    dgEmployee.Columns[3].Header = "Отчество сотрудника";
                    dgEmployee.Columns[4].Header = "Дата рождения сотрудника";
                    dgEmployee.Columns[5].Header = "Гражданство сотрудника";
                    dgEmployee.Columns[6].Header = "Образование сотрудника";
                    dgEmployee.Columns[7].Header = "Логин сотрудника";
                    dgEmployee.Columns[8].Visibility = Visibility.Hidden;
                    dgEmployee.Columns[9].Header = "Должность сотрудника";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }

        private void dgEmployee_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            tbPasswordEmp.IsEnabled = true;
        }

        private void cbRoleEmp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
