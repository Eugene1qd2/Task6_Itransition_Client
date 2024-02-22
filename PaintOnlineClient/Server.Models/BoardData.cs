using PaintOnlineClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaintServer.Models
{
    public class BoardData
    {
        public string boardId { get; set; }
        public string boardName { get; set; }
        public List<LineData> lines { get; set; }
        public BoardData(string boardId, string boardName)
        {
            lines = new List<LineData>();
            this.boardId = boardId;
            this.boardName = boardName;

        }
        public void AppendLines(List<LineData> newLines)
        {
            lines.AddRange(newLines);
        }

        public void AppendLine(LineData newLine)
        {
            lines.Add(newLine);
        }
        public override string ToString()
        {
            return boardId + "+" + boardName;
        }
        public static List<BoardData> GetPaintBoardsFromCommand(string[] command)
        {
            var result = new List<BoardData>();
            if (command.Length > 1)
            {
                var data = command[1].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < data.Length; i++)
                {
                    var boardData = data[i].Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    result.Add(new BoardData(boardData[0], boardData[1]));
                }
            }
            return result;
        }

        public static BoardData GetFromString(string[] command)
        {
            BoardData board = new BoardData(command[0],"some name");
            var linesData = command[1].Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0;i < linesData.Length;i++)
            {
                board.AppendLine(LineData.GetFromString(linesData[i]));
            }
            return board;
        }
    }
}
