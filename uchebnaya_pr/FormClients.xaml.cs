using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
    /// Логика взаимодействия для FormClients.xaml
    /// </summary>
    public partial class FormClients : Window
    {
        private MainWindow mainWindow;
        realEstateDB2Entities context = new realEstateDB2Entities();
        private clients editedClient;
        public enum OperationType
        {
            Add,
            Edit
        }
        private OperationType operationType;

        public FormClients(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;

            operationType = OperationType.Add;
        }
        public FormClients(MainWindow main, clients client)
        {
            InitializeComponent();
            mainWindow = main;
            operationType = OperationType.Edit;
            editedClient = client;

            firstNameTextBox.Text = client.FirstName;
            middleNameTextBox.Text = client.MiddleName;
            lastNameTextBox.Text = client.LastName;
            phoneTextBox.Text = client.Phone;
            emailTextBox.Text = client.Email;
        }
        private ObservableCollection<clients> GetClients()
        {
            var list = context.clients.ToList();
            return new ObservableCollection<clients>(list);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(phoneTextBox.Text) && string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                MessageBox.Show("Укажите хотя бы номер телефона или электронную почту.");
                return;
            }
            if (!string.IsNullOrWhiteSpace(phoneTextBox.Text) && !phoneTextBox.Text.All(char.IsDigit))
            {
                MessageBox.Show("Номер телефона должен содержать только цифры.");
                return;
            }
            if (operationType == OperationType.Add)
            {

                clients newClient = new clients
                {
                    FirstName = firstNameTextBox.Text,
                    MiddleName = middleNameTextBox.Text,
                    LastName = lastNameTextBox.Text,
                    Phone = phoneTextBox.Text,
                    Email = emailTextBox.Text
                };

                context.clients.Add(newClient);
            }
            else if (operationType == OperationType.Edit)
            {
                clients clientUpdated = context.clients.Find(editedClient.Id);

                clientUpdated.FirstName = firstNameTextBox.Text;
                clientUpdated.MiddleName = middleNameTextBox.Text;
                clientUpdated.LastName = lastNameTextBox.Text;
                clientUpdated.Phone = phoneTextBox.Text;
                clientUpdated.Email = emailTextBox.Text;
            }

            context.SaveChanges();
            mainWindow.LoadData();
            this.Close();
        }
    }
}
