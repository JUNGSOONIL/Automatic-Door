using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;

namespace client

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;  
            serialPort1.BaudRate = 9600;
            serialPort1.DataBits = 8;
            serialPort1.Parity = Parity.None;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Open();

        }

        private Socket m_ClientSocket; 

        protected override void Dispose(bool disposing)
        {
            if (m_ClientSocket.Connected)
                m_ClientSocket.Disconnect(false);
            m_ClientSocket.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            byte[] szData = Encoding.Unicode.GetBytes("자동문이 시작됩니다.");
            args.SetBuffer(szData, 0, szData.Length);
            m_ClientSocket.SendAsync(args);
            serialPort1.Write("3");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            byte[] szData = Encoding.Unicode.GetBytes("자동문이 종료됩니다.");
            args.SetBuffer(szData, 0, szData.Length);
            m_ClientSocket.SendAsync(args);
            serialPort1.Write("4");
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000); //포트 대기 설정

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = ipep;

            m_ClientSocket.ConnectAsync(args);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1 != null && serialPort1.IsOpen) { serialPort1.Close(); }
        }
    }
}
