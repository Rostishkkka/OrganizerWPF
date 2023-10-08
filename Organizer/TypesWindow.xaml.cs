using Organizer.Models;
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

namespace Organizer
{
    /// <summary>
    /// Логика взаимодействия для TypesWindow.xaml
    /// </summary>
    public partial class TypesWindow : Window
    {
        public TypesWindow()
        {
            InitializeComponent();
            LoadType();
        }

        private void LoadType()
        {
            var allTypes = DbUtils.db.Types.Where(w => w.Status == 1).Select(s => new
            {
                s.Type
            });
            foreach (var type in allTypes)
            {
                typeComboBox.Items.Add(type.Type);
            }
        }

        private void CancelBut_Click(object obj, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateTypeBut_Click(object obj, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(typeBox.Text))
            {
                MessageBox.Show("В поле названия типа не введены данные.",
                   "Предупрждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Types type = new Types();
            type.Type = typeBox.Text;
            type.Status = 1;
            DbUtils.db.Types.Add(type);
            DbUtils.db.SaveChanges();
            typeComboBox.Items.Add(typeBox.Text);
        }

        private void DeleteTypeBut_Click(object obj, RoutedEventArgs e)
        {
            Types type = DbUtils.db.Types.Where(w => w.Type == typeComboBox.Text && w.Status == 1).FirstOrDefault();
            type.Status = 0;
            DbUtils.db.SaveChanges();
            typeComboBox.Items.Remove(typeComboBox.Text);
        }

        private void ModifyTypeBut_Click(object obj, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(typeBox.Text))
            {
                MessageBox.Show("В поле названия типа не введены данные.",
                   "Предупрждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Types type = DbUtils.db.Types.Where(w => w.Type == typeComboBox.Text && w.Status == 1).FirstOrDefault();
            type.Type = typeBox.Text;
            typeComboBox.Items.Remove(typeComboBox.Text);
            typeComboBox.Items.Add(typeBox.Text);
            DbUtils.db.SaveChanges();
        }
    }
}
