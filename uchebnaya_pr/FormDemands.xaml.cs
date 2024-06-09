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

                demandClientId.ItemsSource = GetClientIds();
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

            demandClientId.SelectedItem = demand.ClientId;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (demandClientId.SelectedValue == null || demandrType.SelectedIndex == null)
                {
                    MessageBox.Show("Пожалуйста, выберите клиента и тип объекта недвижимости.");
                    return;
                }

                if (!double.TryParse(minar.Text, out double minArea) || minArea < 0 ||
                    !double.TryParse(maxar.Text, out double maxArea) || maxArea < 0 ||
                    !int.TryParse(minfloor.Text, out int minFloor) || minFloor < 0 ||
                    !int.TryParse(maxfloor.Text, out int maxFloor) || maxFloor < 0 ||
                    !int.TryParse(minpr.Text, out int minPrice) || minPrice <= 0 ||
                    !int.TryParse(maxpr.Text, out int maxPrice) || maxPrice <= 0 ||
                    !int.TryParse(minroom.Text, out int minRooms) || minRooms < 0 ||
                    !int.TryParse(maxroom.Text, out int maxRooms) || maxRooms < 0)
                {
                    MessageBox.Show("Введите корректные значения для площади, этажей, цены и количества комнат.");
                    return;
                }


                if (operationType == OperationType.Add)
                {
                    demands newdemand = new demands
                    {
                        ClientId = Convert.ToInt32(demandClientId.SelectedValue),
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
                }
                else if (operationType == OperationType.Edit)
                {
                    demands demandToUpdate = context.demands.Find(editedEstate.Id);
                    demandToUpdate.ClientId = Convert.ToInt32(demandClientId.SelectedValue);
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
