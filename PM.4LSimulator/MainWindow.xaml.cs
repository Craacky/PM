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

namespace PM.Tauras4L.Emulator
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
        private SocketServer slaveCamera;

        private bool isBoxRun = true;
        private bool isMasterRun = true;

        List<string> boxCodes = new List<string>(); 
        List<string> productCodes = new List<string>();

        private int box = 0;
        public MainWindow()
        {
            InitializeComponent();

            boxCamera = new SocketServer(23);
            slaveCamera = new SocketServer(24);
            masterCamera = new SocketServer(25);

            boxCamera.Start();
            slaveCamera.Start();
            masterCamera.Start();

           
            BoxCameraProccess();

        }


        void BoxCameraProccess()
        {
           Random random = new Random();
            Task.Run(() => {
                
                Thread.Sleep(5000);

                while (boxNumber != 640)
                {
                    Thread.Sleep(700);

                    string code = string.Empty;
                    int i = random.Next(1, 101);
                    if (i <= 98)
                    {
                        if (random.Next(1, 101) <= 98)
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
                           tb.Items.Add("\n" + message + "")));
                   
                    if (!code.Contains("fail"))
                    {

                        SlaveCameraProccess();
                    }
                    else
                    {
                        Thread.Sleep(700);

                    }
                }
            });
        }

        void SlaveCameraProccess()
        {
            Random random = new Random();
            Thread.Sleep(300);

            string message = "<start>";

            int j = random.Next(1, 101);
            if (j <= 98)
            {
                for (int i = 0; i < 6; i++)
                {
                    string code = String.Empty;
                    if (random.Next(1, 101) <= 98)
                    {
                        code = $"01{productGtin}21{GenerateString(8)}\u001d93{GenerateString(4)}";
                        productCodes.Add(code);
                    }
                    else
                    {
                        if (productCodes.Count > 0)
                        {
                            code = productCodes[random.Next(0, productCodes.Count)];
                        }else
                        {
                            code = $"01{productGtin}21{GenerateString(8)}\u001d93{GenerateString(4)}";
                            productCodes.Add(code);
                        }
                    }


                    if (i == 5)
                    {
                        message += code + "<stop>";
                    }
                    else
                    {
                        message += code + "<next>";
                    }
                }

                slaveCamera.SendMessage(message);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                   tb.Items.Add(message + "")
                  ));

                MasterCameraProccess();
            }
            else
            {
                message = "<start>fail<stop>";

                slaveCamera.SendMessage(message);
                boxNumber--;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                   tb.Items.Add(message + "")
                  ));
                Thread.Sleep(700);
            }

        }

        void MasterCameraProccess()
        {
            Random random = new Random();

            Thread.Sleep(300);
            string message = "<start>";
                bool t = false;
                int j = random.Next(1, 101);
                if (j <= 98)
                {
                    
                    for (int i = 0; i < 6; i++)
                {
                   
                        string code = String.Empty;
                    if (random.Next(1, 101) <= 99)
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


                    if (i == 5)
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
                   
                }
            if (!message.Contains("fail"))
            {

            }
            else
            {
                boxNumber--;
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
                               tb.Items.Add(message + "\n" + boxNumber)
                              ));
            masterCamera.SendMessage(message);
           
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
