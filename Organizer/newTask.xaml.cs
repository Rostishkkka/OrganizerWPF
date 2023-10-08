using Microsoft.IdentityModel.Tokens;
using Organizer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Логика взаимодействия для newTask.xaml
    /// </summary>
    public partial class newTask : Window
    {
        public newTask()
        {
            InitializeComponent();
            CountDay.IsChecked = true;
            dateBox.SelectedDate = DateTime.Now.AddDays(5);
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
                typeBox.Items.Add(type.Type);
            }
        }

        private void CancelBut_Click(object obj, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateTypeBut_Click(object obj, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(newTypeBox.Text))
            {
                MessageBox.Show("В поле названия нового типа не введены данные.",
                    "Предупрждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Types type = new Types();
            type.Type = newTypeBox.Text;
            type.Status = 1;
            DbUtils.db.Types.Add(type);
            DbUtils.db.SaveChanges();
            typeBox.Items.Add(newTypeBox.Text);
        }

        private void CreateBut_Click(object obj, RoutedEventArgs e)
        {

            if(String.IsNullOrEmpty(descrBox.Text) || String.IsNullOrEmpty(advancedBox.Text)
                || String.IsNullOrEmpty(typeBox.Text))
            {
                MessageBox.Show("Не заполнены обязательные данные. " +
                    "К обязательным относятся следующие поля:\n" +
                    "· Описание задачи;\n· Тип задачи;\n· Важность задачи.",
                    "Предупрждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Tasks task = new Tasks();

            task.Description = descrBox.Text;
            if(CountDay.IsChecked == true)
            {
                task.Day = Convert.ToInt32(dayCountBox.Text);
            }
            else if(DateDay.IsChecked == true) 
            {
                task.Day = Convert.ToInt32((dateBox.SelectedDate.Value.Date - DateTime.Now.Date).Days);
            }
            task.Status = 1;
            task.Type = Convert.ToInt32(DbUtils.db.Types.Where(w => w.Status == 1).FirstOrDefault(f => f.Type == typeBox.Text).Id);
            task.Advanced = Convert.ToInt32(advancedBox.Text);
            DbUtils.db.Tasks.Add(task);
            DbUtils.db.SaveChanges();
            this.Close();
        }

        private void CountDay_Checked(object sender, RoutedEventArgs e)
        {
            dateBox.IsEnabled = false;
            dayCountBox.IsEnabled = true;
        }

        private void DateDay_Checked(object sender, RoutedEventArgs e)
        {
            dayCountBox.IsEnabled = false;
            dateBox.IsEnabled = true;
        }
    }
}