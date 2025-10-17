// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static System.Net.Mime.MediaTypeNames;

namespace Odva.Helpers;

/// <summary>
/// A line reader from a <see cref="TextReader"/> that can provide precise source position
/// </summary>
public struct LineReader
{
    private readonly string _text;

    /// <summary>
    /// Initializes a new instance of the <see cref="LineReader"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException">bufferSize cannot be &lt;= 0</exception>
    public LineReader(string text)
    {
        if (text is null)
            ThrowHelper.ArgumentNullException_text();

        _text = text;
        SourcePosition = 0;
    }

    /// <summary>
    /// Gets the char position of the line. Valid for the next line before calling <see cref="ReadLine"/>.
    /// </summary>
    public int SourcePosition { get; private set; }

    /// <summary>
    /// Reads a new line from the underlying <see cref="TextReader"/> and update the <see cref="SourcePosition"/> for the next line.
    /// </summary>
    /// <returns>A new line or null if the end of <see cref="TextReader"/> has been reached</returns>
    public StringSlice ReadLine()
    {
        string? text = _text;
        int end = text.Length;
        int sourcePosition = SourcePosition;
        int newSourcePosition = int.MaxValue;
        NewLine newLine = NewLine.None;

        if ((uint)sourcePosition >= (uint)end)
        {
            text = null;
        }
        else
        {
#if NETCOREAPP3_1_OR_GREATER
            ReadOnlySpan<char> span = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref Unsafe.AsRef(in text.GetPinnableReference()), sourcePosition), end - sourcePosition);
#else
            ReadOnlySpan<char> span = text.AsSpan(sourcePosition);
#endif

            int crlf = span.IndexOfAny('\r', '\n');
            if (crlf >= 0)
            {
                end = sourcePosition + crlf;
                newSourcePosition = end + 1;

#if NETCOREAPP3_1_OR_GREATER
                if (Unsafe.Add(ref Unsafe.AsRef(in text.GetPinnableReference()), end) == '\r')
#else
                if ((uint)end < (uint)text.Length && text[end] == '\r')
#endif
                {
                    if ((uint)newSourcePosition < (uint)text.Length && text[newSourcePosition] == '\n')
                    {
                        newLine = NewLine.CarriageReturnLineFeed;
                        newSourcePosition++;
                    }
                    else
                    {
                        newLine = NewLine.CarriageReturn;
                    }
                }
                else
                {
                    newLine = NewLine.LineFeed;
                }
            }
        }

        SourcePosition = newSourcePosition;
        return new StringSlice(text, sourcePosition, end - 1, newLine, dummy: false);
    }

    public readonly bool IsEnded => AtLastLine();

    private readonly bool AtLastLine()
    {
        string? text = _text;
        int sourcePosition = SourcePosition;
        ReadOnlySpan<char> span = text.AsSpan(sourcePosition);

        int crlf = span.IndexOfAny('\r', '\n');
        return crlf < 0;
    }

    public StringSlice ReadSection()
    {
        string? text = _text;
        int end = text.Length;
        int sourcePosition = SourcePosition;
        int newSourcePosition = int.MaxValue;
        NewLine newLine = NewLine.None;

        if ((uint)sourcePosition >= (uint)end)
        {
            text = null;
        }
        else
        {
#if NETCOREAPP3_1_OR_GREATER
            ReadOnlySpan<char> span = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref Unsafe.AsRef(in text.GetPinnableReference()), sourcePosition), end - sourcePosition);
#else
            ReadOnlySpan<char> span = text.AsSpan(sourcePosition);
#endif

            int startSection = span.IndexOfAny('[', '$');
            sourcePosition += startSection;
            int nextSection = 0;
            char a = ' ';

            do
            {
                nextSection += text.AsSpan(sourcePosition + startSection + nextSection).IndexOf('[');
                if (nextSection == -1)
                    break;

                a = text.Substring(sourcePosition + startSection, nextSection).Reverse().First(f => f == '$' || f == '\r' || f== '\n');
                if(a == '$')
                {
                    nextSection++;
                }

            } while (a != '\r' && a != '\n');



            if (nextSection != -1)
            {
                nextSection++;
            }
            else
            {
                nextSection = span.Length - 1;
            }
            end = sourcePosition + nextSection - 1;
            newSourcePosition = end;

            var t = text.Substring(sourcePosition, nextSection);

#if NETCOREAPP3_1_OR_GREATER
                if (Unsafe.Add(ref Unsafe.AsRef(in text.GetPinnableReference()), end) == '\r')
#else
                if ((uint)end < (uint)text.Length && (text[end] == '\r' || text[end] == '['))
#endif
                {
                    if ((uint)newSourcePosition < (uint)text.Length && text[newSourcePosition] == '[')
                    {
                        newLine = NewLine.CarriageReturnLineFeed;
                        newSourcePosition--;
                    }
                    else
                    {
                        newLine = NewLine.CarriageReturn;
                    }
                }
                else
                {
                    newLine = NewLine.LineFeed;
                }
            
        }

        SourcePosition = newSourcePosition;
        return new StringSlice(text, sourcePosition, end, newLine, dummy: false);
    }

    public StringSlice PeekLine()
    {
        string? text = _text;
        int end = text.Length;
        int sourcePosition = SourcePosition;
        int newSourcePosition = int.MaxValue;
        NewLine newLine = NewLine.None;

        if ((uint)sourcePosition >= (uint)end)
        {
            text = null;
        }
        else
        {
#if NETCOREAPP3_1_OR_GREATER
            ReadOnlySpan<char> span = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref Unsafe.AsRef(in text.GetPinnableReference()), sourcePosition), end - sourcePosition);
#else
            ReadOnlySpan<char> span = text.AsSpan(sourcePosition);
#endif

            int crlf = span.IndexOfAny('\r', '\n');
            if (crlf >= 0)
            {
                end = sourcePosition + crlf;
                newSourcePosition = end + 1;

#if NETCOREAPP3_1_OR_GREATER
                if (Unsafe.Add(ref Unsafe.AsRef(in text.GetPinnableReference()), end) == '\r')
#else
                if ((uint)end < (uint)text.Length && text[end] == '\r')
#endif
                {
                    if ((uint)newSourcePosition < (uint)text.Length && text[newSourcePosition] == '\n')
                    {
                        newLine = NewLine.CarriageReturnLineFeed;
                        newSourcePosition++;
                    }
                    else
                    {
                        newLine = NewLine.CarriageReturn;
                    }
                }
                else
                {
                    newLine = NewLine.LineFeed;
                }
            }
        }

        return new StringSlice(text, sourcePosition, end - 1, newLine, dummy: false);
    }
}