using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Xml;

namespace OP.PRACTICAL_WORK_7
{
    /// <summary>
    /// Логика взаимодействия для AccountantWindow.xaml
    /// </summary>
    public partial class AccountantWindow : Window
    {
        public AccountantWindow()
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

        private void DG_SalaryFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Salary], [Amount_Salary], " +
                        "[First_Name_EMployee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee] from [dbo].[Salary] " +
                        "inner join [dbo].[Employee] on [Employee_ID] = [ID_Employee]",
                        DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_Salary;

                    dgSalary.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgSalary.Columns[0].Visibility = Visibility.Hidden;
                    dgSalary.Columns[1].Header = "Размер оклада";
                    dgSalary.Columns[2].Header = "ФИО сотрудника";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_Salary(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_SalaryFill();
            }
        }

        private void dgSalary_Loaded(object sender, RoutedEventArgs e)
        {
            DG_SalaryFill();
        }

        private void btnDeleteSalary_Click(object sender, RoutedEventArgs e)
        {
            if (dgSalary.Items.Count != 0 & dgSalary.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgSalary.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Salary] " +
                    "where [ID_Salary] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogDelete("Заработные платы");
            }
        }

        private void btnUpdateSalary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgSalary.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Salary] set " +
                    "[Amount_Salary] = '{0}'," +
                    "[Employee_ID] = '{1}' " +
                    "where [ID_Salary] = {2}",
                    tbSalaryAmount.Text, cbEmployeeSalary.SelectedValue, dataRowView[0]), DataBaseClass.act.manipulation);
            }
            catch { /*MessageBox.Show("Ошибка");*/ }

            WriteLogEdit("Заработные платы");
        }

        private void fillcbEmployee()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Employee], [First_Name_Employee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee] from [dbo].[Employee]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbSalary_OnChange;
                cbEmployeeSalary.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbEmployeeSalary.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbEmployeeSalary.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbSalary_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbEmployee();
        }

        private void cbEmployeeSalary_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbEmployee();
        }

        private void dgSalary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgSalary.SelectedItems[0] as DataRowView;
                tbSalaryAmount.Text = dataRowView[1].ToString();
                cbEmployeeSalary.Text = dataRowView[2].ToString();
            }
            catch { }
        }

        private void btnAddSalary_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Salary] ([Amount_Salary], [Employee_ID])" +
                "values ( '{0}', '{1}')",
                tbSalaryAmount.Text, cbEmployeeSalary.SelectedValue),
                DataBaseClass.act.manipulation);
            WriteLogADD("Заработные платы");
        }

        private void DG_EmployeePayFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Employee_Payments], [Date_Employee_Payment], [Prize_Employe], " +
                        "[Fine_Employee], [Final_Employee_Payment], [Amount_Salary] from [dbo].[Employee_Payments] " +
                        "inner join [dbo].[Salary]  on [Salary_ID] = [ID_Salary]", DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_EmployeePay;

                    dgEmployeePayments.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgEmployeePayments.Columns[0].Visibility = Visibility.Hidden;
                    dgEmployeePayments.Columns[1].Header = "Дата выплаты";
                    dgEmployeePayments.Columns[2].Header = "Размер премии";
                    dgEmployeePayments.Columns[3].Header = "Корректировка зарплаты";
                    dgEmployeePayments.Columns[4].Header = "Итоговая сумма выплаты";
                    dgEmployeePayments.Columns[5].Header = "Размер зарплаты";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_EmployeePay(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_EmployeePayFill();
            }
        }

        private void dgEmployeePayments_Loaded(object sender, RoutedEventArgs e)
        {
            DG_EmployeePayFill();
        }

        private void dgEmployeePayments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgEmployeePayments.SelectedItems[0] as DataRowView;
                tbDatePay.Text = dataRowView[1].ToString();
                tbPrizePay.Text = dataRowView[2].ToString();
                tbFinePay.Text = dataRowView[3].ToString();
                cbSalaryPay.Text = dataRowView[5].ToString();
                tbFinalPay.Text = dataRowView[4].ToString();
                //cbSalaryPay.Text = dataRowView[6].ToString();
            }
            catch { }
        }


        private void fillcbSalaryPay()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Salary], [Amount_Salary] from [dbo].[Salary]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbSalary_OnChange;
                cbSalaryPay.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbSalaryPay.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbSalaryPay.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbSalaryPay_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbSalaryPay();
        }

        private void cbSalaryPay_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbSalaryPay();
        }

        private void btnDeleteEmployeePay_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployeePayments.Items.Count != 0 & dgEmployeePayments.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgEmployeePayments.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Employee_Payments] " +
                    "where [ID_Employee_Payments] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogDelete("Выплаты");
            }
        }

        private void btnUpdateEmployeePay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgEmployeePayments.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Employee_Payments] set " +
                    "[Date_Employee_Payment] = '{0}'," +
                    "[Prize_Employe] = '{1}'," +
                    "[Fine_Employee] = '{2}'," +
                    "[Salary_ID] = '{3}'," +
                    "[Final_Employee_Payment] = '{4}' " +
                    "where [ID_Employee_Payments] = {5}",
                    tbDatePay.Text, tbPrizePay.Text, tbFinePay.Text, cbSalaryPay.SelectedValue,
                    tbFinalPay.Text, dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogEdit("Выплаты");
            }
            catch { /*MessageBox.Show("Ошибка");*/ }


        }

        private void btnAddEmployeePay_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Employee_Payments] ([Date_Employee_Payment], " +
                "[Prize_Employe], [Fine_Employee], [Final_Employee_Payment], [Salary_ID]) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');",
                 tbDatePay.Text, tbPrizePay.Text, tbFinePay.Text,
                    tbFinalPay.Text, cbSalaryPay.SelectedValue),
                DataBaseClass.act.manipulation);

            WriteLogADD("Выплаты");
        }

        private void DG_TaxDeductions()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Tax_Deductions], [Tax_Amount], [Final_Employee_Payment] " +
                        "from [dbo].[Tax_Deductions] " +
                        "inner join [dbo].[Employee_Payments]  on [Employee_Payments_ID] = [ID_Employee_Payments]",
                        DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_TaxDeductions;

                    dgTaxDeductions.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgTaxDeductions.Columns[0].Visibility = Visibility.Hidden;
                    dgTaxDeductions.Columns[1].Header = "Размер налога";
                    dgTaxDeductions.Columns[2].Header = "Размер выплаты сотруднику";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_TaxDeductions(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_TaxDeductions();
            }
        }

        private void dgTaxDeductions_Loaded(object sender, RoutedEventArgs e)
        {
            DG_TaxDeductions();
        }

        private void dgTaxDeductions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgTaxDeductions.SelectedItems[0] as DataRowView;
                tbAmountTax.Text = dataRowView[1].ToString();
                cbTaxAmoubtPay.Text = dataRowView[2].ToString();
            }
            catch { }
        }

        private void fillcbTaxAmountPay()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Employee_Payments],[Final_Employee_Payment] " +
                    "from [dbo].[Employee_Payments]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbTaxAmountPay_OnChange;
                cbTaxAmoubtPay.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbTaxAmoubtPay.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName; // Идентификатор
                cbTaxAmoubtPay.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName; // Значение
            };
            Dispatcher.Invoke(action);
        }


        private void cbTaxAmountPay_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbTaxAmountPay();
        }

        private void cbTaxAmoubtPay_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbTaxAmountPay();
        }

        private void btnDeleteTax_Click(object sender, RoutedEventArgs e)
        {
            if (dgTaxDeductions.Items.Count != 0 & dgTaxDeductions.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgTaxDeductions.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Tax_Deductions] " +
                    "where [ID_Tax_Deductions] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogDelete("Налоговые отчисления");
            }
        }

        private void btnUpdateTax_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgTaxDeductions.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Tax_Deductions] set " +
                    "[Tax_Amount] = '{0}'," +
                    "[Employee_Payments_ID] = '{1}' " +
                    "where [ID_Tax_Deductions] = {2}",
                    tbAmountTax.Text, cbTaxAmoubtPay.SelectedValue, dataRowView[0]), DataBaseClass.act.manipulation);
            }
            catch { /*MessageBox.Show("Ошибка");*/ }
        }

        private void btnAddTax_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Tax_Deductions] ([Tax_Amount], " +
                "[Employee_Payments_ID]) " +
                "VALUES ('{0}', '{1}');",
                 tbAmountTax.Text, cbTaxAmoubtPay.SelectedValue),
                DataBaseClass.act.manipulation);
        }

        private void txtSalary_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем значение из текстового поля
            string searchFullName = txtSalary.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchFullName))
                    {
                        // Если текстовое поле пусто, отображаем все записи
                        query = "select [ID_Salary], [Amount_Salary], " +
                                "[First_Name_Employee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee] from [dbo].[Salary] " +
                                "inner join [dbo].[Employee] on [Employee_ID] = [ID_Employee]";
                    }
                    else
                    {
                        // Если есть значение, выполняем поиск
                        query = $"select [ID_Salary], [Amount_Salary], " +
                                "[First_Name_Employee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee] from [dbo].[Salary] " +
                                "inner join [dbo].[Employee] on [Employee_ID] = [ID_Employee] " +
                                $"where [First_Name_Employee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee] like '%{searchFullName}%'";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_Salary;

                    dgSalary.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgSalary.Columns[0].Visibility = Visibility.Hidden;
                    dgSalary.Columns[1].Header = "Размер оклада";
                    dgSalary.Columns[2].Header = "ФИО сотрудника";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }

        private void txtPayments_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPayments_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            // Получаем значение из текстового поля
            string searchPrize = txtPayments.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchPrize))
                    {
                        // Если текстовое поле пусто, отображаем все записи
                        query = "select [ID_Employee_Payments], [Date_Employee_Payment], [Prize_Employe], " +
                                "[Fine_Employee], [Final_Employee_Payment], [Amount_Salary] from [dbo].[Employee_Payments] " +
                                "inner join [dbo].[Salary] on [Salary_ID] = [ID_Salary]";
                    }
                    else
                    {
                        // Если есть значение, выполняем поиск
                        query = $"select [ID_Employee_Payments], [Date_Employee_Payment], [Prize_Employe], " +
                                "[Fine_Employee], [Final_Employee_Payment], [Amount_Salary] from [dbo].[Employee_Payments] " +
                                "inner join [dbo].[Salary] on [Salary_ID] = [ID_Salary] " +
                                $"where [Prize_Employe] = {searchPrize}";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_EmployeePay;

                    dgEmployeePayments.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgEmployeePayments.Columns[0].Visibility = Visibility.Hidden;
                    dgEmployeePayments.Columns[1].Header = "Дата выплаты";
                    dgEmployeePayments.Columns[2].Header = "Размер премии";
                    dgEmployeePayments.Columns[3].Header = "Корректировка зарплаты";
                    dgEmployeePayments.Columns[4].Header = "Итоговая сумма выплаты";
                    dgEmployeePayments.Columns[5].Header = "Размер зарплаты";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }

        private void txtTaxDeductions_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем значение размера налога из TextBox
            string searchAmount = txtTaxDeductions.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchAmount))
                    {
                        // Если TextBox пуст, отображаем все записи
                        query = "select [ID_Tax_Deductions], [Tax_Amount], [Final_Employee_Payment] " +
                                "from [dbo].[Tax_Deductions] " +
                                "inner join [dbo].[Employee_Payments] on [Employee_Payments_ID] = [ID_Employee_Payments]";
                    }
                    else
                    {
                        // Если есть введенное значение, выполняем поиск
                        query = $"select [ID_Tax_Deductions], [Tax_Amount], [Final_Employee_Payment] " +
                                "from [dbo].[Tax_Deductions] " +
                                $"where [Tax_Amount] = {searchAmount}";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);
                    dgTaxDeductions.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgTaxDeductions.Columns[0].Visibility = Visibility.Hidden;
                    dgTaxDeductions.Columns[1].Header = "Размер налога";
                    dgTaxDeductions.Columns[2].Header = "Размер выплаты сотруднику";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void cbTaxAmoubtPay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddTax_Click_1(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Tax_Deductions] ([Tax_Amount], [Employee_Payments_ID])" +
                "values ( '{0}', '{1}')",
                tbAmountTax.Text, cbTaxAmoubtPay.SelectedValue),
                DataBaseClass.act.manipulation);
            WriteLogADD("Налоговые отчисления");
        }

        private void btnUpdateTax_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgTaxDeductions.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Tax_Deductions] set " +
                    "[Tax_Amount] = '{0}'," +
                    "[Employee_Payments_ID] = '{1}' " +
                    "where [ID_Tax_Deductions] = {2}",
                    tbAmountTax.Text, cbTaxAmoubtPay.SelectedValue, dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogEdit("Налоговые отчисления");
            }
            catch { /*MessageBox.Show("Ошибка");*/ }
        }
    }
}
