using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = UsernameTextBox.Text;
            string newPassword = PasswordBox.Password;
            string repeatPassword = ConfirmPasswordBox.Password;

            if (!string.IsNullOrEmpty(newUsername) && !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(repeatPassword))
            {
                if (newPassword != repeatPassword)
                {
                    MessageBox.Show("Пароли не совпадают!");
                    return;
                }

                if (RegisterNewUser(newUsername, newPassword))
                {
                    MessageBox.Show("Пользователь успешно зарегистрирован!");
                    NavigationService.Navigate(new LoginPage());
                }
                else
                {
                    MessageBox.Show("При регистрации произошла ошибка!");

                }
            } 
            else
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        private bool RegisterNewUser(string username, string password)
        {
            using (var db = new TestDbContext())
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    try
                    {
                        string hashedPassword = HashPassword(password);
                        User user = new User { name = username, password = hashedPassword };
                        db.User.Add(user);
                        db.SaveChanges();
                        Console.WriteLine($"Новый пользователь: Логин - {username}, Пароль - {password}");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
