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
using uchebnaya_pr;

namespace UCH_PR
{
    /// <summary>
    /// Логика взаимодействия для FormDemands.xaml
    /// </summary>
    public partial class FormDemands : Window
    {

        private MainWindow mainWindow;
        realEstateDB2Entities context = new realEstateDB2Entities();
        private demands editedEstate;
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
        public void LoadClientsId()
        {
            try
            {

                demandClientId.ItemsSource = GetDemandsCLFirstNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public FormDemands(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;
            LoadClientsId();

            operationType = OperationType.Add;
        }
        public FormDemands(MainWindow main, demands demand)
        {
            InitializeComponent();
            mainWindow = main;
            operationType = OperationType.Edit;
            LoadClientsId();
            editedEstate = demand;

            demandClientId.IsEnabled= false;
            demandrType.IsEnabled=false;

            demandClientId.SelectedItem = demand.clients.FirstName;

            if (demand.TypeId == 1) demandrType.SelectedIndex = 0;
            else if (demand.TypeId == 2) demandrType.SelectedIndex = 1;
            else if (demand.TypeId == 3) demandrType.SelectedIndex = 2;

            realEstateCity.Text = demand.Address_City;
            realEstateHouse.Text = demand.Address_House;
            realEstateStreet.Text = demand.Address_Street;
            realEstateNumber.Text = demand.Address_Number;

            minar.Text = demand.MinArea.ToString();
            maxar.Text = demand.MinArea.ToString();
            minfloor.Text = demand.MinFloor.ToString();
            maxfloor.Text = demand.MaxFloor.ToString();
            minpr.Text = demand.MinPrice.ToString();
            maxpr.Text = demand.MaxPrice.ToString();
            minroom.Text = demand.MinRooms.ToString();
            maxroom.Text = demand.MaxRooms.ToString();
        }
        private ObservableCollection<demands> GetRealEstates()
        {
            var list = context.demands.ToList();
            return new ObservableCollection<demands>(list);
        }

        private ObservableCollection<string> GetDemandsCLFirstNames()
        {
            var list = context.clients.Select(d => d.FirstName).ToList();
            return new ObservableCollection<string>(list);
        }

        private int GetClientIdBySurname(string surname)
        {
            var client = context.clients.FirstOrDefault(s => s.FirstName == surname);
            return client != null ? client.Id : -1; // Return -1 if not found
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (demandClientId.SelectedValue == null || demandrType.SelectedIndex == null)
                {
                    MessageBox.Show("Пожалуйста, выберите клиента и тип объекта недвижимости.");
                    return;
                }

                double? minArea = null;
                double? maxArea = null;
                int? minFloor = null;
                int? maxFloor = null;
                int? minPrice = null;
                int? maxPrice = null;
                int? minRooms = null;
                int? maxRooms = null;

                if (!string.IsNullOrEmpty(minar.Text))
                {
                    if (!double.TryParse(minar.Text, out double minAreaValue) || minAreaValue < 0)
                    {
                        MessageBox.Show("Введите корректное значение для минимальной площади.");
                        return;
                    }
                    minArea = minAreaValue;
                }

                if (!string.IsNullOrEmpty(maxar.Text))
                {
                    if (!double.TryParse(maxar.Text, out double maxAreaValue) || maxAreaValue < 0)
                    {
                        MessageBox.Show("Введите корректное значение для максимальной площади.");
                        return;
                    }
                    maxArea = maxAreaValue;
                }

                if (!string.IsNullOrEmpty(minfloor.Text))
                {
                    if (!int.TryParse(minfloor.Text, out int minFloorValue) || minFloorValue < 0)
                    {
                        MessageBox.Show("Введите корректное значение для минимального этажа.");
                        return;
                    }
                    minFloor = minFloorValue;
                }

                if (!string.IsNullOrEmpty(maxfloor.Text))
                {
                    if (!int.TryParse(maxfloor.Text, out int maxFloorValue) || maxFloorValue < 0)
                    {
                        MessageBox.Show("Введите корректное значение для максимального этажа.");
                        return;
                    }
                    maxFloor = maxFloorValue;
                }

                if (!string.IsNullOrEmpty(minpr.Text))
                {
                    if (!int.TryParse(minpr.Text, out int minPriceValue) || minPriceValue <= 0)
                    {
                        MessageBox.Show("Введите корректное значение для минимальной цены.");
                        return;
                    }
                    minPrice = minPriceValue;
                }

                if (!string.IsNullOrEmpty(maxpr.Text))
                {
                    if (!int.TryParse(maxpr.Text, out int maxPriceValue) || maxPriceValue <= 0)
                    {
                        MessageBox.Show("Введите корректное значение для максимальной цены.");
                        return;
                    }
                    maxPrice = maxPriceValue;
                }

                if (!string.IsNullOrEmpty(minroom.Text))
                {
                    if (!int.TryParse(minroom.Text, out int minRoomsValue) || minRoomsValue < 0)
                    {
                        MessageBox.Show("Введите корректное значение для минимального количества комнат.");
                        return;
                    }
                    minRooms = minRoomsValue;
                }

                if (!string.IsNullOrEmpty(maxroom.Text))
                {
                    if (!int.TryParse(maxroom.Text, out int maxRoomsValue) || maxRoomsValue < 0)
                    {
                        MessageBox.Show("Введите корректное значение для максимального количества комнат.");
                        return;
                    }
                    maxRooms = maxRoomsValue;
                }


                if (operationType == OperationType.Add)
                {
                    demands newdemand = new demands
                    {
                        //ClientId = Convert.ToInt32(demandClientId.SelectedValue),
                        ClientId=GetClientIdBySurname(demandClientId.SelectedValue.ToString()),
                        //Supply_Id = GetSupplyIdBySurname(deaslsIDsupply.SelectedValue.ToString()),
                        TypeId = demandrType.SelectedIndex + 1,
                        Address_City = realEstateCity.Text,
                        Address_House = realEstateHouse.Text,
                        Address_Number = realEstateNumber.Text,
                        Address_Street = realEstateStreet.Text,

                        MinArea= minArea,
                        MaxArea = maxArea,
                        MinFloor = minFloor,
                        MaxFloor = maxFloor,
                        MinPrice = minPrice,
                        MaxPrice= maxPrice,
                        MinRooms= minRooms,
                        MaxRooms= maxRooms
                    };
                    context.demands.Add(newdemand);
                    context.SaveChanges();
                    mainWindow.LoadData();
                }
                else if (operationType == OperationType.Edit)
                {
                    demands demandToUpdate = context.demands.Find(editedEstate.Id);
                    //demandToUpdate.ClientId = Convert.ToInt32(demandClientId.SelectedValue);

                    demandToUpdate.ClientId = GetClientIdBySurname(demandClientId.SelectedValue.ToString());
                    demandToUpdate.TypeId = demandrType.SelectedIndex + 1;
                        demandToUpdate.Address_City = realEstateCity.Text;
                        demandToUpdate.Address_House = realEstateHouse.Text;
                        demandToUpdate.Address_Number = realEstateNumber.Text;
                        demandToUpdate.Address_Street = realEstateStreet.Text;

                    demandToUpdate.MinArea = minArea;
                    demandToUpdate.MaxArea = maxArea;
                    demandToUpdate.MinFloor = minFloor;
                    demandToUpdate.MaxFloor = maxFloor;
                    demandToUpdate.MinPrice = minPrice;
                    demandToUpdate.MaxPrice = maxPrice;
                    demandToUpdate.MinRooms = minRooms;
                    demandToUpdate.MaxRooms = maxRooms;

                    context.SaveChanges();
                    var list = context.demands.ToList();
                    mainWindow.DemandDataGrid.ItemsSource = new ObservableCollection<demands>(list);

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
