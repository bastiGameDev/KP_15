using System;
using System.Collections;
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

namespace OP.PRACTICAL_WORK_7
{
    /// <summary>
    /// Логика взаимодействия для SupplySpecialist_Window.xaml
    /// </summary>
    public partial class SupplySpecialist_Window : Window
    {
        public SupplySpecialist_Window()
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

        private void DG_SupplierFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Supplier], [Name_Supplier], [OGRN_Supplier], [INN_Supplier], [KPP_Supplier], [Legal_Address_Supplier] from [dbo].[Supplier] go", DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_Supplier;

                    dgSupplier.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgSupplier.Columns[0].Visibility = Visibility.Hidden;
                    dgSupplier.Columns[1].Header = "Наименование поставщика";
                    dgSupplier.Columns[2].Header = "ОГРН";
                    dgSupplier.Columns[3].Header = "ИНН";
                    dgSupplier.Columns[4].Header = "КПП ";
                    dgSupplier.Columns[5].Header = "Юридический адрес ";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DG_ContractSupplyProducstFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Contract_Supply_Products], [Number_Contract_Supply_Products], " +
                        "[Date_Conclusion_Contract_Supply_Products], [Subject_Contract_Supply_Products], " +
                        "[Amount_Contract_Supply_Products], [Term_Contract_Supply_Products], " +
                        "[Responsibility_Contract_Supply_Productss], [Ccontroversial_Issues_Contract_Supply_Products], " +
                        "[Name_Supplier] from [dbo].[Contract_Supply_Products] " +
                        "inner join [dbo].[Supplier]  on [Supplier_ID] = [ID_Supplier]", DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_ConractSupplyProducts;

                    dgContractSupplyProducts.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgContractSupplyProducts.Columns[0].Visibility = Visibility.Hidden;
                    dgContractSupplyProducts.Columns[1].Header = "Номер договора поставки";
                    dgContractSupplyProducts.Columns[2].Header = "Дата заключения договора";
                    dgContractSupplyProducts.Columns[3].Header = "Предмет договора";
                    dgContractSupplyProducts.Columns[4].Header = "Сумма договора ";
                    dgContractSupplyProducts.Columns[5].Header = "Срок исполнения обязательств";
                    dgContractSupplyProducts.Columns[6].Header = "Ответственность сторон ";
                    dgContractSupplyProducts.Columns[7].Header = "Рассмотрение споров";
                    dgContractSupplyProducts.Columns[8].Header = "Наименование поставщика";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_ConractSupplyProducts(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_ContractSupplyProducstFill();
            }
        }

        private void DependancyOnChange_Supplier(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_SupplierFill();
            }
        }

        private void dgSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgSupplier.SelectedItems[0] as DataRowView;
                tbNameSupplier.Text = dataRowView[1].ToString();
                tbOGRNSupplier.Text = dataRowView[2].ToString();
                tbINNSupplier.Text = dataRowView[3].ToString();
                tbKPPSupplier.Text = dataRowView[4].ToString();
                tbLegalAddressSupplier.Text = dataRowView[4].ToString();
            }
            catch { }
        }




        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgSupplier_Loaded(object sender, RoutedEventArgs e)
        {
            DG_SupplierFill();
        }

        private void btnAddSupplier_Click(object sender, RoutedEventArgs e)
        {
            try { 
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("insert into [dbo].[Supplier] ([Name_Supplier], [OGRN_Supplier]," +
                " [INN_Supplier], [KPP_Supplier], [Legal_Address_Supplier]) " +
                "values ('{0}', '{1}', '{2}', '{3}', '{4}')", tbNameSupplier.Text, tbOGRNSupplier.Text,
                tbINNSupplier.Text, tbKPPSupplier.Text, tbLegalAddressSupplier.Text), DataBaseClass.act.manipulation);
                WriteLogADD("Поставщики");

            }
            catch 
            { 
                MessageBox.Show("Ошибка!"); 
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверка, является ли введенный символ числом
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // Если не является, отменяем ввод
            }
        }

        private void btnUpdateSupplier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgSupplier.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Supplier] set " +
                    "[Name_Supplier] = '{0}'," +
                    "[OGRN_Supplier] = '{1}'," +
                    "[INN_Supplier] = '{2}'," +
                    "[KPP_Supplier] = '{3}'," +
                    "[Legal_Address_Supplier] = '{4}'" +
                    "where [ID_Supplier] = {5}",
                    tbNameSupplier.Text, tbOGRNSupplier.Text,
                    tbINNSupplier.Text, tbKPPSupplier.Text, tbLegalAddressSupplier.Text, 
                    dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogEdit("Поставщик");

            }
            catch { MessageBox.Show("Ошибка"); }
        }

        private void btnDeleteSupplier_Click(object sender, RoutedEventArgs e)
        {

            if (dgSupplier.Items.Count != 0 & dgSupplier.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgSupplier.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Supplier] where [ID_Supplier] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);
            }
        }

        /*private void dgContractSupplyProducts_Loaded(object sender, RoutedEventArgs e)ы
        {
            DG_ContractSupplyProducstFill();
        }

        private void cbSupplier_ContractSupProdFill()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Contract_Supply_Products], [Name_Supplier] from [dbo].[Contract_Supply_Products] inner join [dbo].[Supplier]  on [Supplier_ID] = [ID_Supplier]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbSupplier_ContractSupProd_Dependency_OnChange;
                cbSupplier_ContractSupProd.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbSupplier_ContractSupProd.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbSupplier_ContractSupProd.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;

            };
            Dispatcher.Invoke(action);
        }

        private void cbSupplier_ContractSupProd_Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                cbSupplier_ContractSupProdFill();
        }*/

        private void dgContractSupplyProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgContractSupplyProducts.SelectedItems[0] as DataRowView;
                tbNumberContractSupplyProducts.Text = dataRowView[1].ToString();
                tbConclusionContractSupplyProducts.Text = dataRowView[2].ToString();
                tbSubjectContractSupplyProducts.Text = dataRowView[3].ToString();
                tbAmountContractSupplyProducts.Text = dataRowView[4].ToString();
                tbTermContractSupplyProducts.Text = dataRowView[5].ToString();
                tbResponbilityContractSupplyProducts.Text = dataRowView[6].ToString();
                tbControversialIssuesContractSupplyProducts.Text = dataRowView[7].ToString();
                cbSupplier_ContractSupProd.Text = dataRowView[8].ToString();
                //tbTermContractSupplyProducts.Text = dataRowView[4].ToString();
                //cbSupplier_ContractSupProd.SelectedValue = dataRowView[5];
            }
            catch { }
        }

        /*private void cbSupplier_ContractSupProd_Loaded(object sender, RoutedEventArgs e)
        {
            cbSupplier_ContractSupProdFill();
        }*/

        private void cbSupplier_ContractSupProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // var selectedItem = cbSupplier_ContractSupProd.SelectedItem as DataRowView;
        }

        private void DG_SupplyProductsFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Contract_Supply_Products], [Number_Contract_Supply_Products], " +
                        "[Date_Conclusion_Contract_Supply_Products], [Subject_Contract_Supply_Products], " +
                        "[Amount_Contract_Supply_Products], [Term_Contract_Supply_Products], " +
                        "[Responsibility_Contract_Supply_Productss], " +
                        "[Ccontroversial_Issues_Contract_Supply_Products], " +
                        "[Name_Supplier] from [dbo].[Contract_Supply_Products] " +
                        "inner join [dbo].[Supplier]  on [Supplier_ID] = [ID_Supplier]", DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_SupplyProducts;

                    dgContractSupplyProducts.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgContractSupplyProducts.Columns[0].Visibility = Visibility.Hidden;
                    dgContractSupplyProducts.Columns[1].Header = "Номер договора поставки";
                    dgContractSupplyProducts.Columns[2].Header = "Дата заключения договора";
                    dgContractSupplyProducts.Columns[3].Header = "Предмет договора";
                    dgContractSupplyProducts.Columns[4].Header = "Сумма договора ";
                    dgContractSupplyProducts.Columns[5].Header = "Срок исполнения обязательств";
                    dgContractSupplyProducts.Columns[6].Header = "Ответственность сторон ";
                    dgContractSupplyProducts.Columns[7].Header = "Рассмотрение споров";
                    dgContractSupplyProducts.Columns[8].Header = "Наименование поставщика";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_SupplyProducts(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_SupplyProductsFill();
            }
        }

        private void dgContractSupplyProducts_Loaded(object sender, RoutedEventArgs e)
        {
            DG_SupplyProductsFill();
        }

        private void fillcbSupplier()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Supplier], [Name_Supplier] from [dbo].[Supplier]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbSupplier_OnChange;
                cbSupplier_ContractSupProd.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbSupplier_ContractSupProd.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbSupplier_ContractSupProd.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbSupplier_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbSupplier();
        }

        private void cbSupplier_ContractSupProd_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbSupplier();
        }

        private void btnDeleteContractSupProd_Click(object sender, RoutedEventArgs e)
        {
            if (dgContractSupplyProducts.Items.Count != 0 & dgContractSupplyProducts.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgContractSupplyProducts.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Contract_Supply_Products] " +
                    "where [ID_Contract_Supply_Products] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogDelete("Договор поставки продукции");
            }
        }

        private void btnUpdateContractSupProd_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            DataRowView dataRowView = dgContractSupplyProducts.SelectedItems[0] as DataRowView;
            dataBaseClass.sqlExecute(String.Format("update [dbo].[Contract_Supply_Products] set " +
                "[Number_Contract_Supply_Products] = '{0}'," +
                "[Date_Conclusion_Contract_Supply_Products] = '{1}'," +
                "[Subject_Contract_Supply_Products] = '{2}'," +
                "[Amount_Contract_Supply_Products] = '{3}'," +
                "[Term_Contract_Supply_Products] = '{4}'," +
                "[Responsibility_Contract_Supply_Productss] = '{5}'," +
                "[Ccontroversial_Issues_Contract_Supply_Products] = '{6}'," +
                "[Supplier_ID] = '{7}'" +
                "where [ID_Contract_Supply_Products] = {8}",
                 tbNumberContractSupplyProducts.Text, tbConclusionContractSupplyProducts.Text,
                 tbSubjectContractSupplyProducts.Text,
                 tbAmountContractSupplyProducts.Text,
                 tbTermContractSupplyProducts.Text,
                 tbResponbilityContractSupplyProducts.Text,
                 tbControversialIssuesContractSupplyProducts.Text,
                 cbSupplier_ContractSupProd.SelectedValue, dataRowView[0]), DataBaseClass.act.manipulation);
            WriteLogEdit("Договор поставки продукции");

        }

        private void btnAddContractSupProd_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Contract_Supply_Products] ([Number_Contract_Supply_Products], [Date_Conclusion_Contract_Supply_Products], [Subject_Contract_Supply_Products], [Amount_Contract_Supply_Products], [Term_Contract_Supply_Products], [Responsibility_Contract_Supply_Productss], [Ccontroversial_Issues_Contract_Supply_Products], [Supplier_ID])" +
                "values ( '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
                tbNumberContractSupplyProducts.Text, tbConclusionContractSupplyProducts.Text, 
                tbSubjectContractSupplyProducts.Text, tbAmountContractSupplyProducts.Text, tbTermContractSupplyProducts.Text,
                tbResponbilityContractSupplyProducts.Text, tbControversialIssuesContractSupplyProducts.Text, 
                cbSupplier_ContractSupProd.SelectedValue), DataBaseClass.act.manipulation);
                WriteLogADD("Договор поставки продукции");


        }

        private void btnDeleteSupplier_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void DG_ContractSupplyFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Supply_Products], [Number_Supply_Products], " +
                        "[First_Name_Employee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee], " +
                        "[Number_Contract_Supply_Products], [Status_Supply] from [dbo].[Supply_Products] " +
                        "inner join [dbo].[Employee]  on [Employee_ID] = [ID_Employee] " +
                        "inner join [dbo].[Contract_Supply_Products]  on [Contract_Supply_Products_ID] = [ID_Contract_Supply_Products] " +
                        "inner join [dbo].[Status_Supply_Products]  on [Status_Supply_Products_ID] = [ID_Status_Supply_Products]", DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_ConractSupply;

                    dgSupplyProducts.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgSupplyProducts.Columns[0].Visibility = Visibility.Hidden;
                    dgSupplyProducts.Columns[1].Header = "Номер поставки";
                    dgSupplyProducts.Columns[2].Header = "ФИО ответственного сотрудника";
                    dgSupplyProducts.Columns[3].Header = "Номер договора поставки";
                    dgSupplyProducts.Columns[4].Header = "Статус поставки";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_ConractSupply(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_ContractSupplyFill();
            }
        }

        private void dgSupplyProducts_Loaded(object sender, RoutedEventArgs e)
        {
            DG_ContractSupplyFill();
        }

        private void dgSupplyProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgSupplyProducts.SelectedItems[0] as DataRowView;
                tbNumberSupply.Text = dataRowView[1].ToString();
                cbConractSupply.Text = dataRowView[3].ToString();
                cbEmployee.Text = dataRowView[2].ToString();
                cbStatus.Text = dataRowView[4].ToString();
            }
            catch { }
        }

        private void fillcbContractSupply()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Contract_Supply_Products], [Number_Contract_Supply_Products] " +
                    "from [dbo].[Contract_Supply_Products]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbContractSupply_OnChange;
                cbConractSupply.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbConractSupply.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbConractSupply.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbContractSupply_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbContractSupply();
        }

        private void cbConractSupply_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbContractSupply();
        }

        private void fillcbEmployee()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Employee], [First_Name_Employee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee] from [dbo].[Employee]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbEmployee_OnChange;
                cbEmployee.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbEmployee.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbEmployee.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbEmployee_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbContractSupply();
        }

        private void cbEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbEmployee();
        }

        private void fillcbStatus()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Status_Supply_Products], [Status_Supply] from [dbo].[Status_Supply_Products]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbStatus_OnChange;
                cbStatus.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbStatus.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbStatus.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbStatus_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbStatus();
        }

        private void cbStatus_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbStatus();
        }

        private void btnDeleteSupplyProd_Click(object sender, RoutedEventArgs e)
        {
            if (dgSupplyProducts.Items.Count != 0 & dgSupplyProducts.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgSupplyProducts.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Supply_Products] " +
                    "where [ID_Supply_Products] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogDelete("Поставка продукции");

            }
        }

        private void btnUpdateSupplyProd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgSupplyProducts.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Supply_Products] set " +
                    "[Number_Supply_Products] = '{0}'," +
                    "[Employee_ID] = '{1}'," +
                    "[Contract_Supply_Products_ID] = '{2}'," +
                    "[Status_Supply_Products_ID] = '{3}'" +
                    "where [ID_Supply_Products] = {4}",
                    tbNumberSupply.Text,cbEmployee.SelectedValue, cbConractSupply.SelectedValue,
                     cbStatus.SelectedValue, dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogEdit("Поставка продукции");

            }
            catch { /*MessageBox.Show("Ошибка");*/ }
        }

        private void btnAddSupplyProd_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Supply_Products] ([Number_Supply_Products], [Employee_ID], [Contract_Supply_Products_ID], [Status_Supply_Products_ID])" +
                "values ( '{0}', '{1}', '{2}', '{3}')",
                tbNumberSupply.Text, cbEmployee.SelectedValue, cbConractSupply.SelectedValue,
                     cbStatus.SelectedValue), DataBaseClass.act.manipulation);
            WriteLogADD("Поставка продукции");

        }

        private void DG_ProductsInStockFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Products_In_Stock], [Name_Position_Products_In_Stock], " +
                        "[Quantity_In_Stock_Products_In_Stock], [Number_Supply_Products] from [dbo].[Products_In_Stock] " +
                        "inner join [dbo].[Supply_Products]  on [Supply_Products_ID] = [ID_Supply_Products]", DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_ProductsInStock;

                    dgProductsInStock.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgProductsInStock.Columns[0].Visibility = Visibility.Hidden;
                    dgProductsInStock.Columns[1].Header = "Наименование позиции";
                    dgProductsInStock.Columns[2].Header = "Кол-во в наличии";
                    dgProductsInStock.Columns[3].Header = "Номер поставки";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_ProductsInStock(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_ProductsInStockFill();
            }
        }

        private void dgProductsInStock_Loaded(object sender, RoutedEventArgs e)
        {
            DG_ProductsInStockFill();
        }

        private void dgProductsInStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgProductsInStock.SelectedItems[0] as DataRowView;
                tbNamePosition.Text = dataRowView[1].ToString();
                tbQuantityStock.Text = dataRowView[2].ToString();
                cbSupplyProducts.Text = dataRowView[3].ToString();
                
            }
            catch { }
        }
        private void fillcbNumberSupply()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Supply_Products], [Number_Supply_Products] from [dbo].[Supply_Products]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbNumberSupply_OnChange;
                cbSupplyProducts.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbSupplyProducts.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbSupplyProducts.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbNumberSupply_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbNumberSupply();
        }
        private void cbSupplyProducts_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbNumberSupply();
        }

        private void btnDeleteproductsInStock_Click(object sender, RoutedEventArgs e)
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

        private void btnUpdateproductsInStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgProductsInStock.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Products_In_Stock] set " +
                    "[Name_Position_Products_In_Stock] = '{0}'," +
                    "[Quantity_In_Stock_Products_In_Stock] = '{1}'," +
                    "[Supply_Products_ID] = '{2}'" +
                    "where [ID_Products_In_Stock] = {3}",
                    tbNamePosition.Text, tbQuantityStock.Text, 
                    cbSupplyProducts.SelectedValue,dataRowView[0]), DataBaseClass.act.manipulation);
                WriteLogEdit("Продукция в наличии");

            }
            catch { /*MessageBox.Show("Ошибка");*/ }
        }

        private void btnAddproductsInStock_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Products_In_Stock] ([Name_Position_Products_In_Stock], [Quantity_In_Stock_Products_In_Stock], [Supply_Products_ID])" +
                "values ( '{0}', '{1}', '{2}')",
                tbNamePosition.Text, tbQuantityStock.Text, cbSupplyProducts.SelectedValue), DataBaseClass.act.manipulation);
            WriteLogADD("Продукция в наличи");

        }

        private void DG_SaleFill()
        {
            try
            {

                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    dataBaseClass.sqlExecute("select [ID_Sale], [Number_Sale], [Name_Position_Products_In_Stock] from [dbo].[Sale] " +
                        "inner join [dbo].[Products_In_Stock]  on [Products_In_Stock_ID] = [ID_Products_In_Stock]", 
                        DataBaseClass.act.select);

                    dataBaseClass.dependency.OnChange += DependancyOnChange_Sale;

                    dgSale.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgSale.Columns[0].Visibility = Visibility.Hidden;
                    dgSale.Columns[1].Header = "Номер продажи";
                    dgSale.Columns[2].Header = "Позиция товара";
                };
                Dispatcher.Invoke(action);
            }
            catch { };
        }

        private void DependancyOnChange_Sale(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                DG_SaleFill();
            }
        }

        private void dgSale_Loaded(object sender, RoutedEventArgs e)
        {
            DG_SaleFill();
        }

        private void dgSale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView dataRowView = dgSale.SelectedItems[0] as DataRowView;
                tbNumberSale.Text = dataRowView[1].ToString();
                cbPosiiton.Text = dataRowView[2].ToString();

            }
            catch { }
        }

        private void fillcbPosition()
        {
            Action action = () =>
            {
                DataBaseClass dataBaseClass = new DataBaseClass();

                dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute("select [ID_Products_In_Stock], [Name_Position_Products_In_Stock] from [dbo].[Products_In_Stock]", DataBaseClass.act.select);
                dataBaseClass.dependency.OnChange += cbPosition_OnChange;
                cbPosiiton.ItemsSource = dataBaseClass.resultTable.DefaultView;
                cbPosiiton.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
                cbPosiiton.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
            };
            Dispatcher.Invoke(action);
        }

        private void cbPosition_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info != SqlNotificationInfo.Invalid)
                fillcbPosition();
        }

        private void cbPosiiton_Loaded(object sender, RoutedEventArgs e)
        {
            fillcbPosition();
        }

        private void btnDeleteSale_Click(object sender, RoutedEventArgs e)
        {
            if (dgSale.Items.Count != 0 & dgSale.SelectedItems.Count != 0)
            {
                DataRowView dataRowView = (DataRowView)dgSale.SelectedItems[0];
                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("delete from [dbo].[Sale] " +
                    "where [ID_Sale] = {0}", dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogDelete("Продажи");

            }
        }

        private void btnUpdateSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseClass dataBaseClass = new DataBaseClass();
                DataRowView dataRowView = dgSale.SelectedItems[0] as DataRowView;
                dataBaseClass.sqlExecute(String.Format("update [dbo].[Sale] set " +
                    "[Number_Sale] = '{0}'," +
                    "[Products_In_Stock_ID] = '{1}'" +
                    "where [ID_Sale] = {2}",
                    tbNumberSale.Text, cbPosiiton.SelectedValue,
                    dataRowView[0]), DataBaseClass.act.manipulation);

                WriteLogEdit("Продажи");

            }
            catch { /*MessageBox.Show("Ошибка");*/ }
        }

        private void btnAddSale_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format("INSERT INTO [dbo].[Sale] ([Number_Sale], [Products_In_Stock_ID])" +
                "values ( '{0}', '{1}')",
                tbNumberSale.Text, cbPosiiton.SelectedValue), DataBaseClass.act.manipulation);
            WriteLogADD("Продажи");

        }

        private void txtContractSupply_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = txtContractSupply.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchValue))
                    {
                        // Если TextBox пуст, отображаем все записи
                        query = "select [ID_Contract_Supply_Products], [Number_Contract_Supply_Products], " +
                                "[Date_Conclusion_Contract_Supply_Products], [Subject_Contract_Supply_Products], " +
                                "[Amount_Contract_Supply_Products], [Term_Contract_Supply_Products], " +
                                "[Responsibility_Contract_Supply_Productss], " +
                                "[Ccontroversial_Issues_Contract_Supply_Products], " +
                                "[Name_Supplier] from [dbo].[Contract_Supply_Products] " +
                                "inner join [dbo].[Supplier] on [Supplier_ID] = [ID_Supplier]";
                    }
                    else
                    {
                        // Если есть введенное значение, выполняем частичный поиск
                        query = $"select [ID_Contract_Supply_Products], [Number_Contract_Supply_Products], " +
                                "[Date_Conclusion_Contract_Supply_Products], [Subject_Contract_Supply_Products], " +
                                "[Amount_Contract_Supply_Products], [Term_Contract_Supply_Products], " +
                                "[Responsibility_Contract_Supply_Productss], " +
                                "[Ccontroversial_Issues_Contract_Supply_Products], " +
                                "[Name_Supplier] from [dbo].[Contract_Supply_Products] " +
                                $"inner join [dbo].[Supplier] on [Supplier_ID] = [ID_Supplier] " +
                                $"where [Number_Contract_Supply_Products] LIKE '%{searchValue}%'";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);
                    dgContractSupplyProducts.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgContractSupplyProducts.Columns[0].Visibility = Visibility.Hidden;
                    dgContractSupplyProducts.Columns[1].Header = "Номер договора поставки";
                    dgContractSupplyProducts.Columns[2].Header = "Дата заключения договора";
                    dgContractSupplyProducts.Columns[3].Header = "Предмет договора";
                    dgContractSupplyProducts.Columns[4].Header = "Сумма договора ";
                    dgContractSupplyProducts.Columns[5].Header = "Срок исполнения обязательств";
                    dgContractSupplyProducts.Columns[6].Header = "Ответственность сторон ";
                    dgContractSupplyProducts.Columns[7].Header = "Рассмотрение споров";
                    dgContractSupplyProducts.Columns[8].Header = "Наименование поставщика";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }

        private void txtSupplier_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = txtSupplier.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchValue))
                    {
                        // Если TextBox пуст, отображаем все записи
                        query = "select [ID_Supplier], [Name_Supplier], [OGRN_Supplier], [INN_Supplier], [KPP_Supplier], [Legal_Address_Supplier] from [dbo].[Supplier]";
                    }
                    else
                    {
                        // Если есть введенное значение, выполняем частичный поиск
                        query = $"select [ID_Supplier], [Name_Supplier], [OGRN_Supplier], [INN_Supplier], [KPP_Supplier], [Legal_Address_Supplier] from [dbo].[Supplier] " +
                                $"where [Name_Supplier] LIKE '%{searchValue}%'";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);
                    dgSupplier.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgSupplier.Columns[0].Visibility = Visibility.Hidden;
                    dgSupplier.Columns[1].Header = "Наименование поставщика";
                    dgSupplier.Columns[2].Header = "ОГРН";
                    dgSupplier.Columns[3].Header = "ИНН";
                    dgSupplier.Columns[4].Header = "КПП ";
                    dgSupplier.Columns[5].Header = "Юридический адрес ";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }

        private void txtSupplyProducts_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = txtSupplyProducts.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchValue))
                    {
                        // Если TextBox пуст, отображаем все записи
                        query = "select [ID_Supply_Products], [Number_Supply_Products], " +
                                "[First_Name_Employee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee], " +
                                "[Number_Contract_Supply_Products], [Status_Supply] from [dbo].[Supply_Products] " +
                                "inner join [dbo].[Employee] on [Employee_ID] = [ID_Employee] " +
                                "inner join [dbo].[Contract_Supply_Products] on [Contract_Supply_Products_ID] = [ID_Contract_Supply_Products] " +
                                "inner join [dbo].[Status_Supply_Products] on [Status_Supply_Products_ID] = [ID_Status_Supply_Products]";
                    }
                    else
                    {
                        // Если есть введенное значение, выполняем частичный поиск
                        query = $"select [ID_Supply_Products], [Number_Supply_Products], " +
                                "[First_Name_Employee]+' '+[Second_Name_Employee]+' '+[Middle_Name_Employee], " +
                                "[Number_Contract_Supply_Products], [Status_Supply] from [dbo].[Supply_Products] " +
                                "inner join [dbo].[Employee] on [Employee_ID] = [ID_Employee] " +
                                "inner join [dbo].[Contract_Supply_Products] on [Contract_Supply_Products_ID] = [ID_Contract_Supply_Products] " +
                                "inner join [dbo].[Status_Supply_Products] on [Status_Supply_Products_ID] = [ID_Status_Supply_Products] " +
                                $"where [Number_Supply_Products] LIKE '%{searchValue}%'";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);
                    dgSupplyProducts.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgSupplyProducts.Columns[0].Visibility = Visibility.Hidden;
                    dgSupplyProducts.Columns[1].Header = "Номер поставки";
                    dgSupplyProducts.Columns[2].Header = "ФИО ответственного сотрудника";
                    dgSupplyProducts.Columns[3].Header = "Номер договора поставки";
                    dgSupplyProducts.Columns[4].Header = "Статус поставки";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }

        private void txtProudctsInStock_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = txtProudctsInStock.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchValue))
                    {
                        // Если TextBox пуст, отображаем все записи
                        query = "select [ID_Products_In_Stock], [Name_Position_Products_In_Stock], " +
                                "[Quantity_In_Stock_Products_In_Stock], [Number_Supply_Products] from [dbo].[Products_In_Stock] " +
                                "inner join [dbo].[Supply_Products] on [Supply_Products_ID] = [ID_Supply_Products]";
                    }
                    else
                    {
                        // Если есть введенное значение, выполняем частичный поиск
                        query = $"select [ID_Products_In_Stock], [Name_Position_Products_In_Stock], " +
                                "[Quantity_In_Stock_Products_In_Stock], [Number_Supply_Products] from [dbo].[Products_In_Stock] " +
                                "inner join [dbo].[Supply_Products] on [Supply_Products_ID] = [ID_Supply_Products] " +
                                $"where [Name_Position_Products_In_Stock] LIKE '%{searchValue}%'";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);
                    dgProductsInStock.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgProductsInStock.Columns[0].Visibility = Visibility.Hidden;
                    dgProductsInStock.Columns[1].Header = "Наименование позиции";
                    dgProductsInStock.Columns[2].Header = "Кол-во в наличии";
                    dgProductsInStock.Columns[3].Header = "Номер поставки";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }

        private void txtSales_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = txtSales.Text.Trim();

            try
            {
                Action action = () =>
                {
                    DataBaseClass dataBaseClass = new DataBaseClass();

                    string query;

                    if (string.IsNullOrEmpty(searchValue))
                    {
                        // Если TextBox пуст, отображаем все записи
                        query = "select [ID_Sale], [Number_Sale], [Name_Position_Products_In_Stock] from [dbo].[Sale] " +
                                "inner join [dbo].[Products_In_Stock] on [Products_In_Stock_ID] = [ID_Products_In_Stock]";
                    }
                    else
                    {
                        // Если есть введенное значение, выполняем частичный поиск
                        query = $"select [ID_Sale], [Number_Sale], [Name_Position_Products_In_Stock] from [dbo].[Sale] " +
                                "inner join [dbo].[Products_In_Stock] on [Products_In_Stock_ID] = [ID_Products_In_Stock] " +
                                $"where [Number_Sale] LIKE '%{searchValue}%'";
                    }

                    dataBaseClass.sqlExecute(query, DataBaseClass.act.select);
                    dgSale.ItemsSource = dataBaseClass.resultTable.DefaultView;
                    dgSale.Columns[0].Visibility = Visibility.Hidden;
                    dgSale.Columns[1].Header = "Номер продажи";
                    dgSale.Columns[2].Header = "Позиция товара";
                };
                Dispatcher.Invoke(action);
            }
            catch { }
        }
    }
}
