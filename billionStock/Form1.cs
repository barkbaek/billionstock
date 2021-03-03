using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Media;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace billionStock
{
    public partial class 빌리언스탁_Form : Form
    {
        빌리언스탁_Form _빌리언스탁;

        // private double 주식수수료율 = 1.0037869;
        private double 주식수수료율 = 1.0033;

        private const int _진행상황최대값 = 100;
        private int 스크린번호 = 1000;
        private int _진행상황현재값 = 0;
        static MongoClient _몽고클라이언트 = new MongoClient("mongodb://localhost:27017/billionStock");
        static IMongoDatabase _몽고디비 = _몽고클라이언트.GetDatabase("billionStock");
        static IMongoCollection<종목> _종목컬렉션 = _몽고디비.GetCollection<종목>("stocks");
        static IMongoCollection<주식체결> _주식체결컬렉션 = _몽고디비.GetCollection<주식체결>("trades");
        static IMongoCollection<부자주식체결> _부자주식체결컬렉션 = _몽고디비.GetCollection<부자주식체결>("richTrades");
        static IMongoCollection<주식호가잔량> _주식호가잔량컬렉션 = _몽고디비.GetCollection<주식호가잔량>("residualQuantities");

        private List<string> _시장코드_목록;
        private Dictionary<string, string> _시장코드의시장명_사전;
        private Dictionary<string, int> _시장코드의종목개수_사전;

        private Dictionary<int, string> _오류코드의오류메시지_사전;
        private Dictionary<string, 종목> _종목코드의종목아_전체사전;
        private Dictionary<string, List<주식체결>> _종목코드의주식체결목록_사전;
       
        private SoundPlayer _사운드플레이어 = new SoundPlayer();


        static 실시간매수추천 _실시간매수추천이야 = 실시간매수추천.GetInstance();
        static 요청데이터관리자 _요청데이터관리자야 = 요청데이터관리자.GetInstance();
        private bool _실시간매수추천_시작했니 = false;

        private bool _8시삼십분지났니 = false;

        private string _시장구분 = "0";
        private Dictionary<string, string> _종목코드의코스피종목_사전;
        private Dictionary<string, string> _종목코드의코스닥종목_사전;
        private Dictionary<string, string> _스크린번호의실시간종목_사전;
        private int _키움응답주식기본정보카운트 = 0;
        private int _순서 = 1;
        List<string> 종목코드_목록 = new List<string>();

        public 당일매수추천종목목록_Form _당일매수추천종목목록이야;

        public 당일주식체결_Form _당일주식체결이야;
        public 당일주식체결_Form _A주식체결이야;
        public 당일주식체결_Form _B주식체결이야;

        public 주식종합_Form _주식종합이야;

        private int _오늘의날짜 = 0;
        
        public 빌리언스탁_Form()
        {
            InitializeComponent();
            컴포넌트_초기화해줘();
            효과음_들려줘("환영");
            axKHOpenAPI1.OnEventConnect += OnEventConnect;
            axKHOpenAPI1.OnReceiveTrData += OnReceiveTrData;
            axKHOpenAPI1.OnReceiveMsg += OnReceiveMsg;
            axKHOpenAPI1.OnReceiveChejanData += OnReceiveChejanData;
            axKHOpenAPI1.OnReceiveRealData += OnReceiveRealData;
        }

        public void 빌리언스탁_저장해줘 (빌리언스탁_Form 빌리언스탁)
        {
            _빌리언스탁 = 빌리언스탁;
        }

        public void 컴포넌트_초기화해줘() {
            _시장코드_목록 = new List<string>();
            _시장코드_목록.Add("0");
            _시장코드_목록.Add("10");
            _시장코드_목록.Add("3");
            _시장코드_목록.Add("8");
            _시장코드_목록.Add("50");
            _시장코드_목록.Add("4");
            _시장코드_목록.Add("5");
            _시장코드_목록.Add("6");
            _시장코드_목록.Add("9");
            _시장코드_목록.Add("30");
            _시장코드의시장명_사전 = new Dictionary<string, string>()
            {
                { "0", "장내" },
                { "10", "코스닥" },
                { "3", "ELW" },
                { "8", "ETF" },
                { "50", "KONEX" },
                { "4", "뮤추얼펀드" },
                { "5", "신주인수권" },
                { "6", "리츠" },
                { "9", "하이얼펀드" },
                { "30", "K-OTC" }
            };
            _시장코드의종목개수_사전 = new Dictionary<string, int>()
            {
                { "0", 0 },
                { "10", 0 },
                { "3", 0 },
                { "8", 0 },
                { "50", 0 },
                { "4", 0 },
                { "5", 0 },
                { "6", 0 },
                { "9", 0 },
                { "30", 0 }
            };
            _오류코드의오류메시지_사전 = new Dictionary<int, string>() {
                { 0, "정상처리" },
                { -10 , "실패" },
                { -100, "사용자정보교환 실패" },
                { -101, "서버 접속 실패" },
                { -102, "버전처리 실패" },
                { -103, "개인방화벽 실패" },
                { -104, "메모리 보호실패" },
                { -105, "함수입력값 오류" },
                { -106, "통신연결 종료" },
                { -200, "시세조회 과부하" },
                { -201, "전문작성 초기화 실패" },
                { -202, "전문작성 입력값 오류" },
                { -203, "데이터 없음" },
                { -204, "조회가능한 종목수 초과. 한번에 조회 가능한 종목개수는 최대 100종목" },
                { -205, "데이터 수신 실패" },
                { -206, "조회가능한 FID수 초과. 한번에 조회 가능한 FID개수는 최대 100개" },
                { -207, "실시간 해제오류" },
                { -300, "입력값 오류" },
                { -301, "계좌비밀번호 없음" },
                { -302, "타인계좌 사용오류" },
                { -303, "주문가격이 20억원을 초과" },
                { -304, "주문가격이 50억원을 초과" },
                { -305, "주문수량이 총발행주수의 1% 초과오류" },
                { -306, "주문수량은 총발행주수의 3% 초과오류" },
                { -307, "주문전송 실패" },
                { -308, "주문전송 과부하" },
                { -309, "주문수량 300계약 초과" },
                { -310, "주문수량 500계약 초과" },
                { -311, "주문전송제한 과부하" },
                { -340, "계좌정보 없음" },
                { -500, "종목코드 없음" }
             };
        }

        private string 스크린번호_알려줘() {
            if (스크린번호 >= 9999) 
            {
                스크린번호 = 1000;
            }
            스크린번호++;
            return 스크린번호.ToString();
        }

        private void OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            string 문자_임시 = "";

            /* 키움증권 변수 */
            string _250최고_임시 = "";
            string _250최고가대비율_임시 = "";
            string _250최고가일_임시 = "";
            string _250최저_임시 = "";
            string _250최저가대비율_임시 = "";
            string _250최저가일_임시 = "";
            string BPS_임시 = "";
            string EPS_임시 = "";
            string EV_임시 = "";
            string KO접근도_임시 = "";
            string PBR_임시 = "";
            string PER_임시 = "";
            string ROE_임시 = "";
            string 감시_임시 = "";
            string 감시안하는이유_임시 = "";
            string 거래대금증감_임시 = "";
            string 거래대비_임시 = "";
            string 거래량_임시 = "";
            string 거래비용_임시 = "";
            string 거래회전율_임시 = "";
            string 결산월_임시 = "";
            string 고가_임시 = "";
            string 기준가_임시 = "";
            string 날짜_임시 = "";
            string 누적거래대금_임시 = "";
            string 누적거래량_임시 = "";
            string 당기순이익_임시 = "";
            string 대비기호_임시 = "";
            string 대용가_임시 = "";
            string 등락율_임시 = "";
            string 매수했니_임시 = "";
            string 매출액_임시 = "";
            string 상장주식_임시 = "";
            string 상한가_임시 = "";
            string 상한가발생시간_임시 = "";
            string 시가_임시 = "";
            string 시가총액_임시 = "";
            string 시가총액비중_임시 = "";
            string 시가총액_억_임시 = "";
            string 시장_임시 = "";
            string 신용비율_임시 = "";
            string 액면가_임시 = "";
            string 액면가단위_임시 = "";
            string 연중최고_임시 = "";
            string 연중최저_임시 = "";
            string 영업이익_임시 = "";
            string 예상체결가_임시 = "";
            string 예상체결수량_임시 = "";
            string 외인소진률_임시 = "";
            string 자본금_임시 = "";
            string 장구분_임시 = "";
            string 저가_임시 = "";
            string 전일거래량대비_계약주_임시 = "";
            string 전일거래량대비_비율_임시 = "";
            string 전일대비_임시 = "";
            string 전일대비기호_임시 = "";
            string 종목명_임시 = "";
            string 종목코드_임시 = "";
            string 체결강도_임시 = "";
            string 체결시간_임시 = "";
            string 총대주금액_임시 = "";
            string 총대출금_임시 = "";
            string 총매입금액_임시 = "";
            string 총수익률_임시 = "";
            string 총융자금액_임시 = "";
            string 총평가금액_임시 = "";
            string 총평가손익금액_임시 = "";
            string 최우선매도호가_임시 = "";
            string 최우선매수호가_임시 = "";
            string 추정예탁자산_임시 = "";
            string 평균가_임시 = "";
            string 하한가_임시 = "";
            string 하한가발생시간_임시 = "";
            string 현재가_임시 = "";

            int _250최고 = 0;
            double _250최고가대비율 = 0;
            int _250최고가일 = 0;
            int _250최저 = 0;
            double _250최저가대비율 = 0;
            int _250최저가일 = 0;
            double BPS = 0;
            double EPS = 0;
            double EV = 0;
            int KO접근도 = 0;
            double PBR = 0;
            double PER = 0;
            double ROE = 0;
            bool 감시 = true;
            string 감시안하는이유 = "";
            long 거래대금증감 = 0;
            double 거래대비 = 0;
            long 거래량 = 0;
            long 거래비용 = 0;
            double 거래회전율 = 0;
            int 결산월 = 0;
            int 고가 = 0;
            int 기준가 = 0;
            int 날짜 = 날짜_알려줘();
            long 누적거래대금 = 0;
            long 누적거래량 = 0;
            long 당기순이익 = 0;
            int 대비기호 = 0;
            int 대용가 = 0;
            double 등락율 = 0;
            bool 매수했니 = true;
            long 매출액 = 0;
            long 상장주식 = 0;
            int 상한가 = 0;
            int 상한가발생시간 = 0;
            int 시가 = 0;
            long 시가총액 = 0;
            string 시가총액비중 = "";
            long 시가총액_억 = 0;
            int 시장 = 0;
            double 신용비율 = 0;
            double 액면가 = 0;
            string 액면가단위 = "";
            int 연중최고 = 0;
            int 연중최저 = 0;
            long 영업이익 = 0;
            int 예상체결가 = 0;
            long 예상체결수량 = 0;
            double 외인소진률 = 0;
            long 자본금 = 0;
            int 장구분 = 0;
            int 저가 = 0;
            long 전일거래량대비_계약주 = 0;
            double 전일거래량대비_비율 = 0;
            int 전일대비 = 0;
            int 전일대비기호 = 0;
            string 종목명 = "";
            string 종목코드 = "";
            double 체결강도 = 0;
            int 체결시간 = 0;
            long 총대주금액 = 0;
            long 총대출금 = 0;
            long 총매입금액 = 0;
            double 총수익률 = 0;
            long 총융자금액 = 0;
            long 총평가금액 = 0;
            long 총평가손익금액 = 0;
            int 최우선매도호가 = 0;
            int 최우선매수호가 = 0;
            long 추정예탁자산 = 0;
            int 평균가 = 0;
            int 하한가 = 0;
            int 하한가발생시간 = 0;
            int 현재가 = 0;

            string 종목번호_임시 = "";
            string 평가손익_임시 = "";
            string 수익률_임시 = "";
            string 매입가_임시 = "";
            string 전일종가_임시 = "";
            string 보유수량_임시 = "";
            string 매매가능수량_임시 = "";
            string 전일매수수량_임시 = "";
            string 전일매도수량_임시 = "";
            string 금일매수수량_임시 = "";
            string 금일매도수량_임시 = "";
            string 매입금액_임시 = "";
            string 매입수수료_임시 = "";
            string 평가금액_임시 = "";
            string 평가수수료_임시 = "";
            string 세금_임시 = "";
            string 수수료합_임시 = "";
            string 보유비중_임시 = "";
            string 신용구분_임시 = "";
            string 신용구분명_임시 = "";
            string 대출일_임시 = "";
            
            try
            {
                if (e.sRQName == "계좌평가잔고내역")
                {
                    Console.WriteLine(
                        String.Format("계좌평가잔고내역\n\n" +
                        "e.nDataLength: {0}\n\n" +
                        "e.sErrorCode: {1}\n\n" +
                        "e.sMessage: {2}\n\n" +
                        "e.sPrevNext: {3}\n\n" +
                        "e.sRecordName: {4}\n\n" +
                        "e.sRQName: {5}\n\n" +
                        "e.sScrNo: {6}\n\n" +
                        "e.sSplmMsg: {7}\n\n" +
                        "e.sTrCode: {8}\n\n",
                        e.nDataLength,
                        e.sErrorCode,
                        e.sMessage,
                        e.sPrevNext,
                        e.sRecordName,
                        e.sRQName,
                        e.sScrNo,
                        e.sSplmMsg,
                        e.sTrCode
                        ));

                    총매입금액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총매입금액").Trim();
                    총매입금액 = 총매입금액_임시 != "" ? (long.Parse(총매입금액_임시)) : 0;
                    
                    총평가금액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총평가금액").Trim();
                    총평가금액 = 총평가금액_임시 != "" ? (long.Parse(총평가금액_임시)) : 0;

                    총평가손익금액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총평가손익금액").Trim();
                    총평가손익금액 = 총평가손익금액_임시 != "" ? (long.Parse(총평가손익금액_임시)) : 0;

                    총수익률_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총수익률(%)").Trim();
                    총수익률 = 총수익률_임시 != "" ? (double.Parse(총수익률_임시)) : 0;

                    추정예탁자산_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "추정예탁자산").Trim();
                    추정예탁자산 = 추정예탁자산_임시 != "" ? (long.Parse(추정예탁자산_임시)) : 0;

                    총대출금_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총대출금").Trim();
                    총대출금 = 총대출금_임시 != "" ? (long.Parse(총대출금_임시)) : 0;

                    총융자금액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총융자금액").Trim();
                    총융자금액 = 총융자금액_임시 != "" ? (long.Parse(총융자금액_임시)) : 0;

                    총대주금액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총대주금액").Trim();
                    총대주금액 = 총대주금액_임시 != "" ? (long.Parse(총대주금액_임시)) : 0;

                    foreach (Form form1 in this.MdiChildren)
                    {
                        if (form1.Text == "주식잔고")
                        {
                            주식잔고_Form 주식잔고야 = (주식잔고_Form)form1;
                            주식잔고야.평가손익금액_label_ChangeForeColor(글자색_알려줘((double)총평가손익금액));
                            주식잔고야.수익률_label_ChangeForeColor(글자색_알려줘((double)총수익률));

                            주식잔고야.매입금액_label_ChangeText( String.Format("{0:#,0}", 총매입금액) );
                            주식잔고야.평가금액_label_ChangeText( String.Format("{0:#,0}", 총평가금액) );
                            주식잔고야.평가손익금액_label_ChangeText( String.Format("{0:#,0}", 총평가손익금액) );
                            주식잔고야.수익률_label_ChangeText( String.Format("{0:0.00}%", 총수익률) );
                            주식잔고야.추정예탁자산_label_ChangeText( String.Format("{0:#,0}", 추정예탁자산) );
                            주식잔고야.대출금_label_ChangeText( String.Format("{0:#,0}", 총대출금) );
                            주식잔고야.융자금액_label_ChangeText( String.Format("{0:#,0}", 총융자금액) );
                            주식잔고야.대주금액_label_ChangeText( String.Format("{0:#,0}", 총대주금액) );

                            /*
                            평가손익금액_label.ForeColor = 글자색_알려줘((double)총평가손익금액);
                            수익률_label.ForeColor = 글자색_알려줘((double)총수익률);

                            매입금액_label.Text = String.Format("{0:#,#}", 총매입금액);
                            평가금액_label.Text = String.Format("{0:#,#}", 총평가금액);
                            평가손익금액_label.Text = String.Format("{0:#,#}", 총평가손익금액);
                            수익률_label.Text = String.Format("{0:0.00}%", 총수익률);
                            추정예탁자산_label.Text = String.Format("{0:#,#}", 추정예탁자산);
                            대출금_label.Text = String.Format("{0:#,#}", 총대출금);
                            융자금액_label.Text = String.Format("{0:#,#}", 총융자금액);
                            대주금액_label.Text = String.Format("{0:#,#}", 총대주금액);
                            */
                        }
                        if (_주식종합이야 != null)
                        {
                            _주식종합이야.평가손익금액_label_ChangeForeColor(글자색_알려줘((double)총평가손익금액));
                            _주식종합이야.수익률_label_ChangeForeColor(글자색_알려줘((double)총수익률));

                            _주식종합이야.매입금액_label_ChangeText(String.Format("{0:#,0}", 총매입금액));
                            _주식종합이야.평가금액_label_ChangeText(String.Format("{0:#,0}", 총평가금액));
                            _주식종합이야.평가손익금액_label_ChangeText(String.Format("{0:#,0}", 총평가손익금액));
                            _주식종합이야.수익률_label_ChangeText(String.Format("{0:0.00}%", 총수익률));
                            _주식종합이야.추정예탁자산_label_ChangeText(String.Format("{0:#,0}", 추정예탁자산));
                            _주식종합이야.대출금_label_ChangeText(String.Format("{0:#,0}", 총대출금));
                            _주식종합이야.융자금액_label_ChangeText(String.Format("{0:#,0}", 총융자금액));
                            _주식종합이야.대주금액_label_ChangeText(String.Format("{0:#,0}", 총대주금액));
                        }
                    }
                    
                    int 멀티데이터개수 = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRQName);
                    Console.WriteLine(e.sRQName + " - 개수: " + 멀티데이터개수);
                    for (int ㄱ = 0; ㄱ < 멀티데이터개수; ㄱ++)
                    {
                        종목번호_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "종목번호").Trim();
                        종목명_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "종목명").Trim();
                        평가손익_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "평가손익").Trim();
                        수익률_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "수익률(%)").Trim();
                        매입가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "매입가").Trim();
                        전일종가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "전일종가").Trim();
                        보유수량_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "보유수량").Trim();
                        매매가능수량_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "매매가능수량").Trim();
                        현재가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "현재가").Trim();
                        전일매수수량_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "전일매수수량").Trim();
                        전일매도수량_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "전일매도수량").Trim();
                        금일매수수량_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "금일매수수량").Trim();
                        금일매도수량_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "금일매도수량").Trim();
                        매입금액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "매입금액").Trim();
                        매입수수료_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "매입수수료").Trim();
                        평가금액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "평가금액").Trim();
                        평가수수료_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "평가수수료").Trim();
                        세금_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "세금").Trim();
                        수수료합_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "수수료합").Trim();
                        보유비중_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "보유비중(%)").Trim();
                        신용구분_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "신용구분").Trim();
                        신용구분명_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "신용구분명").Trim();
                        대출일_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "대출일").Trim();

                        foreach (Form form1 in this.MdiChildren)
                        {
                            if (form1.Text == "주식잔고")
                            {
                                주식잔고_Form 주식잔고야 = (주식잔고_Form)form1;
                                주식잔고야.주식잔고_업데이트해줘(
                                    종목번호_임시
                                    , 종목명_임시
                                    , 평가손익_임시
                                    , 수익률_임시
                                    , 매입가_임시
                                    , 전일종가_임시
                                    , 보유수량_임시
                                    , 매매가능수량_임시
                                    , 현재가_임시
                                    , 전일매수수량_임시
                                    , 전일매도수량_임시
                                    , 금일매수수량_임시
                                    , 금일매도수량_임시
                                    , 매입금액_임시
                                    , 매입수수료_임시
                                    , 평가금액_임시
                                    , 평가수수료_임시
                                    , 세금_임시
                                    , 수수료합_임시
                                    , 보유비중_임시
                                    , 신용구분_임시
                                    , 신용구분명_임시
                                    , 대출일_임시
                                    , ㄱ
                                );
                            }
                        }
                        if (
                            _주식종합이야 != null
                        )
                        {
                            _주식종합이야.보유종목_업데이트해줘(
                                    종목번호_임시
                                    , 종목명_임시
                                    , 평가손익_임시
                                    , 수익률_임시
                                    , 매입가_임시
                                    , 전일종가_임시
                                    , 보유수량_임시
                                    , 매매가능수량_임시
                                    , 현재가_임시
                                    , 전일매수수량_임시
                                    , 전일매도수량_임시
                                    , 금일매수수량_임시
                                    , 금일매도수량_임시
                                    , 매입금액_임시
                                    , 매입수수료_임시
                                    , 평가금액_임시
                                    , 평가수수료_임시
                                    , 세금_임시
                                    , 수수료합_임시
                                    , 보유비중_임시
                                    , 신용구분_임시
                                    , 신용구분명_임시
                                    , 대출일_임시
                                    , ㄱ
                                    , 멀티데이터개수
                                );
                        }
                    }
                    if (멀티데이터개수 == 0 && _주식종합이야 != null)
                    {
                        _주식종합이야.보유종목_초기화해줘();
                    }
                }
                else if (e.sRQName == "장마감후종목데이터")
                {
                    Console.WriteLine("\r\nOnReceiveTrData - 장마감후종목데이터\r\n");
                    int 멀티데이터개수 = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRQName);
                    for (int ㄱ = 0; ㄱ < 멀티데이터개수; ㄱ++)
                    {
                        종목명 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "종목명").Trim();
                        종목코드 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "종목코드").Trim();

                        Console.WriteLine(String.Format("[{0}] {1} {2}\r\n"
                            , (ㄱ + 1)
                            , 종목명
                            , 종목코드));
                        if (_시장구분 == "0")
                        {
                            _종목코드의코스피종목_사전[종목코드] = 종목명;
                        } else
                        {
                            _종목코드의코스닥종목_사전[종목코드] = 종목명;
                        }
                        종목코드_목록.Add(종목코드);
                    }

                    Console.WriteLine(String.Format("\r\n\r\n 총 {0} 개 다운받았습니다. - e.sPrevNext = {1} \r\n\r\n", 멀티데이터개수, e.sPrevNext));

                    if (Int32.Parse(e.sPrevNext) == 2) /* 다음 종목 있을 경우 */
                    {
                        if (_시장구분 == "0") /* 코스피 */
                        {
                            _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                                axKHOpenAPI1.SetInputValue("시장구분", "0");
                                axKHOpenAPI1.SetInputValue("업종코드", "001");
                                int 결과코드 = axKHOpenAPI1.CommRqData("장마감후종목데이터", "OPT20002", 2, "5000");
                                Console.WriteLine("장마감후종목데이터 - 코스피 - 다시시작 결과코드 = " + 결과코드);
                            }));
                        } else
                        {
                            _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                                axKHOpenAPI1.SetInputValue("시장구분", "1");
                                axKHOpenAPI1.SetInputValue("업종코드", "101");
                                int 결과코드 = axKHOpenAPI1.CommRqData("장마감후종목데이터", "OPT20002", 2, "5000");
                                Console.WriteLine("장마감후종목데이터 - 코스닥 - 다시시작 결과코드 = " + 결과코드);
                            }));
                        }
                    } else if (Int32.Parse(e.sPrevNext) == 1) { /* 끝난 경우 */
                        if (_시장구분 == "0") {
                            _시장구분 = "1";
                            _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                                axKHOpenAPI1.SetInputValue("시장구분", "1");
                                axKHOpenAPI1.SetInputValue("업종코드", "101");
                                int 결과코드 = axKHOpenAPI1.CommRqData("장마감후종목데이터", "OPT20002", 2, "5000");
                                Console.WriteLine("장마감후종목데이터 - 코스닥 시작 결과코드 = " + 결과코드);
                            }));
                        } else
                        {
                            Console.WriteLine(String.Format("코스피: {0}, 코스닥 {1}, 총 {2}", _종목코드의코스피종목_사전.Count, _종목코드의코스닥종목_사전.Count, (_종목코드의코스피종목_사전.Count + _종목코드의코스닥종목_사전.Count)));
                            Console.WriteLine("\r\n ========== 장마감후종목데이터 종료되었습니다. ========= \r\n");
                            /* 코스피 코스닥 종목 가져왔으므로 주식기본정보요청 */

                            List<string> 코드목록 = new List<string>();

                            foreach (string 코스피종목코드 in _종목코드의코스피종목_사전.Keys)
                            {
                                코드목록.Add(코스피종목코드);
                            }
                            foreach (string 코스닥종목코드 in _종목코드의코스닥종목_사전.Keys)
                            {
                                코드목록.Add(코스닥종목코드);
                            }

                            _키움응답주식기본정보카운트 = 0;
                            foreach (string 코스피종목코드 in _종목코드의코스피종목_사전.Keys)
                            {
                                _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                                    axKHOpenAPI1.SetInputValue("종목코드", 코스피종목코드);
                                    axKHOpenAPI1.CommRqData("장마감후단일주식기본정보", "opt10001", 0, "5000");
                                }));
                            }
                            foreach (string 코스닥종목코드 in _종목코드의코스닥종목_사전.Keys)
                            {
                                _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                                    axKHOpenAPI1.SetInputValue("종목코드", 코스닥종목코드);
                                    axKHOpenAPI1.CommRqData("장마감후단일주식기본정보", "opt10001", 0, "5000");
                                }));
                            }
                        }
                    }
                }
                else if (e.sRQName == "장마감후단일주식기본정보" || e.sRQName == "누락된단일주식기본정보") {
                    async Task 종목Upsert_하자()
                    {
                        _키움응답주식기본정보카운트++;
                        종목 종목아 = new 종목();
                        try {
                            종목아._종목코드 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim();

                            종목아._종목명 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "종목명").Trim();

                            결산월_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "결산월").Trim();
                            종목아._결산월 = 결산월_임시 != "" ? (Int32.Parse(결산월_임시)) : 0;

                            액면가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "액면가").Trim();
                            종목아._액면가 = 액면가_임시 != "" ? (double.Parse(액면가_임시)) : 0;

                            자본금_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "자본금").Trim();
                            종목아._자본금 = 자본금_임시 != "" ? (long.Parse(자본금_임시)) : 0;

                            상장주식_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "상장주식").Trim();
                            종목아._상장주식 = 상장주식_임시 != "" ? (long.Parse(상장주식_임시)) : 0;

                            신용비율_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "신용비율").Trim();
                            종목아._신용비율 = 신용비율_임시 != "" ? (double.Parse(신용비율_임시)) : 0;

                            연중최고_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "연중최고").Trim();
                            종목아._연중최고 = 연중최고_임시 != "" ? (Math.Abs(Int32.Parse(연중최고_임시))) : 0;

                            연중최저_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "연중최저").Trim();
                            종목아._연중최저 = 연중최저_임시 != "" ? (Math.Abs(Int32.Parse(연중최저_임시))) : 0;

                            시가총액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "시가총액").Trim();
                            종목아._시가총액 = 시가총액_임시 != "" ? (long.Parse(시가총액_임시)) : 0;

                            종목아._시가총액비중 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "시가총액비중").Trim();

                            외인소진률_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "외인소진률").Trim();
                            종목아._외인소진률 = 외인소진률_임시 != "" ? (double.Parse(외인소진률_임시)) : 0;

                            대용가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "대용가").Trim();
                            종목아._대용가 = 대용가_임시 != "" ? (Int32.Parse(대용가_임시)) : 0;

                            PER_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "PER").Trim();
                            종목아._PER = PER_임시 != "" ? (double.Parse(PER_임시)) : 0;

                            EPS_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "EPS").Trim();
                            종목아._EPS = EPS_임시 != "" ? (double.Parse(EPS_임시)) : 0;

                            ROE_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "ROE").Trim();
                            종목아._ROE = ROE_임시 != "" ? (double.Parse(ROE_임시)) : 0;

                            PBR_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "PBR").Trim();
                            종목아._PBR = PBR_임시 != "" ? (double.Parse(PBR_임시)) : 0;

                            EV_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "EV").Trim();
                            종목아._EV = EV_임시 != "" ? (double.Parse(EV_임시)) : 0;

                            BPS_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "BPS").Trim();
                            종목아._BPS = BPS_임시 != "" ? (double.Parse(BPS_임시)) : 0;

                            매출액_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매출액").Trim();
                            종목아._매출액 = 매출액_임시 != "" ? (long.Parse(매출액_임시)) : 0;

                            영업이익_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "영업이익").Trim();
                            종목아._영업이익 = 영업이익_임시 != "" ? (long.Parse(영업이익_임시)) : 0;

                            당기순이익_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "당기순이익").Trim();
                            종목아._당기순이익 = 당기순이익_임시 != "" ? (long.Parse(당기순이익_임시)) : 0;

                            _250최고_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "250최고").Trim();
                            종목아._250최고 = _250최고_임시 != "" ? (Math.Abs(Int32.Parse(_250최고_임시))) : 0;

                            _250최저_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "250최저").Trim();
                            종목아._250최저 = _250최저_임시 != "" ? (Math.Abs(Int32.Parse(_250최저_임시))) : 0;

                            시가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "시가").Trim();
                            종목아._시가 = 시가_임시 != "" ? (Math.Abs(Int32.Parse(시가_임시))) : 0;

                            고가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "고가").Trim();
                            종목아._고가 = 고가_임시 != "" ? (Math.Abs(Int32.Parse(고가_임시))) : 0;

                            저가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "저가").Trim();
                            종목아._저가 = 저가_임시 != "" ? (Math.Abs(Int32.Parse(저가_임시))) : 0;

                            상한가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "상한가").Trim();
                            종목아._상한가 = 상한가_임시 != "" ? (Math.Abs(Int32.Parse(상한가_임시))) : 0;

                            하한가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "하한가").Trim();
                            종목아._하한가 = 하한가_임시 != "" ? (Math.Abs(Int32.Parse(하한가_임시))) : 0;

                            기준가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "기준가").Trim();
                            종목아._기준가 = 기준가_임시 != "" ? (Math.Abs(Int32.Parse(기준가_임시))) : 0;

                            예상체결가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "예상체결가").Trim();
                            종목아._예상체결가 = 예상체결가_임시 != "" ? (Math.Abs(Int32.Parse(예상체결가_임시))) : 0;

                            예상체결수량_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "예상체결수량").Trim();
                            종목아._예상체결수량 = 예상체결수량_임시 != "" ? (long.Parse(예상체결수량_임시)) : 0;

                            _250최고가일_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "250최고가일").Trim();
                            종목아._250최고가일 = _250최고가일_임시 != "" ? (Int32.Parse(_250최고가일_임시)) : 0;

                            _250최고가대비율_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "250최고가대비율").Trim();
                            종목아._250최고가대비율 = _250최고가대비율_임시 != "" ? (double.Parse(_250최고가대비율_임시)) : 0;

                            _250최저가일_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "250최저가일").Trim();
                            종목아._250최저가일 = _250최저가일_임시 != "" ? (Int32.Parse(_250최저가일_임시)) : 0;

                            _250최저가대비율_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "250최저가대비율").Trim();
                            종목아._250최저가대비율 = _250최저가대비율_임시 != "" ? (double.Parse(_250최저가대비율_임시)) : 0;

                            현재가_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "현재가").Trim();
                            종목아._현재가 = 현재가_임시 != "" ? (Math.Abs(Int32.Parse(현재가_임시))) : 0;

                            대비기호_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "대비기호").Trim();
                            종목아._대비기호 = 대비기호_임시 != "" ? (Int32.Parse(대비기호_임시)) : 0;

                            전일대비_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "전일대비").Trim();
                            종목아._전일대비 = 전일대비_임시 != "" ? (Int32.Parse(전일대비_임시)) : 0;

                            등락율_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "등락율").Trim();
                            종목아._등락율 = 등락율_임시 != "" ? (double.Parse(등락율_임시)) : 0;

                            거래량_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "거래량").Trim();
                            종목아._거래량 = 거래량_임시 != "" ? (long.Parse(거래량_임시)) : 0;

                            거래대비_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "거래대비").Trim();
                            종목아._거래대비 = 거래대비_임시 != "" ? (double.Parse(거래대비_임시)) : 0;

                            종목아._액면가단위 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "액면가단위").Trim();

                            if (_종목코드의코스피종목_사전.ContainsKey(종목아._종목코드))
                            {
                                종목아._시장 = 0;
                            }
                            else
                            {
                                종목아._시장 = 1;
                            }

                            종목아._업종_목록 = new List<string>();
                            종목아._테마_목록 = new List<string>();
                            종목아._메모_목록 = new List<string>();

                            종목아._감시안하는이유_목록 = new List<string>();
                            if (종목아._액면가 <= 0)
                            {
                                종목아._계속감시안할까 = true;
                                종목아._감시 = false;
                                종목아._감시안하는이유_목록.Add("액면가없음");
                            }
                            else
                            {
                                종목아._계속감시안할까 = false;
                                종목아._감시 = true;
                            }
                            if (종목아._종목코드.StartsWith("9") == true)
                            {
                                종목아._계속감시안할까 = true;
                                종목아._감시 = false;
                                종목아._감시안하는이유_목록.Add("중국주임");
                            }
                            if (종목아._시가 <= 0)
                            {
                                종목아._감시 = false;
                                종목아._감시안하는이유_목록.Add("시가없음");
                            }

                            종목아._생성날짜 = _오늘의날짜;
                            종목아._수정날짜 = _오늘의날짜;

                            var builder = Builders<종목>.Filter;
                            var query = builder.Eq(x => x._종목코드, 종목아._종목코드);
                            var result = await _종목컬렉션.Find(query).Limit(1).ToListAsync();
                            종목 반환도큐먼트 = result.FirstOrDefault();
                            if (반환도큐먼트 == null)
                            {
                                _종목컬렉션.InsertOne(종목아);
                            }
                            else
                            {
                                if (반환도큐먼트._계속감시안할까 == true)
                                {
                                    종목아._감시 = false;
                                    종목아._감시안하는이유_목록.Add("계속감시안함");
                                }

                                var updateDef = Builders<종목>
                                    .Update
                                    .Set("name", 종목아._종목명)
                                    .Set("settleAccountMonth", 종목아._결산월)
                                    .Set("faceValue", 종목아._액면가)
                                    .Set("capital", 종목아._자본금)
                                    .Set("listedShares", 종목아._상장주식)
                                    .Set("creditRate", 종목아._신용비율)
                                    .Set("highestOfYear", 종목아._연중최고)
                                    .Set("lowestOfYear", 종목아._연중최저)
                                    .Set("marketCapitalization", 종목아._시가총액)
                                    .Set("marketCapitalizationWeight", 종목아._시가총액비중)
                                    .Set("foreignConsumptionRate", 종목아._외인소진률)
                                    .Set("substitutePrice", 종목아._대용가)
                                    .Set("per", 종목아._PER)
                                    .Set("eps", 종목아._EPS)
                                    .Set("roe", 종목아._ROE)
                                    .Set("pbr", 종목아._PBR)
                                    .Set("ev", 종목아._EV)
                                    .Set("bps", 종목아._BPS)
                                    .Set("sales", 종목아._매출액)
                                    .Set("businessProfit", 종목아._영업이익)
                                    .Set("netProfit", 종목아._당기순이익)
                                    .Set("highest250", 종목아._250최고)
                                    .Set("lowest250", 종목아._250최저)
                                    .Set("open", 종목아._시가)
                                    .Set("high", 종목아._고가)
                                    .Set("low", 종목아._저가)
                                    .Set("upperLimitPrice", 종목아._상한가)
                                    .Set("lowerLimitPrice", 종목아._하한가)
                                    .Set("standardPrice", 종목아._기준가)
                                    .Set("estimatedSettlementPrice", 종목아._예상체결가)
                                    .Set("estimatedSettlementVolume", 종목아._예상체결수량)
                                    .Set("highest250Date", 종목아._250최고가일)
                                    .Set("highest250ChangeRate", 종목아._250최고가대비율)
                                    .Set("lowest250Date", 종목아._250최저가일)
                                    .Set("lowest250ChangeRate", 종목아._250최저가대비율)
                                    .Set("current", 종목아._현재가)
                                    .Set("changeSymbol", 종목아._대비기호)
                                    .Set("netChange", 종목아._전일대비)
                                    .Set("netChangeRate", 종목아._등락율)
                                    .Set("volume", 종목아._거래량)
                                    .Set("volumeChangeRate", 종목아._거래대비)
                                    .Set("faceValueUnit", 종목아._액면가단위)
                                    .Set("market", 종목아._시장)
                                    .Set("monitor", 종목아._감시)
                                    .Set("notMonitorReasons", 종목아._감시안하는이유_목록)
                                    .Set("updatedAt", 종목아._수정날짜);
                                _종목컬렉션.UpdateOne(x => x._종목코드 == 반환도큐먼트._종목코드, updateDef);
                            }

                            if ((_종목코드의코스피종목_사전.Count + _종목코드의코스닥종목_사전.Count) == _키움응답주식기본정보카운트)
                            {
                                Console.WriteLine("\r\n ======== 장마감후단일주식기본정보 || 누락된단일주식기본정보 완료하였습니다. ======== \r\n");
                            }
                        }
                        catch (Exception EX)
                        {
                            Console.WriteLine(EX);
                            Console.WriteLine(String.Format("{0} = {1}", 종목아._종목코드, 종목아._종목명));
                        }
                    }
                    종목Upsert_하자();
                }
                else if (e.sRQName == "주식호가요청")
                {
                    주식호가잔량 주식호가잔량아 = new 주식호가잔량();

                    if (_주식종합이야 != null)
                    {
                        _주식종합이야.로그_찍어줘(String.Format("{0} - OnReceiveTrData - 에도 주식호가잔량 도착했습니다.", DateTime.Now));
                    }

                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "호가잔량기준시간").Trim();
                    주식호가잔량아._호가시간 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 시간_알려줘();
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도최우선호가").Trim();
                    주식호가잔량아._매도호가1 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도2차선호가").Trim();
                    주식호가잔량아._매도호가2 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도3차선호가").Trim();
                    주식호가잔량아._매도호가3 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도4차선호가").Trim();
                    주식호가잔량아._매도호가4 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도5차선호가").Trim();
                    주식호가잔량아._매도호가5 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도6차선호가").Trim();
                    주식호가잔량아._매도호가6 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도7차선호가").Trim();
                    주식호가잔량아._매도호가7 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도8차선호가").Trim();
                    주식호가잔량아._매도호가8 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도9차선호가").Trim();
                    주식호가잔량아._매도호가9 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도10차선호가").Trim();
                    주식호가잔량아._매도호가10 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수최우선호가").Trim();
                    주식호가잔량아._매수호가1 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수2차선호가").Trim();
                    주식호가잔량아._매수호가2 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수3차선호가").Trim();
                    주식호가잔량아._매수호가3 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수4차선호가").Trim();
                    주식호가잔량아._매수호가4 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수5차선호가").Trim();
                    주식호가잔량아._매수호가5 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수6우선호가").Trim();
                    주식호가잔량아._매수호가6 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수7차선호가").Trim();
                    주식호가잔량아._매수호가7 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수8차선호가").Trim();
                    주식호가잔량아._매수호가8 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수9차선호가").Trim();
                    주식호가잔량아._매수호가9 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수10차선호가").Trim();
                    주식호가잔량아._매수호가10 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도최우선잔량").Trim();
                    주식호가잔량아._매도호가수량1 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도2차선잔량").Trim();
                    주식호가잔량아._매도호가수량2 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도3차선잔량").Trim();
                    주식호가잔량아._매도호가수량3 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도4차선잔량").Trim();
                    주식호가잔량아._매도호가수량4 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도5차선잔량").Trim();
                    주식호가잔량아._매도호가수량5 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도6우선잔량").Trim();
                    주식호가잔량아._매도호가수량6 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도7차선잔량").Trim();
                    주식호가잔량아._매도호가수량7 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도8차선잔량").Trim();
                    주식호가잔량아._매도호가수량8 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도9차선잔량").Trim();
                    주식호가잔량아._매도호가수량9 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도10차선잔량").Trim();
                    주식호가잔량아._매도호가수량10 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수최우선잔량").Trim();
                    주식호가잔량아._매수호가수량1 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수2차선잔량").Trim();
                    주식호가잔량아._매수호가수량2 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수3차선잔량").Trim();
                    주식호가잔량아._매수호가수량3 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수4차선잔량").Trim();
                    주식호가잔량아._매수호가수량4 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수5차선잔량").Trim();
                    주식호가잔량아._매수호가수량5 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수6우선잔량").Trim();
                    주식호가잔량아._매수호가수량6 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수7차선잔량").Trim();
                    주식호가잔량아._매수호가수량7 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수8차선잔량").Trim();
                    주식호가잔량아._매수호가수량8 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수9차선잔량").Trim();
                    주식호가잔량아._매수호가수량9 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수10차선잔량").Trim();
                    주식호가잔량아._매수호가수량10 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도1차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비1 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도2차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비2 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도3차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비3 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도4차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비4 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도5차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비5 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도6차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비6 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도7차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비7 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도8차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비8 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도9차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비9 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매도10차선잔량대비").Trim();
                    주식호가잔량아._매도호가직전대비10 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수1차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비1 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수2차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비2 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수3차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비3 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수4차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비4 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수5차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비5 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수6차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비6 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수7차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비7 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수8차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비8 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수9차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비9 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "매수10차선잔량대비").Trim();
                    주식호가잔량아._매수호가직전대비10 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총매도잔량").Trim();
                    주식호가잔량아._매도호가총잔량 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총매수잔량").Trim();
                    주식호가잔량아._매수호가총잔량 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총매도잔량직전대비").Trim();
                    주식호가잔량아._매도호가총잔량직전대비 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                    문자_임시 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, 0, "총매수잔량직전대비").Trim();
                    주식호가잔량아._매수호가총잔량직전대비 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                    if (_주식종합이야 != null)
                    {
                        _주식종합이야.주식호가_업데이트해줘(0, 주식호가잔량아);
                    }
                    /*
                    주식호가잔량아._순매도잔량 = axKHOpenAPI1.GetCommRealData(e.sRealType, 138).Trim();
                    주식호가잔량아._순매수잔량 = axKHOpenAPI1.GetCommRealData(e.sRealType, 128).Trim();
                    
                    주식호가잔량아._매도비율 = axKHOpenAPI1.GetCommRealData(e.sRealType, 139).Trim();
                    주식호가잔량아._매수비율 = axKHOpenAPI1.GetCommRealData(e.sRealType, 129).Trim();

                    주식호가잔량아._누적거래량 = axKHOpenAPI1.GetCommRealData(e.sRealType, 13).Trim();

                    주식호가잔량아._전일거래량대비예상체결률 = axKHOpenAPI1.GetCommRealData(e.sRealType, 299).Trim();

                    주식호가잔량아._장운영구분 = axKHOpenAPI1.GetCommRealData(e.sRealType, 215).Trim();

                    주식호가잔량아._투자자별ticker = axKHOpenAPI1.GetCommRealData(e.sRealType, 216).Trim();
                    */
                }
            }
            catch (Exception EX)
            {
                Console.WriteLine(String.Format("{0} 오류 잡음. OnReceiveTrData", EX));
            }
        }

        private void OnReceiveMsg(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            Console.WriteLine("\r\n\r\n=================== " + DateTime.Now +  " ===================\r\n");
            Console.WriteLine("===================OnReceiveMsg() - e.sMsg : " + e.sMsg.Trim() + "================= \r\n");
            Console.WriteLine("===================OnReceiveMsg() - e.sRQName : " + e.sRQName.Trim() + "================= \r\n");
            Console.WriteLine("===================OnReceiveMsg() - e.sScrNo : " + e.sScrNo.Trim() + "================= \r\n");
            Console.WriteLine("===================OnReceiveMsg() - e.sTrCode : " + e.sTrCode.Trim() + "================= \r\n");
        }

        /* 체결, 접수, 잔고 결과 전달 Event */
        /* 주문 요청 후 주문 접수, 체결 통보, 잔고 통보를 수신할 때마다 호출됨. GetChejanData 함수 사용해 상세 정보 획득 가능. */
        private void OnReceiveChejanData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            int itemCnt = e.nItemCnt;
            string fidList = e.sFIdList;
            string gubun = e.sGubun; /* 0: 체결시, 1: 잔고전달, 4: 파생잔고 전달 */
            string 문자_임시 = "";
            string 로그내용;

            if (e.sGubun.Equals("0"))
            {
                주문체결 주문체결이야 = new 주문체결();
                주문체결이야._계좌번호 = axKHOpenAPI1.GetChejanData(9201).Trim();
                주문체결이야._주문상태 = axKHOpenAPI1.GetChejanData(913).Trim();
                주문체결이야._주문번호 = axKHOpenAPI1.GetChejanData(9203).Trim();
                주문체결이야._관리자사번 = axKHOpenAPI1.GetChejanData(9205).Trim();
                주문체결이야._종목코드 = axKHOpenAPI1.GetChejanData(9001).Trim();
                주문체결이야._주문업무분류 = axKHOpenAPI1.GetChejanData(912).Trim();
                주문체결이야._주문상태 = axKHOpenAPI1.GetChejanData(913).Trim();
                주문체결이야._종목명 = axKHOpenAPI1.GetChejanData(302).Trim();

                문자_임시 = axKHOpenAPI1.GetChejanData(900).Trim();
                주문체결이야._주문수량 = 문자_임시 != "" ? long.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(901).Trim();
                주문체결이야._주문가격 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(902).Trim();
                주문체결이야._미체결수량 = 문자_임시 != "" ? long.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(903).Trim();
                주문체결이야._체결누계금액 = 문자_임시 != "" ? long.Parse(문자_임시) : 0;

                주문체결이야._원주문번호 = axKHOpenAPI1.GetChejanData(904).Trim();
                주문체결이야._주문구분 = axKHOpenAPI1.GetChejanData(905).Trim();
                주문체결이야._매매구분 = axKHOpenAPI1.GetChejanData(906).Trim();
                주문체결이야._매도수구분 = axKHOpenAPI1.GetChejanData(907).Trim();

                문자_임시 = axKHOpenAPI1.GetChejanData(908).Trim();
                주문체결이야._주문체결시간 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                주문체결이야._체결번호 = axKHOpenAPI1.GetChejanData(909).Trim();

                문자_임시 = axKHOpenAPI1.GetChejanData(910).Trim();
                주문체결이야._체결가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(911).Trim();
                주문체결이야._체결량 = 문자_임시 != "" ? long.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(10).Trim();
                주문체결이야._현재가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(27).Trim();
                주문체결이야._최우선매도호가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(28).Trim();
                주문체결이야._최우선매수호가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(914).Trim();
                주문체결이야._단위체결가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(915).Trim();
                주문체결이야._단위체결량 = 문자_임시 != "" ? long.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(938).Trim();
                주문체결이야._당일매매수수료 = 문자_임시 != "" ? double.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(939).Trim();
                주문체결이야._당일매매세금 = 문자_임시 != "" ? double.Parse(문자_임시) : 0;

                주문체결이야._거부사유 = axKHOpenAPI1.GetChejanData(919).Trim();

                문자_임시 = axKHOpenAPI1.GetChejanData(920).Trim();
                주문체결이야._화면번호 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(921).Trim();
                주문체결이야._터미널번호 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                주문체결이야._신용구분 = axKHOpenAPI1.GetChejanData(922).Trim();

                주문체결이야._대출일 = axKHOpenAPI1.GetChejanData(923).Trim();

                주문체결이야._기타_목록 = new List<string>();
                주문체결이야._기타_목록.Add(axKHOpenAPI1.GetChejanData(949).Trim());
                주문체결이야._기타_목록.Add(axKHOpenAPI1.GetChejanData(10010).Trim());

                주문체결이야._등록시간 = 시간_알려줘();

                주문체결이야._생성날짜 = 날짜_알려줘();

                if (주문체결이야._주문상태 == "접수")
                {

                }
                else if (주문체결이야._주문상태 == "체결")
                {

                }


                로그내용 = String.Format(
                    "계좌번호: {0}, " +
                    "주문번호: {1}, " +
                    "관리자사번: {2}, " +
                    "종목코드: {3}, " +
                    "주문업무분류: {4}, " +
                    "주문상태: {5}, " +
                    "종목명: {6}, " +
                    "주문수량: {7}, " +
                    "주문가격: {8}, " +
                    "미체결수량: {9}, " +
                    "체결누계금액: {10}, " +
                    "원주문번호: {11}, " +
                    "주문구분: {12}, " +
                    "매매구분: {13}, " +
                    "매도수구분: {14}, " +
                    "주문체결시간: {15}, " +
                    "체결번호: {16}, " +
                    "체결가: {17}, " +
                    "체결량: {18}, " +
                    "현재가: {19}, " +
                    "최우선매도호가: {20}, " +
                    "최우선매수호가: {21}, " +
                    "단위체결가: {22}, " +
                    "단위체결량: {23}, " +
                    "당일매매수수료: {24}, " +
                    "당일매매세금: {25}, " +
                    "거부사유: {26}, " +
                    "화면번호: {27}, " +
                    "터미널번호: {28}, " +
                    "신용구분: {29}, " +
                    "대출일: {30}, " +
                    "기타_목록[0]: {31}, " +
                    "기타_목록[1]: {32}, " +
                    "등록시간: {33}, " +
                    "생성날짜: {34}, "

                    , 주문체결이야._계좌번호
                    , 주문체결이야._주문번호
                    , 주문체결이야._관리자사번
                    , 주문체결이야._종목코드
                    , 주문체결이야._주문업무분류
                    , 주문체결이야._주문상태
                    , 주문체결이야._종목명
                    , 주문체결이야._주문수량
                    , 주문체결이야._주문가격
                    , 주문체결이야._미체결수량
                    , 주문체결이야._체결누계금액
                    , 주문체결이야._원주문번호
                    , 주문체결이야._주문구분
                    , 주문체결이야._매매구분
                    , 주문체결이야._매도수구분
                    , 주문체결이야._주문체결시간
                    , 주문체결이야._체결번호
                    , 주문체결이야._체결가
                    , 주문체결이야._체결량
                    , 주문체결이야._현재가
                    , 주문체결이야._최우선매도호가
                    , 주문체결이야._최우선매수호가
                    , 주문체결이야._단위체결가
                    , 주문체결이야._단위체결량
                    , 주문체결이야._당일매매수수료
                    , 주문체결이야._당일매매세금
                    , 주문체결이야._거부사유
                    , 주문체결이야._화면번호
                    , 주문체결이야._터미널번호
                    , 주문체결이야._신용구분
                    , 주문체결이야._대출일
                    , 주문체결이야._기타_목록[0]
                    , 주문체결이야._기타_목록[1]
                    , 주문체결이야._등록시간
                    , 주문체결이야._생성날짜
                    );

                if (_주식종합이야 != null)
                {
                    _주식종합이야.로그_찍어줘(로그내용);
                }
            }
            else if (e.sGubun.Equals("1")) /* 잔고전달 */
            {
                잔고 잔고야 = new 잔고();
                잔고야._계좌번호 = axKHOpenAPI1.GetChejanData(9201).Trim();

                잔고야._종목코드 = axKHOpenAPI1.GetChejanData(9001).Trim();

                잔고야._신용구분 = axKHOpenAPI1.GetChejanData(917).Trim();

                잔고야._대출일 = axKHOpenAPI1.GetChejanData(916).Trim();

                잔고야._종목명 = axKHOpenAPI1.GetChejanData(302).Trim();

                문자_임시 = axKHOpenAPI1.GetChejanData(10).Trim();
                잔고야._현재가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(930).Trim();
                잔고야._보유수량 = 문자_임시 != "" ? long.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(931).Trim();
                잔고야._매입단가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(932).Trim();
                잔고야._총매입가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(933).Trim();
                잔고야._주문가능수량 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(945).Trim();
                잔고야._당일순매수량 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                잔고야._매도매수구분 = axKHOpenAPI1.GetChejanData(946).Trim();

                잔고야._당일총매도손일 = axKHOpenAPI1.GetChejanData(950).Trim();

                문자_임시 = axKHOpenAPI1.GetChejanData(951).Trim();
                잔고야._예수금 = 문자_임시 != "" ? long.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(27).Trim();
                잔고야._최우선매도호가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(28).Trim();
                잔고야._최우선매수호가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(307).Trim();
                잔고야._기준가 = 문자_임시 != "" ? Int32.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(8019).Trim();
                잔고야._손익율 = 문자_임시 != "" ? double.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(957).Trim();
                잔고야._신용금액 = 문자_임시 != "" ? double.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(958).Trim();
                잔고야._신용이자 = 문자_임시 != "" ? double.Parse(문자_임시) : 0;

                잔고야._만기일 = axKHOpenAPI1.GetChejanData(918).Trim();

                잔고야._당일실현손익유가 = axKHOpenAPI1.GetChejanData(990).Trim();

                문자_임시 = axKHOpenAPI1.GetChejanData(991).Trim();
                잔고야._당일실현손익률유가 = 문자_임시 != "" ? double.Parse(문자_임시) : 0;

                잔고야._당일실현손익신용 = axKHOpenAPI1.GetChejanData(992).Trim();

                문자_임시 = axKHOpenAPI1.GetChejanData(993).Trim();
                잔고야._당일실현손익률신용 = 문자_임시 != "" ? double.Parse(문자_임시) : 0;

                문자_임시 = axKHOpenAPI1.GetChejanData(959).Trim();
                잔고야._담보대출수량 = 문자_임시 != "" ? long.Parse(문자_임시) : 0;

                잔고야._ExtraItem = axKHOpenAPI1.GetChejanData(924).Trim();

                잔고야._기타_목록 = new List<string>();
                잔고야._기타_목록.Add(axKHOpenAPI1.GetChejanData(10010).Trim());
                잔고야._기타_목록.Add(axKHOpenAPI1.GetChejanData(25).Trim());
                잔고야._기타_목록.Add(axKHOpenAPI1.GetChejanData(11).Trim());
                잔고야._기타_목록.Add(axKHOpenAPI1.GetChejanData(12).Trim());
                잔고야._기타_목록.Add(axKHOpenAPI1.GetChejanData(306).Trim());

                잔고야._등록시간 = 시간_알려줘();

                잔고야._생성날짜 = 날짜_알려줘();

                로그내용 = String.Format(
                    "계좌번호: {0}, " +
                    "종목코드: {1}, " +
                    "신용구분: {2}, " +
                    "대출일: {3}, " +
                    "종목명: {4}, " +
                    "현재가: {5}, " +
                    "보유수량: {6}, " +
                    "매입단가: {7}, " +
                    "총매입가: {8}, " +
                    "주문가능수량: {9}, " +
                    "당일순매수량: {10}, " +
                    "매도매수구분: {11}, " +
                    "당일총매도손일: {12}, " +
                    "예수금: {13}, " +
                    "최우선매도호가: {14}, " +
                    "최우선매수호가: {15}, " +
                    "기준가: {16}, " +
                    "손익율: {17}, " +
                    "신용금액: {18}, " +
                    "신용이자: {19}, " +
                    "만기일: {20}, " +
                    "당일실현손익유가: {21}, " +
                    "당일실현손익률유가: {22}, " +
                    "당일실현손익신용: {23}, " +
                    "당일실현손익률신용: {24}, " +
                    "담보대출수량: {25}, " +
                    "ExtraItem: {26}, " +
                    "기타_목록[0]: {27}, " +
                    "기타_목록[1]: {28}, " +
                    "기타_목록[2]: {29}, " +
                    "기타_목록[3]: {30}, " +
                    "기타_목록[4]: {31}, " +
                    "등록시간: {32}, " +
                    "생성날짜: {33}, "

                    , 잔고야._계좌번호
                    , 잔고야._종목코드
                    , 잔고야._신용구분
                    , 잔고야._대출일
                    , 잔고야._종목명
                    , 잔고야._현재가
                    , 잔고야._보유수량
                    , 잔고야._매입단가
                    , 잔고야._총매입가
                    , 잔고야._주문가능수량
                    , 잔고야._당일순매수량
                    , 잔고야._매도매수구분
                    , 잔고야._당일총매도손일
                    , 잔고야._예수금
                    , 잔고야._최우선매도호가
                    , 잔고야._최우선매수호가
                    , 잔고야._기준가
                    , 잔고야._손익율
                    , 잔고야._신용금액
                    , 잔고야._신용이자
                    , 잔고야._만기일
                    , 잔고야._당일실현손익유가
                    , 잔고야._당일실현손익률유가
                    , 잔고야._당일실현손익신용
                    , 잔고야._당일실현손익률신용
                    , 잔고야._담보대출수량
                    , 잔고야._ExtraItem
                    , 잔고야._기타_목록[0]
                    , 잔고야._기타_목록[1]
                    , 잔고야._기타_목록[2]
                    , 잔고야._기타_목록[3]
                    , 잔고야._기타_목록[4]
                    , 잔고야._등록시간
                    , 잔고야._생성날짜
                    );

                if (_주식종합이야 != null)
                {
                    _주식종합이야.로그_찍어줘(로그내용);
                }
            }
            else if (e.sGubun.Equals("4"))
            {
                if (_주식종합이야 != null)
                {

                }
            }

            Console.WriteLine("\r\n\r\n=================== " + DateTime.Now + " ===================\r\n");
            Console.WriteLine("=================== OnReceiveChejanData() - e.nItemCnt : " + e.nItemCnt + " ================= \r\n");
            Console.WriteLine("=================== OnReceiveChejanData() - e.sFIdList : " + e.sFIdList.Trim() + " ================= \r\n");
            Console.WriteLine("=================== OnReceiveChejanData() - e.sGubun : " + e.sGubun.Trim() + " ================= \r\n");
        }

        private void OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            /* 키움증권 변수 */
            string _250최고_임시 = "";
            string _250최고가대비율_임시 = "";
            string _250최고가일_임시 = "";
            string _250최저_임시 = "";
            string _250최저가대비율_임시 = "";
            string _250최저가일_임시 = "";
            string BPS_임시 = "";
            string EPS_임시 = "";
            string EV_임시 = "";
            string KO접근도_임시 = "";
            string PBR_임시 = "";
            string PER_임시 = "";
            string ROE_임시 = "";
            string 감시_임시 = "";
            string 감시안하는이유_임시 = "";
            string 거래대금_임시 = "";
            string 거래대금증감_임시 = "";
            string 거래대비_임시 = "";
            string 거래량_임시 = "";
            string 거래비용_임시 = "";
            string 거래회전율_임시 = "";
            string 결산월_임시 = "";
            string 고가_임시 = "";
            string 기준가_임시 = "";
            string 날짜_임시 = "";
            string 누적거래대금_임시 = "";
            string 누적거래량_임시 = "";
            string 당기순이익_임시 = "";
            string 대비기호_임시 = "";
            string 대용가_임시 = "";
            string 등락율_임시 = "";
            string 매수했니_임시 = "";
            string 매출액_임시 = "";
            string 상장주식_임시 = "";
            string 상한가_임시 = "";
            string 상한가발생시간_임시 = "";
            string 시가_임시 = "";
            string 시가총액_임시 = "";
            string 시가총액비중_임시 = "";
            string 시가총액_억_임시 = "";
            string 시장_임시 = "";
            string 신용비율_임시 = "";
            string 액면가_임시 = "";
            string 액면가단위_임시 = "";
            string 연중최고_임시 = "";
            string 연중최저_임시 = "";
            string 영업이익_임시 = "";
            string 예상체결가_임시 = "";
            string 예상체결수량_임시 = "";
            string 외인소진률_임시 = "";
            string 자본금_임시 = "";
            string 장구분_임시 = "";
            string 저가_임시 = "";
            string 전일거래량대비_계약주_임시 = "";
            string 전일거래량대비_비율_임시 = "";
            string 전일대비_임시 = "";
            string 전일대비기호_임시 = "";
            string 종목명_임시 = "";
            string 종목코드_임시 = "";
            string 체결강도_임시 = "";
            string 체결시간_임시 = "";
            string 총대주금액_임시 = "";
            string 총대출금_임시 = "";
            string 총매입금액_임시 = "";
            string 총수익률_임시 = "";
            string 총융자금액_임시 = "";
            string 총평가금액_임시 = "";
            string 총평가손익금액_임시 = "";
            string 최우선매도호가_임시 = "";
            string 최우선매수호가_임시 = "";
            string 추정예탁자산_임시 = "";
            string 평균가_임시 = "";
            string 하한가_임시 = "";
            string 하한가발생시간_임시 = "";
            string 현재가_임시 = "";

            int _250최고 = 0;
            double _250최고가대비율 = 0;
            int _250최고가일 = 0;
            int _250최저 = 0;
            double _250최저가대비율 = 0;
            int _250최저가일 = 0;
            double BPS = 0;
            double EPS = 0;
            double EV = 0;
            int KO접근도 = 0;
            double PBR = 0;
            double PER = 0;
            double ROE = 0;
            bool 감시 = true;
            string 감시안하는이유 = "";
            long 거래대금 = 0;
            long 거래대금증감 = 0;
            double 거래대비 = 0;
            long 거래량 = 0;
            long 거래비용 = 0;
            double 거래회전율 = 0;
            int 결산월 = 0;
            int 고가 = 0;
            int 기준가 = 0;
            int 날짜 = 날짜_알려줘();
            long 누적거래대금 = 0;
            long 누적거래량 = 0;
            long 당기순이익 = 0;
            int 대비기호 = 0;
            int 대용가 = 0;
            double 등락율 = 0;
            bool 매수했니 = true;
            long 매출액 = 0;
            long 상장주식 = 0;
            int 상한가 = 0;
            int 상한가발생시간 = 0;
            int 시가 = 0;
            long 시가총액 = 0;
            string 시가총액비중 = "";
            long 시가총액_억 = 0;
            int 시장 = 0;
            double 신용비율 = 0;
            double 액면가 = 0;
            string 액면가단위 = "";
            int 연중최고 = 0;
            int 연중최저 = 0;
            long 영업이익 = 0;
            int 예상체결가 = 0;
            long 예상체결수량 = 0;
            double 외인소진률 = 0;
            long 자본금 = 0;
            int 장구분 = 0;
            int 저가 = 0;
            long 전일거래량대비_계약주 = 0;
            double 전일거래량대비_비율 = 0;
            int 전일대비 = 0;
            int 전일대비기호 = 0;
            string 종목명 = "";
            string 종목코드 = "";
            double 체결강도 = 0;
            int 체결시간 = 0;
            long 총대주금액 = 0;
            long 총대출금 = 0;
            long 총매입금액 = 0;
            double 총수익률 = 0;
            long 총융자금액 = 0;
            long 총평가금액 = 0;
            long 총평가손익금액 = 0;
            int 최우선매도호가 = 0;
            int 최우선매수호가 = 0;
            long 추정예탁자산 = 0;
            int 평균가 = 0;
            int 하한가 = 0;
            int 하한가발생시간 = 0;
            int 현재가 = 0;

            string 문자_임시 = "";

            주식체결 주식체결아;
            부자주식체결 부자주식체결아;
            주식호가잔량 주식호가잔량아;
            
            try
            {
                if (e.sRealType == "주식체결")
                {
                    주식체결아 = new 주식체결();
                    주식체결아._종목코드 = e.sRealKey.Trim();

                    체결시간_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 20).Trim();
                    주식체결아._체결시간 = 체결시간_임시 != "" ? Int32.Parse(체결시간_임시) : 0;

                    주식체결아._등록시간 = 시간_알려줘();
                    주식체결아._순서 = _순서++;

                    현재가_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 10).Trim();
                    주식체결아._현재가 = 현재가_임시 != "" ? (Math.Abs(Int32.Parse(현재가_임시))) : 0;

                    전일대비_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 11).Trim();
                    주식체결아._전일대비 = 전일대비_임시 != "" ? (Int32.Parse(전일대비_임시)) : 0;

                    등락율_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 12).Trim();
                    주식체결아._등락율 = 등락율_임시 != "" ? (double.Parse(등락율_임시)) : 0;

                    최우선매도호가_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 27).Trim();
                    주식체결아._최우선매도호가 = 최우선매도호가_임시 != "" ? (Math.Abs(Int32.Parse(최우선매도호가_임시))) : 0;

                    최우선매수호가_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 28).Trim();
                    주식체결아._최우선매수호가 = 최우선매수호가_임시 != "" ? (Math.Abs(Int32.Parse(최우선매수호가_임시))) : 0;

                    거래량_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 15).Trim();
                    주식체결아._거래량 = 거래량_임시 != "" ? (Int32.Parse(거래량_임시)) : 0;

                    주식체결아._매수했니 = 주식체결아._거래량 > 0;
                    주식체결아._거래량 = Math.Abs(주식체결아._거래량);

                    누적거래량_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 13).Trim();
                    주식체결아._누적거래량 = 누적거래량_임시 != "" ? (Int32.Parse(누적거래량_임시)) : 0;

                    누적거래대금_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 14).Trim();
                    주식체결아._누적거래대금 = 누적거래대금_임시 != "" ? (Int32.Parse(누적거래대금_임시)) : 0;

                    시가_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 16).Trim();
                    주식체결아._시가 = 시가_임시 != "" ? (Math.Abs(Int32.Parse(시가_임시))) : 0;

                    고가_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 17).Trim();
                    주식체결아._고가 = 고가_임시 != "" ? (Math.Abs(Int32.Parse(고가_임시))) : 0;

                    저가_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 18).Trim();
                    주식체결아._저가 = 저가_임시 != "" ? (Math.Abs(Int32.Parse(저가_임시))) : 0;

                    전일대비기호_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 25).Trim();
                    주식체결아._전일대비기호 = 전일대비기호_임시 != "" ? (Int32.Parse(전일대비기호_임시)) : 0;

                    전일거래량대비_계약주_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 26).Trim();
                    주식체결아._전일거래량대비_계약주 = 전일거래량대비_계약주_임시 != "" ? (long.Parse(전일거래량대비_계약주_임시)) : 0;

                    거래대금증감_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 29).Trim();
                    주식체결아._거래대금증감 = 거래대금증감_임시 != "" ? (long.Parse(거래대금증감_임시)) : 0;

                    전일거래량대비_비율_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 30).Trim();
                    주식체결아._전일거래량대비_비율 = 전일거래량대비_비율_임시 != "" ? (double.Parse(전일거래량대비_비율_임시)) : 0;

                    거래회전율_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 31).Trim();
                    주식체결아._거래회전율 = 거래회전율_임시 != "" ? (double.Parse(거래회전율_임시)) : 0;

                    거래비용_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 32).Trim();
                    주식체결아._거래비용 = 거래비용_임시 != "" ? (Int32.Parse(거래비용_임시)) : 0;

                    체결강도_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 228).Trim();
                    주식체결아._체결강도 = 체결강도_임시 != "" ? (double.Parse(체결강도_임시)) : 0;

                    시가총액_억_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 311).Trim();
                    주식체결아._시가총액_억 = 시가총액_억_임시 != "" ? (Int32.Parse(시가총액_억_임시)) : 0;

                    장구분_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 290).Trim();
                    주식체결아._장구분 = 장구분_임시 != "" ? (Int32.Parse(장구분_임시)) : 0;

                    KO접근도_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 691).Trim();
                    주식체결아._KO접근도 = KO접근도_임시 != "" ? (Int32.Parse(KO접근도_임시)) : 0;

                    상한가발생시간_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 567).Trim();
                    주식체결아._상한가발생시간 = 상한가발생시간_임시 != "" ? (Int32.Parse(상한가발생시간_임시)) : 0;

                    하한가발생시간_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 568).Trim();
                    주식체결아._하한가발생시간 = 하한가발생시간_임시 != "" ? (Int32.Parse(하한가발생시간_임시)) : 0;

                    주식체결아._거래대금 = 주식체결아._현재가 * 주식체결아._거래량;

                    if (주식체결아._누적거래량 != 0)
                    {
                        주식체결아._평균가 = (int)((주식체결아._누적거래대금 * 1000000) / 주식체결아._누적거래량);
                    } else
                    {
                        주식체결아._평균가 = 0;
                    }
                    주식체결아._생성날짜 = 날짜_알려줘();
                    _주식체결컬렉션.InsertOne(주식체결아);
                    
                    if (주식체결아._거래대금 >= 100000000)
                    {
                        부자주식체결아 = new 부자주식체결();
                        부자주식체결아._종목코드 = 주식체결아._종목코드;
                        부자주식체결아._체결시간 = 주식체결아._체결시간;
                        부자주식체결아._등록시간 = 주식체결아._등록시간;
                        부자주식체결아._순서 = 주식체결아._순서;
                        부자주식체결아._현재가 = 주식체결아._현재가;
                        부자주식체결아._등락율 = 주식체결아._등락율;
                        부자주식체결아._최우선매도호가 = 주식체결아._최우선매도호가;
                        부자주식체결아._최우선매수호가 = 주식체결아._최우선매수호가;
                        부자주식체결아._거래량 = 주식체결아._거래량;
                        부자주식체결아._거래대금 = 주식체결아._거래대금;
                        부자주식체결아._시가 = 주식체결아._시가;
                        부자주식체결아._고가 = 주식체결아._고가;
                        부자주식체결아._저가 = 주식체결아._저가;
                        부자주식체결아._매수했니 = 주식체결아._매수했니;
                        부자주식체결아._생성날짜 = 주식체결아._생성날짜;
                        _부자주식체결컬렉션.InsertOne(부자주식체결아);
                    }

                    if (
                        _주식종합이야 != null &&
                        _주식종합이야.추천_혹은_보유종목이니(주식체결아._종목코드) == true
                        )
                    {
                        _주식종합이야.주식체결_전송해줘(주식체결아);
                    }
                }
                else if (e.sRealType == "주식호가잔량") {
                    Console.WriteLine(String.Format("{0} = 종목코드: {1}, 주식호가잔량\r\n", DateTime.Now, e.sRealKey.Trim()));
                    종목코드 = e.sRealKey.Trim();

                    if (_주식종합이야 != null)
                    {
                        //_주식종합이야.로그_찍어줘(String.Format("{0} - OnReceiveRealData - 주식호가잔량 - 종목코드 = {1}", DateTime.Now, 종목코드));
                    }

                    if (
                        _주식종합이야 != null &&
                        _주식종합이야.선택된종목코드_알려줘() == 종목코드
                    )
                    {
                        주식호가잔량아 = new 주식호가잔량();
                        주식호가잔량아._종목코드 = e.sRealKey.Trim();

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 21).Trim(); ;
                        주식호가잔량아._호가시간 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 시간_알려줘();

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 41).Trim();
                        주식호가잔량아._매도호가1 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 42).Trim();
                        주식호가잔량아._매도호가2 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 43).Trim();
                        주식호가잔량아._매도호가3 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 44).Trim();
                        주식호가잔량아._매도호가4 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 45).Trim();
                        주식호가잔량아._매도호가5 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 46).Trim();
                        주식호가잔량아._매도호가6 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 47).Trim();
                        주식호가잔량아._매도호가7 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 48).Trim();
                        주식호가잔량아._매도호가8 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 49).Trim();
                        주식호가잔량아._매도호가9 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 50).Trim();
                        주식호가잔량아._매도호가10 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 51).Trim();
                        주식호가잔량아._매수호가1 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 52).Trim();
                        주식호가잔량아._매수호가2 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 53).Trim();
                        주식호가잔량아._매수호가3 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 54).Trim();
                        주식호가잔량아._매수호가4 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 55).Trim();
                        주식호가잔량아._매수호가5 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 56).Trim();
                        주식호가잔량아._매수호가6 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 57).Trim();
                        주식호가잔량아._매수호가7 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 58).Trim();
                        주식호가잔량아._매수호가8 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 59).Trim();
                        주식호가잔량아._매수호가9 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 60).Trim();
                        주식호가잔량아._매수호가10 = 문자_임시 != "" ? (Int32.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 61).Trim();
                        주식호가잔량아._매도호가수량1 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 62).Trim();
                        주식호가잔량아._매도호가수량2 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 63).Trim();
                        주식호가잔량아._매도호가수량3 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 64).Trim();
                        주식호가잔량아._매도호가수량4 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 65).Trim();
                        주식호가잔량아._매도호가수량5 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 66).Trim();
                        주식호가잔량아._매도호가수량6 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 67).Trim();
                        주식호가잔량아._매도호가수량7 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 68).Trim();
                        주식호가잔량아._매도호가수량8 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 69).Trim();
                        주식호가잔량아._매도호가수량9 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 70).Trim();
                        주식호가잔량아._매도호가수량10 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 71).Trim();
                        주식호가잔량아._매수호가수량1 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 72).Trim();
                        주식호가잔량아._매수호가수량2 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 73).Trim();
                        주식호가잔량아._매수호가수량3 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 74).Trim();
                        주식호가잔량아._매수호가수량4 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 75).Trim();
                        주식호가잔량아._매수호가수량5 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 76).Trim();
                        주식호가잔량아._매수호가수량6 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 77).Trim();
                        주식호가잔량아._매수호가수량7 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 78).Trim();
                        주식호가잔량아._매수호가수량8 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 79).Trim();
                        주식호가잔량아._매수호가수량9 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 80).Trim();
                        주식호가잔량아._매수호가수량10 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 81).Trim();
                        주식호가잔량아._매도호가직전대비1 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 82).Trim();
                        주식호가잔량아._매도호가직전대비2 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 83).Trim();
                        주식호가잔량아._매도호가직전대비3 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 84).Trim();
                        주식호가잔량아._매도호가직전대비4 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 85).Trim();
                        주식호가잔량아._매도호가직전대비5 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 86).Trim();
                        주식호가잔량아._매도호가직전대비6 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 87).Trim();
                        주식호가잔량아._매도호가직전대비7 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 88).Trim();
                        주식호가잔량아._매도호가직전대비8 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 89).Trim();
                        주식호가잔량아._매도호가직전대비9 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 90).Trim();
                        주식호가잔량아._매도호가직전대비10 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 91).Trim();
                        주식호가잔량아._매수호가직전대비1 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 92).Trim();
                        주식호가잔량아._매수호가직전대비2 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 93).Trim();
                        주식호가잔량아._매수호가직전대비3 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 94).Trim();
                        주식호가잔량아._매수호가직전대비4 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 95).Trim();
                        주식호가잔량아._매수호가직전대비5 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 96).Trim();
                        주식호가잔량아._매수호가직전대비6 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 97).Trim();
                        주식호가잔량아._매수호가직전대비7 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 98).Trim();
                        주식호가잔량아._매수호가직전대비8 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 99).Trim();
                        주식호가잔량아._매수호가직전대비9 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 100).Trim();
                        주식호가잔량아._매수호가직전대비10 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;
                        
                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 121).Trim();
                        주식호가잔량아._매도호가총잔량 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 125).Trim();
                        주식호가잔량아._매수호가총잔량 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 122).Trim();
                        주식호가잔량아._매도호가총잔량직전대비 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        문자_임시 = axKHOpenAPI1.GetCommRealData(e.sRealType, 126).Trim();
                        주식호가잔량아._매수호가총잔량직전대비 = 문자_임시 != "" ? (long.Parse(문자_임시)) : 0;

                        if (_주식종합이야 != null)
                        {
                            _주식종합이야.주식호가_업데이트해줘(1, 주식호가잔량아);
                        }
                    }

                    /*
                    
                    주식호가잔량아._종목코드 = e.sRealKey.Trim();

                    

                    주식호가잔량아._순매도잔량 = axKHOpenAPI1.GetCommRealData(e.sRealType, 138).Trim();
                    주식호가잔량아._순매수잔량 = axKHOpenAPI1.GetCommRealData(e.sRealType, 128).Trim();

                    주식호가잔량아._매도비율 = axKHOpenAPI1.GetCommRealData(e.sRealType, 139).Trim();
                    주식호가잔량아._매수비율 = axKHOpenAPI1.GetCommRealData(e.sRealType, 129).Trim();

                    주식호가잔량아._누적거래량 = axKHOpenAPI1.GetCommRealData(e.sRealType, 13).Trim();

                    주식호가잔량아._전일거래량대비예상체결률 = axKHOpenAPI1.GetCommRealData(e.sRealType, 299).Trim();

                    주식호가잔량아._장운영구분 = axKHOpenAPI1.GetCommRealData(e.sRealType, 215).Trim();

                    주식호가잔량아._투자자별ticker = axKHOpenAPI1.GetCommRealData(e.sRealType, 216).Trim();
                    */
                    /*
                    var builder = Builders<주식호가잔량>.Filter;
                    var query = builder.Eq(x => x._종목코드, 주식호가잔량아._종목코드);
                    var updateDef = Builders<주식호가잔량>.Update
                        .Set("code", 주식호가잔량아._종목코드)
                        .Set("time", 주식호가잔량아._호가시간)
                        .Set("offer1", 주식호가잔량아._매도호가1)
                        .Set("offer2", 주식호가잔량아._매도호가2)
                        .Set("offer3", 주식호가잔량아._매도호가3)
                        .Set("offer4", 주식호가잔량아._매도호가4)
                        .Set("offer5", 주식호가잔량아._매도호가5)
                        .Set("offer6", 주식호가잔량아._매도호가6)
                        .Set("offer7", 주식호가잔량아._매도호가7)
                        .Set("offer8", 주식호가잔량아._매도호가8)
                        .Set("offer9", 주식호가잔량아._매도호가9)
                        .Set("offer10", 주식호가잔량아._매도호가10)
                        .Set("bid1", 주식호가잔량아._매수호가1)
                        .Set("bid2", 주식호가잔량아._매수호가2)
                        .Set("bid3", 주식호가잔량아._매수호가3)
                        .Set("bid4", 주식호가잔량아._매수호가4)
                        .Set("bid5", 주식호가잔량아._매수호가5)
                        .Set("bid6", 주식호가잔량아._매수호가6)
                        .Set("bid7", 주식호가잔량아._매수호가7)
                        .Set("bid8", 주식호가잔량아._매수호가8)
                        .Set("bid9", 주식호가잔량아._매수호가9)
                        .Set("bid10", 주식호가잔량아._매수호가10)
                        .Set("offerQuantity1", 주식호가잔량아._매도호가수량1)
                        .Set("offerQuantity2", 주식호가잔량아._매도호가수량2)
                        .Set("offerQuantity3", 주식호가잔량아._매도호가수량3)
                        .Set("offerQuantity4", 주식호가잔량아._매도호가수량4)
                        .Set("offerQuantity5", 주식호가잔량아._매도호가수량5)
                        .Set("offerQuantity6", 주식호가잔량아._매도호가수량6)
                        .Set("offerQuantity7", 주식호가잔량아._매도호가수량7)
                        .Set("offerQuantity8", 주식호가잔량아._매도호가수량8)
                        .Set("offerQuantity9", 주식호가잔량아._매도호가수량9)
                        .Set("offerQuantity10", 주식호가잔량아._매도호가수량10)
                        .Set("bidQuantity1", 주식호가잔량아._매수호가수량1)
                        .Set("bidQuantity2", 주식호가잔량아._매수호가수량2)
                        .Set("bidQuantity3", 주식호가잔량아._매수호가수량3)
                        .Set("bidQuantity4", 주식호가잔량아._매수호가수량4)
                        .Set("bidQuantity5", 주식호가잔량아._매수호가수량5)
                        .Set("bidQuantity6", 주식호가잔량아._매수호가수량6)
                        .Set("bidQuantity7", 주식호가잔량아._매수호가수량7)
                        .Set("bidQuantity8", 주식호가잔량아._매수호가수량8)
                        .Set("bidQuantity9", 주식호가잔량아._매수호가수량9)
                        .Set("bidQuantity10", 주식호가잔량아._매수호가수량10)
                        .Set("offerChange1", 주식호가잔량아._매도호가직전대비1)
                        .Set("offerChange2", 주식호가잔량아._매도호가직전대비2)
                        .Set("offerChange3", 주식호가잔량아._매도호가직전대비3)
                        .Set("offerChange4", 주식호가잔량아._매도호가직전대비4)
                        .Set("offerChange5", 주식호가잔량아._매도호가직전대비5)
                        .Set("offerChange6", 주식호가잔량아._매도호가직전대비6)
                        .Set("offerChange7", 주식호가잔량아._매도호가직전대비7)
                        .Set("offerChange8", 주식호가잔량아._매도호가직전대비8)
                        .Set("offerChange9", 주식호가잔량아._매도호가직전대비9)
                        .Set("offerChange10", 주식호가잔량아._매도호가직전대비10)
                        .Set("bidChange1", 주식호가잔량아._매수호가직전대비1)
                        .Set("bidChange2", 주식호가잔량아._매수호가직전대비2)
                        .Set("bidChange3", 주식호가잔량아._매수호가직전대비3)
                        .Set("bidChange4", 주식호가잔량아._매수호가직전대비4)
                        .Set("bidChange5", 주식호가잔량아._매수호가직전대비5)
                        .Set("bidChange6", 주식호가잔량아._매수호가직전대비6)
                        .Set("bidChange7", 주식호가잔량아._매수호가직전대비7)
                        .Set("bidChange8", 주식호가잔량아._매수호가직전대비8)
                        .Set("bidChange9", 주식호가잔량아._매수호가직전대비9)
                        .Set("bidChange10", 주식호가잔량아._매수호가직전대비10)
                        .Set("offerTotalQuantity", 주식호가잔량아._매도호가총잔량)
                        .Set("bidTotalQuantity", 주식호가잔량아._매수호가총잔량)
                        .Set("offerTotalQuantityChange", 주식호가잔량아._매도호가총잔량직전대비)
                        .Set("bidTotalQuantityChange", 주식호가잔량아._매수호가총잔량직전대비)
                        .Set("netOfferQuantity", 주식호가잔량아._순매도잔량)
                        .Set("netBidQuantity", 주식호가잔량아._순매수잔량)
                        .Set("offerRate", 주식호가잔량아._매도비율)
                        .Set("bidRate", 주식호가잔량아._매수비율)
                        .Set("cumulativeVolume", 주식호가잔량아._누적거래량)
                        .Set("volumeChangeEstimatedSettlementRate", 주식호가잔량아._전일거래량대비예상체결률)
                        .Set("timeType", 주식호가잔량아._장운영구분)
                        .Set("tickerPerInvestor", 주식호가잔량아._투자자별ticker);
                        //_주식호가잔량컬렉션.UpdateOne(x => x._종목코드 == 주식호가잔량아._종목코드, updateDef, new UpdateOptions() { IsUpsert = true }); 
                    */
                }
                else
                {
                }
            } catch (Exception EX)
            {
                Console.WriteLine("{0} 오류 잡음. - OnReceiveRealData", EX);
            }
            if (_8시삼십분지났니 == false)
            {
                Console.WriteLine("\r\n\r\n===================OnReceiveRealData() - e.sRealType: " + e.sRealType.Trim() + "================= \r\n");
                Console.WriteLine("===================OnReceiveRealData() - e.sRealKey: " + e.sRealKey.Trim() + "================= \r\n");
                Console.WriteLine("===================OnReceiveRealData() - e.sRealData: " + e.sRealData.Trim() + "================= \r\n");
                if (시간_알려줘() > 83059)
                {
                    _8시삼십분지났니 = true;
                }
            }
        }

        // 로그인 후 시작
        public void 시작하자()
        {
            _종목코드의주식체결목록_사전 = new Dictionary<string, List<주식체결>>();
            _요청데이터관리자야.시작하자(axKHOpenAPI1);
            _종목코드의코스피종목_사전 = new Dictionary<string, string>();
            _종목코드의코스닥종목_사전 = new Dictionary<string, string>();
            _스크린번호의실시간종목_사전 = new Dictionary<string, string>();
            /*_요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                axKHOpenAPI1.SetRealRemove("All", "All");
                Console.WriteLine("실시간 시세해지하였습니다.\r\n\r\n");
            }));*/
            /* _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => { 보유계좌_목록_가져와줘(); })); */
            _오늘의날짜 = 날짜_알려줘();
        }

        /*
        public void 보유계좌_목록_가져와줘 ()
        {
            List<string> 보유계좌_목록 = 목록_만들어줘(axKHOpenAPI1.GetLoginInfo("ACCLIST"));
            this.Invoke(new Action(delegate() {
                foreach (string 보유계좌 in 보유계좌_목록)
                {
                    계좌번호_comboBox.Items.Add(보유계좌);
                }
                계좌번호_comboBox.SelectedItem = 보유계좌_목록[0];
            }));
        }
        */

        /**** 커스텀 메서드 ****/
        /* 프로그레스 바 */
        public void 진행상황_시작해줘()
        {
            _진행상황현재값 = 0;
            진행상황_progressBar.Value = 0;
            진행상황_timer.Enabled = true;
        }

        public void 진행상황_끝내줘()
        {
            _진행상황현재값 = _진행상황최대값;
            진행상황_progressBar.Value = _진행상황최대값;
            진행상황_timer.Enabled = false;
        }

        /* 효과음 플레이 */
        public void 효과음_들려줘(string 명령어)
        {
            string 효과음;
            if (명령어 == "환영")
            {
                효과음 = @"D:\\soundPlayer\\start2.wav";
            }
            else if (명령어 == "작업끝") {
                효과음 = @"D:\\soundPlayer\\trumpet.wav";
            }
            else if (명령어 == "손절")
            {
                효과음 = @"D:\\soundPlayer\\minus.wav";
            }
            else if (명령어 == "익절")
            {
                효과음 = @"D:\\soundPlayer\\plus.wav";
            }
            else if (명령어 == "매수추천")
            {
                효과음 = @"D:\\soundPlayer\\letsgo.wav";
            }
            else if (명령어 == "매수접수")
            {
                효과음 = @"D:\\soundPlayer\\bidRequest.wav";
            }
            else if (명령어 == "매수체결")
            {
                효과음 = @"D:\\soundPlayer\\bidAccept.wav";
            }
            else if (명령어 == "매도접수")
            {
                효과음 = @"D:\\soundPlayer\\offerRequest.wav";
            }
            else if (명령어 == "매도체결")
            {
                효과음 = @"D:\\soundPlayer\\offerAccept.wav";
            }
            else if (
                명령어 == "정정접수" ||
                명령어 == "취소접수"
            )
            {
                효과음 = @"D:\\soundPlayer\\orderRequest.wav";
            }
            else
            {
                효과음 = @"D:\\soundPlayer\\button.wav";
            }

            _사운드플레이어 = new SoundPlayer(효과음);
            _사운드플레이어.Play();
        }

        /*
         * 로그인 정보
          "ACCOUNT_CNT" : 보유계좌 수를 반환합니다.
          "ACCLIST" 또는 "ACCNO" : 구분자 ';'로 연결된 보유계좌 목록을 반환합니다.
          "USER_ID" : 사용자 ID를 반환합니다.
          "USER_NAME" : 사용자 명을 반환합니다.
          "KEY_BSECGB" : 키보드 보안 해지여부를 반환합니다.(0 : 정상, 1 : 해지)
          "FIREW_SECGB" : 방화벽 설정여부를 반환합니다.(0 : 미설정, 1 : 설정, 2 : 해지)
          "GetServerGubun" : 접속서버 구분을 반환합니다.(1 : 모의투자, 나머지 : 실서버)
         */
        public void 로그인정보_알려줘()
        {
            string 보유계좌개수 = axKHOpenAPI1.GetLoginInfo("ACCOUNT_CNT");
            List<string> 보유계좌_목록 = 목록_만들어줘(axKHOpenAPI1.GetLoginInfo("ACCLIST"));
            string 사용자아이디 = axKHOpenAPI1.GetLoginInfo("USER_ID");
            string 사용자이름 = axKHOpenAPI1.GetLoginInfo("USER_NAME");
            string 키보드보안 = axKHOpenAPI1.GetLoginInfo("KEY_BSECGB");
            string 방화벽 = axKHOpenAPI1.GetLoginInfo("FIREW_SECGB");
            string 서버 = axKHOpenAPI1.GetLoginInfo("GetServerGubun");
            로그인정보_Form 로그인정보야 = new 로그인정보_Form(보유계좌개수, 보유계좌_목록, 사용자아이디, 사용자이름, 키보드보안, 방화벽, 서버);
            //로그인정보야.Owner = this;
            로그인정보야.MdiParent = this;
            로그인정보야.FormClosed += new FormClosedEventHandler(창_닫혔어);
            로그인정보야.Text = "로그인정보";
            로그인정보야.Show();
        }

        public void 창_닫혔어(object sender, FormClosedEventArgs e)
        {
            Form 닫힌창 = (Form)sender;
            if (닫힌창.Text == "당일매수추천종목목록")
            {
                _당일매수추천종목목록이야.실시간스레드_멈춰줘();
                _당일매수추천종목목록이야 = null;
            } else if (닫힌창.Text == "당일주식체결") {
                _당일주식체결이야.실시간스레드_멈춰줘();
                _당일주식체결이야 = null;
            }
            else if (닫힌창.Text == "A주식체결")
            {
                _A주식체결이야.실시간스레드_멈춰줘();
                _A주식체결이야 = null;
                if (_당일매수추천종목목록이야 != null)
                {
                    _당일매수추천종목목록이야.당일주식체결_Form_저장해줘_A(null);
                }
            }
            else if (닫힌창.Text == "B주식체결")
            {
                _B주식체결이야.실시간스레드_멈춰줘();
                _B주식체결이야 = null;
                if (_당일매수추천종목목록이야 != null)
                {
                    _당일매수추천종목목록이야.당일주식체결_Form_저장해줘_B(null);
                }
            }
            else if (닫힌창.Text == "주식종합")
            {
                _주식종합이야.실시간스레드_멈춰줘();
                _주식종합이야 = null;
            }
            //Console.WriteLine("창_닫혔어 - Text = " + 닫힌창.Text);
        }

        /* string을 ;로 parsing하여 List로 반환 */
        public List<string> 목록_만들어줘(string 코드) {
            List<string> 목록 = 코드.Split(';').ToList();
            if (목록.Count <= 0)
            {
                return 목록;
            }
            int 마지막인덱스 = 목록.Count - 1;
            if (목록[마지막인덱스] == "") {
                목록.RemoveAt(목록.Count - 1);
            }
            return 목록;
        }

        /* 화면명을 입력하세요 Placeholder  */
        private void 화면명을입력하세요_panel_Enter(object sender, EventArgs e)
        {
            화면명을입력하세요_textBox.Text = "";
        }
        private void 화면명을입력하세요_panel_Leave(object sender, EventArgs e)
        {
            if (화면명을입력하세요_textBox.Text == "")
            {
                화면명을입력하세요_textBox.Text = "화면명을 입력하세요";
            }
        }

        /* 진행상황 타이머 */
        private void 진행상황_timer_Tick(object sender, EventArgs e)
        {
            if (_진행상황현재값 < _진행상황최대값)
            {
                _진행상황현재값++;
            }
            else
            {
                _진행상황현재값 = 0;
            }
            진행상황_progressBar.Value = _진행상황현재값;
        }

        private int 날짜_알려줘 ()
        {
            DateTime date = DateTime.Now;
            int 연도 = date.Year;
            int 월 = date.Month;
            int 일 = date.Day;
            string 날짜 = String.Format("{0}{1}{2}", 연도, 월 < 10 ? ("0" + 월) : ("" + 월), (일 < 10 ? ("0" + 일) : ("" + 일)));
            return Int32.Parse(날짜);
        }

        private int 시간_알려줘 ()
        {
            DateTime date = DateTime.Now;
            int 시 = date.Hour;
            int 분 = date.Minute;
            int 초 = date.Second;
            string 시간 = String.Format("{0}{1}{2}", 시, (분 < 10 ? ("0" + 분) : ("" + 분)), (초 < 10 ? ("0" + 초) : ("" + 초)));
            return Int32.Parse(시간);
        }

        /* 시계 */
        private void 시계_timer_Tick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            int 연도 = date.Year;
            string 월;
            string 일;
            string 요일;
            int 시 = date.Hour;
            int 분 = date.Minute;
            int 초 = date.Second;
            StringBuilder 문장 = new StringBuilder();

            if (date.Month < 10)
            {
                월 = "0" + date.Month;
            } else
            {
                월 = "" + date.Month;
            }

            if (date.Day < 10)
            {
                일 = "0" + date.Day;
            }
            else
            {
                일 = "" + date.Day;
            }

            switch ((int)date.DayOfWeek)
            {
                case 0:
                    요일 = "일";
                    break;
                case 1:
                    요일 = "월";
                    break;
                case 2:
                    요일 = "화";
                    break;
                case 3:
                    요일 = "수";
                    break;
                case 4:
                    요일 = "목";
                    break;
                case 5:
                    요일 = "금";
                    break;
                case 6:
                    요일 = "토";
                    break;
                default:
                    요일 = "??";
                    break;
            }

            문장.Append(String.Format("{0}년 {1}월 {2}일 {3}요일 ", 연도, 월, 일, 요일));

            if (시 < 12)
            {
                문장.Append("오전 ");
            } else
            {
                문장.Append("오후 ");
                시 -= 12;
            }

            if (시 < 10)
            {
                문장.Append("0" + 시);
            }
            else
            {
                문장.Append(시);
            }
            문장.Append(":");
            if (분 < 10)
            {
                문장.Append("0" + 분);
            }
            else
            {
                문장.Append(분);
            }
            문장.Append(":");
            if (초 < 10)
            {
                문장.Append("0" + 초);
            }
            else
            {
                문장.Append(초);
            }

            시계_label.Text = 문장.ToString();
        }

        /* 키보드 키 감지 이벤트 = ShortCut, 단축키 */
        private void 빌리언스탁_Form_KeyDown(object sender, KeyEventArgs e)
        {
            //e.KeyboardDevice.Modifiers;
            /* 주식종합 */
            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.B && _주식종합이야 != null)
            {
                _주식종합이야.매수하자();
            }
            else if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.S && _주식종합이야 != null)
            {
                _주식종합이야.매도하자();
            }
            else if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.E && _주식종합이야 != null)
            {
                _주식종합이야.정정하자();
            }
            else if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.C && _주식종합이야 != null)
            {
                _주식종합이야.취소하자();
            }

            // Ctrl + Q
            /*
            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }
            */
        }

        /**** 메뉴 ****/
        /* 파일 - 종료 */
        private void 종료_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /* 사용자 - 로그인 */
        private void 로그인_해줘() {
            if (로그인하세요_label.Text == "로그인하세요") {
                효과음_들려줘("클릭");
                진행상황_시작해줘();
                axKHOpenAPI1.CommConnect();
            }
        }
        private void 로그인_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            로그인_해줘();
        }
        private void 로그인하세요_label_Click(object sender, EventArgs e)
        {
            로그인_해줘();
        }
        /* 사용자 - 로그인 정보 */
        private void 로그인정보_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            효과음_들려줘("클릭");
            진행상황_시작해줘();
            long state = axKHOpenAPI1.GetConnectState();
            if (state == 0)
            {
                로그인하세요_label.Text = "로그인하세요";
                MessageBox.Show(String.Format("로그인하지 않은 상태입니다.\n로그인하세요."), "로그인 정보", MessageBoxButtons.OK);
            }
            else
            {
                로그인정보_알려줘();
            }
            진행상황_끝내줘();
        }
        /* 로그인 상태 */
        private void 로그인하세요_label_TextChanged(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.TopMost = false;
        }
        private void OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            진행상황_끝내줘();
            if (e.nErrCode != 0)
            {
                로그인하세요_label.Text = "로그인하세요";
                if (_오류코드의오류메시지_사전.ContainsKey(e.nErrCode))
                {
                    if (MessageBox.Show(String.Format("로그인 실패하였습니다.\n원인: {0}\n\n안전을 위해 프로그램을 종료합니다.", _오류코드의오류메시지_사전[e.nErrCode]), "로그인 실패", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    if (MessageBox.Show(String.Format("로그인 실패하였습니다.\n원인: 알수없음\n\n안전을 위해 프로그램을 종료합니다."), "로그인 실패", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }
            }
            else
            {
                if (axKHOpenAPI1.GetLoginInfo("GetServerGubun") == "1")
                {
                    로그인하세요_label.Text = "모의투자 접속중";
                }
                else
                {
                    로그인하세요_label.Text = "실서버 접속중";
                }
                시작하자();
            }
        }
        
        private Color 글자색_알려줘(double 값)
        {
            if (값 > 0)
            {
                return System.Drawing.Color.Firebrick;
            } else if (값 < 0)
            {
                return System.Drawing.Color.RoyalBlue;
            } else
            {
                return System.Drawing.Color.Chartreuse;
            }
        }

        private void 빌리언스탁_Form_Load(object sender, EventArgs e)
        {
            MdiClient ctlMDI;
            foreach (Control ctl in this.Controls) {
                try {
                    //MdiClient type인지 알아내기위해 컨트롤을 MdiClient 로 타입캐스팅 합니다.
                    ctlMDI = (MdiClient) ctl; // MdiClient type일 경우 배경색을 위에서 설정한 폼의 배경색으로 변경합니다.
                    ctlMDI.BackColor = this.BackColor;
                } catch (InvalidCastException exc) {
                    // 타입캐스팅 에러를 잡아내고 이를 무시합니다.
                }
            }
            // 자식폼을 화면에 표시합니다.
            //Form2 frm = new Form2();
            //frm.MdiParent = this;
            //frm.Show();
        }

        private void 장마감후종목데이터저장_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            if (date.Hour >= 16 || date.Hour <= 13)
            {
                _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                    axKHOpenAPI1.SetRealRemove("All", "All");
                    Console.WriteLine("실시간 시세해지하였습니다.\r\n\r\n");
                }));
                _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                    axKHOpenAPI1.SetInputValue("시장구분", "0");
                    axKHOpenAPI1.SetInputValue("업종코드", "001");
                    int 결과코드 = axKHOpenAPI1.CommRqData("장마감후종목데이터", "OPT20002", 0, "5000");
                    Console.WriteLine("장마감후종목데이터 - 코스피 시작 결과코드 = " + 결과코드);
                }));
            } else
            {
                MessageBox.Show(String.Format("장마감후종목데이터 다운로드는 오후 4시부터 아침 6시 이전에 시도해주세요."), "장마감후종목데이터 다운로드 실패", MessageBoxButtons.OK);
            }
        }

        private void 실시간데이터등록초기화_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long state = axKHOpenAPI1.GetConnectState();
            if (state == 0)
            {
                로그인하세요_label.Text = "로그인하세요";
                MessageBox.Show(String.Format("로그인하지 않은 상태입니다.\n로그인하세요."), "로그인 정보", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    실시간매수추천_시작해줘();
                    var builder = Builders<종목>.Filter;
                    var query = builder.Eq(x => x._감시, true);
                    var list = _종목컬렉션.Find(query).ToList();
                    List<string> 종목코드_목록 = new List<string>();
                    List<string> FID_목록 = new List<string>();
                    foreach (종목 종목아 in list)
                    {
                        종목코드_목록.Add(종목아._종목코드);
                    }

                    string 호가FID = "";
                    for (int i = 41; i <= 100; i++)
                    {
                        호가FID += (";" + i);
                    }
                    _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                        axKHOpenAPI1.SetRealRemove("All", "All");
                        Console.WriteLine("실시간 시세해지하였습니다.\r\n\r\n");
                    }));

                    string 종목코드정리 = "";
                    int 카운트 = 0;
                    int 카운트100회수 = 0;
                    for (int ㄴ = 0; ㄴ < 종목코드_목록.Count; ㄴ++)
                    {
                        카운트++;
                        if (카운트 == 1)
                        {
                            종목코드정리 = 종목코드_목록[ㄴ];
                        }
                        else if (카운트 > 1 && 카운트 <= 100)
                        {
                            종목코드정리 = 종목코드정리 + ";" + 종목코드_목록[ㄴ];
                        }
                        if (카운트 == 100)
                        {
                            카운트 = 0;
                            카운트100회수++;
                            _스크린번호의실시간종목_사전[스크린번호_알려줘()] = 종목코드정리;
                        }
                    }
                    for (int ㄷ = ((카운트100회수 * 100) - 1); ㄷ < 종목코드_목록.Count; ㄷ++)
                    {
                        if (ㄷ == ((카운트100회수 * 100) - 1))
                        {
                            종목코드정리 = 종목코드_목록[ㄷ];
                        }
                        else
                        {
                            종목코드정리 = 종목코드정리 + ";" + 종목코드_목록[ㄷ];
                        }
                    }
                    _스크린번호의실시간종목_사전[스크린번호_알려줘()] = 종목코드정리;
                    foreach (string 스크린번호 in _스크린번호의실시간종목_사전.Keys)
                    {
                        _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                            Console.WriteLine(String.Format("_스크린번호의실시간종목 스크린번호 : {0}\r\n종목코드\r\n{1}\r\n\r\n", 스크린번호, _스크린번호의실시간종목_사전[스크린번호]));
                            axKHOpenAPI1.SetRealReg(스크린번호, _스크린번호의실시간종목_사전[스크린번호], "20;10;11;12;27;28;15;13;14;16;17;18;25;26;29;30;31;32;228;311;290;691;567;568", "1");
                        }));
                    }
                    /* , ("20;10;11;12;27;28;15;13;14;16;17;18;25;26;29;30;31;32;228;311;290;691;567;568" + ";21" + 호가FID + ";121;122;125;126;128;129;138;139;215;216;299") */
                }
                catch (Exception EX)
                {
                    Console.WriteLine("실시간데이터등록초기화 = 오류발생 = EX: {0}", EX);
                }
            }
        }

        private void 당일종목순위데이터저장_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /* 메뉴 클릭 이벤트 */
        private void 트랜잭션_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender.Equals(주식기본정보_opt10001_ToolStripMenuItem)) {
                Console.WriteLine("주식기본정보_opt10001_ToolStripMenuItem");
            }
            else if (sender.Equals(주식거래원_opt10002_ToolStripMenuItem))
            {
                Console.WriteLine("주식거래원_opt10002_ToolStripMenuItem");
            }
            else if (sender.Equals(체결정보_opt10003_ToolStripMenuItem))
            {
                Console.WriteLine("체결정보_opt10003_ToolStripMenuItem");
            }
            else if (sender.Equals(주식호가_opt10004_ToolStripMenuItem))
            {
                Console.WriteLine("주식호가_opt10004_ToolStripMenuItem");
            }
            else if (sender.Equals(주식일주월시분_opt10005_ToolStripMenuItem))
            {
                Console.WriteLine("주식일주월시분_opt10005_ToolStripMenuItem");
            }
            else if (sender.Equals(주식시분_OPT10006_ToolStripMenuItem))
            {
                Console.WriteLine("주식시분_OPT10006_ToolStripMenuItem");
            }
            else if (sender.Equals(시세표성정보_opt10007_ToolStripMenuItem))
            {
                Console.WriteLine("시세표성정보_opt10007_ToolStripMenuItem");
            }
            else if (sender.Equals(주식외국인_opt10008_ToolStripMenuItem))
            {
                Console.WriteLine("주식외국인_opt10008_ToolStripMenuItem");
            }
            else if (sender.Equals(주식기관_OPT10009_ToolStripMenuItem))
            {
                Console.WriteLine("주식기관_OPT10009_ToolStripMenuItem");
            }
            else if (sender.Equals(업종프로그램_OPT10010_ToolStripMenuItem))
            {
                Console.WriteLine("업종프로그램_OPT10010_ToolStripMenuItem");
            }
            else if (sender.Equals(주문체결_opt10012_ToolStripMenuItem))
            {
                Console.WriteLine("주문체결_opt10012_ToolStripMenuItem");
            }
            else if (sender.Equals(신용매매동향_opt10013_ToolStripMenuItem))
            {
                Console.WriteLine("신용매매동향_opt10013_ToolStripMenuItem");
            }
            else if (sender.Equals(공매도추이_opt10014_ToolStripMenuItem))
            {
                Console.WriteLine("공매도추이_opt10014_ToolStripMenuItem");
            }
            else if (sender.Equals(일별거래상세_opt10015_ToolStripMenuItem))
            {
                Console.WriteLine("일별거래상세_opt10015_ToolStripMenuItem");
            }
            else if (sender.Equals(신고저가_OPT10016_ToolStripMenuItem))
            {
                Console.WriteLine("신고저가_OPT10016_ToolStripMenuItem");
            }
            else if (sender.Equals(상하한가_opt10017_ToolStripMenuItem))
            {
                Console.WriteLine("상하한가_opt10017_ToolStripMenuItem");
            }
            else if (sender.Equals(고저가근접_OPT10018_ToolStripMenuItem))
            {
                Console.WriteLine("고저가근접_OPT10018_ToolStripMenuItem");
            }
            else if (sender.Equals(가격급등락_opt10019_ToolStripMenuItem))
            {
                Console.WriteLine("가격급등락_opt10019_ToolStripMenuItem");
            }
            else if (sender.Equals(호가잔량상위_OPT10020_ToolStripMenuItem))
            {
                Console.WriteLine("호가잔량상위_OPT10020_ToolStripMenuItem");
            }
            else if (sender.Equals(호가잔량급증_OPT10021_ToolStripMenuItem))
            {
                Console.WriteLine("호가잔량급증_OPT10021_ToolStripMenuItem");
            }
            else if (sender.Equals(잔량율급증_OPT10022_ToolStripMenuItem))
            {
                Console.WriteLine("잔량율급증_OPT10022_ToolStripMenuItem");
            }
            else if (sender.Equals(거래량급증_OPT10023_ToolStripMenuItem))
            {
                Console.WriteLine("거래량급증_OPT10023_ToolStripMenuItem");
            }
            else if (sender.Equals(거래량갱신_OPT10024_ToolStripMenuItem))
            {
                Console.WriteLine("거래량갱신_OPT10024_ToolStripMenuItem");
            }
            else if (sender.Equals(매물대집중_OPT10025_ToolStripMenuItem))
            {
                Console.WriteLine("매물대집중_OPT10025_ToolStripMenuItem");
            }
            else if (sender.Equals(고저PER_opt10026_ToolStripMenuItem))
            {
                Console.WriteLine("고저PER_opt10026_ToolStripMenuItem");
            }
            else if (sender.Equals(전일대비등락률상위_opt10027_ToolStripMenuItem))
            {
                Console.WriteLine("전일대비등락률상위_opt10027_ToolStripMenuItem");
            }
            else if (sender.Equals(시가대비등락률_opt10028_ToolStripMenuItem))
            {
                Console.WriteLine("시가대비등락률_opt10028_ToolStripMenuItem");
            }
            else if (sender.Equals(예상체결등락률상위_OPT10029_ToolStripMenuItem))
            {
                Console.WriteLine("예상체결등락률상위_OPT10029_ToolStripMenuItem");
            }
            else if (sender.Equals(당일거래량상위_OPT10030_ToolStripMenuItem))
            {
                Console.WriteLine("당일거래량상위_OPT10030_ToolStripMenuItem");
                당일거래량상위_OPT10030 당일거래량상위_OPT10030이야 = new 당일거래량상위_OPT10030();
                당일거래량상위_OPT10030이야.MdiParent = this;
                당일거래량상위_OPT10030이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                당일거래량상위_OPT10030이야.Text = "당일거래량상위_OPT10030";
                당일거래량상위_OPT10030이야.Show();
            }
            else if (sender.Equals(전일거래량상위_OPT10031_ToolStripMenuItem))
            {
                Console.WriteLine("전일거래량상위_OPT10031_ToolStripMenuItem");
            }
            else if (sender.Equals(거래대금상위_OPT10032_ToolStripMenuItem))
            {
                Console.WriteLine("거래대금상위_OPT10032_ToolStripMenuItem");
            }
            else if (sender.Equals(신용비율상위_opt10033_ToolStripMenuItem))
            {
                Console.WriteLine("신용비율상위_opt10033_ToolStripMenuItem");
            }
            else if (sender.Equals(외인기간별매매상위_OPT10034_ToolStripMenuItem))
            {
                Console.WriteLine("외인기간별매매상위_OPT10034_ToolStripMenuItem");
            }
            else if (sender.Equals(외인연속순매매상위_OPT10035_ToolStripMenuItem))
            {
                Console.WriteLine("외인연속순매매상위_OPT10035_ToolStripMenuItem");
            }
            else if (sender.Equals(매매상위_OPT10036_ToolStripMenuItem))
            {
                Console.WriteLine("매매상위_OPT10036_ToolStripMenuItem");
            }
            else if (sender.Equals(외국계창구매매상위_opt10037_ToolStripMenuItem))
            {
                Console.WriteLine("외국계창구매매상위_opt10037_ToolStripMenuItem");
            }
            else if (sender.Equals(종목별증권사순위_opt10038_ToolStripMenuItem))
            {
                Console.WriteLine("종목별증권사순위_opt10038_ToolStripMenuItem");
            }
            else if (sender.Equals(증권사별매매상위_OPT10039_ToolStripMenuItem))
            {
                Console.WriteLine("증권사별매매상위_OPT10039_ToolStripMenuItem");
            }
            else if (sender.Equals(당일주요거래원_opt10040_ToolStripMenuItem))
            {
                Console.WriteLine("당일주요거래원_opt10040_ToolStripMenuItem");
            }
            else if (sender.Equals(조기종료통화단위_opt10041_ToolStripMenuItem))
            {
                Console.WriteLine("조기종료통화단위_opt10041_ToolStripMenuItem");
            }
            else if (sender.Equals(순매수거래원순위_opt10042_ToolStripMenuItem))
            {
                Console.WriteLine("순매수거래원순위_opt10042_ToolStripMenuItem");
            }
            else if (sender.Equals(거래원매물대분석_opt10043_ToolStripMenuItem))
            {
                Console.WriteLine("거래원매물대분석_opt10043_ToolStripMenuItem");
            }
            else if (sender.Equals(일별기관매매종목_OPT10044_ToolStripMenuItem))
            {
                Console.WriteLine("일별기관매매종목_OPT10044_ToolStripMenuItem");
            }
            else if (sender.Equals(종목별기관매매추이_opt10045_ToolStripMenuItem))
            {
                Console.WriteLine("종목별기관매매추이_opt10045_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW일별민감도지표_OPT10048_ToolStripMenuItem))
            {
                Console.WriteLine("ELW일별민감도지표_OPT10048_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW투자지표_OPT10049_ToolStripMenuItem))
            {
                Console.WriteLine("ELW투자지표_OPT10049_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW민감도지표_OPT10050_ToolStripMenuItem))
            {
                Console.WriteLine("ELW민감도지표_OPT10050_ToolStripMenuItem");
            }
            else if (sender.Equals(업종별투자자순매수_OPT10051_ToolStripMenuItem))
            {
                Console.WriteLine("업종별투자자순매수_OPT10051_ToolStripMenuItem");
            }
            else if (sender.Equals(당일상위이탈원_opt10053_ToolStripMenuItem))
            {
                Console.WriteLine("당일상위이탈원_opt10053_ToolStripMenuItem");
            }
            else if (sender.Equals(투자자별일별매매종목_OPT10058_ToolStripMenuItem))
            {
                Console.WriteLine("투자자별일별매매종목_OPT10058_ToolStripMenuItem");
            }
            else if (sender.Equals(종목별투자자기관별_opt10059_ToolStripMenuItem))
            {
                Console.WriteLine("종목별투자자기관별_opt10059_ToolStripMenuItem");
            }
            else if (sender.Equals(종목별투자자기관별차트_opt10060_ToolStripMenuItem))
            {
                Console.WriteLine("종목별투자자기관별차트_opt10060_ToolStripMenuItem");
            }
            else if (sender.Equals(종목별투자자기관별합계_opt10061_ToolStripMenuItem))
            {
                Console.WriteLine("종목별투자자기관별합계_opt10061_ToolStripMenuItem");
            }
            else if (sender.Equals(동일순매매순위_opt10062_ToolStripMenuItem))
            {
                Console.WriteLine("동일순매매순위_opt10062_ToolStripMenuItem");
            }
            else if (sender.Equals(장중투자자별매매_opt10063_ToolStripMenuItem))
            {
                Console.WriteLine("장중투자자별매매_opt10063_ToolStripMenuItem");
            }
            else if (sender.Equals(장중투자자별매매차트_opt10064_ToolStripMenuItem))
            {
                Console.WriteLine("장중투자자별매매차트_opt10064_ToolStripMenuItem");
            }
            else if (sender.Equals(장중투자자별매매상위_OPT10065_ToolStripMenuItem))
            {
                Console.WriteLine("장중투자자별매매상위_OPT10065_ToolStripMenuItem");
            }
            else if (sender.Equals(장중투자자별매매차트_opt10066_ToolStripMenuItem))
            {
                Console.WriteLine("장중투자자별매매차트_opt10066_ToolStripMenuItem");
            }
            else if (sender.Equals(대차거래내역_OPT10067_ToolStripMenuItem))
            {
                Console.WriteLine("대차거래내역_OPT10067_ToolStripMenuItem");
            }
            else if (sender.Equals(대차거래추이_OPT10068_ToolStripMenuItem))
            {
                Console.WriteLine("대차거래추이_OPT10068_ToolStripMenuItem");
            }
            else if (sender.Equals(대차거래상위10종목_OPT10069_ToolStripMenuItem))
            {
                Console.WriteLine("대차거래상위10종목_OPT10069_ToolStripMenuItem");
            }
            else if (sender.Equals(당일주요거래원_opt10070_ToolStripMenuItem))
            {
                Console.WriteLine("당일주요거래원_opt10070_ToolStripMenuItem");
            }
            else if (sender.Equals(시간대별전일비거래비중_OPT10071_ToolStripMenuItem))
            {
                Console.WriteLine("시간대별전일비거래비중_OPT10071_ToolStripMenuItem");
            }
            else if (sender.Equals(일자별종목별실현손익_OPT10072_ToolStripMenuItem))
            {
                Console.WriteLine("일자별종목별실현손익_OPT10072_ToolStripMenuItem");
            }
            else if (sender.Equals(일자별종목별실현손익_opt10073_ToolStripMenuItem))
            {
                Console.WriteLine("일자별종목별실현손익_opt10073_ToolStripMenuItem");
            }
            else if (sender.Equals(일자별실현손익_opt10074_ToolStripMenuItem))
            {
                Console.WriteLine("일자별실현손익_opt10074_ToolStripMenuItem");
            }
            else if (sender.Equals(실시간미체결_opt10075_ToolStripMenuItem))
            {
                Console.WriteLine("실시간미체결_opt10075_ToolStripMenuItem");
            }
            else if (sender.Equals(실시간체결_OPT10076_ToolStripMenuItem))
            {
                Console.WriteLine("실시간체결_OPT10076_ToolStripMenuItem");
            }
            else if (sender.Equals(당일실현손익상세_opt10077_ToolStripMenuItem))
            {
                Console.WriteLine("당일실현손익상세_opt10077_ToolStripMenuItem");
            }
            else if (sender.Equals(증권사별종목매매동향_OPT10078_ToolStripMenuItem))
            {
                Console.WriteLine("증권사별종목매매동향_OPT10078_ToolStripMenuItem");
            }
            else if (sender.Equals(주식틱차트조회_opt10079_ToolStripMenuItem))
            {
                Console.WriteLine("주식틱차트조회_opt10079_ToolStripMenuItem");
            }
            else if (sender.Equals(주식분봉차트조회_opt10080_ToolStripMenuItem))
            {
                Console.WriteLine("주식분봉차트조회_opt10080_ToolStripMenuItem");
            }
            else if (sender.Equals(주식일봉차트조회_opt10081_ToolStripMenuItem))
            {
                Console.WriteLine("주식일봉차트조회_opt10081_ToolStripMenuItem");
            }
            else if (sender.Equals(주식주봉차트조회_opt10082_ToolStripMenuItem))
            {
                Console.WriteLine("주식주봉차트조회_opt10082_ToolStripMenuItem");
            }
            else if (sender.Equals(주식월봉차트조회_opt10083_ToolStripMenuItem))
            {
                Console.WriteLine("주식월봉차트조회_opt10083_ToolStripMenuItem");
            }
            else if (sender.Equals(당일전일체결_opt10084_ToolStripMenuItem))
            {
                Console.WriteLine("당일전일체결_opt10084_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌수익률_opt10085_ToolStripMenuItem))
            {
                Console.WriteLine("계좌수익률_opt10085_ToolStripMenuItem");
            }
            else if (sender.Equals(일별주가_opt10086_ToolStripMenuItem))
            {
                Console.WriteLine("일별주가_opt10086_ToolStripMenuItem");
            }
            else if (sender.Equals(시간외단일가_opt10087_ToolStripMenuItem))
            {
                Console.WriteLine("시간외단일가_opt10087_ToolStripMenuItem");
            }
            else if (sender.Equals(주식년봉차트조회_opt10094_ToolStripMenuItem))
            {
                Console.WriteLine("주식년봉차트조회_opt10094_ToolStripMenuItem");
            }
            else if (sender.Equals(업종현재가_opt20001_ToolStripMenuItem))
            {
                Console.WriteLine("업종현재가_opt20001_ToolStripMenuItem");
            }
            else if (sender.Equals(업종별주가_OPT20002_ToolStripMenuItem))
            {
                Console.WriteLine("업종별주가_OPT20002_ToolStripMenuItem");
            }
            else if (sender.Equals(전업종지수_opt20003_ToolStripMenuItem))
            {
                Console.WriteLine("전업종지수_opt20003_ToolStripMenuItem");
            }
            else if (sender.Equals(업종틱차트조회_opt20004_ToolStripMenuItem))
            {
                Console.WriteLine("업종틱차트조회_opt20004_ToolStripMenuItem");
            }
            else if (sender.Equals(업종분봉조회_opt20005_ToolStripMenuItem))
            {
                Console.WriteLine("업종분봉조회_opt20005_ToolStripMenuItem");
            }
            else if (sender.Equals(업종일봉조회_opt20006_ToolStripMenuItem))
            {
                Console.WriteLine("업종일봉조회_opt20006_ToolStripMenuItem");
            }
            else if (sender.Equals(업종주봉조회_opt20007_ToolStripMenuItem))
            {
                Console.WriteLine("업종주봉조회_opt20007_ToolStripMenuItem");
            }
            else if (sender.Equals(업종월봉조회_opt20008_ToolStripMenuItem))
            {
                Console.WriteLine("업종월봉조회_opt20008_ToolStripMenuItem");
            }
            else if (sender.Equals(업종현재가일별_opt20009_ToolStripMenuItem))
            {
                Console.WriteLine("업종현재가일별_opt20009_ToolStripMenuItem");
            }
            else if (sender.Equals(업종년봉조회_opt20019_ToolStripMenuItem))
            {
                Console.WriteLine("업종년봉조회_opt20019_ToolStripMenuItem");
            }
            else if (sender.Equals(대차거래추이_종목별_opt20068_ToolStripMenuItem))
            {
                Console.WriteLine("대차거래추이_종목별_opt20068_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW가격급등락_OPT30001_ToolStripMenuItem))
            {
                Console.WriteLine("ELW가격급등락_OPT30001_ToolStripMenuItem");
            }
            else if (sender.Equals(거래원별ELW순매매상위_OPT30002_ToolStripMenuItem))
            {
                Console.WriteLine("거래원별ELW순매매상위_OPT30002_ToolStripMenuItem");
            }
            else if (sender.Equals(ELWLP보유일별추이_OPT30003_ToolStripMenuItem))
            {
                Console.WriteLine("ELWLP보유일별추이_OPT30003_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW괴리율_OPT30004_ToolStripMenuItem))
            {
                Console.WriteLine("ELW괴리율_OPT30004_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW조건검색_opt30005_ToolStripMenuItem))
            {
                Console.WriteLine("ELW조건검색_opt30005_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW종목상세_opt30006_ToolStripMenuItem))
            {
                Console.WriteLine("ELW종목상세_opt30006_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW종목상세_OPT30007_ToolStripMenuItem))
            {
                Console.WriteLine("ELW종목상세_OPT30007_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW민감도지표_OPT30008_ToolStripMenuItem))
            {
                Console.WriteLine("ELW민감도지표_OPT30008_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW등락률순위_OPT30009_ToolStripMenuItem))
            {
                Console.WriteLine("ELW등락률순위_OPT30009_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW잔량순위_OPT30010_ToolStripMenuItem))
            {
                Console.WriteLine("ELW잔량순위_OPT30010_ToolStripMenuItem");
            }
            else if (sender.Equals(ELW근접율_OPT30011_ToolStripMenuItem))
            {
                Console.WriteLine("ELW근접율_OPT30011_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF수익율_OPT40001_ToolStripMenuItem))
            {
                Console.WriteLine("ETF수익율_OPT40001_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF종목정보_opt40002_ToolStripMenuItem))
            {
                Console.WriteLine("ETF종목정보_opt40002_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF일별추이_OPT40003_ToolStripMenuItem))
            {
                Console.WriteLine("ETF일별추이_OPT40003_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF전체시세_opt40004_ToolStripMenuItem))
            {
                Console.WriteLine("ETF전체시세_opt40004_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF일별추이_OPT40005_ToolStripMenuItem))
            {
                Console.WriteLine("ETF일별추이_OPT40005_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF시간대별추이_opt40006_ToolStripMenuItem))
            {
                Console.WriteLine("ETF시간대별추이_opt40006_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF시간대별체결_opt40007_ToolStripMenuItem))
            {
                Console.WriteLine("ETF시간대별체결_opt40007_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF시간대별체결_opt40008_ToolStripMenuItem))
            {
                Console.WriteLine("ETF시간대별체결_opt40008_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF시간대별체결_OPT40009_ToolStripMenuItem))
            {
                Console.WriteLine("ETF시간대별체결_OPT40009_ToolStripMenuItem");
            }
            else if (sender.Equals(ETF시간대별추이_opt40010_ToolStripMenuItem))
            {
                Console.WriteLine("ETF시간대별추이_opt40010_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵현재가정보_opt50001_ToolStripMenuItem))
            {
                Console.WriteLine("선옵현재가정보_opt50001_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵일자별체결_opt50002_ToolStripMenuItem))
            {
                Console.WriteLine("선옵일자별체결_opt50002_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵시고저가_OPT50003_ToolStripMenuItem))
            {
                Console.WriteLine("선옵시고저가_OPT50003_ToolStripMenuItem");
            }
            else if (sender.Equals(콜옵션행사가_opt50004_ToolStripMenuItem))
            {
                Console.WriteLine("콜옵션행사가_opt50004_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵시간별거래량_OPT50005_ToolStripMenuItem))
            {
                Console.WriteLine("선옵시간별거래량_OPT50005_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵체결추이_OPT50006_ToolStripMenuItem))
            {
                Console.WriteLine("선옵체결추이_OPT50006_ToolStripMenuItem");
            }
            else if (sender.Equals(선물시세추이_opt50007_ToolStripMenuItem))
            {
                Console.WriteLine("선물시세추이_opt50007_ToolStripMenuItem");
            }
            else if (sender.Equals(프로그램매매추이차트_opt50008_ToolStripMenuItem))
            {
                Console.WriteLine("프로그램매매추이차트_opt50008_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵시간별잔량_OPT50009_ToolStripMenuItem))
            {
                Console.WriteLine("선옵시간별잔량_OPT50009_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵호가잔량추이_OPT50010_ToolStripMenuItem))
            {
                Console.WriteLine("선옵호가잔량추이_OPT50010_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵호가잔량추이_OPT50011_ToolStripMenuItem))
            {
                Console.WriteLine("선옵호가잔량추이_OPT50011_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵타임스프레드차트_OPT50012_ToolStripMenuItem))
            {
                Console.WriteLine("선옵타임스프레드차트_OPT50012_ToolStripMenuItem");
            }
            else if (sender.Equals(선물가격대별비중차트_OPT50013_ToolStripMenuItem))
            {
                Console.WriteLine("선물가격대별비중차트_OPT50013_ToolStripMenuItem");
            }
            else if (sender.Equals(선물가격대별비중차트_OPT50014_ToolStripMenuItem))
            {
                Console.WriteLine("선물가격대별비중차트_OPT50014_ToolStripMenuItem");
            }
            else if (sender.Equals(선물미결제약정일차트_opt50015_ToolStripMenuItem))
            {
                Console.WriteLine("선물미결제약정일차트_opt50015_ToolStripMenuItem");
            }
            else if (sender.Equals(베이시스추이차트_OPT50016_ToolStripMenuItem))
            {
                Console.WriteLine("베이시스추이차트_OPT50016_ToolStripMenuItem");
            }
            else if (sender.Equals(베이시스추이차트_OPT50017_ToolStripMenuItem))
            {
                Console.WriteLine("베이시스추이차트_OPT50017_ToolStripMenuItem");
            }
            else if (sender.Equals(풋콜옵션비율차트_OPT50018_ToolStripMenuItem))
            {
                Console.WriteLine("풋콜옵션비율차트_OPT50018_ToolStripMenuItem");
            }
            else if (sender.Equals(선물옵션현재가정보_OPT50019_ToolStripMenuItem))
            {
                Console.WriteLine("선물옵션현재가정보_OPT50019_ToolStripMenuItem");
            }
            else if (sender.Equals(복수종목결제월별시세_opt50020_ToolStripMenuItem))
            {
                Console.WriteLine("복수종목결제월별시세_opt50020_ToolStripMenuItem");
            }
            else if (sender.Equals(콜종목결제월별시세_OPT50021_ToolStripMenuItem))
            {
                Console.WriteLine("콜종목결제월별시세_OPT50021_ToolStripMenuItem");
            }
            else if (sender.Equals(풋종목결제월별시세_OPT50022_ToolStripMenuItem))
            {
                Console.WriteLine("풋종목결제월별시세_OPT50022_ToolStripMenuItem");
            }
            else if (sender.Equals(민감도지표추이_opt50023_ToolStripMenuItem))
            {
                Console.WriteLine("민감도지표추이_opt50023_ToolStripMenuItem");
            }
            else if (sender.Equals(일별변동성분석그래프_OPT50024_ToolStripMenuItem))
            {
                Console.WriteLine("일별변동성분석그래프_OPT50024_ToolStripMenuItem");
            }
            else if (sender.Equals(시간별변동성분석그래프_OPT50025_ToolStripMenuItem))
            {
                Console.WriteLine("시간별변동성분석그래프_OPT50025_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵주문체결_OPT50026_ToolStripMenuItem))
            {
                Console.WriteLine("선옵주문체결_OPT50026_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵잔고_OPT50027_ToolStripMenuItem))
            {
                Console.WriteLine("선옵잔고_OPT50027_ToolStripMenuItem");
            }
            else if (sender.Equals(선물틱차트_opt50028_ToolStripMenuItem))
            {
                Console.WriteLine("선물틱차트_opt50028_ToolStripMenuItem");
            }
            else if (sender.Equals(선물옵션분차트_OPT50029_ToolStripMenuItem))
            {
                Console.WriteLine("선물옵션분차트_OPT50029_ToolStripMenuItem");
            }
            else if (sender.Equals(선물옵션일차트_OPT50030_ToolStripMenuItem))
            {
                Console.WriteLine("선물옵션일차트_OPT50030_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵잔고손익_OPT50031_ToolStripMenuItem))
            {
                Console.WriteLine("선옵잔고손익_OPT50031_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵당일실현손익_OPT50032_ToolStripMenuItem))
            {
                Console.WriteLine("선옵당일실현손익_OPT50032_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵잔존일조회_OPT50033_ToolStripMenuItem))
            {
                Console.WriteLine("선옵잔존일조회_OPT50033_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵전일가격_OPT50034_ToolStripMenuItem))
            {
                Console.WriteLine("선옵전일가격_OPT50034_ToolStripMenuItem");
            }
            else if (sender.Equals(지수변동성차트_OPT50035_ToolStripMenuItem))
            {
                Console.WriteLine("지수변동성차트_OPT50035_ToolStripMenuItem");
            }
            else if (sender.Equals(주요지수변동성차트_OPT50036_ToolStripMenuItem))
            {
                Console.WriteLine("주요지수변동성차트_OPT50036_ToolStripMenuItem");
            }
            else if (sender.Equals(코스피200지수_OPT50037_ToolStripMenuItem))
            {
                Console.WriteLine("코스피200지수_OPT50037_ToolStripMenuItem");
            }
            else if (sender.Equals(투자자별만기손익차트_opt50038_ToolStripMenuItem))
            {
                Console.WriteLine("투자자별만기손익차트_opt50038_ToolStripMenuItem");
            }
            else if (sender.Equals(투자자별포지션종합_opt50039_ToolStripMenuItem))
            {
                Console.WriteLine("투자자별포지션종합_opt50039_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵시고저가_OPT50040_ToolStripMenuItem))
            {
                Console.WriteLine("선옵시고저가_OPT50040_ToolStripMenuItem");
            }
            else if (sender.Equals(주식선물거래량상위종목_OPT50043_ToolStripMenuItem))
            {
                Console.WriteLine("주식선물거래량상위종목_OPT50043_ToolStripMenuItem");
            }
            else if (sender.Equals(주식선물시세표_opt50044_ToolStripMenuItem))
            {
                Console.WriteLine("주식선물시세표_opt50044_ToolStripMenuItem");
            }
            else if (sender.Equals(선물미결제약정분차트_opt50062_ToolStripMenuItem))
            {
                Console.WriteLine("선물미결제약정분차트_opt50062_ToolStripMenuItem");
            }
            else if (sender.Equals(옵션미결제약정일차트_opt50063_ToolStripMenuItem))
            {
                Console.WriteLine("옵션미결제약정일차트_opt50063_ToolStripMenuItem");
            }
            else if (sender.Equals(옵션미결제약정분차트_opt50064_ToolStripMenuItem))
            {
                Console.WriteLine("옵션미결제약정분차트_opt50064_ToolStripMenuItem");
            }
            else if (sender.Equals(풋옵션행사가_opt50065_ToolStripMenuItem))
            {
                Console.WriteLine("풋옵션행사가_opt50065_ToolStripMenuItem");
            }
            else if (sender.Equals(옵션틱차트_opt50066_ToolStripMenuItem))
            {
                Console.WriteLine("옵션틱차트_opt50066_ToolStripMenuItem");
            }
            else if (sender.Equals(옵션분차트_opt50067_ToolStripMenuItem))
            {
                Console.WriteLine("옵션분차트_opt50067_ToolStripMenuItem");
            }
            else if (sender.Equals(옵션일차트_opt50068_ToolStripMenuItem))
            {
                Console.WriteLine("옵션일차트_opt50068_ToolStripMenuItem");
            }
            else if (sender.Equals(선물주차트_opt50071_ToolStripMenuItem))
            {
                Console.WriteLine("선물주차트_opt50071_ToolStripMenuItem");
            }
            else if (sender.Equals(선물월차트_opt50072_ToolStripMenuItem))
            {
                Console.WriteLine("선물월차트_opt50072_ToolStripMenuItem");
            }
            else if (sender.Equals(선물년차트_opt50073_ToolStripMenuItem))
            {
                Console.WriteLine("선물년차트_opt50073_ToolStripMenuItem");
            }
            else if (sender.Equals(테마그룹별_OPT90001_ToolStripMenuItem))
            {
                Console.WriteLine("테마그룹별_OPT90001_ToolStripMenuItem");
            }
            else if (sender.Equals(테마구성종목_opt90002_ToolStripMenuItem))
            {
                Console.WriteLine("테마구성종목_opt90002_ToolStripMenuItem");
            }
            else if (sender.Equals(프로그램순매수상위50_opt90003_ToolStripMenuItem))
            {
                Console.WriteLine("프로그램순매수상위50_opt90003_ToolStripMenuItem");
            }
            else if (sender.Equals(종목별프로그램매매현황_OPT90004_ToolStripMenuItem))
            {
                Console.WriteLine("종목별프로그램매매현황_OPT90004_ToolStripMenuItem");
            }
            else if (sender.Equals(프로그램매매추이_OPT90005_ToolStripMenuItem))
            {
                Console.WriteLine("프로그램매매추이_OPT90005_ToolStripMenuItem");
            }
            else if (sender.Equals(프로그램매매차익잔고추이_OPT90006_ToolStripMenuItem))
            {
                Console.WriteLine("프로그램매매차익잔고추이_OPT90006_ToolStripMenuItem");
            }
            else if (sender.Equals(프로그램매매누적추이_OPT90007_ToolStripMenuItem))
            {
                Console.WriteLine("프로그램매매누적추이_OPT90007_ToolStripMenuItem");
            }
            else if (sender.Equals(종목시간별프로그램매매추이_opt90008_ToolStripMenuItem))
            {
                Console.WriteLine("종목시간별프로그램매매추이_opt90008_ToolStripMenuItem");
            }
            else if (sender.Equals(외국인기관매매상위_opt90009_ToolStripMenuItem))
            {
                Console.WriteLine("외국인기관매매상위_opt90009_ToolStripMenuItem");
            }
            else if (sender.Equals(차익잔고현황_OPT90010_ToolStripMenuItem))
            {
                Console.WriteLine("차익잔고현황_OPT90010_ToolStripMenuItem");
            }
            else if (sender.Equals(차익잔고현황_opt90011_ToolStripMenuItem))
            {
                Console.WriteLine("차익잔고현황_opt90011_ToolStripMenuItem");
            }
            else if (sender.Equals(대차거래내역_OPT90012_ToolStripMenuItem))
            {
                Console.WriteLine("대차거래내역_OPT90012_ToolStripMenuItem");
            }
            else if (sender.Equals(종목일별프로그램매매추이_opt90013_ToolStripMenuItem))
            {
                Console.WriteLine("종목일별프로그램매매추이_opt90013_ToolStripMenuItem");
            }
            else if (sender.Equals(대차거래상위10종목_opt99999_ToolStripMenuItem))
            {
                Console.WriteLine("대차거래상위10종목_opt99999_ToolStripMenuItem");
            }
            else if (sender.Equals(선물전체시세_OPTFOFID_ToolStripMenuItem))
            {
                Console.WriteLine("선물전체시세_OPTFOFID_ToolStripMenuItem");
            }
            else if (sender.Equals(관심종목정보_OPTKWFID_ToolStripMenuItem))
            {
                Console.WriteLine("관심종목정보_OPTKWFID_ToolStripMenuItem");
            }
            else if (sender.Equals(관심종목투자자정보_OPTKWINV_ToolStripMenuItem))
            {
                Console.WriteLine("관심종목투자자정보_OPTKWINV_ToolStripMenuItem");
            }
            else if (sender.Equals(관심종목프로그램정보_OPTKWPRO_ToolStripMenuItem))
            {
                Console.WriteLine("관심종목프로그램정보_OPTKWPRO_ToolStripMenuItem");
            }
            else if (sender.Equals(예수금상세현황_opw00001_ToolStripMenuItem))
            {
                Console.WriteLine("예수금상세현황_opw00001_ToolStripMenuItem");
            }
            else if (sender.Equals(일별추정예탁자산현황_OPW00002_ToolStripMenuItem))
            {
                Console.WriteLine("일별추정예탁자산현황_OPW00002_ToolStripMenuItem");
            }
            else if (sender.Equals(추정자산조회_OPW00003_ToolStripMenuItem))
            {
                Console.WriteLine("추정자산조회_OPW00003_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌평가현황_OPW00004_ToolStripMenuItem))
            {
                Console.WriteLine("계좌평가현황_OPW00004_ToolStripMenuItem");
            }
            else if (sender.Equals(체결잔고_opw00005_ToolStripMenuItem))
            {
                Console.WriteLine("체결잔고_opw00005_ToolStripMenuItem");
            }
            else if (sender.Equals(관리자별주문체결내역_OPW00006_ToolStripMenuItem))
            {
                Console.WriteLine("관리자별주문체결내역_OPW00006_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌별주문체결내역상세_OPW00007_ToolStripMenuItem))
            {
                Console.WriteLine("계좌별주문체결내역상세_OPW00007_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌별익일결제예정내역_opw00008_ToolStripMenuItem))
            {
                Console.WriteLine("계좌별익일결제예정내역_opw00008_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌별주문체결현황_opw00009_ToolStripMenuItem))
            {
                Console.WriteLine("계좌별주문체결현황_opw00009_ToolStripMenuItem");
            }
            else if (sender.Equals(주문인출가능금액_opw00010_ToolStripMenuItem))
            {
                Console.WriteLine("주문인출가능금액_opw00010_ToolStripMenuItem");
            }
            else if (sender.Equals(증거금율별주문가능수량조회_opw00011_ToolStripMenuItem))
            {
                Console.WriteLine("증거금율별주문가능수량조회_opw00011_ToolStripMenuItem");
            }
            else if (sender.Equals(신용보증금율별주문가능수량조회_OPW00012_ToolStripMenuItem))
            {
                Console.WriteLine("신용보증금율별주문가능수량조회_OPW00012_ToolStripMenuItem");
            }
            else if (sender.Equals(증거금세부내역조회_opw00013_ToolStripMenuItem))
            {
                Console.WriteLine("증거금세부내역조회_opw00013_ToolStripMenuItem");
            }
            else if (sender.Equals(비밀번호일치여부_OPW00014_ToolStripMenuItem))
            {
                Console.WriteLine("비밀번호일치여부_OPW00014_ToolStripMenuItem");
            }
            else if (sender.Equals(위탁종합거래내역_OPW00015_ToolStripMenuItem))
            {
                Console.WriteLine("위탁종합거래내역_OPW00015_ToolStripMenuItem");
            }
            else if (sender.Equals(일별계좌수익률상세현황_OPW00016_ToolStripMenuItem))
            {
                Console.WriteLine("일별계좌수익률상세현황_OPW00016_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌별당일현황_OPW00017_ToolStripMenuItem))
            {
                Console.WriteLine("계좌별당일현황_OPW00017_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌평가잔고내역_opw00018_ToolStripMenuItem))
            {
                Console.WriteLine("계좌평가잔고내역_opw00018_ToolStripMenuItem");
            }
            else if (sender.Equals(선물옵션청산주문위탁증거금가계산_OPW20001_ToolStripMenuItem))
            {
                Console.WriteLine("선물옵션청산주문위탁증거금가계산_OPW20001_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵당일매매변동현황_OPW20002_ToolStripMenuItem))
            {
                Console.WriteLine("선옵당일매매변동현황_OPW20002_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵기간손익조회_opw20003_ToolStripMenuItem))
            {
                Console.WriteLine("선옵기간손익조회_opw20003_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵주문체결내역상세_OPW20004_ToolStripMenuItem))
            {
                Console.WriteLine("선옵주문체결내역상세_OPW20004_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵주문체결내역상세평균가_OPW20005_ToolStripMenuItem))
            {
                Console.WriteLine("선옵주문체결내역상세평균가_OPW20005_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵잔고상세현황_opw20006_ToolStripMenuItem))
            {
                Console.WriteLine("선옵잔고상세현황_opw20006_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵잔고현황정산가기준_OPW20007_ToolStripMenuItem))
            {
                Console.WriteLine("선옵잔고현황정산가기준_OPW20007_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌별결제예상내역조회_OPW20008_ToolStripMenuItem))
            {
                Console.WriteLine("계좌별결제예상내역조회_OPW20008_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵계좌별주문가능수량_opw20009_ToolStripMenuItem))
            {
                Console.WriteLine("선옵계좌별주문가능수량_opw20009_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵예탁금및증거금조회_OPW20010_ToolStripMenuItem))
            {
                Console.WriteLine("선옵예탁금및증거금조회_OPW20010_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵계좌예비증거금상세_OPW20011_ToolStripMenuItem))
            {
                Console.WriteLine("선옵계좌예비증거금상세_OPW20011_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵증거금상세내역_opw20012_ToolStripMenuItem))
            {
                Console.WriteLine("선옵증거금상세내역_opw20012_ToolStripMenuItem");
            }
            else if (sender.Equals(계좌미결제청산가능수량조회_OPW20013_ToolStripMenuItem))
            {
                Console.WriteLine("계좌미결제청산가능수량조회_OPW20013_ToolStripMenuItem");
            }
            else if (sender.Equals(선옵실시간증거금산출_OPW20014_ToolStripMenuItem))
            {
                Console.WriteLine("선옵실시간증거금산출_OPW20014_ToolStripMenuItem");
            }
            else if (sender.Equals(옵션매도주문증거금현황_opw20015_ToolStripMenuItem))
            {
                Console.WriteLine("옵션매도주문증거금현황_opw20015_ToolStripMenuItem");
            }
            else if (sender.Equals(신용융자가능종목_opw20016_ToolStripMenuItem))
            {
                Console.WriteLine("신용융자가능종목_opw20016_ToolStripMenuItem");
            }
            else if (sender.Equals(신용융자가능문의_opw20017_ToolStripMenuItem))
            {
                Console.WriteLine("신용융자가능문의_opw20017_ToolStripMenuItem");
            }
        }

        private void 파일읽기테스트ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamReader SR = new StreamReader("054780_키이스트_부분.txt");
            int count = 0;
            string line = "";
            while (line != null)
            {
                line = SR.ReadLine();
                if (line != null)
                {
                    count++;
                }
            }
            MessageBox.Show( String.Format( "제일테크노스 총 라인 수는 {0}입니다.", count ));
        }

        private void 누락된단일주식기본정보저장_ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MessageBox.Show(String.Format("누락된 종목정보 등록후 다시 시도해주세요."), "누락된 종목정보 등록필요", MessageBoxButtons.OK);
            return;

            _키움응답주식기본정보카운트 = 0;
            _종목코드의코스피종목_사전["900140"] = "엘브이엠씨홀딩스";
            _종목코드의코스닥종목_사전["900040"] = "차이나그레이트";
            _종목코드의코스닥종목_사전["900070"] = "글로벌에스엠";
            _종목코드의코스닥종목_사전["900080"] = "에스앤씨엔진그룹";
            _종목코드의코스닥종목_사전["900090"] = "차이나하오란";
            _종목코드의코스닥종목_사전["900100"] = "뉴프라이드";
            _종목코드의코스닥종목_사전["900110"] = "이스트아시아홀딩스";
            _종목코드의코스닥종목_사전["900120"] = "씨케이에이치";
            _종목코드의코스닥종목_사전["900250"] = "크리스탈신소재";
            _종목코드의코스닥종목_사전["900260"] = "로스웰";
            _종목코드의코스닥종목_사전["900270"] = "헝셩그룹";
            _종목코드의코스닥종목_사전["900280"] = "골든센츄리";
            _종목코드의코스닥종목_사전["900290"] = "GRT";
            _종목코드의코스닥종목_사전["900300"] = "오가닉티코스메틱";
            _종목코드의코스닥종목_사전["900310"] = "컬러레이";

            _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                axKHOpenAPI1.SetRealRemove("All", "All");
                Console.WriteLine("실시간 시세해지하였습니다.\r\n\r\n");
            }));
            foreach (string 코스피종목코드 in _종목코드의코스피종목_사전.Keys)
            {
                _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                    axKHOpenAPI1.SetInputValue("종목코드", 코스피종목코드);
                    axKHOpenAPI1.CommRqData("누락된단일주식기본정보", "opt10001", 0, "5000");
                }));
            }
            foreach (string 코스닥종목코드 in _종목코드의코스닥종목_사전.Keys)
            {
                _요청데이터관리자야.트랜잭션_요청해줘(new Task(() => {
                    axKHOpenAPI1.SetInputValue("종목코드", 코스닥종목코드);
                    axKHOpenAPI1.CommRqData("누락된단일주식기본정보", "opt10001", 0, "5000");
                }));
            }
        }

        private void 실시간매수추천_시작해줘 ()
        {
            if (_실시간매수추천_시작했니 == false)
            {
                _실시간매수추천_시작했니 = true;
                _실시간매수추천이야.시작하자(axKHOpenAPI1);
                MessageBox.Show(String.Format("실시간매수추천을 시작합니다."), "실시간매수추천", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show(String.Format("실시간매수추천 이미 시작하였습니다."), "실시간매수추천", MessageBoxButtons.OK);
            }
        }

        private void 실시간매수추천시작_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            실시간매수추천_시작해줘();
        }

        private void 당일주식체결_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_당일주식체결이야 == null)
            {
                _당일주식체결이야 = new 당일주식체결_Form();
                _당일주식체결이야.당일주식체결_Form_저장해줘(_당일주식체결이야);
                _당일주식체결이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                _당일주식체결이야.Text = "당일주식체결";
                Console.WriteLine("비어있으므로 당일주식체결 객체 생성합니다.");
                _당일주식체결이야.Show();
            } else
            {
                _당일주식체결이야.Activate();
            }
        }

        private void 당일매수추천종목목록_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_당일매수추천종목목록이야 == null)
            {
                _당일매수추천종목목록이야 = new 당일매수추천종목목록_Form();
                _당일매수추천종목목록이야.당일매수추천종목목록_Form_저장해줘(_당일매수추천종목목록이야);
                _당일매수추천종목목록이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                _당일매수추천종목목록이야.Text = "당일매수추천종목목록";
                Console.WriteLine("비어있으므로 당일매수추천종목목록 객체 생성합니다.");
                _당일매수추천종목목록이야.Show();

                if (_A주식체결이야 == null)
                {
                    _A주식체결이야 = new 당일주식체결_Form();
                    _A주식체결이야.당일주식체결_Form_저장해줘(_A주식체결이야);
                    _A주식체결이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                    _A주식체결이야.Text = "A주식체결";
                    Console.WriteLine("A주식체결 객체 생성합니다.");
                    _A주식체결이야.Show();
                }
                _당일매수추천종목목록이야.당일주식체결_Form_저장해줘_A(_A주식체결이야);

                if (_B주식체결이야 == null)
                {
                    _B주식체결이야 = new 당일주식체결_Form();
                    _B주식체결이야.당일주식체결_Form_저장해줘(_B주식체결이야);
                    _B주식체결이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                    _B주식체결이야.Text = "B주식체결";
                    Console.WriteLine("B주식체결 객체 생성합니다.");
                    _B주식체결이야.Show();
                }
                _당일매수추천종목목록이야.당일주식체결_Form_저장해줘_B(_B주식체결이야);

                if (_주식종합이야 == null)
                {
                    _주식종합이야 = new 주식종합_Form();
                    _주식종합이야.MdiParent = this;
                    _주식종합이야.빌리언스탁_저장해줘(_빌리언스탁);
                    _주식종합이야.주식종합_Form_저장해줘(_주식종합이야);
                    _주식종합이야.시작하자(axKHOpenAPI1);
                    _주식종합이야.접속중_변경해줘(로그인하세요_label.Text);
                    _주식종합이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                    _주식종합이야.Text = "주식종합";
                    Console.WriteLine("비어있으므로 주식종합 객체 생성합니다.");
                    _주식종합이야.Show();
                }
                _당일매수추천종목목록이야.주식종합_Form_저장해줘(_주식종합이야);
            }
            else
            {
                if (_A주식체결이야 == null)
                {
                    _A주식체결이야 = new 당일주식체결_Form();
                    _A주식체결이야.당일주식체결_Form_저장해줘(_A주식체결이야);
                    _A주식체결이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                    _A주식체결이야.Text = "A주식체결";
                    Console.WriteLine("A주식체결 객체 생성합니다.");
                    _A주식체결이야.Show();
                    _당일매수추천종목목록이야.당일주식체결_Form_저장해줘_A(_A주식체결이야);
                }
                if (_B주식체결이야 == null)
                {
                    _B주식체결이야 = new 당일주식체결_Form();
                    _B주식체결이야.당일주식체결_Form_저장해줘(_B주식체결이야);
                    _B주식체결이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                    _B주식체결이야.Text = "B주식체결";
                    Console.WriteLine("B주식체결 객체 생성합니다.");
                    _B주식체결이야.Show();
                    _당일매수추천종목목록이야.당일주식체결_Form_저장해줘_B(_B주식체결이야);
                }
                if (_주식종합이야 == null)
                {
                    _주식종합이야 = new 주식종합_Form();
                    _주식종합이야.MdiParent = this;
                    _주식종합이야.빌리언스탁_저장해줘(_빌리언스탁);
                    _주식종합이야.주식종합_Form_저장해줘(_주식종합이야);
                    _주식종합이야.시작하자(axKHOpenAPI1);
                    _주식종합이야.접속중_변경해줘(로그인하세요_label.Text);
                    _주식종합이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                    _주식종합이야.Text = "주식종합";
                    Console.WriteLine("비어있으므로 주식종합 객체 생성합니다.");
                    _주식종합이야.Show();
                    _당일매수추천종목목록이야.주식종합_Form_저장해줘(_주식종합이야);
                }
                _당일매수추천종목목록이야.Activate();
            }
        }

        private void 손익분기계산기_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new 손익분기계산기_Form().Show();
        }

        private void 주식종합_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long state = axKHOpenAPI1.GetConnectState();
            if (state == 0)
            {
                로그인하세요_label.Text = "로그인하세요";
                MessageBox.Show(String.Format("로그인하지 않은 상태입니다.\n로그인하세요."), "로그인 정보", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    if (_주식종합이야 == null)
                    {
                        if (_주식종합이야 == null)
                        {
                            _주식종합이야 = new 주식종합_Form();
                            _주식종합이야.MdiParent = this;
                            _주식종합이야.빌리언스탁_저장해줘(_빌리언스탁);
                            _주식종합이야.주식종합_Form_저장해줘(_주식종합이야);
                            _주식종합이야.시작하자(axKHOpenAPI1);
                            _주식종합이야.접속중_변경해줘(로그인하세요_label.Text);
                            _주식종합이야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                            _주식종합이야.Text = "주식종합";
                            Console.WriteLine("비어있으므로 주식종합 객체 생성합니다.");
                            _주식종합이야.Show();
                        }
                        else
                        {
                            _주식종합이야.Activate();
                        }
                    }
                }
                catch (Exception EX)
                {
                    Console.WriteLine("주식종합 클릭이벤트 = 오류발생 = EX: {0}", EX);
                }
            }
        }

        public void 트랜잭션_요청해줘(Task 작업)
        {
            _요청데이터관리자야.트랜잭션_요청해줘(작업);
        }

        private void 주식잔고_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long state = axKHOpenAPI1.GetConnectState();
            if (state == 0)
            {
                로그인하세요_label.Text = "로그인하세요";
                MessageBox.Show(String.Format("로그인하지 않은 상태입니다.\n로그인하세요."), "로그인 정보", MessageBoxButtons.OK);
            } else
            {
                주식잔고_Form 주식잔고야 = new 주식잔고_Form();
                //주식잔고야.Owner = this;
                주식잔고야.MdiParent = this;
                주식잔고야.FormClosed += new FormClosedEventHandler(창_닫혔어);
                주식잔고야.Text = "주식잔고";
                주식잔고야.시작하자(axKHOpenAPI1);
                주식잔고야.Show();
            }
        }

        private void 순위집계저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("순위집계저장을 클릭하였습니다.");
        }

        private void 메뉴_menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}