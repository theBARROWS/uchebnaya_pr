﻿using System;
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
    /// Логика взаимодействия для FormDeals.xaml
    /// </summary>
    public partial class FormDeals : Window
    {


        private MainWindow mainWindow;
        realEstateDB2Entities context = new realEstateDB2Entities();
        private deals editeddeal;
        public enum OperationType
        {
            Add,
            Edit
        }
        private OperationType operationType;

        private ObservableCollection<int> GetSupplyIds()
        {
            var list = context.supplies.Select(client => client.Id).ToList();
            return new ObservableCollection<int>(list);
        }
        private ObservableCollection<int> GetDemandsIds()
        {
            var list = context.demands.Select(client => client.Id).ToList();
            return new ObservableCollection<int>(list);
        }
        public void LoadIds()
        {
            try
            {
                deaslsIDsupply.ItemsSource = GetSupplyIds();
                dealsIDdemand.ItemsSource = GetDemandsIds();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public FormDeals(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            LoadIds();

            operationType = OperationType.Add;
        }
        public FormDeals(MainWindow main, deals deal)
        {
            InitializeComponent();
            mainWindow = main;
            operationType = OperationType.Edit;
            LoadIds();
            editeddeal = deal;

            //suppliesAGID.IsEnabled = false;
            //suppliesCLID.IsEnabled = false;

            deaslsIDsupply.SelectedIndex = deal.Supply_Id;
            dealsIDdemand.SelectedIndex = deal.Demand_Id;

        }
        private ObservableCollection<deals> GetRealEstates()
        {
            var list = context.deals.ToList();
            return new ObservableCollection<deals>(list);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (operationType == OperationType.Add)
                {
                    deals newdeal = new deals
                    {
                        Demand_Id = Convert.ToInt32(dealsIDdemand.SelectedValue),
                        Supply_Id = Convert.ToInt32(deaslsIDsupply.SelectedValue),
                    };
                    context.deals.Add(newdeal);
                }
                else if (operationType == OperationType.Edit)
                {
                    deals dealtoUpdate = context.deals.Find(editeddeal.Id);
                    dealtoUpdate.Demand_Id = Convert.ToInt32(dealsIDdemand.SelectedValue);
                    dealtoUpdate.Supply_Id = Convert.ToInt32(deaslsIDsupply.SelectedValue);

                }

                context.SaveChanges();
                mainWindow.LoadData();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Введите все данные!");
            }
        }
    }
}