using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using System.Data.SQLite;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                Console.WriteLine("connecting...");

                tcpClient.Connect("127.0.0.1", 8888);//127.0.0.1 is you own address
                Console.WriteLine("connected");

                //init variables needed for communication with server
                string str;
                Stream stream = tcpClient.GetStream();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] ba;

                byte[] serverReply = new byte[100];
                //read from the server and put in serverReply with only size of 100
                int size = stream.Read(serverReply, 0, 100);

                //conver the serverReply (size tells the amount it actually read might be less then 100)
                for (int i = 0; i < size; ++i)
                {
                    Console.Write(Convert.ToChar(serverReply[i]));
                }

                Console.WriteLine();

                //get string
                str = Console.ReadLine();
                stream = tcpClient.GetStream();

                //must convert string to binary data
                encoding = new ASCIIEncoding();
                ba = encoding.GetBytes(str);
                Console.WriteLine("tranmiting databasename..");

                //write to stream that connected to
                stream.Write(ba, 0, ba.Length);
                bool cont = true;
                while (cont)
                {
                    //get string
                    str = Console.ReadLine();
                    stream = tcpClient.GetStream();

                    //must convert string to binary data
                    encoding = new ASCIIEncoding();
                    ba = encoding.GetBytes(str);

                    //write to stream that connected to
                    stream.Write(ba, 0, ba.Length);


                    serverReply = new byte[100];
                    size = stream.Read(serverReply, 0, 100);
                    string reply = encoding.GetString(serverReply, 0, size);
                    Console.WriteLine(reply);
                    if (reply == "commandError")//there was an error
                    {
                        //do nothing
                        Console.WriteLine("command error");
                        continue;
                    }

                    if(reply == "quit")
                    {
                        Console.WriteLine("quiting recieved");
                        cont = false;
                        continue;
                    }


                    //not best approach
                    /*byte[] intReply = new byte[4];
                    stream.Read(intReply, 0, 4);
                    int tables = BitConverter.ToInt32(intReply, 0);
                    stream.Read(intReply, 0, 4);
                    int rows = BitConverter.ToInt32(intReply, 0);
                    stream.Read(intReply, 0, 4);
                    int columns = BitConverter.ToInt32(intReply, 0);

                    Console.WriteLine(tables + " " + rows + " " + columns);
                    for (int t = 0; t < tables; ++t)
                        for (int r = 0; r < rows; ++r)
                        {
                            for (int c = 0; c < columns; ++c)
                            {
                                serverReply = new byte[1];
                                size = stream.Read(serverReply, 0, 1);
                                string rep = encoding.GetString(serverReply, 0, size);
                                Console.WriteLine("\n" + rep);
                                if (rep == "i")
                                {
                                    serverReply = new byte[4];
                                    size = stream.Read(serverReply, 0, 4);
                                    rep = "" + BitConverter.ToInt32(serverReply, 0);
                                    Console.Write(rep + '\t');
                                }
                                else if (rep == "v")
                                {
                                    serverReply = new byte[100];
                                    size = stream.Read(serverReply, 0, 100);
                                    rep = encoding.GetString(serverReply, 0, size);
                                    Console.Write(rep + '\t');
                                }
                            }
                            Console.WriteLine();
                        }

                    Console.WriteLine("end");*/
                    //get recived data(doesnt work)
                    /*MemoryStream ms = new MemoryStream();
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    ms.Write(serverReply, 0, 100);
                    ms.Seek(0, SeekOrigin.Begin);
                    SQLiteDataReader reader = binFormatter.Deserialize(ms);
                    Console.WriteLine("2");
                    
                    for (int i = 0; i < reader.FieldCount; ++i)
                        Console.Write(reader.GetName(i) + "\t");
                    Console.WriteLine();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; ++i)//go through all the names in that row
                            Console.Write(reader[reader.GetName(i)] + "\t");
                        Console.WriteLine();
                    }*/
                }

                /*byte[] userDataLen = new byte[4];//for int 32, 32 bits == 4 bytes
                stream.Read(userDataLen, 0, 4);
                Int32 ReturnSize = BitConverter.ToInt32(userDataLen);
                serverReply = new byte[];
                ReturnSize stream.Read(serverReply, 0, ReturnSize);

                serverReply = new byte[100];
                //read from the server and put in serverReply with only size of 100
                size = stream.Read(serverReply, 0, 100);*/


                //end connection
                tcpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("error client");
            }
        }
    }
}