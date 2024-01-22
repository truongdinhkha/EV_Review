using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Guna.Charts.WinForms;
using System.Web.UI.WebControls;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace BookShopManagement.Forms
{
    public partial class Chat_user : Form
    {
        private const int Port_Number = 1234;
        private const string Server_IP = "127.0.0.1";
        Socket client;
        NetworkStream stream;
        public Chat_user()
        {
            InitializeComponent();
        }

        private void Chat_user_Load(object sender, EventArgs e)
        {
           
        }
        private void ReadMessages(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            stream = client.GetStream();

            // Buffer for reading data
            byte[] bytes = new byte[1024];
            int bytesRead;

            while (true)
            {
                try
                {
                    // Read incoming message
                    bytesRead = stream.Read(bytes, 0, bytes.Length);
                    if (bytesRead == 0)
                    {
                        // Server disconnected
                        break;
                    }

                    // Convert the bytes received to a string and display it.
                    else
                    {
                 string message = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                    listView1.Items.Add(new ListViewItem("User:") + message);
                    }
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e);
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string TinNhan = guna2TextBox1.Text; 
           
            string Name = guna2TextBox4.Text;

            string request = $"Chat_user/{Name}/{TinNhan}";
            byte[] requestBytes = Encoding.ASCII.GetBytes(request);

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(Server_IP, Port_Number);
                    NetworkStream stream = client.GetStream();

                    // Gửi yêu cầu đăng ký
                    stream.Write(requestBytes, 0, requestBytes.Length);

                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.ASCII.GetString(responseBuffer, 0, bytesRead);
                    listView1.Items.Add(new ListViewItem("ME:  ") + TinNhan);
                    guna2TextBox1.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối đến server: " + ex.Message);
                }
                Thread listen = new Thread(Receive);
            }
        }
        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024];
                    client?.Receive(data);
                    string mess = Encoding.UTF8.GetString(data);
                    AddMess(mess);

                }
            }
            catch
            {
                Close();
            }
        }
        void AddMess(string s)
        {
            
            listView1.Items.Add(new ListViewItem() { Text = s });
           
        }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
         
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Thoát?");
            try
            {
                client?.Send(Encoding.UTF8.GetBytes(guna2TextBox4.Text + " đã rời phòng "));
                client?.Close();
            }
            catch { }
        }
    }
}

