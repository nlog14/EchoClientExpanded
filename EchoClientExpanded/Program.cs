﻿using System;
using System.IO;
using System.Net.Sockets;

namespace EchoClientExpanded
{
    class Program
    {
        static void Main(string[] args)
        {
            //Writing to the console to be able to differentiate what is running
            Console.WriteLine("Client");

            //added a bool to keep track of when to end the loop
            bool keepSending = true;

            //Moved the socket, stream, reader and writer before the readline
            //so it can reuse the same objects until the connection is closed

            //Creates a socket towards the server, here it calls the 127.0.0.1 address which is always the local computer
            //the 7 is the port number the socket connects to
            //This line establishes the connection, meaning we have an open connection when this line has executed
            //If no server is responding an exception is thrown
            TcpClient socket = new TcpClient("127.0.0.1", 7);

            //Gets the stream object from the socket. The stream object is able to recieve and send data
            NetworkStream ns = socket.GetStream();
            //The StreamReader is an easier way to read data from a Stream, it uses the NetworkStream
            StreamReader reader = new StreamReader(ns);
            //The StreamWriter is an easier way to write data to a Stream, it uses the NetworkStream
            StreamWriter writer = new StreamWriter(ns);

            while (keepSending)
            {
                //Read message from the console up until the user presses enter (a line break)
                //This function is blocking, meaning it will not execute any more code until the line break occurs
                string message = Console.ReadLine();

                //writes the message the user typed in the console to the server and appends a line break (cr lf)
                //notice the Line part of WriteLine
                writer.WriteLine(message);
                //Makes sure that the message isn't buffered somewhere, and actually send to the client
                //Always remember to flush after you
                writer.Flush();

                //Here it reads all data send until a line break (cr lf) is received
                //notice the Line part of the ReadLine
                string response = reader.ReadLine();

                //Writes the response it got from the server to the console
                //it should be the same as the line send, it the server is a simple Echo Server
                Console.WriteLine(response);

                //if the user types close (case in-sensitive) the clients sends the message to the server
                //and the sets the bool to false, so the loop stops
                if (message.ToLower() == "close")
                {
                    keepSending = false;
                }
            }

            //closes the connection, as it can only send one message before the server closes the connection
            socket.Close();
        }
    }
}
