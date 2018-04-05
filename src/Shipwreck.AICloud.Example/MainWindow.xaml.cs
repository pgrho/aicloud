using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Shipwreck.AICloud.Example
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var speakers = typeof(KnownSpeakers).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Select(f => f.GetValue(null)).OfType<string>().ToList();

            speakerNameComboBox.ItemsSource = speakers;
            speakerNameComboBox.SelectedItem = speakers.FirstOrDefault();
        }
        private MediaPlayer _MediaPlayer;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _MediaPlayer = null;

            try
            {
                using (var c = new AICloudClient())
                {
                    c.UserName = userNameTextBox.Text;
                    c.Password = passwordBox.Password;

                    var r = await c.SynthesisSpeechAsync(new SynthesisSpeechParameter()
                    {
                        SpeakerName = speakerNameComboBox.SelectedItem?.ToString(),
                        Extension = Extension.Mp3,
                        Text = textTextBox.Text
                    });

                    var tmp = Path.GetTempFileName();
                    File.Delete(tmp);
                    tmp = Path.ChangeExtension(tmp, ".mp3");

                    await r.SaveAsync(tmp);

                    _MediaPlayer = new MediaPlayer();
                    _MediaPlayer.MediaEnded += _MediaPlayer_MediaEnded;
                    _MediaPlayer.MediaFailed += _MediaPlayer_MediaEnded;
                    _MediaPlayer.Open(new Uri(tmp));
                    _MediaPlayer.Play();
                }
            }
            catch (AICloudException ex) when (ex.RawMessage != null)
            {
                MessageBox.Show(this, $"{ex.Code:D}: {ex.RawMessage}\r\n{ex.RawDetail}");
                _MediaPlayer = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
                _MediaPlayer = null;
            }
        }

        private void _MediaPlayer_MediaEnded(object sender, EventArgs e)
            => _MediaPlayer = null;
    }
}