using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
        private ObservableCollection<string> GetREIds()
        {
            var list = context.realEstate.Select(d =>
        $"{d.Address_City}, {d.Address_Street}, {d.Address_House} {d.Address_Number}").ToList();
            return new ObservableCollection<string>(list);
        }



        private ObservableCollection<string> GetCLFIRSTNAME()
        {
            var list = context.clients.Select(d => d.FirstName).ToList();
            return new ObservableCollection<string>(list);
        }

        private ObservableCollection<string> GetAGENTFIRSTNAME()
        {
            var list = context.agents.Select(d => d.FirstName).ToList();
            return new ObservableCollection<string>(list);
        }
        private ObservableCollection<string> GetRESTATESADDRESS()
        {
            //    var list = context.realEstate.Select(d =>
            //$"{d.Address_City}, {d.Address_Street}, {d.Address_House} {d.Address_Number}").ToList();
            //    return new ObservableCollection<string>(list);
            var list = context.realEstate.ToList();
            var formattedList = list.Select(d => $"{d.Address_City}, {d.Address_Street}, {d.Address_House} {d.Address_Number}").ToList();
            return new ObservableCollection<string>(formattedList);

        }

        private int GetClientIdBYFIRSTNAME(string surname)
        {
            var supply = context.clients.FirstOrDefault(s => s.FirstName == surname);
            return supply != null ? supply.Id : -1; // Return -1 if not found
        }

        private int GetAgentIdBYFIRSTNAME(string surname)
        {
            var supply = context.agents.FirstOrDefault(s => s.FirstName == surname);
            return supply != null ? supply.Id : -1; // Return -1 if not found
        }
        private int GetREbyAddress(string surname)
        {
            var supply = context.realEstate.FirstOrDefault(s =>
    (s.Address_City + ", " + s.Address_Street + ", " + s.Address_House + " " + s.Address_Number) == surname);
            return supply != null ? supply.Id : -1; // Return -1 if not found
            //var supply = context.realEstate.FirstOrDefault(s => $"{s.Address_City}, {s.Address_Street}, {s.Address_House} {s.Address_Number}" == surname);
            //return supply != null ? supply.Id : -1; // Return -1 if not found
        }

        public void LoadIds()
        {
            try
            {
                //suppliesAGID.ItemsSource = GetAgentIds();
                //suppliesCLID.ItemsSource = GetClientIds();
                //suppliesREID.ItemsSource = GetREIds();

                suppliesAGID.ItemsSource = GetAGENTFIRSTNAME();
                suppliesCLID.ItemsSource = GetCLFIRSTNAME();
                suppliesREID.ItemsSource = GetRESTATESADDRESS();

                //demandClientId.ItemsSource = GetDemandsCLFirstNames();
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

            suppliesAGID.SelectedItem = supply.agents.FirstName;
            suppliesCLID.SelectedItem = supply.clients.FirstName;
            suppliesREID.SelectedIndex = supply.RealEstateId-1;

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
                        //ClientId = Convert.ToInt32(suppliesCLID.SelectedValue),
                        ClientId = GetClientIdBYFIRSTNAME(suppliesCLID.SelectedValue.ToString()),

                        AgentId = GetAgentIdBYFIRSTNAME(suppliesAGID.SelectedValue.ToString()),

                        RealEstateId = GetREbyAddress(suppliesREID.SelectedValue.ToString()),
                        //AgentId = Convert.ToInt32(suppliesAGID.SelectedValue),
                        //RealEstateId = Convert.ToInt32(suppliesREID.SelectedValue),

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
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //}
        }
    }
}
