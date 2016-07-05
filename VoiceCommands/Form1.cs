using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace VoiceCommands
{
    public partial class Form1 : Form
    {

        #region Variable Declaration

        SpeechSynthesizer ss = new SpeechSynthesizer();
        PromptBuilder pb = new PromptBuilder();
        SpeechRecognitionEngine engine = new SpeechRecognitionEngine();

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32")]
        public static extern void
        LockWorkStation();

        private void engine_SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            switch (e.Result.Text.ToString())
            {
                case "hello":
                    ss.SpeakAsync("hello handsome");
                    break;

                case "what is the current time":
                    ss.SpeakAsync("current time is " + DateTime.Now.ToLongTimeString());
                    break;

                case "how are you":
                    ss.SpeakAsync("i'm feeling great today");
                    break;

                case "shut down":
                    Process.Start("shutdown", "/s /t 0");
                    break;

                case "thank you":
                    ss.SpeakAsync("pleasure is mine");
                    break;

                case "protect":
                    LockWorkStation();
                    break;
            }
        }

        private void listenBtn_Click(object sender, EventArgs e)
        {
            listenBtn.Enabled = false;
            Choices clist = new Choices();
            string[] keyWordList = { "hello", "how are you", "what is the current time", "shut down", "thank you", "protect", "open edge" };
            clist.Add(keyWordList);
            Grammar gr = new Grammar(new GrammarBuilder(clist));

            try
            {
                engine.RequestRecognizerUpdate();
                engine.LoadGrammar(gr);
                engine.SpeechRecognized += engine_SpeechRecognised;
                engine.SetInputToDefaultAudioDevice();
                engine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
