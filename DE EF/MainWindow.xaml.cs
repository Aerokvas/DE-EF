using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DE_EF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            MainFrame.Navigate(new LoginPage());
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вход выполнен!");
        }

        private void LoadData()
        {
            using (var context = new TestDbContext())
            {
                var data = context.User.Include("Post").ToList();
                userGrid.ItemsSource = data;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var user = ((Button)sender).DataContext as User;
            if (user != null)
            {
                using (var context = new TestDbContext())
                {
                    context.User.Attach(user);
                    context.User.Remove(user);
                    context.SaveChanges();
                }
                LoadData();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var user = ((Button)sender).DataContext as User;
            if (user != null)
            {
                var editWindow = new EditUserWindow(user);
                if (editWindow.ShowDialog() == true)
                {
                    using (var context = new TestDbContext())
                    {
                        var existingUser = context.User.Include("Post").FirstOrDefault(u => u.id == user.id);
                       
                        if (existingUser != null)
                        {
                            existingUser.name = user.name;
                            existingUser.password = user.password;
                            existingUser.post_id = user.post_id;
                            existingUser.Post = context.Post.Find(user.post_id);
                            context.SaveChanges();
                        }
                    }
                    LoadData();
                }
            }
        }
    }
}
