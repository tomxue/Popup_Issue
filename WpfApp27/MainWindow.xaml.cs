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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp27
{
    public class DataModel : INotifyPropertyChanged
    {
        public int Number { get; set; }
        public string PopInfo { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private bool popup_IsOpen;
        public bool Popup_IsOpen
        {
            get => popup_IsOpen;
            set
            {
                popup_IsOpen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Popup_IsOpen)));
            }
        }
    }

    public static class ButtonInfo
    {
        public static Button button;
        public static int index;
        public static bool isOverPopUp;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer PopUpTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            
            PopUpTimer.Interval = new TimeSpan(0, 0, 1);
            PopUpTimer.Tick += new EventHandler((senderObject, eventArguments) => DisplayPopUp(senderObject, eventArguments, null));

            ShowUI();
        }

        private void ShowUI()
        {
            for (int i = 0; i < 10; i++)
            {
                DataModel dm = new DataModel();
                dm.Number = i + 1;
                dm.PopInfo = dm.Number + ", this is a popup";
                dm.Popup_IsOpen = false;
                listBox.Items.Add(dm);
            }
        }

        private void ItemButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            int index = (int)(button.CommandParameter) - 1;
            if (index < 0)
                return;

            ButtonInfo.button = button;
            ButtonInfo.index = index;
            Debug.WriteLine("MainWindow: " + "ItemButton_MouseEnter" + $", index = {index}");

            // stop the timer if it is already set so that we can start new timer
            PopUpTimer.Stop();
            PopUpTimer.Start();
        }

        private void DisplayPopUp(object sender, EventArgs e, Button button)
        {
            int index = ButtonInfo.index;
            (listBox.Items[index] as DataModel).Popup_IsOpen = false;
            (listBox.Items[index] as DataModel).Popup_IsOpen = true;
            PopUpTimer.Stop();

            Debug.WriteLine("MainWindow: " + "DisplayPopUp" + $", index = {ButtonInfo.index}");
        }

        private void ItemButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            int index = (int)(button.CommandParameter) - 1;
            if (index < 0)
                return;

            if (PopUpTimer.IsEnabled)
                PopUpTimer.Stop();

            //(listBox.Items[index] as DataModel).Popup_IsOpen = false;
            Debug.WriteLine("MainWindow: " + "ItemButton_MouseLeave" + $", index = {index}");
        }
    }
}
