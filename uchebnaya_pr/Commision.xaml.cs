using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Commision.xaml
    /// </summary>
    public partial class Commision : Window
    {
        private MainWindow mainWindow;
        realEstateDB2Entities context = new realEstateDB2Entities();
        public Commision()
        {
            InitializeComponent();
        }
        public Commision(MainWindow main, deals deal)
        {
            InitializeComponent();
            mainWindow = main;

            int salePrice = Convert.ToInt32(deal.supplies.Price);
            double sellerCommission = 0;
            double buyerCommission = 0;
            double sellerAgentShare = 0.45; 
            double buyerAgentShare = 0.45; 

            if (deal.supplies.realEstate.TypeId == 1) 
            {
                sellerCommission = 36000 + 0.01 * salePrice;
            }
            else if (deal.supplies.realEstate.TypeId == 2) 
            {
                sellerCommission = 30000 + 0.02 * salePrice;
            }
            else if (deal.supplies.realEstate.TypeId == 3) 
            {
                sellerCommission = 30000 + 0.01 * salePrice;
            }
            buyerCommission = 0.03 * salePrice;

            if (deal.supplies.agents.DealShare.HasValue)
            {
                sellerAgentShare = deal.supplies.agents.DealShare.Value / 100.0;
                buyerAgentShare = deal.supplies.agents.DealShare.Value / 100.0;
            }

            double sellerAgentCommission = sellerCommission * sellerAgentShare;

            double buyerAgentCommission = buyerCommission * buyerAgentShare;

            double companyCommission = (sellerCommission * (1 - sellerAgentShare)) + (buyerCommission * (1 - buyerAgentShare));

            pr1.Text = sellerCommission.ToString("N2");
            pr2.Text = buyerCommission.ToString("N2");
            pr3.Text = sellerAgentCommission.ToString("N2");
            pr4.Text = buyerAgentCommission.ToString("N2");
            pr5.Text = companyCommission.ToString("N2");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                        this.Close();
        }
    }
}
