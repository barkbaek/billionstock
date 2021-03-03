using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace billionStock
{
    public class 일봉차트
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("code")]
        public string _종목코드 { get; set; }
        [BsonElement("name")]
        public string _종목명 { get; set; }

        public 일봉차트 () {}
    }

    public class 매수추천
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("code")]
        public string _종목코드 { get; set; }
        [BsonElement("name")]
        public string _종목명 { get; set; }
        [BsonElement("tradeCount")]
        public int _체결개수 { get; set; }
        [BsonElement("tradeStrength")]
        public double _체결강도 { get; set; }
        [BsonElement("cumulativeVolume")]
        public long _누적거래량 { get; set; }
        [BsonElement("cumulativeCost")]
        public long _누적거래대금 { get; set; }
        [BsonElement("cumulativeOfferCost")]
        public double _매도거래대금 { get; set; }
        [BsonElement("cumulativeBidCost")]
        public double _매수거래대금 { get; set; }
        [BsonElement("bidCurrentArray")]
        public List<int> _매수현재가_목록 { get; set; }
        [BsonElement("firstOffer")]
        public int _최우선매도호가 { get; set; }
        [BsonElement("firstBid")]
        public int _최우선매수호가 { get; set; }
        [BsonElement("breakThroughTickCount")]
        public int _돌파틱개수 { get; set; }
        [BsonElement("netChangeRate")]
        public double _등락율 { get; set; }
        [BsonElement("current")]
        public int _현재가 { get; set; }
        [BsonElement("time")]
        public int _체결시간 { get; set; }
        public 매수추천() {}
    }

    public class 매수추천종목
    {
        [BsonElement("_id")]
        public string _아이디 { get; set; }
        [BsonElement("name")]
        public string _종목명 { get; set; }
        [BsonElement("step")]
        public string _단계 { get; set; }
        [BsonElement("ranking")]
        public int _순위 { get; set; }
        [BsonElement("market")]
        public int _시장 { get; set; }
        [BsonElement("firstTime")]
        public int _첫추출시간 { get; set; }
        [BsonElement("firstNetChangeRate")]
        public double _첫추출등락율 { get; set; }
        [BsonElement("firstCurrent")]
        public int _첫추출호가 { get; set; }
        [BsonElement("lastTime")]
        public int _끝추출시간 { get; set; }
        [BsonElement("lastNetChangeRate")]
        public double _끝추출등락율 { get; set; }
        [BsonElement("lastCurrent")]
        public int _끝추출호가 { get; set; }
        [BsonElement("current")]
        public int _현재가 { get; set; }
        [BsonElement("firstOffer")]
        public int _최우선매도호가 { get; set; }
        [BsonElement("firstBid")]
        public int _최우선매수호가 { get; set; }
        [BsonElement("count")]
        public int _추출개수 { get; set; }
        [BsonElement("richEarningRate")]
        public double _세력순수익률 { get; set; }
        [BsonElement("richBidCost")]
        public long _세력매수거래대금 { get; set; }
        [BsonElement("cumulativeVolume")]
        public long _누적거래량 { get; set; }
        [BsonElement("isRecommended")]
        public bool _매수추천있니 { get; set; }

        public 매수추천종목 () {}
    }

    public class 유보종목
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("code")]
        public string _종목코드 { get; set; }
    }

    public class 보유종목
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("code")]
        public string _종목코드 { get; set; }

        [BsonElement("name")]
        public string _종목명 { get; set; }

        [BsonElement("estimatedProfitAndLoss")]
        public long _평가손익 { get; set; }

        [BsonElement("earningRate")]
        public double _수익률 { get; set; }

        [BsonElement("purchasePrice")]
        public int _매입가 { get; set; }

        [BsonElement("changeClose")]
        public int _전일종가 { get; set; }

        [BsonElement("ownQuantity")]
        public long _보유수량 { get; set; }

        [BsonElement("possibleTradeQuantity")]
        public long _매매가능수량 { get; set; }

        [BsonElement("current")]
        public int _현재가 { get; set; }

        [BsonElement("purchaseCost")]
        public long _매입금액 { get; set; }

        [BsonElement("purchaseCommission")]
        public long _매입수수료 { get; set; }

        [BsonElement("estimatedCost")]
        public long _평가금액 { get; set; }

        [BsonElement("estimatedCommission")]
        public long _평가수수료 { get; set; }

        [BsonElement("tax")]
        public long _세금 { get; set; }
        
        [BsonElement("totalCommission")]
        public long _수수료합 { get; set; }

        [BsonElement("retainedPercentage")]
        public double _보유비중 { get; set; }

        [BsonElement("creditSort")]
        public int _신용구분 { get; set; }

        [BsonElement("creditSortName")]
        public string _신용구분명 { get; set; }

        [BsonElement("loanDate")]
        public string _대출일 { get; set; }
    }
    
    public class 종목
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("code")]
        public string _종목코드 { get; set; }
        [BsonElement("name")]
        public string _종목명 { get; set; }
        [BsonElement("settleAccountMonth")]
        public int _결산월 { get; set; }
        [BsonElement("faceValue")]
        public double _액면가 { get; set; }
        [BsonElement("capital")]
        public long _자본금 { get; set; }
        [BsonElement("listedShares")]
        public long _상장주식 { get; set; }
        [BsonElement("creditRate")]
        public double _신용비율 { get; set; }
        [BsonElement("highestOfYear")]
        public int _연중최고 { get; set; }
        [BsonElement("lowestOfYear")]
        public int _연중최저 { get; set; }
        [BsonElement("marketCapitalization")]
        public long _시가총액 { get; set; }
        [BsonElement("marketCapitalizationWeight")]
        public string _시가총액비중 { get; set; }
        [BsonElement("foreignConsumptionRate")]
        public double _외인소진률 { get; set; }
        [BsonElement("substitutePrice")]
        public int _대용가 { get; set; }
        [BsonElement("per")]
        public double _PER { get; set; }
        [BsonElement("eps")]
        public double _EPS { get; set; }
        [BsonElement("roe")]
        public double _ROE { get; set; }
        [BsonElement("pbr")]
        public double _PBR { get; set; }
        [BsonElement("ev")]
        public double _EV { get; set; }
        [BsonElement("bps")]
        public double _BPS { get; set; }
        [BsonElement("sales")]
        public long _매출액 { get; set; }
        [BsonElement("businessProfit")]
        public long _영업이익 { get; set; }
        [BsonElement("netProfit")]
        public long _당기순이익 { get; set; }
        [BsonElement("highest250")]
        public int _250최고 { get; set; }
        [BsonElement("lowest250")]
        public int _250최저 { get; set; }
        [BsonElement("open")]
        public int _시가 { get; set; }
        [BsonElement("high")]
        public int _고가 { get; set; }
        [BsonElement("low")]
        public int _저가 { get; set; }
        [BsonElement("upperLimitPrice")]
        public int _상한가 { get; set; }
        [BsonElement("lowerLimitPrice")]
        public int _하한가 { get; set; }
        [BsonElement("standardPrice")]
        public int _기준가 { get; set; }
        [BsonElement("estimatedSettlementPrice")]
        public int _예상체결가 { get; set; }
        [BsonElement("estimatedSettlementVolume")]
        public long _예상체결수량 { get; set; }
        [BsonElement("highest250Date")]
        public int _250최고가일 { get; set; }
        [BsonElement("highest250ChangeRate")]
        public double _250최고가대비율 { get; set; }
        [BsonElement("lowest250Date")]
        public int _250최저가일 { get; set; }
        [BsonElement("lowest250ChangeRate")]
        public double _250최저가대비율 { get; set; }
        [BsonElement("current")]
        public int _현재가 { get; set; }
        [BsonElement("changeSymbol")]
        public int _대비기호 { get; set; }
        [BsonElement("netChange")]
        public int _전일대비 { get; set; }
        [BsonElement("netChangeRate")]
        public double _등락율 { get; set; }
        [BsonElement("volume")]
        public long _거래량 { get; set; }
        [BsonElement("volumeChangeRate")]
        public double _거래대비 { get; set; }
        [BsonElement("faceValueUnit")]
        public string _액면가단위 { get; set; }
        [BsonElement("market")]
        public int _시장 { get; set; }
        [BsonElement("types")]
        public List<string> _업종_목록 { get; set; }
        [BsonElement("themes")]
        public List<string> _테마_목록 { get; set; }
        [BsonElement("memo")]
        public List<string> _메모_목록 { get; set; }
        [BsonElement("keepMonitorFalse")]
        public bool _계속감시안할까 { get; set; }
        [BsonElement("monitor")]
        public bool _감시 { get; set; }
        [BsonElement("notMonitorReasons")]
        public List<string> _감시안하는이유_목록 { get; set; }
        [BsonElement("createdAt")]
        public int _생성날짜 { get; set; }
        [BsonElement("updatedAt")]
        public int _수정날짜 { get; set; }

        public 종목 () {}
    }

    public class 주식체결
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("code")]
        public string _종목코드 { get; set; }
        [BsonElement("time")]
        public int _체결시간 { get; set; }
        [BsonElement("registeredTime")]
        public int _등록시간 { get; set; }
        [BsonElement("order")]
        public int _순서 { get; set; }
        [BsonElement("current")]
        public int _현재가 { get; set; }
        [BsonElement("average")]
        public int _평균가 { get; set; }
        [BsonElement("netChange")]
        public int _전일대비 { get; set; }
        [BsonElement("netChangeRate")]
        public double _등락율 { get; set; }
        [BsonElement("firstOffer")]
        public int _최우선매도호가 { get; set; }
        [BsonElement("firstBid")]
        public int _최우선매수호가 { get; set; }
        [BsonElement("volume")]
        public long _거래량 { get; set; }
        [BsonElement("cost")]
        public long _거래대금 { get; set; }
        [BsonElement("cumulativeVolume")]
        public long _누적거래량 { get; set; }
        [BsonElement("cumulativeCost")]
        public long _누적거래대금 { get; set; }
        [BsonElement("open")]
        public int _시가 { get; set; }
        [BsonElement("high")]
        public int _고가 { get; set; }
        [BsonElement("low")]
        public int _저가 { get; set; }
        [BsonElement("netChangeSymbol")]
        public int _전일대비기호 { get; set; }
        [BsonElement("volumeChangeContractShare")]
        public long _전일거래량대비_계약주 { get; set; }
        [BsonElement("transactionPriceChange")]
        public long _거래대금증감 { get; set; }
        [BsonElement("volumeChangeRate")]
        public double _전일거래량대비_비율 { get; set; }
        [BsonElement("transactionTurnoverRate")]
        public double _거래회전율 { get; set; }
        [BsonElement("transactionCost")]
        public long _거래비용 { get; set; }
        [BsonElement("tradeStrength")]
        public double _체결강도 { get; set; }
        [BsonElement("marketCapitalization")]
        public long _시가총액_억 { get; set; }
        [BsonElement("timeType")]
        public int _장구분 { get; set; }
        [BsonElement("koAccessibility")]
        public int _KO접근도 { get; set; }
        [BsonElement("upperLimitPriceTime")]
        public int _상한가발생시간 { get; set; }
        [BsonElement("lowerLimitPriceTime")]
        public int _하한가발생시간 { get; set; }
        [BsonElement("buy")]
        public bool _매수했니 { get; set; }
        [BsonElement("createdAt")]
        public int _생성날짜 { get; set; }

        public 주식체결 () {}
    }

    public class 부자주식체결
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("code")]
        public string _종목코드 { get; set; }
        [BsonElement("time")]
        public int _체결시간 { get; set; }
        [BsonElement("registeredTime")]
        public int _등록시간 { get; set; }
        [BsonElement("order")]
        public int _순서 { get; set; }
        [BsonElement("current")]
        public int _현재가 { get; set; }
        [BsonElement("netChangeRate")]
        public double _등락율 { get; set; }
        [BsonElement("firstOffer")]
        public int _최우선매도호가 { get; set; }
        [BsonElement("firstBid")]
        public int _최우선매수호가 { get; set; }
        [BsonElement("volume")]
        public long _거래량 { get; set; }
        [BsonElement("cost")]
        public long _거래대금 { get; set; }
        [BsonElement("open")]
        public int _시가 { get; set; }
        [BsonElement("high")]
        public int _고가 { get; set; }
        [BsonElement("low")]
        public int _저가 { get; set; }
        [BsonElement("buy")]
        public bool _매수했니 { get; set; }
        [BsonElement("createdAt")]
        public int _생성날짜 { get; set; }

        public 부자주식체결() { }
    }

    public class 주식호가잔량
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("code")]
        public string _종목코드 { get; set; }
        [BsonElement("time")]
        public int _호가시간 { get; set; }
        [BsonElement("offer1")]
        public int _매도호가1 { get; set; }
        [BsonElement("offer2")]
        public int _매도호가2 { get; set; }
        [BsonElement("offer3")]
        public int _매도호가3 { get; set; }
        [BsonElement("offer4")]
        public int _매도호가4 { get; set; }
        [BsonElement("offer5")]
        public int _매도호가5 { get; set; }
        [BsonElement("offer6")]
        public int _매도호가6 { get; set; }
        [BsonElement("offer7")]
        public int _매도호가7 { get; set; }
        [BsonElement("offer8")]
        public int _매도호가8 { get; set; }
        [BsonElement("offer9")]
        public int _매도호가9 { get; set; }
        [BsonElement("offer10")]
        public int _매도호가10 { get; set; }

        [BsonElement("bid1")]
        public int _매수호가1 { get; set; }
        [BsonElement("bid2")]
        public int _매수호가2 { get; set; }
        [BsonElement("bid3")]
        public int _매수호가3 { get; set; }
        [BsonElement("bid4")]
        public int _매수호가4 { get; set; }
        [BsonElement("bid5")]
        public int _매수호가5 { get; set; }
        [BsonElement("bid6")]
        public int _매수호가6 { get; set; }
        [BsonElement("bid7")]
        public int _매수호가7 { get; set; }
        [BsonElement("bid8")]
        public int _매수호가8 { get; set; }
        [BsonElement("bid9")]
        public int _매수호가9 { get; set; }
        [BsonElement("bid10")]
        public int _매수호가10 { get; set; }

        [BsonElement("offerQuantity1")]
        public long _매도호가수량1 { get; set; }
        [BsonElement("offerQuantity2")]
        public long _매도호가수량2 { get; set; }
        [BsonElement("offerQuantity3")]
        public long _매도호가수량3 { get; set; }
        [BsonElement("offerQuantity4")]
        public long _매도호가수량4 { get; set; }
        [BsonElement("offerQuantity5")]
        public long _매도호가수량5 { get; set; }
        [BsonElement("offerQuantity6")]
        public long _매도호가수량6 { get; set; }
        [BsonElement("offerQuantity7")]
        public long _매도호가수량7 { get; set; }
        [BsonElement("offerQuantity8")]
        public long _매도호가수량8 { get; set; }
        [BsonElement("offerQuantity9")]
        public long _매도호가수량9 { get; set; }
        [BsonElement("offerQuantity10")]
        public long _매도호가수량10 { get; set; }

        [BsonElement("bidQuantity1")]
        public long _매수호가수량1 { get; set; }
        [BsonElement("bidQuantity2")]
        public long _매수호가수량2 { get; set; }
        [BsonElement("bidQuantity3")]
        public long _매수호가수량3 { get; set; }
        [BsonElement("bidQuantity4")]
        public long _매수호가수량4 { get; set; }
        [BsonElement("bidQuantity5")]
        public long _매수호가수량5 { get; set; }
        [BsonElement("bidQuantity6")]
        public long _매수호가수량6 { get; set; }
        [BsonElement("bidQuantity7")]
        public long _매수호가수량7 { get; set; }
        [BsonElement("bidQuantity8")]
        public long _매수호가수량8 { get; set; }
        [BsonElement("bidQuantity9")]
        public long _매수호가수량9 { get; set; }
        [BsonElement("bidQuantity10")]
        public long _매수호가수량10 { get; set; }

        [BsonElement("offerChange1")]
        public long _매도호가직전대비1 { get; set; }
        [BsonElement("offerChange2")]
        public long _매도호가직전대비2 { get; set; }
        [BsonElement("offerChange3")]
        public long _매도호가직전대비3 { get; set; }
        [BsonElement("offerChange4")]
        public long _매도호가직전대비4 { get; set; }
        [BsonElement("offerChange5")]
        public long _매도호가직전대비5 { get; set; }
        [BsonElement("offerChange6")]
        public long _매도호가직전대비6 { get; set; }
        [BsonElement("offerChange7")]
        public long _매도호가직전대비7 { get; set; }
        [BsonElement("offerChange8")]
        public long _매도호가직전대비8 { get; set; }
        [BsonElement("offerChange9")]
        public long _매도호가직전대비9 { get; set; }
        [BsonElement("offerChange10")]
        public long _매도호가직전대비10 { get; set; }

        [BsonElement("bidChange1")]
        public long _매수호가직전대비1 { get; set; }
        [BsonElement("bidChange2")]
        public long _매수호가직전대비2 { get; set; }
        [BsonElement("bidChange3")]
        public long _매수호가직전대비3 { get; set; }
        [BsonElement("bidChange4")]
        public long _매수호가직전대비4 { get; set; }
        [BsonElement("bidChange5")]
        public long _매수호가직전대비5 { get; set; }
        [BsonElement("bidChange6")]
        public long _매수호가직전대비6 { get; set; }
        [BsonElement("bidChange7")]
        public long _매수호가직전대비7 { get; set; }
        [BsonElement("bidChange8")]
        public long _매수호가직전대비8 { get; set; }
        [BsonElement("bidChange9")]
        public long _매수호가직전대비9 { get; set; }
        [BsonElement("bidChange10")]
        public long _매수호가직전대비10 { get; set; }

        [BsonElement("offerTotalQuantity")]
        public long _매도호가총잔량 { get; set; }
        [BsonElement("bidTotalQuantity")]
        public long _매수호가총잔량 { get; set; }

        [BsonElement("offerTotalQuantityChange")]
        public long _매도호가총잔량직전대비 { get; set; }
        [BsonElement("bidTotalQuantityChange")]
        public long _매수호가총잔량직전대비 { get; set; }

        [BsonElement("netOfferQuantity")]
        public long _순매도잔량 { get; set; }
        [BsonElement("netBidQuantity")]
        public long _순매수잔량 { get; set; }

        [BsonElement("offerRate")]
        public double _매도비율 { get; set; }
        [BsonElement("bidRate")]
        public double _매수비율 { get; set; }
        
        [BsonElement("cumulativeVolume")]
        public long _누적거래량 { get; set; }

        [BsonElement("volumeChangeEstimatedSettlementRate")]
        public double _전일거래량대비예상체결률 { get; set; }

        [BsonElement("timeType")]
        public string _장운영구분 { get; set; }

        [BsonElement("tickerPerInvestor")]
        public string _투자자별ticker { get; set; }

        [BsonElement("createdAt")]
        public int _생성날짜 { get; set; }

        public 주식호가잔량 () {}
    }

    /* 주식종합 - 접수/체결 */
    public class 주문체결
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("account")] /* 9201 */
        public string _계좌번호 { get; set; }

        [BsonElement("orderNumber")] /* 9203 */
        public string _주문번호 { get; set; }

        [BsonElement("adminNumber")] /* 9205 */
        public string _관리자사번 { get; set; }

        [BsonElement("code")] /* 9001 */
        public string _종목코드 { get; set; }

        [BsonElement("orderTaskSort")] /* 912 */
        public string _주문업무분류 { get; set; }

        [BsonElement("orderStatus")] /* 913 */
        public string _주문상태 { get; set; }

        [BsonElement("name")] /* 302 */
        public string _종목명 { get; set; }

        [BsonElement("orderQuantity")] /* 900 */
        public long _주문수량 { get; set; }

        [BsonElement("orderPrice")] /* 901 */
        public int _주문가격 { get; set; }

        [BsonElement("notTradedQuantity")] /* 902 */
        public long _미체결수량 { get; set; }

        [BsonElement("tradeCumulativeCost")] /* 903 */
        public long _체결누계금액 { get; set; }

        [BsonElement("origOrderNumber")] /* 904 */
        public string _원주문번호 { get; set; }

        [BsonElement("orderSort")] /* 905 */
        public string _주문구분 { get; set; }

        [BsonElement("tradeSort")] /* 906 */
        public string _매매구분 { get; set; }

        [BsonElement("offerBidSort")] /* 907 */
        public string _매도수구분 { get; set; }

        [BsonElement("time")] /* 908 */
        public int _주문체결시간 { get; set; }

        [BsonElement("tradeNumber")] /* 909 */
        public string _체결번호 { get; set; }

        [BsonElement("tradePrice")] /* 910 */
        public int _체결가 { get; set; }

        [BsonElement("tradeQuantity")] /* 911 */
        public long _체결량 { get; set; }

        [BsonElement("current")] /* 10 */
        public int _현재가 { get; set; }

        [BsonElement("firstOffer")] /* 27 */
        public int _최우선매도호가 { get; set; }

        [BsonElement("firstBid")] /* 28 */
        public int _최우선매수호가 { get; set; }

        [BsonElement("unitTradePrice")] /* 914 */
        public int _단위체결가 { get; set; }

        [BsonElement("unitTradeQuantity")] /* 915 */
        public long _단위체결량 { get; set; }

        [BsonElement("todayTradeCommission")] /* 938 */
        public double _당일매매수수료 { get; set; }

        [BsonElement("todayTradeTax")] /* 939 */
        public double _당일매매세금 { get; set; }

        [BsonElement("rejectReasons")] /* 919 */
        public string _거부사유 { get; set; }

        [BsonElement("screenNumber")] /* 920 */
        public int _화면번호 { get; set; }

        [BsonElement("terminalNumber")] /* 921 */
        public int _터미널번호 { get; set; }

        [BsonElement("creditSort")] /* 922 */
        public string _신용구분 { get; set; }

        [BsonElement("loanDate")] /* 923 */
        public string _대출일 { get; set; }
        
        [BsonElement("otherArray")] /* 949 10010  */
        public List<string> _기타_목록 { get; set; }

        [BsonElement("registeredTime")]
        public int _등록시간 { get; set; }

        [BsonElement("createdAt")]
        public int _생성날짜 { get; set; }
    }

    /* 주식종합 - 파생잔고 */
    public class 파생잔고
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("otherArray")]
        public List<string> _기타_목록 { get; set; }

        [BsonElement("registeredTime")]
        public int _등록시간 { get; set; }

        [BsonElement("createdAt")]
        public int _생성날짜 { get; set; }
    }

    /* 주식종합 - 잔고 */
    public class 잔고
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("account")] /* 9201 */
        public string _계좌번호 { get; set; }

        [BsonElement("code")] /* 9001 */
        public string _종목코드 { get; set; }

        [BsonElement("creditSort")] /* 917 */
        public string _신용구분 { get; set; }

        [BsonElement("loanDate")] /* 916 */
        public string _대출일 { get; set; }

        [BsonElement("name")] /* 302 */
        public string _종목명 { get; set; }

        [BsonElement("current")] /* 10 */
        public int _현재가 { get; set; }

        [BsonElement("ownQuantity")] /* 930 */
        public long _보유수량 { get; set; }

        [BsonElement("purchasePrice")] /* 931 */
        public int _매입단가 { get; set; }

        [BsonElement("totalPurchasePrice")] /* 932 */
        public int _총매입가 { get; set; }

        [BsonElement("possibleOrderQuantity")] /* 933 */
        public long _주문가능수량 { get; set; }

        [BsonElement("todayNetBidQuantity")] /* 945 */
        public long _당일순매수량 { get; set; }

        [BsonElement("offerBidSort")] /* 946 */
        public string _매도매수구분 { get; set; }

        [BsonElement("todayTotalOfferProfit")] /* 950 */
        public string _당일총매도손일 { get; set; }

        [BsonElement("reservedCost")] /* 951 */
        public long _예수금 { get; set; }

        [BsonElement("firstOffer")] /* 27 */
        public int _최우선매도호가 { get; set; }

        [BsonElement("firstBid")] /* 28 */
        public int _최우선매수호가 { get; set; }

        [BsonElement("standardPrice")] /* 307 */
        public int _기준가 { get; set; }

        [BsonElement("profitAndLossRate")] /* 8019 */
        public double _손익율 { get; set; }

        [BsonElement("creditCost")] /* 957 */
        public double _신용금액 { get; set; }

        [BsonElement("creditInterest")] /* 958 */
        public double _신용이자 { get; set; }

        [BsonElement("expiryDate")] /* 918 */
        public string _만기일 { get; set; }

        [BsonElement("todayProfitAndLossSecurity")] /* 990 */
        public string _당일실현손익유가 { get; set; }

        [BsonElement("todayProfitAndLossRateSecurity")] /* 991 */
        public double _당일실현손익률유가 { get; set; }

        [BsonElement("todayProfitAndLossCredit")] /* 992 */
        public string _당일실현손익신용 { get; set; }

        [BsonElement("todayProfitAndLossRateCredit")] /* 993 */
        public double _당일실현손익률신용 { get; set; }

        [BsonElement("mortgageLoanQuantity")] /* 959 */
        public long _담보대출수량 { get; set; }

        [BsonElement("extraItem")] /* 924 */
        public string _ExtraItem { get; set; }

        [BsonElement("otherArray")] /* 10010 25 11 12 306  */
        public List<string> _기타_목록 { get; set; }

        [BsonElement("registeredTime")]
        public int _등록시간 { get; set; }

        [BsonElement("createdAt")]
        public int _생성날짜 { get; set; }
    }

    public class 종목테스트
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("codeList")]
        public List<string> _코드목록 { get; set; }

        public 종목테스트 () {}
    }

    public class 종목비교테스트
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("list")]
        public List<string> _목록 { get; set; }
        [BsonElement("stock")]
        public int _종목 { get; set; }
        [BsonElement("trade")]
        public int _체결 { get; set; }

        public 종목비교테스트() { }
    }
}