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
using System.Windows.Shapes;

namespace DE_EF
{
    /// <summary>
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private User _user;

        public EditUserWindow(User user)
        {
            InitializeComponent();
            _user = user;
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new TestDbContext())
            {
                PostComboBox.ItemsSource = context.Post.ToList();
            }
            NameTextBox.Text = _user.name;
            PasswordBox.Password = _user.password;
            PostComboBox.SelectedValue = _user.post_id;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _user.name = NameTextBox.Text;
            _user.password = PasswordBox.Password;
            _user.post_id = (int)PostComboBox.SelectedValue;
            _user.Post = PostComboBox.SelectedItem as Post;
            DialogResult = true;
        }
    }
}
