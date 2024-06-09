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
    /// Логика взаимодействия для FormAgents.xaml
    /// </summary>
    public partial class FormAgents : Window
    {
        private MainWindow mainWindow;
        realEstateDB2Entities context = new realEstateDB2Entities();
        private agents editedAgent;
        public enum OperationType
        {
            Add,
            Edit
        }
        private OperationType operationType;

        public FormAgents(MainWindow main)
        {
            InitializeComponent();
            mainWindow = main;

            operationType = OperationType.Add;
        }
        public FormAgents(MainWindow main, agents agent)
        {
            InitializeComponent();
            mainWindow = main;
            operationType = OperationType.Edit;
            editedAgent = agent;

            firstNameTextBox.Text = agent.FirstName;
            middleNameTextBox.Text = agent.MiddleName;
            lastNameTextBox.Text = agent.LastName;
            dealShareTextBox.Text = agent.DealShare.ToString();
        }
        private ObservableCollection<agents> GetClients()
        {
            var list = context.agents.ToList();
            return new ObservableCollection<agents>(list);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text) ||
              string.IsNullOrWhiteSpace(lastNameTextBox.Text) ||
              string.IsNullOrWhiteSpace(middleNameTextBox.Text))
            {
                MessageBox.Show("Все поля ФИО обязательны для заполнения.");
                return;
            }

            int? dealShare = null;
            if (!string.IsNullOrWhiteSpace(dealShareTextBox.Text))
            {
                if (int.TryParse(dealShareTextBox.Text, out int dealShareValue))
                {
                    if (dealShareValue < 0 || dealShareValue > 100)
                    {
                        MessageBox.Show("Доля от комиссии должна быть числом от 0 до 100.");
                        return;
                    }
                    dealShare = dealShareValue;
                }
                else
                {
                    MessageBox.Show("Введите корректное значение для доли от комиссии.");
                    return;
                }
            }

            if (operationType == OperationType.Add)
            {
                agents newAgent = new agents
                {
                    FirstName = firstNameTextBox.Text,
                    MiddleName = middleNameTextBox.Text,
                    LastName = lastNameTextBox.Text,
                    DealShare = dealShare,
                };

                context.agents.Add(newAgent);
            }
            else if (operationType == OperationType.Edit)
            {
                agents agentUpdated = context.agents.Find(editedAgent.Id);

                agentUpdated.FirstName = firstNameTextBox.Text;
                agentUpdated.MiddleName = middleNameTextBox.Text;
                agentUpdated.LastName = lastNameTextBox.Text;
                agentUpdated.DealShare = dealShare;
            }

            context.SaveChanges();
            mainWindow.LoadData();
            this.Close();
        }
    }
}
