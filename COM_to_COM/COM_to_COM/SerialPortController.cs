using System.IO.Ports;
using System.Threading;

namespace COM_to_COM
{
    class SerialPortController
    {
        public delegate void ShowDataHandler(string data, bool sender);

        private SerialPort serialPort;
        private ShowDataHandler handler;

        public SerialPortController(ShowDataHandler _handler)
        {
            handler = _handler;
        }

        public void Connect(string portName, int baudRate)
        {
            serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            serialPort.Open();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
        }

        public void Disconnect()
        {
            serialPort.Close();
        }

        public void SendData(byte[] data)
        {
            serialPort.RtsEnable = true;
            serialPort.Write(data, 0, data.Length);
            Thread.Sleep(100);
            serialPort.RtsEnable = false;
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[serialPort.BytesToRead];
            serialPort.Read(data, 0, data.Length);
            handler.Invoke(System.Text.Encoding.Default.GetString(data), false);
        }
    }
}
