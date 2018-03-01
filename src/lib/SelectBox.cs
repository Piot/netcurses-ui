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
	public class SelectBox
	{
		public delegate void OnSelectDelegate(string selection);

		public OnSelectDelegate OnSelect;

		public delegate void OnFocusDelegate(string selection);

		public OnFocusDelegate OnFocus;

		string[] selections;
		uint selectionIndex;
		ConsoleArea root;
		uint upperIndex;

		public SelectBox(string[] selections)
		{
			this.selections = selections;
			var recommendedHeight = selections.Length + 2;
			root = new ConsoleArea(new Size(20, Math.Min(10, recommendedHeight)));
			root.Background = ConsoleColor.DarkBlue;
			root.Foreground = ConsoleColor.White;
			ConsoleBorder.Draw(root, root.Size.Width / 2, root.Size.Height / 2, root.Size.Width, root.Size.Height);
			Redraw();
		}

		public ConsoleArea Area
		{
			get
			{
				return root;
			}
		}

		public void FeedInput(ConsoleKeyInfo info)
		{
			switch (info.Key)
			{
			case ConsoleKey.UpArrow:

				if (selectionIndex > 0)
				{
					selectionIndex--;
					OnFocus ?.Invoke(selections[selectionIndex]);
					Redraw();
				}
				break;
			case ConsoleKey.DownArrow :

				if (selectionIndex < selections.Length - 1)
				{
					selectionIndex++;
					OnFocus ?.Invoke(selections[selectionIndex]);
					Redraw();
				}
				break;
			case ConsoleKey.Enter :
				OnSelect ?.Invoke(selections[selectionIndex]);
				break;
			}
		}

		void Redraw()
		{
			var visibleLineCount = root.Size.Height - 2;

			if (selectionIndex >= upperIndex + visibleLineCount - 1)
			{
				var newUpper = (int)selectionIndex - visibleLineCount + 1;

				if (newUpper < 0)
				{
					newUpper = 0;
				}
				upperIndex = (uint)newUpper;
			}
			else if (selectionIndex < upperIndex)
			{
				upperIndex = selectionIndex;
			}
			for (var i = 0; i < visibleLineCount; ++i)
			{
				root.Move(new Position(1, i + 1));
				var selectionIndexToDraw = upperIndex + i;

				if (selectionIndexToDraw >= 0 && selectionIndexToDraw < selections.Length)
				{
					var isSelected = (selectionIndexToDraw == selectionIndex);

					if (isSelected)
					{
						root.Background = ConsoleColor.Cyan;
					}
					else
					{
						root.Background = ConsoleColor.DarkBlue;
					}
					ConsoleText.Draw(root, selections[selectionIndexToDraw], 0, root.Size.Width - 2, ConsoleText.TextAlign.Center);
				}
				else
				{
					root.Background = ConsoleColor.DarkBlue;
					root.ClearCharacters(root.Size.Width - 2);
				}
			}
		}

		public void Update()
		{
		}
	}
}
