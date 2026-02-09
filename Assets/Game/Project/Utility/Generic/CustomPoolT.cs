using System;
using System.Collections.Generic;

namespace Game.Project.Utility.Generic
{
    /// <summary>
    /// 커스텀 제네릭 풀
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomPoolT<T> where T : class
    {
        private readonly Stack<T> _stack = new Stack<T>();
        private readonly Func<T> _createFunc;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;

        public CustomPoolT(Func<T> createFunc, Action<T> onGet = null, Action<T> onRelease = null, int initialCount = 30)
        {
            _createFunc = createFunc;
            _onGet = onGet;
            _onRelease = onRelease;

            for (int i = 0; i < initialCount; i++)
            {
                _stack.Push(_createFunc());
            }
        }
        public T Get()
        {
            T item = _stack.Count > 0 ? _stack.Pop() : _createFunc();
            _onGet?.Invoke(item);
            return item;
        }
        public void Release(T item)
        {
            _onRelease?.Invoke(item);
            _stack.Push(item);
        }
    }
}
