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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Week9Linq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NORTHWNDEntities nw = new NORTHWNDEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Ex1Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in nw.Categories
                        join p in nw.Products on c.CategoryName equals p.Category.CategoryName
                        orderby c.CategoryName
                        select new { Category = c.CategoryName, Product = p.ProductName };

            var result = query.ToList();
            Ex1lbDisplay.ItemsSource = result;
            Ex1tblkCount.Text = result.Count.ToString();
        }

        private void Ex2Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in nw.Products
                        orderby p.Category.CategoryName, p.ProductName
                        select new { Category = p.Category.CategoryName, Product = p.ProductName };

            var result = query.ToList();
            Ex2lbDisplay.ItemsSource = result;
            Ex2tblkCount.Text = result.Count.ToString();
        }

        private void Ex3Button_Click(object sender, RoutedEventArgs e)
        {
            var query1 = (from detail in nw.Order_Details
                          where detail.ProductID == 7
                          select detail);

            var query2 = (from detail in nw.Order_Details
                         where detail.ProductID == 7
                         select detail.UnitPrice * detail.Quantity);


            int NumberOfOrders = query1.Count();
            decimal totalvalue = query2.Sum();
            decimal averageValue = query2.Average();

            Ex3tblkCount.Text = string.Format("Total number of orders {0}\nValue of orders {1:C}\nValue Average Order value {2:C}",
                NumberOfOrders, totalvalue, averageValue);
        }

        private void Ex4Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in nw.Customers
                        where customer.Orders.Count >= 20
                        select new
                        {
                            name = customer.CompanyName,
                            ordercount = customer.Orders.Count
                        };

            Ex4lbDisplay.ItemsSource = query.ToList();
        }

        private void Ex5Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in nw.Customers
                        where customer.Orders.Count < 3
                        select new
                        {
                            Company = customer.CompanyName,
                            City = customer.City,
                            region = customer.Region,
                            ordercount = customer.Orders.Count
                        };

            Ex5lbDisplay.ItemsSource = query.ToList();
        }

        private void Ex6Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from customer in nw.Customers
                        orderby customer.CompanyName
                        select customer.CompanyName;

            Ex6lbDisplay.ItemsSource = query.ToList();
        }

        private void Ex6lbDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string company = (string)Ex6lbDisplay.SelectedItem;

            if(company != null)
            {
                var query = (from detail in nw.Order_Details
                             where detail.Order.Customer.CompanyName == company
                             select detail.UnitPrice * detail.Quantity).Sum();

                Ex6tblkCount.Text = string.Format("Total for supplier {0}\n\n{1:C}", company, query);
            }
        }

        private void Ex7Button_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in nw.Products
                        group p by p.Category.CategoryName into g
                        orderby g.Count() descending
                        select new
                        {
                            Category = g.Key,
                            count = g.Count()
                        };

            Ex7lbDisplay.ItemsSource = query.ToList();
        }
    }
}
