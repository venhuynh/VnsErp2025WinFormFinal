using System;

namespace Common.Helpers
{
    /// <summary>
    /// Helper class để chuyển đổi số thành chữ tiếng Việt
    /// </summary>
    public static class NumberToWordsHelper
    {
        private static readonly string[] Ones = 
        {
            "", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"
        };

        private static readonly string[] Tens = 
        {
            "", "mười", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", 
            "sáu mươi", "bảy mươi", "tám mươi", "chín mươi"
        };

        private static readonly string[] Hundreds = 
        {
            "", "một trăm", "hai trăm", "ba trăm", "bốn trăm", "năm trăm",
            "sáu trăm", "bảy trăm", "tám trăm", "chín trăm"
        };

        /// <summary>
        /// Chuyển đổi số thành chữ tiếng Việt
        /// </summary>
        /// <param name="number">Số cần chuyển đổi</param>
        /// <returns>Chuỗi chữ tiếng Việt</returns>
        public static string ConvertToWords(decimal number)
        {
            if (number == 0)
                return "không đồng";

            // Làm tròn đến hàng đơn vị
            long integerPart = (long)Math.Round(number, 0, MidpointRounding.AwayFromZero);

            if (integerPart == 0)
                return "không đồng";

            if (integerPart < 0)
                return "âm " + ConvertToWords(-integerPart);

            string result = ConvertNumber(integerPart);
            
            // Thêm "đồng" vào cuối
            if (!string.IsNullOrWhiteSpace(result))
            {
                result = result.Trim() + " đồng";
            }

            return result;
        }

        /// <summary>
        /// Chuyển đổi số nguyên thành chữ
        /// </summary>
        private static string ConvertNumber(long number)
        {
            if (number == 0)
                return "";

            if (number < 0)
                return "âm " + ConvertNumber(-number);

            if (number < 10)
                return Ones[number];

            if (number < 100)
                return ConvertTens(number);

            if (number < 1000)
                return ConvertHundreds(number);

            if (number < 1000000)
                return ConvertThousands(number);

            if (number < 1000000000)
                return ConvertMillions(number);

            return ConvertBillions(number);
        }

        /// <summary>
        /// Chuyển đổi hàng chục
        /// </summary>
        private static string ConvertTens(long number)
        {
            if (number < 10)
                return Ones[number];

            if (number == 10)
                return "mười";

            long tens = number / 10;
            long ones = number % 10;

            if (ones == 0)
                return Tens[tens];

            if (ones == 1 && tens > 1)
                return Tens[tens] + " mốt";

            if (ones == 5 && tens > 1)
                return Tens[tens] + " lăm";

            return Tens[tens] + " " + Ones[ones];
        }

        /// <summary>
        /// Chuyển đổi hàng trăm
        /// </summary>
        private static string ConvertHundreds(long number)
        {
            long hundreds = number / 100;
            long remainder = number % 100;

            if (remainder == 0)
                return Hundreds[hundreds];

            string result = Hundreds[hundreds];
            
            if (remainder < 10)
            {
                if (hundreds > 0)
                    result += " linh " + Ones[remainder];
                else
                    result = Ones[remainder];
            }
            else
            {
                result += " " + ConvertTens(remainder);
            }

            return result;
        }

        /// <summary>
        /// Chuyển đổi hàng nghìn
        /// </summary>
        private static string ConvertThousands(long number)
        {
            long thousands = number / 1000;
            long remainder = number % 1000;

            string result = ConvertNumber(thousands) + " nghìn";

            if (remainder == 0)
                return result;

            if (remainder < 100)
            {
                if (remainder < 10)
                    result += " không trăm linh " + Ones[remainder];
                else
                    result += " không trăm " + ConvertTens(remainder);
            }
            else
            {
                result += " " + ConvertHundreds(remainder);
            }

            return result;
        }

        /// <summary>
        /// Chuyển đổi hàng triệu
        /// </summary>
        private static string ConvertMillions(long number)
        {
            long millions = number / 1000000;
            long remainder = number % 1000000;

            string result = ConvertNumber(millions) + " triệu";

            if (remainder == 0)
                return result;

            if (remainder < 1000)
            {
                if (remainder < 100)
                {
                    if (remainder < 10)
                        result += " không trăm linh " + Ones[remainder];
                    else
                        result += " không trăm " + ConvertTens(remainder);
                }
                else
                {
                    result += " " + ConvertHundreds(remainder);
                }
            }
            else
            {
                result += " " + ConvertThousands(remainder);
            }

            return result;
        }

        /// <summary>
        /// Chuyển đổi hàng tỷ
        /// </summary>
        private static string ConvertBillions(long number)
        {
            long billions = number / 1000000000;
            long remainder = number % 1000000000;

            string result = ConvertNumber(billions) + " tỷ";

            if (remainder == 0)
                return result;

            if (remainder < 1000000)
            {
                if (remainder < 1000)
                {
                    if (remainder < 100)
                    {
                        if (remainder < 10)
                            result += " không trăm linh " + Ones[remainder];
                        else
                            result += " không trăm " + ConvertTens(remainder);
                    }
                    else
                    {
                        result += " " + ConvertHundreds(remainder);
                    }
                }
                else
                {
                    result += " " + ConvertThousands(remainder);
                }
            }
            else
            {
                result += " " + ConvertMillions(remainder);
            }

            return result;
        }
    }
}

