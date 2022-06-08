using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Utils
{
    public class SavedPointsEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly List<T> _elements;
        private readonly Stack<int> _savedPoints;

        private int _currentIndex;

        public SavedPointsEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
            _elements = new List<T>();
            _savedPoints = new Stack<int>();
            _currentIndex = 1;
        }

        public T Current => _currentIndex < _elements.Count ? _elements[_currentIndex] : _enumerator.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public void CreateSavePoint()
        {
            _savedPoints.Push(_elements.Count - 1);
        }

        public bool MoveNext()
        {
            if (_currentIndex + 1 < _elements.Count)
            {
                _currentIndex++;
                return true;
            }

            if (_enumerator.MoveNext())
            {
                _elements.Add(_enumerator.Current);
                _currentIndex++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _enumerator.Reset();
            _elements.Clear();
            _savedPoints.Clear();
            _currentIndex = 1;
        }

        public void RestoreLastSavedPoint()
        {
            if (_savedPoints.Count == 0) throw new InvalidOperationException();
            _currentIndex = _savedPoints.Pop();
        }

        public void RemoveLastSavedPoint()
        {
            if (_savedPoints.Count == 0) throw new InvalidOperationException();
            _savedPoints.Pop();
        }
    }
}
