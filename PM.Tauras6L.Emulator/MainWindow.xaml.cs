using PM.Protocols.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace PM.Tauras6L.Emulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string productGtin = "04810319010739";
        private string boxGtin = "64810319010731";
        private string lotNumber = "0101";

        private int boxNumber = 0;

        private SocketServer boxCamera;
        private SocketServer masterCamera;

        private bool isBoxRun = true;
        private bool isMasterRun = true;

        List<string> boxCodes = new List<string>();
        List<string> productCodes = new List<string>();

        private int box = 0;
        public MainWindow()
        {
            InitializeComponent();

            boxCamera = new SocketServer(23);
            masterCamera = new SocketServer(24);
            boxCamera.MessageReceived += BoxCamera_MessageReceived;
            masterCamera.MessageReceived += MasterCamera_MessageReceived;
            boxCamera.Start();
            masterCamera.Start();


            BoxCameraProccess();

        }

        private void MasterCamera_MessageReceived(SocketServer server, DateTime datetime, string message, SocketClient client)
        {
            //if (message.Contains("6L"))
            //{
            //    isMasterRun = true;
            //}
            //else
            //{
            //    isMasterRun = false;
            //}
        }

        private void BoxCamera_MessageReceived(SocketServer server, DateTime datetime, string message, SocketClient client)
        {
            //if(message.Contains("6L"))
            //{
            //    isBoxRun = true;
            //}
            //else
            //{
            //    isBoxRun = false;
            //}
        }


        void BoxCameraProccess()
        {
            Random random = new Random();
            Task.Run(() => {

                Thread.Sleep(5000);
                while (boxNumber != 640)
                {
                    if (isBoxRun)
                    {
                        string code = string.Empty;
                        int i = random.Next(1, 101);
                        if (i <= 97)
                        {
                            if (random.Next(1, 101) <= 97)
                            {
                                boxNumber++;
                                code = $"01{boxGtin}11{DateTime.Now.ToString("yyMMdd")}17{DateTime.Now.ToString("yyMMdd")}10{lotNumber}\u001d21{boxNumber}";
                                boxCodes.Add(code);
                            }
                            else
                            {
                                if (boxCodes.Count > 0)
                                    code = boxCodes[random.Next(0, boxCodes.Count)];
                            }
                        }
                        else
                        {
                            code = "fail";
                        }
                        string message = $"<start>{code}<stop>";
                        boxCamera.SendMessage(message);

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                               tb.Items.Add(message + "")
                              ));
                        if (!code.Contains("fail"))
                        {

                            MasterCameraProccess();
                        }
                        else
                        {

                            Thread.Sleep(700);
                        }
                    }
                }
            });
        }

        void MasterCameraProccess()
        {
            Random random = new Random();

            Thread.Sleep(200);

            string message = "<start>";
            bool t = false;
            int j = random.Next(1, 101);
            if (j <= 97)
            {
                box++;
                for (int i = 0; i < 12; i++)
                {

                    string code = String.Empty;
                    if (random.Next(1, 101) <= 97)
                    {
                        code = $"01{productGtin}21{GenerateString(8)}\u001d93{GenerateString(4)}";
                        productCodes.Add(code);
                    }
                    else
                    {
                        if (productCodes.Count > 0)
                        {
                            code = productCodes[random.Next(0, productCodes.Count)];
                            t = true;
                        }
                    }


                    if (i == 11)
                    {
                        message += code + "<stop>";
                    }
                    else
                    {
                        message += code + "<next>";
                    }
                }
            }
            else
            {
                message = "<start>fail<stop>";
                box--;

            }

            Application.Current.Dispatcher.Invoke(new Action(() =>
                               tb.Items.Add(message + "\n" + box +"\n")
                              ));
            masterCamera.SendMessage(message);


            Thread.Sleep(1000);

        }

        private string GenerateString(int countSymbols)
        {
            string generatedString = "";
            Random random = new Random();
            for (int i = 0; i < countSymbols; i++)
            {
                generatedString += (char)random.Next(33, 127);
            }
            return generatedString;
        }
    }
}
