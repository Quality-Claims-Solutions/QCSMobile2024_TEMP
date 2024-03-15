namespace QCSMobile2024.Shared.Utilities
{
    public class PhoneFormatter
    {
        public static string Format(string phoneNumber)
        {
            return FormatPhoneNumber(phoneNumber);
        }

        static string ext = " x";
        static string previous;
        public static string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;
            var output = new List<char>();
            var extension = new List<char>();
            int index = 0;
            //var first = true;
            phoneNumber = GetNumberOnly(phoneNumber);
            if (phoneNumber.StartsWith("1") || phoneNumber.StartsWith("+1"))
            {
                output.Add('1');
                //output.Add('-');
            }
            phoneNumber = phoneNumber.TrimStart('+', '1');


            foreach (var c in phoneNumber)
            {
                if (char.IsNumber(c))
                {
                    // skips the '1' at the beginning of a phone number
                    //if (first && index == 0)
                    //{
                    //    first = false;
                    //    if (c == '1')
                    //    {
                    //        output.Add(c); 
                    //        continue;
                    //    }
                    //}
                    index++;
                    if (index == 1)
                        output.Add('(');
                    if (index == 4)
                    {
                        output.Add(')');
                        output.Add(' ');
                    }
                    if (index == 7)
                        output.Add('-');
                    if (index == 11)
                        output.AddRange(ext.ToCharArray());

                    output.Add(c);


                    if (index == 16)
                        break; // max 6 extensions
                }

            }
            previous = new string(output.ToArray());
            return previous;
        }

        public static string RemoveExtension(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;
            var length = phoneNumber.Length;
            var extLength = ext.Length;
            if (phoneNumber.Substring(length - extLength - 1, extLength) == ext)
                return phoneNumber.Remove(length - extLength - 1, ext.Length);
            return phoneNumber;
        }

        public static bool IsValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            phoneNumber = FormatPhoneNumber(phoneNumber);
            return phoneNumber.Length >= 13;
        }

        public static string GetMainNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;
            int count = 0;
            var output = new List<char>();

            phoneNumber = phoneNumber.TrimStart('1', '+');
            foreach (var c in phoneNumber)
            {
                if (char.IsNumber(c))
                {
                    output.Add(c);
                    count++;
                    if (count == 10)
                        break;
                }
            }
            return new string(output.ToArray());
        }

        public static string GetExtension(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;
            int count = 0;
            var output = new List<char>();
            phoneNumber = phoneNumber.TrimStart('1', '+');
            foreach (var c in phoneNumber)
            {
                if (char.IsNumber(c))
                {
                    count++;
                    if (count <= 10)
                        continue;
                    output.Add(c);
                    if (output.Count == 6)
                        break; // 6 is the max extention length allowed in the database
                }
            }
            return new string(output.ToArray());
        }

        public static string GetDialerNumberWithExtension(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;
            var main = GetMainNumber(phoneNumber);
            var ext = GetExtension(phoneNumber);
            if (string.IsNullOrWhiteSpace(ext) == true)
                return main;
            return main + ";" + ext;
        }

        public static string GetNumberOnly(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return string.Empty;
            var output = new List<char>();

            if (phoneNumber[0] == '+')
                output.Add('+');


            foreach (var c in phoneNumber)
            {
                if (char.IsNumber(c))
                {
                    output.Add(c);
                }
            }
            return new string(output.ToArray());
        }

        public static string GetDbSearchString(string phoneNumber)
        {
            phoneNumber = FormatPhoneNumber(GetMainNumber(phoneNumber));
            phoneNumber = phoneNumber.Replace('(', '%').Replace(')', '%').Replace('-', '%').Replace(" ", "") + '%';
            return phoneNumber;
        }
    }
}
