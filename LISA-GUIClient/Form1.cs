namespace LISA_GUIClient
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Reflection.Metadata.Ecma335;
    using System.Runtime.CompilerServices;
    using System.Threading;
    public partial class Form1 : Form
    {
        private ChatClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new ChatClient(MessageHandler);
            //Console.WriteLine("Enter server IP:");
            string ip = "127.0.0.1"; //Console.ReadLine();
            client.ConnectToServer(ip, 8888);
        }
        private void MessageHandler(string msg)
        {
            richTextBox1.Text += msg + Environment.NewLine;
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            //send message
            if (textBox1.Text != string.Empty)
            {
                client.SendMessage(textBox1.Text);
                MessageHandler(textBox1.Text);
                textBox1.Text = string.Empty;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                client.SendMessage(textBox1.Text);
                MessageHandler(textBox1.Text);
                textBox1.Text = string.Empty;
            }
        }
    }
    class ChatClient
    {
        public ChatClient(Action<string> messagehandler)
        {
            ShowMessage = messagehandler;
        }
        private Action<string> ShowMessage;
        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;

        public void ConnectToServer(string host, int port)
        {
            this.tcpClient = new TcpClient(host, port);
            Console.WriteLine("Connected to server.");
            this.reader = new StreamReader(tcpClient.GetStream());
            this.writer = new StreamWriter(tcpClient.GetStream());
            writer.AutoFlush = true;
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    string message = reader.ReadLine();
                    ShowMessage(message);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Disconnected from server.");
            }
        }

        public void SendMessage(string message)
        {
            this.writer.WriteLine(message);
        }
    }

}