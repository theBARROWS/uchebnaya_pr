﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using uchebnaya_pr.Models;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;
using UCH_PR;

namespace uchebnaya_pr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        realEstateDB2Entities context = new realEstateDB2Entities();
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private ObservableCollection<clients> GetClients()
        {
            var list = context.clients.ToList();
            return new ObservableCollection<clients>(list);
        }
        private ObservableCollection<agents> GetAgents()
        {
            var list = context.agents.ToList();
            return new ObservableCollection<agents>(list);
        }
        private ObservableCollection<realEstate> GetRealEstates()
        {
            var list = context.realEstate.Include(re => re.type).ToList();
            return new ObservableCollection<realEstate>(list);
        }
        private ObservableCollection<demands> GetDemands()
        {
            var list = context.demands.ToList();
            return new ObservableCollection<demands>(list);
        }
        private ObservableCollection<supplies> GetSupplies()
        {
            var list = context.supplies.ToList();
            return new ObservableCollection<supplies>(list);
        }
        private ObservableCollection<deals> GetDeals()
        {
            var list = context.deals.ToList();
            return new ObservableCollection<deals>(list);
        }

        public void LoadData()
        {
            try
            {

                RealEstateDataGrid.ItemsSource = GetRealEstates();
                ClientDataGrid.ItemsSource =  GetClients();
                AgentDataGrid.ItemsSource = GetAgents();
                DemandDataGrid.ItemsSource = GetDemands();
                SuppliesDataGrid.ItemsSource = GetSupplies();
                DealsDataGrid.ItemsSource = GetDeals();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = ClientDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    clients client = (clients)ClientDataGrid.SelectedItem;
                    //DataGrid.Items.Remove(selectedItem);
                    context.clients.Remove(client);
                    context.SaveChanges();

                    LoadData();
                }
            }
            catch
            {
                MessageBox.Show("Нельзя удалить клиента с существующими сделками/потребностями.");
            }
        }


        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            FormClients form = new FormClients(this);
            form.ShowDialog();
            //LoadData();
        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ClientDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                clients client = (clients)ClientDataGrid.SelectedItem;
                FormClients form = new FormClients(this, client);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования.");
            }
        }

        private void AddRealEstateButton_Click(object sender, RoutedEventArgs e)
        {
            FormRealEstates form = new FormRealEstates(this);
            form.ShowDialog();
        }
        private void EditRealEstateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = RealEstateDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                realEstate realEstate = (realEstate)RealEstateDataGrid.SelectedItem;
                FormRealEstates form = new FormRealEstates(this, realEstate); 
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите недвижимость для редактирования.");
            }
        }
        private void DeleteRealEstateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = RealEstateDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    realEstate realEstate = (realEstate)RealEstateDataGrid.SelectedItem;
                    //DataGrid.Items.Remove(selectedItem);
                    context.realEstate.Remove(realEstate);
                    context.SaveChanges();

                    LoadData();
                }
            }
            catch
            {
                MessageBox.Show("Нельзя удалить недвижимость с существующими сделками/потребностями.");
            }
        }

        private void AddAgentButton_Click(object sender, RoutedEventArgs e)
        {
            FormAgents form = new FormAgents(this);
            form.ShowDialog();
        }
        private void EditAgentButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = AgentDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                agents agent = (agents)AgentDataGrid.SelectedItem;
                FormAgents form = new FormAgents(this, agent); 
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите агента для редактирования.");
            }
        }
        private void DeleteAgentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = AgentDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    agents agent = (agents)AgentDataGrid.SelectedItem;
                    //DataGrid.Items.Remove(selectedItem);
                    context.agents.Remove(agent);
                    context.SaveChanges();

                    LoadData();
                }
            }
            catch
            {
                MessageBox.Show("Нельзя удалить агента с существующими сделками/предложениями.");
            }
        }
        private void AddSuppliesButton_Click(object sender, RoutedEventArgs e)
        {
            FormSupplies form = new FormSupplies(this);
            form.ShowDialog();
        }
        private void EditSuppliesButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = SuppliesDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                supplies supplies = (supplies)SuppliesDataGrid.SelectedItem;
                if (supplies.deals != null && supplies.deals.Any())
                {
                    MessageBox.Show("Это предложение нельзя редактировать, так как оно уже связано со сделкой.");
                }
                else
                {
                    FormSupplies form = new FormSupplies(this, supplies);
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Выберите предложение для редактирования.");
            }
        }
        private void DeleteSuppliesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = SuppliesDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    supplies sypply = (supplies)SuppliesDataGrid.SelectedItem;
                    //DataGrid.Items.Remove(selectedItem);
                    context.supplies.Remove(sypply);
                    context.SaveChanges();

                    LoadData();
                }
            }
            catch
            {
                MessageBox.Show("Нельзя удалить предложение, участвующее в сделке.");
            }
        }
        private void AddDemandButton_Click(object sender, RoutedEventArgs e)
        {
            FormDemands form = new FormDemands(this);
            form.ShowDialog();
        }
        private void EditDemandButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DemandDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                demands demand = (demands)DemandDataGrid.SelectedItem;
                if (demand.deals != null && demand.deals.Any())
                {
                    MessageBox.Show("Эту потребность нельзя редактировать, так как она уже связана со сделкой.");
                }
                else
                {
                    FormDemands form = new FormDemands(this, demand); 
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Выберите потребность для редактирования.");
            }
        }
        private void DeleteDemandButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = DemandDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    demands demand = (demands)DemandDataGrid.SelectedItem;
                    //DataGrid.Items.Remove(selectedItem);
                    context.demands.Remove(demand);
                    context.SaveChanges();

                    LoadData();
                }
            }
            catch
            {
                MessageBox.Show("Нельзя удалить потребность, участвующую в сделке.");
            }
        }
        private void AddDealsButton_Click(object sender, RoutedEventArgs e)
        {
            FormDeals form = new FormDeals(this);
            form.ShowDialog();
        }
        private void EditDealsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DealsDataGrid.SelectedItem;
            if (selectedItem != null)
            {
                deals deals = (deals)DealsDataGrid.SelectedItem;
                FormDeals form = new FormDeals(this, deals);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите сделку для редактирования.");
            }
        }
        private void DeleteDealsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = DealsDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    deals deals = (deals)DealsDataGrid.SelectedItem;
                    //DataGrid.Items.Remove(selectedItem);
                    context.deals.Remove(deals); 
                    context.SaveChanges();

                    LoadData();
                }
            }
            catch
            {
                MessageBox.Show("Нельзя удалить сделку?");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = DealsDataGrid.SelectedItem;
                if (selectedItem != null)
                {
                    deals deals = (deals)DealsDataGrid.SelectedItem;
                    Commision form = new Commision(this, deals);
                    form.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("Что-то не так!");
            }
        }
    }
}