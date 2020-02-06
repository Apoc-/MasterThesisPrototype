using System.Collections.Generic;

namespace UI
{
    public class Dialogue
    {
        private List<string> _lines;
        private int _currentLine = 0;

        public Dialogue(List<string> lines)
        {
            _lines = lines;
        }

        public string GetNextLine()
        {
            if (_currentLine >= _lines.Count) return null;
            
            var line = _lines[_currentLine];
            _currentLine += 1;
            return line;
        }
    }
}