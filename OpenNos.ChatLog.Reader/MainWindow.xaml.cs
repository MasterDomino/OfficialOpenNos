using OpenNos.ChatLog.Shared;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace OpenNos.ChatLog.Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ChatLogEntry> _logs;
        private LogFileReader _reader;
        public MainWindow()
        {
            InitializeComponent();
            _logs = new List<ChatLogEntry>();
            _reader = new LogFileReader();
            recursiveFileOpen("chatlogs");
            _logs = _logs.OrderBy(s => s.Timestamp).ToList();
            resultlistbox.ItemsSource = _logs;
            typedropdown.Items.Add("All");
            typedropdown.Items.Add(ChatLogType.Map);
            typedropdown.Items.Add(ChatLogType.Speaker);
            typedropdown.Items.Add(ChatLogType.Whisper);
            typedropdown.Items.Add(ChatLogType.Group);
            typedropdown.Items.Add(ChatLogType.Family);
            typedropdown.Items.Add(ChatLogType.BuddyTalk);
            typedropdown.SelectedIndex = 0;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {

        }

        private void CloseFile(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton(object sender, RoutedEventArgs e)
        {
            List<ChatLogEntry> tmp = _logs;
            if (!string.IsNullOrWhiteSpace(senderbox.Text))
            {
                tmp = tmp.Where(s => s.Sender.ToLower().Contains(senderbox.Text.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(senderidbox.Text) && long.TryParse(senderidbox.Text, out long senderid))
            {
                tmp = tmp.Where(s => s.SenderId == senderid).ToList();
            }
            if (!string.IsNullOrWhiteSpace(receiverbox.Text))
            {
                tmp = tmp.Where(s => s.Receiver.ToLower().Contains(receiverbox.Text.ToLower())).ToList();
            }
            if (!string.IsNullOrWhiteSpace(receiveridbox.Text) && long.TryParse(receiveridbox.Text, out long receiverid))
            {
                tmp = tmp.Where(s => s.ReceiverId == receiverid).ToList();
            }
            if (!string.IsNullOrWhiteSpace(messagebox.Text))
            {
                tmp = tmp.Where(s => s.Message.ToLower().Contains(messagebox.Text.ToLower())).ToList();
            }
            if (datestartpicker.Value != null)
            {
                tmp = tmp.Where(s => s.Timestamp >= datestartpicker.Value).ToList();
            }
            if (dateendpicker.Value != null)
            {
                tmp = tmp.Where(s => s.Timestamp <= dateendpicker.Value).ToList();
            }
            if (typedropdown.SelectedIndex != 0)
            {
                tmp = tmp.Where(s => s.MessageType == (ChatLogType)typedropdown.SelectedValue).ToList();
            }
            resultlistbox.ItemsSource = tmp;
        }

        private void recursiveFileOpen(string dir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    foreach (string s in Directory.GetFiles(d).Where(s => s.EndsWith(".onc")))
                    {
                        _logs.AddRange(_reader.ReadLogFile(s));
                    }
                    recursiveFileOpen(d);
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong while opening Chat Log Files. Exiting...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
        }
    }
}
