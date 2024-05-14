using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TsiErp.Business.Utilities.FinanceUtilities.TCMBExchange
{
    public static class TCMBCurrenciesExchange
    {


        public static Dictionary<string, CurrencyModel> GetAllCurrenciesTodaysExchangeRates()
        {
            try
            {
                return GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }



        public static Dictionary<string, CurrencyModel> GetAllCurrenciesHistoricalExchangeRates(int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                return GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }



        public static Dictionary<string, CurrencyModel> GetAllCurrenciesHistoricalExchangeRates(DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                return GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }



        public static CurrencyModel GetTodaysExchangeRates(TCMBCurrencyCode Currency)
        {
            try
            {
                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (CurrencyRates.Keys.Contains(Currency.ToString()))
                {
                    return CurrencyRates[Currency.ToString()];
                }
                else
                {
                    throw new Exception("The specified currency(" + Currency.ToString() + ") was not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static CurrencyModel GetHistoricalExchangeRates(TCMBCurrencyCode Currency, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (CurrencyRates.Keys.Contains(Currency.ToString()))
                {
                    return CurrencyRates[Currency.ToString()];
                }
                else
                {
                    throw new Exception("The specified currency(" + Currency.ToString() + ") was not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static CurrencyModel GetHistoricalExchangeRates(TCMBCurrencyCode Currency, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (CurrencyRates.Keys.Contains(Currency.ToString()))
                {
                    return CurrencyRates[Currency.ToString()];
                }
                else
                {
                    throw new Exception("The specified currency(" + Currency.ToString() + ") was not found!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static CurrencyModel GetTodaysCrossRates(TCMBCurrencyCode ToCurrencyCode, TCMBCurrencyCode FromCurrencyCode)
        {
            try
            {
                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return new CurrencyModel(
                        OtherCurrency.Name,
                        OtherCurrency.Code,
                        OtherCurrency.Code + "/" + MainCurrency.Code,
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexSelling == 0 || MainCurrency.ForexSelling == 0) ? 0 : Math.Round((OtherCurrency.ForexSelling / MainCurrency.ForexSelling), 4),
                        (OtherCurrency.BanknoteBuying == 0 || MainCurrency.BanknoteBuying == 0) ? 0 : Math.Round((OtherCurrency.BanknoteBuying / MainCurrency.BanknoteBuying), 4),
                        (OtherCurrency.BanknoteSelling == 0 || MainCurrency.BanknoteSelling == 0) ? 0 : Math.Round((OtherCurrency.BanknoteSelling / MainCurrency.BanknoteSelling), 4)
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double GetTodaysCrossRate(TCMBCurrencyCode ToCurrencyCode, TCMBCurrencyCode FromCurrencyCode)
        {
            try
            {
                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static CurrencyModel GetHistoricalCrossRates(TCMBCurrencyCode ToCurrencyCode, TCMBCurrencyCode FromCurrencyCode, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return new CurrencyModel(
                        OtherCurrency.Name,
                        OtherCurrency.Code,
                        OtherCurrency.Code + "/" + MainCurrency.Code,
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4)
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double GetHistoricalCrossRate(TCMBCurrencyCode ToCurrencyCode, TCMBCurrencyCode FromCurrencyCode, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static CurrencyModel GetHistoricalCrossRates(TCMBCurrencyCode ToCurrencyCode, TCMBCurrencyCode FromCurrencyCode, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return new CurrencyModel(
                        OtherCurrency.Name,
                        OtherCurrency.Code,
                        OtherCurrency.Code + "/" + MainCurrency.Code,
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4),
                        (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4)
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double GetHistoricalCrossRate(TCMBCurrencyCode ToCurrencyCode, TCMBCurrencyCode FromCurrencyCode, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round((OtherCurrency.ForexBuying / MainCurrency.ForexBuying), 4);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateTodaysExchange(double Amount, TCMBCurrencyCode FromCurrencyCode, TCMBCurrencyCode ToCurrencyCode)
        {
            try
            {
                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateTodaysExchange(double Amount, TCMBCurrencyCode FromCurrencyCode, TCMBCurrencyCode ToCurrencyCode, TCMBExchangeType exchangeType)
        {
            try
            {
                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/today.xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    switch (exchangeType)
                    {
                        case TCMBExchangeType.ForexBuying:
                            return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                        case TCMBExchangeType.ForexSelling:
                            return (OtherCurrency.ForexSelling == 0 || MainCurrency.ForexSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexSelling / OtherCurrency.ForexSelling), 4);
                        case TCMBExchangeType.BanknoteBuying:
                            return (OtherCurrency.BanknoteBuying == 0 || MainCurrency.BanknoteBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteBuying / OtherCurrency.BanknoteBuying), 4);
                        case TCMBExchangeType.BanknoteSelling:
                            return (OtherCurrency.BanknoteSelling == 0 || MainCurrency.BanknoteSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteSelling / OtherCurrency.BanknoteSelling), 4);
                        default:
                            return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateHistoricalExchange(double Amount, TCMBCurrencyCode FromCurrencyCode, TCMBCurrencyCode ToCurrencyCode, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateHistoricalExchange(double Amount, TCMBCurrencyCode FromCurrencyCode, TCMBCurrencyCode ToCurrencyCode, TCMBExchangeType exchangeType, DateTime date)
        {
            try
            {
                string SYear = String.Format("{0:0000}", date.Year);
                string SMonth = String.Format("{0:00}", date.Month);
                string SDay = String.Format("{0:00}", date.Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    switch (exchangeType)
                    {
                        case TCMBExchangeType.ForexBuying:
                            return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                        case TCMBExchangeType.ForexSelling:
                            return (OtherCurrency.ForexSelling == 0 || MainCurrency.ForexSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexSelling / OtherCurrency.ForexSelling), 4);
                        case TCMBExchangeType.BanknoteBuying:
                            return (OtherCurrency.BanknoteBuying == 0 || MainCurrency.BanknoteBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteBuying / OtherCurrency.BanknoteBuying), 4);
                        case TCMBExchangeType.BanknoteSelling:
                            return (OtherCurrency.BanknoteSelling == 0 || MainCurrency.BanknoteSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteSelling / OtherCurrency.BanknoteSelling), 4);
                        default:
                            return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateHistoricalExchange(double Amount, TCMBCurrencyCode FromCurrencyCode, TCMBCurrencyCode ToCurrencyCode, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        public static double CalculateHistoricalExchange(double Amount, TCMBCurrencyCode FromCurrencyCode, TCMBCurrencyCode ToCurrencyCode, TCMBExchangeType exchangeType, int Year, int Month, int Day)
        {
            try
            {
                string SYear = String.Format("{0:0000}", Year);
                string SMonth = String.Format("{0:00}", Month);
                string SDay = String.Format("{0:00}", Day);

                Dictionary<string, CurrencyModel> CurrencyRates = GetCurrencyRates("http://www.tcmb.gov.tr/kurlar/" + SYear + SMonth + "/" + SDay + SMonth + SYear + ".xml");

                if (!CurrencyRates.Keys.Contains(FromCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + FromCurrencyCode.ToString() + ") was not found!");
                }
                else if (!CurrencyRates.Keys.Contains(ToCurrencyCode.ToString()))
                {
                    throw new Exception("The specified currency(" + ToCurrencyCode.ToString() + ") was not found!");
                }
                else
                {
                    CurrencyModel MainCurrency = CurrencyRates[FromCurrencyCode.ToString()];
                    CurrencyModel OtherCurrency = CurrencyRates[ToCurrencyCode.ToString()];

                    switch (exchangeType)
                    {
                        case TCMBExchangeType.ForexBuying:
                            return (OtherCurrency.ForexBuying == 0 || MainCurrency.ForexBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexBuying / OtherCurrency.ForexBuying), 4);
                        case TCMBExchangeType.ForexSelling:
                            return (OtherCurrency.ForexSelling == 0 || MainCurrency.ForexSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.ForexSelling / OtherCurrency.ForexSelling), 4);
                        case TCMBExchangeType.BanknoteBuying:
                            return (OtherCurrency.BanknoteBuying == 0 || MainCurrency.BanknoteBuying == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteBuying / OtherCurrency.BanknoteBuying), 4);
                        case TCMBExchangeType.BanknoteSelling:
                            return (OtherCurrency.BanknoteSelling == 0 || MainCurrency.BanknoteSelling == 0) ? 0 : Math.Round(Amount * (MainCurrency.BanknoteSelling / OtherCurrency.BanknoteSelling), 4);
                        default:
                            return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The date specified may be a weekend or a public holiday!");
            }
        }

        private static Dictionary<string, CurrencyModel> GetCurrencyRates(string Link)
        {
            try
            {
                XmlTextReader rdr = new XmlTextReader(Link);
                // XmlTextReader nesnesini yaratıyoruz ve parametre olarak xml dokümanın urlsini veriyoruz
                // XmlTextReader urlsi belirtilen xml dokümanlarına hızlı ve forward-only giriş imkanı sağlar.
                XmlDocument myxml = new XmlDocument();
                // XmlDocument nesnesini yaratıyoruz.
                myxml.Load(rdr);
                // Load metodu ile xml yüklüyoruz
                XmlNode tarih = myxml.SelectSingleNode("/Tarih_Date/@Tarih");
                XmlNodeList mylist = myxml.SelectNodes("/Tarih_Date/Currency");
                XmlNodeList adi = myxml.SelectNodes("/Tarih_Date/Currency/Isim");
                XmlNodeList kod = myxml.SelectNodes("/Tarih_Date/Currency/@Kod");
                XmlNodeList doviz_alis = myxml.SelectNodes("/Tarih_Date/Currency/ForexBuying");
                XmlNodeList doviz_satis = myxml.SelectNodes("/Tarih_Date/Currency/ForexSelling");
                XmlNodeList efektif_alis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteBuying");
                XmlNodeList efektif_satis = myxml.SelectNodes("/Tarih_Date/Currency/BanknoteSelling");

                Dictionary<string, CurrencyModel> ExchangeRates = new Dictionary<string, CurrencyModel>();

                ExchangeRates.Add("TRY", new CurrencyModel("Türk Lirası", "TRY", "TRY/TRY", 1, 1, 1, 1));

                for (int i = 0; i < adi.Count; i++)
                {
                    CurrencyModel cur = new CurrencyModel(adi.Item(i).InnerText.ToString(),
                        kod.Item(i).InnerText.ToString(),
                        kod.Item(i).InnerText.ToString() + "/TRY",
                        (String.IsNullOrWhiteSpace(doviz_alis.Item(i).InnerText.ToString())) ? 0 : Convert.ToDouble(doviz_alis.Item(i).InnerText.ToString().Replace(".", ",")),
                        (String.IsNullOrWhiteSpace(doviz_satis.Item(i).InnerText.ToString())) ? 0 : Convert.ToDouble(doviz_satis.Item(i).InnerText.ToString().Replace(".", ",")),
                        (String.IsNullOrWhiteSpace(efektif_alis.Item(i).InnerText.ToString())) ? 0 : Convert.ToDouble(efektif_alis.Item(i).InnerText.ToString().Replace(".", ",")),
                        (String.IsNullOrWhiteSpace(efektif_satis.Item(i).InnerText.ToString())) ? 0 : Convert.ToDouble(efektif_satis.Item(i).InnerText.ToString().Replace(".", ","))
                        );

                    ExchangeRates.Add(kod.Item(i).InnerText.ToString(), cur);
                }

                return ExchangeRates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
