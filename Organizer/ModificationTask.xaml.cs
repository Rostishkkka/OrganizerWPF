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
    /// Логика взаимодействия для ModificationTask.xaml
    /// </summary>
    public partial class ModificationTask : Window
    {
        int idTaskUpdate;
        public ModificationTask(Tasks task)
        {
            InitializeComponent();
            CountDay.IsChecked = true;
            dateBox.SelectedDate = DateTime.Now.AddDays(task.Day);
            LoadType();         
            descrBox.Text = task.Description;
            typeBox.Text = DbUtils.db.Types.FirstOrDefault(f => f.Id == task.Type).Type;
            advancedBox.Text = task.Advanced.ToString();
            dayCountBox.Text = task.Day.ToString();
            idTaskUpdate = task.Id;
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

        private void DeleteBut_Click(object obj, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить задачу?", "Подтверждение.", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            Tasks taskDelete = DbUtils.db.Tasks.FirstOrDefault(f => f.Id == idTaskUpdate);
            taskDelete.Status = 0;
            DbUtils.db.SaveChanges();
            this.Close();
        }

        private void ModificBut_Click(object obj, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(descrBox.Text))
            {
                MessageBox.Show("Не заполнены обязательные данные. " +
                    "К обязательным относятся следующие поля:\n" +
                    "· Описание задачи.",
                    "Предупрждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Tasks taskUpdate = DbUtils.db.Tasks.FirstOrDefault(f => f.Id == idTaskUpdate);
            taskUpdate.Description = descrBox.Text;
            if (CountDay.IsChecked == true)
            {
                taskUpdate.Day = Convert.ToInt32(dayCountBox.Text);
            }
            else if (DateDay.IsChecked == true)
            {
                taskUpdate.Day = Convert.ToInt32((dateBox.SelectedDate.Value.Date - DateTime.Now.Date).Days);
            }
            taskUpdate.Status = 1;
            taskUpdate.Type = Convert.ToInt32(DbUtils.db.Types.Where(w => w.Status == 1).FirstOrDefault(f => f.Type == typeBox.Text).Id);
            taskUpdate.Advanced = Convert.ToInt32(advancedBox.Text);
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

        private void PutBut_Click(object sender, RoutedEventArgs e)
        {
            Tasks tasks = DbUtils.db.Tasks.FirstOrDefault(f => f.Id == idTaskUpdate);
            tasks.Status = 2;
            DbUtils.db.SaveChanges();
            this.Close();
        }
    }
}
