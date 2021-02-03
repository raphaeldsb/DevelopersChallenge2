using BankTransactionConciliationAPI.Models.Exceptions;
using BankTransactionConciliationAPI.Parsers;
using Xunit;

namespace BankTransactionConciliationAPI.Tests.Parsers
{
    public class OfxParserTests
    {
        [Fact]        
        public void Convert_Should_Throws_An_Exception_When_OFX_Is_Invalid()
        {
            // arrange
            var ofxParser = new OfxParser();
            var ofx = "<OFX>....</OFX>";

            // act & assert
            Assert.Throws<InvalidOfxException>(() => 
            {
                var result = ofxParser.Convert(ofx);
            });
        }

        [Fact]
        public void Convert_Should_Parses_A_Valid_OFX()
        {
            // arrange
            var ofxParser = new OfxParser();

            #region OFX DATA
            var ofx = @"OFXHEADER:100
                        DATA:OFXSGML
                        VERSION:102
                        SECURITY:NONE
                        ENCODING:USASCII
                        CHARSET:1252
                        COMPRESSION:NONE
                        OLDFILEUID:NONE
                        NEWFILEUID:NONE

                        <OFX>
                        <SIGNONMSGSRSV1>
                        <SONRS>
                        <STATUS>
                        <CODE>0
                        <SEVERITY>INFO
                        </STATUS>
                        <DTSERVER>20140318100000[-03:EST]
                        <LANGUAGE>POR
                        </SONRS>
                        </SIGNONMSGSRSV1>
                        <BANKMSGSRSV1>
                        <STMTTRNRS>
                        <TRNUID>1001
                        <STATUS>
                        <CODE>0
                        <SEVERITY>INFO
                        </STATUS>
                        <STMTRS>
                        <CURDEF>BRL
                        <BANKACCTFROM>
                        <BANKID>0341
                        <ACCTID>7037300576
                        <ACCTTYPE>CHECKING
                        </BANKACCTFROM>
                        <BANKTRANLIST>
                        <DTSTART>20140201100000[-03:EST]
                        <DTEND>2014020100000[-03:EST]
                        <STMTTRN>
                        <TRNTYPE>DEBIT
                        <DTPOSTED>20140203100000[-03:EST]
                        <TRNAMT>-140.00
                        <MEMO>CXE     001958 SAQUE    
                        </STMTTRN>
                        <STMTTRN>
                        <TRNTYPE>DEBIT
                        <DTPOSTED>20140204100000[-03:EST]
                        <TRNAMT>-102.19
                        <MEMO>RSHOP-SUPERMERCAD-03/02 
                        </STMTTRN>
                        </BANKTRANLIST>
                        <LEDGERBAL>
                        <BALAMT>-4021.44
                        <DTASOF>20140318100000[-03:EST]
                        </LEDGERBAL>
                        </STMTRS>
                        </STMTTRNRS>
                        </BANKMSGSRSV1>
                        </OFX>";
            #endregion

            // act
            var result = ofxParser.Convert(ofx);

            // assert
            Assert.Equal(2, result.Count);
            Assert.Equal("341", result[0].Bank);
            Assert.Equal("7037300576", result[0].AccountNumber);
            Assert.Equal("CHECKING", result[0].AccountType);
            Assert.Equal(-140.00m, result[0].Amount);
            Assert.Equal("CXE     001958 SAQUE", result[0].Description);
            Assert.Equal("DFD30381B1261CFDFC55FFB981ECE4D62D94A223", result[0].Id);
            Assert.Equal("2014-02-03 10:00:00", result[0].TransactionDate.ToString("yyyy-MM-dd hh:mm:ss"));
            Assert.Equal("DEBIT", result[0].TransactionType);
            Assert.Equal("341", result[1].Bank);
            Assert.Equal("7037300576", result[1].AccountNumber);
            Assert.Equal("CHECKING", result[1].AccountType);
            Assert.Equal(-102.19m,  result[1].Amount);
            Assert.Equal("RSHOP-SUPERMERCAD-03/02", result[1].Description);
            Assert.Equal("5595384799D6D2EC41F176FE142256E436736792", result[1].Id);
            Assert.Equal("2014-02-04 10:00:00", result[1].TransactionDate.ToString("yyyy-MM-dd hh:mm:ss"));
            Assert.Equal("DEBIT", result[1].TransactionType);
        }
    }
}
