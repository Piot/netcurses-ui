/*

MIT License

Copyright (c) 2017 Peter Bjorklund

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/
﻿ using System;
using Netcurses;

namespace Piot.NetCursesUi
{
	public class WindowStack
	{
		ArrayStack<IUiWindow> windows = new ArrayStack<IUiWindow>();
		ConsoleArea area = new ConsoleArea(new Size(64, 16));

		public WindowStack()
		{
			area.Clear();
		}

		public void PushWindow(IUiWindow window)
		{
			windows.Push(window);
		}

		public void PopWindow(IUiWindow window)
		{
			windows.Remove(window);
		}

		public void FeedInput(ConsoleKeyInfo key)
		{
			if (key.Key == ConsoleKey.Escape)
			{
				if (!windows.IsEmpty)
				{
					PopWindow(windows.Top);
				}
				return;
			}

			if (windows.IsEmpty)
			{
				return;
			}

			var top = windows.Top;
			top.FeedInput(key);
		}

		public bool IsEmpty
		{
			get
			{
				return windows.IsEmpty;
			}
		}

		public void Update()
		{
			var windowList = windows.Collection;

			foreach (var window in windowList)
			{
				window.Update();
			}
			foreach (var window in windowList)
			{
				area.Copy(window.Area, new Position(0, 0));
			}
		}

		public ConsoleArea Area
		{
			get
			{
				return area;
			}
		}
	}
}
