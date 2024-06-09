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
    /// Логика взаимодействия для FormRealEstates.xaml
    /// </summary>
    public partial class FormRealEstates : Window
    {

        private MainWindow mainWindow;
        realEstateDB2Entities context = new realEstateDB2Entities();
        private realEstate editedEstate;
        public enum OperationType
        {
            Add,
            Edit
        }
        private OperationType operationType;

        public FormRealEstates(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;

            operationType = OperationType.Add;
        }
        public FormRealEstates(MainWindow main, realEstate realEstate)
        {
            InitializeComponent();
            mainWindow = main;
            operationType = OperationType.Edit;
            editedEstate = realEstate;


            if (realEstate.TypeId == 1) realEstateType.SelectedIndex = 0;
            else if (realEstate.TypeId == 2) realEstateType.SelectedIndex = 1;
            else if (realEstate.TypeId == 3) realEstateType.SelectedIndex = 2;

            realEstateCity.Text = realEstate.Address_City;
            realEstateHouse.Text = realEstate.Address_House;
            realEstateStreet.Text = realEstate.Address_Street;
            realEstateNumber.Text = realEstate.Address_Number;

            realEstateTude.Text = realEstate.Coordinate_latitude;
            realEstateLongtitude.Text = realEstate.Coordinate_longtitude;

            realEstateFloors.Text = realEstate.Floors.ToString();
            realEstateRooms.Text = realEstate.Rooms.ToString();
            realEstateArea.Text = realEstate.TotalArea.ToString();
        }
        private ObservableCollection<realEstate> GetRealEstates()
        {
            var list = context.realEstate.ToList();
            return new ObservableCollection<realEstate>(list);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                double? longitude = null;
                double? latitude = null;
                if (!string.IsNullOrWhiteSpace(realEstateLongtitude.Text) || !string.IsNullOrWhiteSpace(realEstateTude.Text))
                {
                    if (double.TryParse(realEstateLongtitude.Text, out double lon) && double.TryParse(realEstateTude.Text, out double lat))
                    {
                        if (lon >= -180 && lon <= 180 && lat >= -90 && lat <= 90)
                        {
                            longitude = lon;
                            latitude = lat;
                        }
                        else
                        {
                            MessageBox.Show("Координаты должны быть в диапазоне: широта от -90 до +90, долгота от -180 до +180.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите корректные значения координат.");
                        return;
                    }
                }

                //bool hasAddress = !string.IsNullOrWhiteSpace(realEstateCity.Text) || !string.IsNullOrWhiteSpace(realEstateStreet.Text) ||
                //          !string.IsNullOrWhiteSpace(realEstateHouse.Text) || !string.IsNullOrWhiteSpace(realEstateNumber.Text);

                double? totalArea = null;
                if (!string.IsNullOrWhiteSpace(realEstateArea.Text))
                {
                    if (double.TryParse(realEstateArea.Text, out double area))
                    {
                        totalArea = area;
                    }
                    else
                    {
                        MessageBox.Show("Введите корректное значение для общей площади.");
                        return;
                    }
                }

                int? rooms = null;
                if (!string.IsNullOrWhiteSpace(realEstateRooms.Text))
                {
                    if (int.TryParse(realEstateRooms.Text, out int roomCount))
                    {
                        rooms = roomCount;
                    }
                    else
                    {
                        MessageBox.Show("Введите корректное значение для количества комнат.");
                        return;
                    }
                }
                int? floors = null;
                if (!string.IsNullOrWhiteSpace(realEstateFloors.Text))
                {
                    if (int.TryParse(realEstateFloors.Text, out int floorCount))
                    {
                        floors = floorCount;
                    }
                    else
                    {
                        MessageBox.Show("Введите корректное значение для количества этажей.");
                        return;
                    }
                }

                if (operationType == OperationType.Add)
                {

                        realEstate newrealEstate = new realEstate
                        {
                            TypeId = realEstateType.SelectedIndex + 1,
                            Address_City = realEstateCity?.Text,
                            Address_House = realEstateHouse?.Text,
                            Address_Number = realEstateNumber?.Text,
                            Address_Street = realEstateStreet?.Text,

                            Coordinate_longtitude = realEstateLongtitude?.Text,
                            Coordinate_latitude = realEstateTude?.Text,

                            TotalArea = totalArea,
                            Rooms = rooms,
                            Floors = floors
                        };
                        context.realEstate.Add(newrealEstate);
                }
                else if (operationType == OperationType.Edit)
                {
                    realEstate estateToUpdate = context.realEstate.Find(editedEstate.Id);

                    estateToUpdate.TypeId = realEstateType.SelectedIndex + 1;
                    estateToUpdate.Address_City = realEstateCity.Text;
                    estateToUpdate.Address_House = realEstateHouse.Text;
                    estateToUpdate.Address_Number = realEstateNumber.Text;
                    estateToUpdate.Address_Street = realEstateStreet.Text;


                    estateToUpdate.Coordinate_longtitude = realEstateLongtitude.Text;
                    estateToUpdate.Coordinate_latitude = realEstateTude.Text;


                    estateToUpdate.TotalArea = totalArea;
                    estateToUpdate.Rooms = rooms;
                    estateToUpdate.Floors = floors;
                    //estateToUpdate.TotalArea = Convert.ToDouble(realEstateArea.Text);
                    //estateToUpdate.Rooms = Convert.ToInt32(realEstateRooms.Text);
                    //estateToUpdate.Floors = Convert.ToInt32(realEstateFloors.Text);

                }

                context.SaveChanges();
                mainWindow.LoadData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
