/**********************************************
 *  MySqlResultPrinter.cs
 *  Ian Effendi
 *  ---
 *  Formatter that can print IResultSets.
 *********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Database
{

    /// <summary>
    /// Printer that can print result sets and other things.
    /// </summary>
    public class MySqlResultPrinter : IResultPrinter
    {
        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Character printed that represents a corner.
        /// </summary>
        private char cornerCharacter = '+';

        /// <summary>
        /// Character that represents a column wall.
        /// </summary>
        private char wallCharacter = '|';

        /// <summary>
        /// Character printed that represents a divisor edge.
        /// </summary>
        private char edgeCharacter = '-';

        /// <summary>
        /// Character printed that represents a masked character.
        /// </summary>
        private char maskCharacter = '*';

        /// <summary>
        /// Character printed that represents an empty space.
        /// </summary>
        private char spaceCharacter = ' ';

        /// <summary>
        /// Character printed that represents a newline.
        /// </summary>
        private char newlineCharacter = '\n';

        /// <summary>
        /// Maximum length of a column before terms are truncated. (Includes padding).
        /// </summary>
        private int maxCharacters = 50;

        /// <summary>
        /// Value to append to strings for padding (after the result).
        /// </summary>
        private int paddingLength = 2;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Returns corner character.
        /// </summary>
        public char Corner
        {
            get { return this.cornerCharacter; }
        }

        /// <summary>
        /// Returns wall character.
        /// </summary>
        public char Wall
        {
            get { return this.wallCharacter; }
        }

        /// <summary>
        /// Edge of a divisor.
        /// </summary>
        public char Edge
        {
            get { return this.edgeCharacter; }
        }

        /// <summary>
        /// Character mask.
        /// </summary>
        public char Mask
        {
            get { return this.maskCharacter; }
        }

        /// <summary>
        /// Whitespace representation.
        /// </summary>
        public char Space
        {
            get { return this.spaceCharacter; }
        }

        /// <summary>
        /// Newline character.
        /// </summary>
        public char Newline
        {
            get { return this.newlineCharacter; }
        }
        
        /// <summary>
        /// Maximum length of the printer columns.
        /// </summary>
        public int MaxLength
        {
            get { return this.maxCharacters; }
        }

        /// <summary>
        /// Reference to amount of padding to display.
        /// </summary>
        public int PaddingLength
        {
            get { return this.paddingLength; }
        }

        /// <summary>
        /// Returns string version of padding of length <see cref="PaddingLength"/>.
        /// </summary>
        public string Padding
        {
            get { return this.Repeat(this.Space, this.PaddingLength); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Result printer created based off of default values.
        /// </summary>
        /// <param name="maxLength">Maximum column length.</param>
        /// <param name="padding">Padding character count.</param>
        /// <param name="wall">Wall character.</param>
        /// <param name="edge">Divisor edge character.</param>
        /// <param name="space">Space character.</param>
        /// <param name="mask">Mask character.</param>
        /// <param name="corner">Corner character.</param>
        public MySqlResultPrinter(int maxLength,
            int padding = 1,
            char wall = '|',
            char edge = '-',
            char space = ' ',
            char mask = '*',
            char corner = '+')
        {
            this.paddingLength = padding;
            this.maxCharacters = maxLength;
            this.wallCharacter = wall;
            this.edgeCharacter = edge;
            this.spaceCharacter = space;
            this.maskCharacter = mask;
            this.cornerCharacter = corner;
        }

        /// <summary>
        /// Printer of MySQL result set.
        /// </summary>
        public MySqlResultPrinter()
            : this(50)
        {
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        //////////////////////
        // Formatting Methods.

        /// <summary>
        /// Return a string filled with a repeating character.
        /// </summary>
        /// <param name="c">Character to repeat.</param>
        /// <param name="amount">Number of times to repeat a character.</param>
        /// <returns>Return string filled with repeated characters.</returns>
        public string Repeat(char c, int amount)
        {
            if (amount == 0) { return ""; }
            return new string(c, amount);
        }

        /// <summary>
        /// Format text to be truncated or be filled with padding to fill length.
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="length">Length to limit total string length to.</param>
        /// <returns>Returns formatted string.</returns>
        private string Format(string text, int length)
        {
            if (length == 0) { return ""; }
            string format = text.Trim();
            string padding = this.Padding;
            string ellipses = "";
            string buffer = "";
            string formatted = "";
            
            int currentLength = format.Length;
            if (currentLength == length) { return $"{this.Space}{format}{padding}"; }

            // If not match at first sight, continue down the line.            
            while (currentLength != length)
            {
                // Update the formatted string.
                formatted = $"{format}{ellipses}{buffer}";
                currentLength = formatted.Length;

                // If the input data no longer is appearing due to the length of the object, just return the string as-is regardless.
                if (currentLength == length || (ellipses.Length == 3 && format.Length <= 2)) { break; }

                // If longer than set length, shorten the response.
                if (currentLength > length)
                {
                    // Provide ellipses.
                    ellipses = "...";

                    // Shorten the text by one character.
                    format = format.Substring(0, format.Length - 1);
                }

                // If shorter than set length, lengthen the response.
                if (currentLength < length)
                {
                    // Add buffer.
                    buffer = $"{buffer}{this.Space}";
                }
            }
            // Return the formatted object.
            return this.Space + formatted + padding;
        }

        /// <summary>
        /// Format an entry, without any walls.
        /// </summary>
        /// <param name="item">Value to format.</param>
        /// <param name="length">Length of space to fill.</param>
        /// <returns>Returns formatted string.</returns>
        public string FormatText(string item, int length)
        {
            // Walls aren't counted as part of the length, BUT, the extra space here is.
            if (length == 0) { return ""; }
            return this.Format(item, length);
        }

        /// <summary>
        /// Format entry, providing walls for the left and right edges.
        /// </summary>
        /// <param name="item">Value to format.</param>
        /// <param name="length">Length of space to fill.</param>
        /// <returns>Returns formatted string.</returns>
        public string FormatTextSegment(string item, int length)
        {
            // Walls aren't counted as part of the length, BUT, the extra space here is.
            if (length == 0) { return ""; }
            string left = $"{this.Wall}";
            string right = $"{this.Wall}";
            return left + this.FormatText(item, length) + right;
        }

        /// <summary>
        /// Format entry, providing the left wall.
        /// </summary>
        /// <param name="item">Value to format.</param>
        /// <param name="length">Length of space to fill.</param>
        /// <returns>Returns formatted string.</returns>
        public string FormatTextLeft(string item, int length)
        {
            // Walls aren't counted as part of the length, BUT, the extra space here is.
            if (length == 0) { return ""; }
            string left = $"{this.Wall}";
            return left + this.FormatText(item, length);
        }

        /// <summary>
        /// Format entry, providing the right wall.
        /// </summary>
        /// <param name="item">Value to format.</param>
        /// <param name="length">Length of space to fill.</param>
        /// <returns>Returns formatted string.</returns>
        public string FormatTextRight(string item, int length)
        {
            // Walls aren't counted as part of the length, BUT, the extra space here is.
            if (length == 0) { return ""; }
            string right = $"{this.Wall}";
            return this.FormatText(item, length) + right;
        }

        /// <summary>
        /// Format a row of entries, based on input values.
        /// </summary>
        /// <param name="entries">Entries to format.</param>
        /// <param name="columnLengths">Lengths to print each segment.</param>
        /// <returns>Returns formatted string.</returns>
        public string FormatText(List<string> entries, List<int> columnLengths = null)
        {
            if (entries == null) { return ""; }
            switch (entries.Count)
            {
                case 0:
                    // No segment, if no lengths.
                    return "";
                case 1:
                    // Overload of single term.
                    int length = (columnLengths == null || columnLengths.Count == 0) ? entries[0].Length : columnLengths[0];
                    return $"{this.FormatTextSegment(entries[0], length)}";
                case 2:
                    // Overload of two terms.
                    int lengthA = (columnLengths == null || columnLengths.Count <= 0) ? entries[0].Length : columnLengths[0];
                    int lengthB = (columnLengths == null || columnLengths.Count <= 1) ? entries[1].Length : columnLengths[1];
                    return $"{this.FormatTextSegment(entries[0], lengthA)}{this.FormatTextRight(entries[1], lengthB)}";
            }

            int columnLength = (columnLengths == null || columnLengths.Count <= 0) ? entries[0].Length : columnLengths[0];
            string format = $"{this.FormatTextSegment(entries[0], columnLength)}";

            for (int index = 1; index < entries.Count; index++)
            {
                columnLength = (columnLengths == null || columnLengths.Count < (index + 1)) ? entries[index].Length : columnLengths[index];
                format += $"{this.FormatTextRight(entries[index], columnLength)}";
            }

            return format;
        }

        /// <summary>
        /// Generate divisor edge of specified length.
        /// </summary>
        /// <param name="length">Length of the divisor.</param>
        /// <returns>Returns constructed string.</returns>
        private string GenerateDivisorEdge(int length)
        {
            return $"{Repeat(this.Edge, length)}";
        }

        /// <summary>
        /// Generates an entire divisor segment bookended by corner characters.
        /// </summary>
        /// <param name="length">Length of the divisor.</param>
        /// <returns>Returns constructed string.</returns>
        private string GenerateDivisorSegment(int length)
        {
            return $"{this.Corner}{this.GenerateDivisorEdge(1 + length + this.PaddingLength)}{this.Corner}";
        }

        /// <summary>
        /// Generate the left segment of a corner.
        /// </summary>
        /// <param name="length">Length of the divisor edge and corner composite.</param>
        /// <returns>Returns constructed string.</returns>
        private string GenerateLeftCorner(int length)
        {
            return $"{this.Corner}{this.GenerateDivisorEdge(1 + length + this.PaddingLength)}";
        }

        /// <summary>
        /// Generate the right segment of a corner.
        /// </summary>
        /// <param name="length">Length of the divisor edge and corner composite.</param>
        /// <returns>Returns constructed string.</returns>
        private string GenerateRightCorner(int length)
        {
            return $"{this.GenerateDivisorEdge(1 + length + this.PaddingLength)}{this.Corner}";
        }

        /// <summary>
        /// Generate a divisor separated by corner pieces.
        /// </summary>
        /// <param name="segmentLength">Total length of segment.</param>
        /// <returns>Returns generated string.</returns>
        public string GenerateDivisor(int segmentLength)
        {
            return this.GenerateDivisorSegment(segmentLength);
        }

        /// <summary>
        /// Generate a divisor separated by corner pieces.
        /// </summary>
        /// <param name="segmentLengths">Each segment's total length is stored in this collection.</param>
        /// <returns>Returns generated string.</returns>
        public string GenerateDivisor(List<int> segmentLengths)
        {
            if (segmentLengths == null) { return ""; }
            switch (segmentLengths.Count)
            {
                case 0:
                    // No segment, if no lengths.
                    return "";
                case 1:
                    // Overload of single term.
                    return $"{this.GenerateDivisor(segmentLengths[0])}";
                case 2:
                    // Overload of two terms.
                    return $"{this.GenerateDivisor(segmentLengths[0])}{this.GenerateRightCorner(segmentLengths[1])}";
            }

            string format = $"{this.GenerateDivisor(segmentLengths[0])}";

            for (int index = 1; index < segmentLengths.Count; index++)
            {
                format += $"{this.GenerateRightCorner(segmentLengths[index])}";
            }

            return format;
        }
        
        //////////////////////
        // Max Length Methods.

        /// <summary>
        /// Return the maximum length, implied by the input values as string lengths.
        /// </summary>
        /// <param name="a">Length of string a.</param>
        /// <param name="b">Length of string b.</param>
        /// <returns>Returns maximum value.</returns>
        public int GetMaximumLength(int a, int b)
        {
            // Initial base value.
            int max = 5;

            // Compare value of the term.
            max = Math.Max(a, max);
            max = Math.Max(b, max);

            // Clamp the maximum length found.
            return Math.Min(max, this.MaxLength);
        }
        
        /// <summary>
        /// Selects the maximum length from a collection of terms.
        /// </summary>
        /// <param name="terms">Terms to compare.</param>
        /// <returns>Returns integer denoting the maximum length.</returns>
        public int GetMaximumLength(List<int> terms)
        {
            // Initial base value.
            int max = 5;

            // Compare value of each string.
            foreach (int term in terms)
            {
                max = this.GetMaximumLength(term, max);
            }

            // Clamp the maximum length found.
            return max;
        }

        /// <summary>
        /// Selects the maximum length from a collection of terms.
        /// </summary>
        /// <param name="terms">Terms to compare.</param>
        /// <returns>Returns integer denoting the maximum length.</returns>
        public int GetMaximumLength(params int[] terms)
        {
            return this.GetMaximumLength(terms.ToList<int>());
        }

        /// <summary>
        /// Selects the maximum length from a collection of terms.
        /// </summary>
        /// <param name="terms">Terms to measure.</param>
        /// <returns>Returns integer denoting the maximum length.</returns>
        public int GetMaximumLength(List<string> terms)
        {
            // Initial base value.
            int max = 5;

            // Compare value of each string.
            foreach (string term in terms)
            {
                max = this.GetMaximumLength(term.Length, max);
            }

            // Clamp the maximum length found.
            return max;
        }

        /// <summary>
        /// Selects the maximum length from a collection of terms.
        /// </summary>
        /// <param name="terms">Terms to measure.</param>
        /// <returns>Returns integer denoting the maximum length.</returns>
        public int GetMaximumLength(params string[] terms)
        {
            return this.GetMaximumLength(terms.ToList<string>());
        }

        /// <summary>
        /// Get the maximum length from two strings.
        /// </summary>
        /// <param name="a">Term a.</param>
        /// <param name="b">Term b.</param>
        /// <returns>Returns integer denoting the maximum length.</returns>
        public int GetMaximumLength(string a, string b)
        {
            // Base value.
            int max = 5;

            // Find max from key and value.
            max = this.GetMaximumLength(a.Length, b.Length);

            // Clamp value.
            return max;
        }

        /// <summary>
        /// Get the maximum length from a key/value pair of strings.
        /// </summary>
        /// <param name="data">Data to parse length from.</param>
        /// <returns>Returns integer denoting the maximum length.</returns>
        public int GetMaximumLength(KeyValuePair<string, string> data)
        {
            return this.GetMaximumLength(data.Key, data.Value);
        }

        /// <summary>
        /// Get the maximum length from an entry.
        /// </summary>
        /// <param name="entry">Entry to get data to parse length from.</param>
        /// <returns>Returns integer denoting the maximum length.</returns>
        public int GetMaximumLength(IEntry entry)
        {
            return this.GetMaximumLength(entry.GetData());
        }

        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input, by comparing lists of column lengths.
        /// </summary>
        /// <param name="a">Collection of column lengths to compare. Primary comparison.</param>
        /// <param name="b">Collection of column lengths to compare.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(List<int> a, List<int> b)
        {
            List<int> collection = new List<int>();
            if (a == null || b == null || a.Count <= 0) { return collection; }

            // Primary count takes precedence: we do not care about the length of secondary collection. (it could be empty for all we care).
            for (int index = 0; index < a.Count; index++)
            {
                if (index >= b.Count)
                {
                    // If value doesn't exist at a particular index, just input the field's length.
                    collection.Add(a[index]);
                }
                else
                {
                    // Add the maximum length from the comparison.
                    collection.Add(this.GetMaximumLength(a[index], b[index]));
                }
            }

            return collection;
        }

        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input.
        /// </summary>
        /// <param name="dataCollection">Data collection to parse length from.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(List<KeyValuePair<string, string>> dataCollection)
        {
            List<int> collection = new List<int>();
            if (dataCollection == null || dataCollection.Count <= 0) { return collection; }
            foreach (KeyValuePair<string, string> datum in dataCollection)
            {
                // Get the maximum length for the pair in this column.
                collection.Add(this.GetMaximumLength(datum));
            }
            return collection;
        }

        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input.
        /// </summary>
        /// <param name="dataCollection">Data collection to parse length from.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(params KeyValuePair<string, string>[] dataCollection)
        {
            return this.GetMaximumLengths(dataCollection.ToList<KeyValuePair<string, string>>());
        }

        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input.
        /// </summary>
        /// <param name="fields">Collection of fields to parse through.</param>
        /// <param name="values">Collection of values associated with each field.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(List<string> fields, List<string> values)
        {
            List<int> collection = new List<int>();
            if (fields == null || values == null || fields.Count <= 0) { return collection; }

            // Field count takes precedence: we do not care about the length of values. (it could be empty for all we care).
            for (int index = 0; index < fields.Count; index++)
            {
                if (index >= values.Count)
                {
                    // If value doesn't exist at a particular index, just input the field's length.
                    collection.Add(fields[index].Length);
                }
                else
                {
                    // Add the maximum length from the comparison.
                    collection.Add(this.GetMaximumLength(fields[index], values[index]));
                }
            }

            return collection;
        }

        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input.
        /// </summary>
        /// <param name="entries">Entry collection to parse length from.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(List<IEntry> entries)
        {
            List<int> collection = new List<int>();
            if (entries == null || entries.Count <= 0) { return collection; }
            foreach (IEntry entry in entries)
            {
                // Get the maximum length for the pair in this column.
                collection.Add(this.GetMaximumLength(entry));
            }
            return collection;
        }

        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input.
        /// </summary>
        /// <param name="entries">Entry collection to parse length from.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(params IEntry[] entries)
        {
            return this.GetMaximumLengths(entries.ToList<IEntry>());
        }

        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input.
        /// </summary>
        /// <param name="row">Row to parse length from.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(IRow row)
        {
            if (row == null) { return new List<int>(); }
            return this.GetMaximumLengths(row.Entries);
        }
        
        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input, by comparing lists of column lengths.
        /// </summary>
        /// <param name="table">2D collection of column length collections to check against.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(List<List<int>> table)
        {
            if(table == null || table.Count <= 0) { return new List<int>(); }
            List<int> collection = table[0];

            for (int i = 0; i < table.Count; i++)
            {
                // Current columns in this table row of column lengths.
                int currentColumnCount = table[i].Count;
                
                // For every column in the table, check against current maximum lengths. 
                for (int j = 0; j < currentColumnCount; j++)
                {
                    // If j is greater than the collection count, simply add the current collection's terms to the list.
                    if (j >= collection.Count)
                    {
                        collection.Add(table[i][j]);
                    }
                    else // If j is within collection's bounds, get the maximum length.
                    {
                        collection[j] = this.GetMaximumLength(collection[j], table[i][j]);
                    }
                }
            }

            return collection;
        }
        
        /// <summary>
        /// Get an index-ordered collection of string maximum lengths for each column input, by comparing rows in a <see cref="IResultSet"/>.
        /// </summary>
        /// <param name="set">2D collection of column length collections (as <see cref="IRow"/>s) to check against.</param>
        /// <returns>Returns collection denoting the maximum length for each column.</returns>
        public List<int> GetMaximumLengths(IResultSet set)
        { 
            // If set is empty, return an empty list.
            if(set == null || set.IsEmpty) { return new List<int>(); }
            List<List<int>> maximumLengthCollectionByRow = this.GetCollectionOfMaximumLengths(set);
            return (maximumLengthCollectionByRow == null || maximumLengthCollectionByRow.Count <= 0) ? new List<int>() : this.GetMaximumLengths(maximumLengthCollectionByRow);
        }

        /// <summary>
        /// Get a 2D collection of maximum lengths per row from a IResultSet.
        /// </summary>
        /// <param name="set">Collection of <see cref="IRow"/>s fashioned as a <see cref="IResultSet"/>.</param>
        /// <returns>Returns 2D collection of collections containing the maximum length for each column.</returns>
        public List<List<int>> GetCollectionOfMaximumLengths(IResultSet set)
        {
            // If set is empty, return an empty list.
            if(set == null || set.IsEmpty) { return new List<List<int>>(); }

            // Create 2D collection, to first store each row's maximum length.
            List<List<int>> rowCollections = new List<List<int>>();

            // Loop through the set.
            foreach (IRow row in set.Rows)
            {
                rowCollections.Add(this.GetMaximumLengths(row));
            }

            // Get the collection of maximum lengths, one for every row.
            return rowCollections;
        }

        //////////////////////
        // Get Entry Methods.


        /// <summary>
        /// Return a formatted entry.
        /// </summary>
        /// <param name="field">Field to print.</param>
        /// <param name="value">Value to print.</param>
        /// <returns>Returns formatted entry in tabular format.</returns>
        public string FormatEntry(string field, string value)
        {
            int length = this.GetMaximumLength(field, value);
            string formattedField = this.FormatTextSegment(field, length);
            string formattedValue = this.FormatTextSegment(value, length);

            string format = "";
            format += $"{this.GenerateDivisorSegment(length)}" + this.Newline;
            format += $"{formattedField}" + this.Newline;
            format += $"{this.GenerateDivisorSegment(length)}" + this.Newline;
            format += $"{this.GenerateDivisorSegment(length)}" + this.Newline;
            format += $"{formattedValue}" + this.Newline;
            format += $"{this.GenerateDivisorSegment(length)}" + this.Newline;
            return format;
        }

        /// <summary>
        /// Return a formatted entry.
        /// </summary>
        /// <param name="data">Entry data to format.</param>
        /// <returns>Returns formatted entry in tabular format.</returns>
        public string FormatEntry(KeyValuePair<string, string> data)
        {
            if(data.Key.Length == 0) { return "No field data. Cannot print entry from this data."; }
            return this.FormatEntry(data.Key, data.Value);
        }

        /// <summary>
        /// Return a formatted entry.
        /// </summary>
        /// <param name="entry">Entry to format.</param>
        /// <returns>Returns formatted entry in tabular format.</returns>
        public string FormatEntry(IEntry entry)
        {
            if(entry == null) { return "Null entry object."; }
            return this.FormatEntry(entry.GetField(), entry.GetValue());            
        }

        /// <summary>
        /// Return a row of formatted fields.
        /// </summary>
        /// <param name="fields">The header fields.</param>
        /// <returns>Returns formatted fields in a tabular row.</returns>
        public string FormatHeader(List<string> fields)
        {
            if(fields == null || fields.Count == 0) { return "No fields to print."; }

            string formattedFields = $"{this.FormatTextSegment(fields[0], fields[0].Length)}";
            string divisor = $"{this.GenerateDivisorSegment(fields[0].Length)}";
            for (int index = 1; index < fields.Count; index++)
            {
                formattedFields += $"{this.FormatTextRight(fields[index], fields[index].Length)}";
                divisor += $"{this.GenerateRightCorner(fields[index].Length)}";
            }
            int length = formattedFields.Length;
            
            string format = "";
            format += $"{divisor}" + this.Newline;
            format += $"{formattedFields}" + this.Newline;
            format += $"{divisor}" + this.Newline;
            return format;


        }

        /// <summary>
        /// Return a row of formatted fields.
        /// </summary>
        /// <param name="entries">Entry to get headers from.</param>
        /// <returns>Returns formatted fields in a tabular row.</returns>
        public string FormatHeader(List<IEntry> entries)
        {
            if (entries == null || entries.Count == 0) { return "No entries to print."; }
            List<string> fields = new List<string>();
            foreach (IEntry entry in entries)
            {
                fields.Add(entry.GetField());
            }
            return this.FormatHeader(fields);
        }

        /// <summary>
        /// Return a row of formatted fields.
        /// </summary>
        /// <param name="entries">Entry to get headers from.</param>
        /// <returns>Returns formatted fields in a tabular row.</returns>
        public string FormatHeader(params IEntry[] entries)
        {
            return this.FormatHeader(entries.ToList<IEntry>());
        }
        
        public string FormatHeader(IRow header)
        {
            throw new NotImplementedException();
        }

        public string FormatRow(IRow row)
        {
            throw new NotImplementedException();
        }

        public string FormatResultSet(IResultSet results)
        {
            throw new NotImplementedException();
        }
                
        //////////////////////
        // Accessor(s).

        //////////////////////
        // Mutator(s).


    }
}
