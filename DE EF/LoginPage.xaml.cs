using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new TestDbContext())
            {
                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        User user = db.User.SingleOrDefault(u => u.name == username);

                        if (user != null && VerifyPassword(password, user.password))
                        {
                            MessageBox.Show("Успешно!", $"Привет, {user.name}!");
                        }
                        else
                        {
                            MessageBox.Show("Неверное имя пользователя или пароль!");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Произошла ошибка при входе!");
                    }
                }
                else
                {
                    MessageBox.Show("Не все поля заполнены!");
                }
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                string enteredPasswordHash = builder.ToString();
                return enteredPasswordHash == hashedPassword;
            }
        }
    }
}
