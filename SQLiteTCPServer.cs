using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//tcp server
using System.Net;
using System.Net.Sockets;
//using System.IO;

//sqlite
using System.Data.SQLite;

//dataset
using System.Data;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    IPAddress iPAddress = IPAddress.Parse("127.0.0.1");

                    TcpListener tcpListen = new TcpListener(iPAddress, 8888);

                    tcpListen.Start();
                    Console.WriteLine("ther server at port 8888");
                    Console.WriteLine("local end point: " + tcpListen.LocalEndpoint);
                    Console.WriteLine("watnig for connection...");

                    Socket client = tcpListen.AcceptSocket();
                    Console.WriteLine("connection from: " + client.RemoteEndPoint);

                    //recieving data from client
                    byte[] clientData = new byte[100];
                    int size;
                    /*Console.WriteLine("recived...");
                    for (int i = 0; i < size; ++i)
                    {
                        Console.Write(Convert.ToChar(clientData[i]));
                    }
                    Console.WriteLine("");*/
                    ASCIIEncoding encosing = new ASCIIEncoding();

                    sendMessage(client, "enter database name: ");

                    Console.WriteLine("sent acnoledgement");

                    ///////////////////////////////////////////CONNECT TO DATABASE ON THE SERVER
                    //get message from client
                    clientData = new byte[100];
                    size = client.Receive(clientData);

                    //convert to string
                    string msg = encosing.GetString(clientData, 0, size);//0 is the starting point of the bytes and size is the size it should go to atherwise it will have a seriese of blanks that will mess up the SQLiteConnection
                    SQLiteConnection m_dbConnection;

                    Console.WriteLine("connecting to database\"" + msg + "\"...");
                    string databaseConnect = ("Data Source=" + msg + ".sqlite;Version=3;FailIfMissing=True;");
                    try
                    {
                        m_dbConnection = new SQLiteConnection(databaseConnect);
                        m_dbConnection.Open();
                    }
                    catch (Exception e)
                    {
                        //set up sqlite database since its not there(maybe should ask if they want to create it)
                        SQLiteConnection.CreateFile(msg + ".sqlite");
                        m_dbConnection = new SQLiteConnection(databaseConnect);
                        m_dbConnection.Open();
                        Console.WriteLine("databaseConnect not created so im creating it");

                    }
                    //////////////////////////////////////////
                    bool cont = true;
                    while (cont)
                    {
                        clientData = new byte[100];
                        size = client.Receive(clientData);

                        //convert to string
                        string sqlMsg = encosing.GetString(clientData, 0, size);
                        Console.WriteLine(sqlMsg);
                        try
                        {
                            SQLiteCommand sqlMsgComment = new SQLiteCommand(sqlMsg, m_dbConnection);
                            SQLiteCommand MsgComment = new SQLiteCommand(sqlMsg, m_dbConnection);
                            SQLiteDataReader readerComment = sqlMsgComment.ExecuteReader();
                            client.Send(encosing.GetBytes("commandExecuted"));
                            /*SQLiteDataAdapter adapter = new SQLiteDataAdapter(MsgComment);
                            DataSet dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            uint tables = (uint)dataSet.Tables.Count;
                            uint rows = (uint)dataSet.Tables[0].Rows.Count;
                            uint columns = (uint)dataSet.Tables[0].Columns.Count;
                            client.Send(BitConverter.GetBytes(tables));//tables
                            client.Send(BitConverter.GetBytes(rows));//rows
                            client.Send(BitConverter.GetBytes(columns));//columns
                            foreach (DataTable table in dataSet.Tables)
                            {
                                foreach (DataRow row in table.Rows)
                                {
                                    foreach (DataColumn column in table.Columns)
                                    {
                                        if(Type.GetTypeCode(row[column].GetType()) == TypeCode.Int32)
                                            client.Send(encosing.GetBytes("i"));
                                        else if (Type.GetTypeCode(row[column].GetType()) == TypeCode.String)
                                            client.Send(encosing.GetBytes("v"));
                                        string s = "" + row[column];
                                        Console.Write(s + '\t');
                                        client.Send(encosing.GetBytes(s));
                                    }
                                }
                            }*/
                            //send table as string data
                            /*string row = "";
                            for (int i = 0; i < readerComment.FieldCount; ++i)
                            {
                                row += readerComment.GetName(i) + "\t";
                                Console.Write(readerComment.GetName(i) + "\t");
                            }
                            Console.WriteLine();
                            row += '\n';

                            while (readerComment.Read())
                            {
                                for (int i = 0; i < readerComment.FieldCount; ++i)//go through all the names in that row
                                {
                                    row += readerComment[readerComment.GetName(i)] + "\t";
                                }
                                row += '\n';
                                Console.WriteLine();
                            }
                            //string s = Convert.ToString(row);
                            client.Send(encosing.GetBytes(row.ToCharArray()));*/
                        }
                        catch (Exception e)
                        {
                            if (sqlMsg == "quit")
                            {
                                cont = false;
                                client.Send(encosing.GetBytes("quit"));
                            }
                            else
                                client.Send(encosing.GetBytes("commandError"));
                        }
                    }

                    //print all of the data in the highscore table(need to change this so i can ask what command the user wants to do and then return the sqlitedatareader so they can look at the data)
                    string sql = "SELECT * FROM highscores ORDER BY score desc";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);

                    m_dbConnection.Close();

                    //clean up
                    client.Close();
                    tcpListen.Stop();
                    Console.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nerror - " + e.StackTrace);
            }
        }

        static void sendMessage(Socket reciever, string msg)
        {
            //Console.WriteLine("sending message: " + msg);
            ASCIIEncoding encosing = new ASCIIEncoding();

            //send reply
            reciever.Send(encosing.GetBytes("Enter the database name you would like to connect to:"));
        }
    }
}