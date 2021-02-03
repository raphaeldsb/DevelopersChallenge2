﻿using BankTransactionConciliationAPI.Models;
using BankTransactionConciliationAPI.Models.AutoGenerated;
using BankTransactionConciliationAPI.Models.Exceptions;
using BankTransactionConciliationAPI.Parsers.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace BankTransactionConciliationAPI.Parsers
{
    public class OfxParser : IOfxParser
    {
        public List<BankTransaction> Convert(string ofx)
        {
            try
            {
                var xml = this.TransformOfxToXml(ofx);
                var bankTransactions = this.MapXmlToBankTransactions(xml);
                return bankTransactions;
            }
            catch (Exception e)
            {
                throw new InvalidOfxException();
            }
        }

        private string TransformOfxToXml(string ofx)
        {
            var xml = this.RemoveOfxHeader(ofx);
            xml = this.CloseTags(xml);
            
            return xml;
        }

        private List<BankTransaction> MapXmlToBankTransactions(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OFX), new XmlRootAttribute("OFX"));
            StringReader xmlReader = new StringReader(xml);
            var ofxObject = (OFX) serializer.Deserialize(xmlReader);

            var bankTransactions = new List<BankTransaction>();

            var bank = ofxObject.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID;
            var accountNumber = ofxObject.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID;
            var accountType = ofxObject.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE;

            foreach (var transaction in ofxObject.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN)
            {
                var temp = new BankTransaction(
                    bank.ToString(),
                    accountNumber.ToString(),
                    accountType,
                    this.ConvertDate(transaction.DTPOSTED),
                    transaction.TRNTYPE,
                    transaction.TRNAMT,
                    transaction.MEMO);

                bankTransactions.Add(temp);
            }

            return bankTransactions;
        }

        private DateTime ConvertDate(string date)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            
            var dateParts = date.Split("["); // ignoring timezone
            var dateTime = DateTime.ParseExact(dateParts[0], "yyyyMMddhhmmss", provider);

            return dateTime;
        }

        private string RemoveOfxHeader(string ofx)
        {
            var position = ofx.IndexOf("<OFX>", System.StringComparison.InvariantCultureIgnoreCase);
            
            if (position < 0)
            {
                return ofx;
            }
            
            return ofx.Substring(position);
        }

        private string CloseTags(string ofx)
        {
            var lines = ofx.Replace("\r", "").Split("\n");

            for (int i = 0; i < lines.Length; i++)
            {
                 lines[i] = this.CloseTag(lines[i]);
            }

            return string.Join("\n", lines);
        }

        private string CloseTag(string line)
        {
            if (line.EndsWith(">"))
            {
                return line;
            }

            line = line.Replace("\t", "").Trim();

            var tagClose = $"</{line.Substring(1, line.IndexOf(">"))}";

            return $"{line}{tagClose}";
        }
    }
}
