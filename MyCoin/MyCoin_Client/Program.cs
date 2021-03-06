using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Dextrey.Crypto;
using System.Security.Cryptography;

namespace MyCoin_Client
{
	class MainClass
	{
		private static int _port = 2565;
		private static IPAddress _ip = IPAddress.Parse("127.0.0.1");	
		private static TcpClient client = new TcpClient();
		private static NetworkStream stream = null;

		public static void Main (string[] args)
		{
			PolymorphicCryptWrapper pm = new PolymorphicCryptWrapper(new TripleDESCryptoServiceProvider());
			try{
				client.Connect(_ip,_port);
				stream = client.GetStream();
			}catch(Exception e){
				_Log(e);
			}
		Begin:

			string message = Console.ReadLine();
			Send (message);
			switch(message){
			case "gen":
				string _coin = Recive();
				_Log ("Coin! {0}",_coin);
				break;
			case "smt":
				string _re = Recive();
				if(_re == "valid"){
					_Log ("You have found a coin!");
				}else{
					_Log("Coin not valid");
				}
				break;
			case "diff":
				string diffaculty = Recive();
				_Log(diffaculty);
				break;
			case "disc":
				client.Close();
				stream.Dispose();
				stream.Close();
				Process.GetCurrentProcess().Kill();
				break;
			}

			goto Begin;
			//_Log(Recive());
		}
		private static string Recive()
		{
			Byte[] _bytes = new Byte[1024];
			try{			
				String responseData = String.Empty;
				Int32 bytes = stream.Read(_bytes, 0, _bytes.Length);
				responseData = Encoding.ASCII.GetString(_bytes, 0, bytes);
				return responseData;     
			}catch(SocketException e){
				_Log (e);
			}
			return "Error!";
		}
		private static void Send(string message)
		{
			Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);     
			try{
				stream.Write(data,0,data.Length);
				_Log ("Sent: {0}",message);
			}catch(SocketException e){
				_Log (e);
			}
		}

		//OVERLOAD: used for displaying socket exceptions to the console
		private static void _Log(Exception e)
		{
			string myDate;
			myDate = DateTime.Now.ToString("MM/dd/yyyy");
			Console.WriteLine("{0} {1} | {2} | {3}" + Environment.NewLine,"[Error]",DateTime.Now.ToShortTimeString(),myDate,e);
		}
		//Used to display text to the console
		private static void _Log(string message)
		{
			string myDate;
			myDate = DateTime.Now.ToString("MM/dd/yyyy");
			Console.WriteLine("{0} {1} | {2} | {3}" + Environment.NewLine,"[Client]",DateTime.Now.ToShortTimeString(),myDate,message);
		}
		//For displaying to the console like the console would but with time and date
		private static void _Log(string message , object arg1)
		{
			string myDate;
			myDate = DateTime.Now.ToString("MM/dd/yyyy");
			string _message = message.Replace("{0}",arg1.ToString());
			Console.WriteLine("{0} {1} | {2} | {3}" + Environment.NewLine,"[Client]",DateTime.Now.ToShortTimeString(),myDate,_message);
		}
	}
}
