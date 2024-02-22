using OnlinePaintServer.Models;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PaintOnlineClient.Models
{
    public class ClientObject
    {
        int port = 8888;
        string host = "localhost";
        protected internal string Id { get; } = Guid.NewGuid().ToString();
        protected StreamWriter Writer { get; set; }
        protected StreamReader Reader { get; set; }
        protected Stream Stream { get; set; }

        TcpClient client;

        public delegate void BoardsHandler(List<BoardData> boards);
        public event BoardsHandler OnGetBoards;

        public delegate void BoardLineDataHandler(LineData newLine);
        public event BoardLineDataHandler OnAppendLine;

        public delegate void BoardDataHandler(BoardData boardData);
        public event BoardDataHandler OnGetBoardData;

        public delegate void BoardClearHandler(string boardId);
        public event BoardClearHandler OnClearBoard;

        public bool isConnected
        {
            get { return client.Connected; }
            private set { }
        }

        public ClientObject()
        { }

        public async Task ConnnectToServer()
        {
            try
            {
                client= new TcpClient();
                await client.ConnectAsync(host, port);

                Stream = client.GetStream();
                Reader = new StreamReader(Stream);
                Writer = new StreamWriter(Stream);

                Task.Run(ListenForSomething);
                await Console.Out.WriteLineAsync("connected");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        public async Task ListenForSomething()
        {
            while (true)
            {
                try
                {
                    string message = await Reader.ReadLineAsync();
                    if (message != null)
                    {
                        var command = message.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                        switch (command[0])
                        {
                            case "boards":
                                List<BoardData> boards = BoardData.GetPaintBoardsFromCommand(command);
                                OnGetBoards(boards);
                                break;
                            case "add":
                                LineData line = LineData.GetFromString(command[1]);
                                await Console.Out.WriteLineAsync("got update");
                                OnAppendLine(line);
                                break;
                            case "clear":
                                await Console.Out.WriteLineAsync("got update");
                                OnClearBoard(command[1]);
                                break;
                            default:
                                if (command.Length > 1)
                                {
                                    BoardData boardData = BoardData.GetFromString(command);
                                    OnGetBoardData(boardData);
                                }
                                break;
                        }

                    }
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }
            }
        }

        public async Task SendMessageAsync(string message)
        {
            await Writer.WriteLineAsync(message);
            await Writer.FlushAsync();
        }

        public async Task AskForBoardDataAsync(string boardId)
        {
            await SendMessageAsync($"*get*{boardId}");
        }

        public async Task TellLineAdded(string id,LineData line)
        {
            await SendMessageAsync($"*append*{id}|" + line.ToString());
        }

        public async Task TellToClearBoard(string id)
        {
            await SendMessageAsync($"*clear*{id}");
        }

        public async Task<string> CreateNewBoardAsync(string boardName)
        {
            string boardId = Guid.NewGuid().ToString();
            await SendMessageAsync($"*create*{boardId}+{boardName}");
            return boardId;
        }

        // закрытие подключения
        protected internal void Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }
}
