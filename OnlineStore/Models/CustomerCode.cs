using System;

namespace OnlineStore.Models
{
    /// <summary>
    /// XXXX-YYYY
    /// </summary>
    public class CustomerCode
    {
        /// <summary>
        /// XXXX - число заказчика
        /// </summary>
        public int? Number { get; set; }

        /// <summary>
        /// YYYY - год регистрации
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// XXXX-YYYY
        /// </summary>
        public string Code { get; set; }

        public override string ToString()
        {
            return Number + "-" + Year;
        }

        public static CustomerCode FromString(string str)
        {
            var splitted = str.Split('-', StringSplitOptions.RemoveEmptyEntries);

            var customerCode = new CustomerCode
            {
                Code = str,
                Number = splitted.Length == 2 && int.TryParse(splitted[0], out var code) ? code : (int?) null,
                Year = splitted.Length == 2 && int.TryParse(splitted[1], out var year) ? year : (int?) null,
            };

            return customerCode;
        }
    }
}
