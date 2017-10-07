using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COM_to_COM
{
    public partial class Form1 : Form
    {
        private SerialPortController serialPort;

        public Form1()
        {
            InitializeComponent();
            serialPort = new SerialPortController(ShowData);
        }

        private void canConnect(object sender, EventArgs e)
        {
            button1.Enabled = (textBox1.Text.Length != 0) && (comboBox1.SelectedIndex > -1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (button1.Text)
            {
                case "Connect": Connect(); break;
                case "Disconnect": Disconnect(); break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort.SendData(Encoding.Default.GetBytes(richTextBox2.Text));
            ShowData(richTextBox2.Text, true);
            richTextBox2.Text = "";
        }

        private void ShowData(string data, bool sender)
        {
            richTextBox1.Text += ((sender ? "↑" : "↓") + " [" + DateTime.Now.ToString("HH:mm:ss tt") + "] " + data + "\n");
        }

        private void Connect()
        {
            serialPort.Connect(textBox1.Text, Int32.Parse(comboBox1.SelectedItem.ToString()));
            button1.Text = "Disconnect";
            textBox1.Enabled = false;
            comboBox1.Enabled = false;
            button2.Enabled = true;
        }

        private void Disconnect()
        {
            serialPort.Disconnect();
            button1.Text = "Connect";
            textBox1.Enabled = true;
            comboBox1.Enabled = true;
            button2.Enabled = false;
        }
    }
}
