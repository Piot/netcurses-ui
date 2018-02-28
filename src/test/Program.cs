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
﻿using System;
using System.Threading;
using Netcurses;
using Piot.NetCursesUi;

namespace NetCursesUiTest
{
	class Program
	{
		static void Main()
		{
			var rootWindow = new RootWindow();
			var selectBox = new SelectBox(new string[] { "First", "Second", "Last", "Another",
			                                             "2",
			                                             "3",
			                                             "4",
			                                             "5",
			                                             "6",
			                                             "7",
			                                             "8",
			                                             "9",
			                                             "10",
			                                             "11",
			                                             "12",});

			while (true)
			{
				rootWindow.Update();
				var info = rootWindow.ReadKey();

				if (info.Key == ConsoleKey.Escape)
				{
					return;
				}
				selectBox.FeedInput(info);
				selectBox.Update();
				selectBox.OnSelect += (selection) => {
					rootWindow.RootArea.Move(new Position(10,20));
					rootWindow.RootArea.ClearLine();
					rootWindow.RootArea.AddString($"You selected '{selection}'");
				};
				selectBox.OnFocus += (selection) => {
					rootWindow.RootArea.Move(new Position(10, 22));
					rootWindow.RootArea.ClearLine();
					rootWindow.RootArea.AddString($"You focused '{selection}'");
				};
				rootWindow.RootArea.Copy(selectBox.Area, new Position((rootWindow.RootArea.Size.Width - selectBox.Area.Size.Width) / 2, 0));
				Thread.Sleep(33);
			}
		}
	}
}
