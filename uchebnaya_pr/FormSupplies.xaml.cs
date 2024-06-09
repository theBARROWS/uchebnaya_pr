using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using uchebnaya_pr.Models;

namespace uchebnaya_pr
{
    /// <summary>
    /// Логика взаимодействия для FormSupplies.xaml
    /// </summary>
    public partial class FormSupplies : Window
    {


        private MainWindow mainWindow;
        realEstateDB2Entities context = new realEstateDB2Entities();
        private supplies editedsupply;
        public enum OperationType
        {
            Add,
            Edit
        }
        private OperationType operationType;

        private ObservableCollection<int> GetClientIds()
        {
            var list = context.clients.Select(client => client.Id).ToList();
            return new ObservableCollection<int>(list);
        }
        private ObservableCollection<int> GetAgentIds()
        {
            var list = context.agents.Select(client => client.Id).ToList();
            return new ObservableCollection<int>(list);
        }
        private ObservableCollection<int> GetREIds()
        {
            var list = context.realEstate.Select(client => client.Id).ToList();
            return new ObservableCollection<int>(list);
        }
        public void LoadIds()
        {
            try
            {
                suppliesAGID.ItemsSource = GetAgentIds();
                suppliesCLID.ItemsSource = GetClientIds();
                suppliesREID.ItemsSource = GetREIds();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public FormSupplies(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            LoadIds();

            operationType = OperationType.Add;
        }
        public FormSupplies(MainWindow main, supplies supply)
        {
            InitializeComponent();
            mainWindow = main;
            operationType = OperationType.Edit;
            LoadIds();
            editedsupply = supply;

            suppliesAGID.IsEnabled = false;
            suppliesCLID.IsEnabled = false;
            suppliesREID.IsEnabled = false;

            suppliesAGID.SelectedIndex = supply.AgentId;
            suppliesCLID.SelectedIndex = supply.ClientId;
            suppliesREID.SelectedIndex = supply.RealEstateId;

            priceTB.Text = supply.Price.ToString();
        }
        private ObservableCollection<supplies> GetRealEstates()
        {
            var list = context.supplies.ToList();
            return new ObservableCollection<supplies>(list);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (suppliesCLID.SelectedValue == null || suppliesAGID.SelectedValue == null || suppliesREID.SelectedValue == null)
                {
                    MessageBox.Show("Пожалуйста, выберите клиента, риэлтора и объект недвижимости.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(priceTB.Text) || !int.TryParse(priceTB.Text, out int price) || price <= 0)
                {
                    MessageBox.Show("Цена должна быть целым положительным числом.");
                    return;
                }
                if (operationType == OperationType.Add)
                {
                    supplies newsupply = new supplies
                    {
                        ClientId = Convert.ToInt32(suppliesCLID.SelectedValue),
                        AgentId = Convert.ToInt32(suppliesAGID.SelectedValue),
                        RealEstateId = Convert.ToInt32(suppliesREID.SelectedValue),

                        Price = Convert.ToInt32(priceTB.Text)
                    };
                    context.supplies.Add(newsupply);
                    context.SaveChanges();
                    mainWindow.LoadData();
                }
                else if (operationType == OperationType.Edit)
                {
                    supplies supplyToUpdate = context.supplies.Find(editedsupply.Id);
                    supplyToUpdate.Price = Convert.ToInt32(priceTB.Text);

                    context.SaveChanges();
                    var list = context.supplies.ToList();
                    mainWindow.SuppliesDataGrid.ItemsSource = new ObservableCollection<supplies>(list);
                }

                this.Close();
            }
            catch
            {
                MessageBox.Show("Введите все данные!");
            }
        }
    }
}
