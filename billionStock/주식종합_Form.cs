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
    public partial class 주식종합_Form : Form
    {
        /*
         매수 | 매도 | 정정 | 주식체결 | 잔고 | 코스피, 코스닥 지수
         */

        List<string> _로봇종목코드_목록;
        private int _로봇개수 = 1;

        public 빌리언스탁_Form _빌리언스탁;
        private 주식종합_Form _주식종합이야;

        private static AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        
        //private double 주식수수료율 = 1.0037869;
        private double 주식수수료율 = 1.0033;

        static MongoClient _몽고클라이언트 = new MongoClient("mongodb://localhost:27017/billionStock");
        static IMongoDatabase _몽고디비 = _몽고클라이언트.GetDatabase("billionStock");
        static IMongoCollection<종목> _종목컬렉션 = _몽고디비.GetCollection<종목>("stocks");
        static IMongoCollection<주식체결> _주식체결컬렉션 = _몽고디비.GetCollection<주식체결>("trades");
        static IMongoCollection<매수추천> _매수추천컬렉션 = _몽고디비.GetCollection<매수추천>("bidRecommended");
        static IMongoCollection<유보종목> _유보종목컬렉션 = _몽고디비.GetCollection<유보종목>("skip");

        SoundPlayer _사운드플레이어;

        종목 _선택된종목아;

        private DataGridViewCellStyle _레드셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _그린셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _블루셀스타일 = new DataGridViewCellStyle();

        private DataGridViewCellStyle _강한레드셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _강한그린셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _강한블루셀스타일 = new DataGridViewCellStyle();

        private DataGridViewCellStyle _약한레드셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _약한그린셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _약한블루셀스타일 = new DataGridViewCellStyle();

        private DataGridViewCellStyle _블랙셀스타일 = new DataGridViewCellStyle();

        private DataGridViewCellStyle _코스피셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _코스닥셀스타일 = new DataGridViewCellStyle();

        Queue<Task> 요청작업큐 = new Queue<Task>(); // TR요청 작업 큐

        /*
1초 5회 제한 사용시: 20초과. 예: 25
1분 100회 제한 사용시: 600초과. 예: 610
1시간 1000회 제한 사용시: 3600초과. 예: 3610
*/

        public int 실시간주기 = 3610;
        Thread _실시간스레드;
        bool _작업중이니 = false;

        bool _추천종목_똑같니 = false;
        bool _추천종목_처음이니 = true;
        Dictionary<string, 매수추천종목> _종목코드의추천종목_사전;
        Dictionary<string, 보유종목> _종목코드의보유종목_사전;
        Dictionary<string, string> _종목코드의단계_사전;

        long _자동매매_매수금액 = 500000;
        int _자동매매_동시종목개수 = 2;
        int _자동매매_보유종목개수 = 0;
        int _자동매매_매수시점 = 1;
        double _자동매매_주도주익절시점 = 10.0;
        double _자동매매_익절시점 = 10.0;
        double _자동매매_손절시점 = -2.5;

        private Dictionary<int, string> _오류코드의오류메시지_사전;

        public 주식종합_Form()
        {
            InitializeComponent();
            _레드셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            _그린셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            _블루셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            _레드셀스타일.ForeColor = Color.White;
            _그린셀스타일.ForeColor = Color.Black;
            _블루셀스타일.ForeColor = Color.White;

            _강한레드셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            _강한그린셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            _강한블루셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            _강한레드셀스타일.ForeColor = Color.White;
            _강한그린셀스타일.ForeColor = Color.White;
            _강한블루셀스타일.ForeColor = Color.White;

            _약한레드셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            _약한그린셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            _약한블루셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            _약한레드셀스타일.ForeColor = Color.Black;
            _약한그린셀스타일.ForeColor = Color.Black;
            _약한블루셀스타일.ForeColor = Color.Black;

            _블랙셀스타일.BackColor = Color.Black;
            _블랙셀스타일.ForeColor = Color.White;

            _코스피셀스타일.BackColor = Color.Black;
            _코스피셀스타일.ForeColor = Color.Gold;

            _코스닥셀스타일.BackColor = Color.Black;
            _코스닥셀스타일.ForeColor = Color.DarkOrange;

            _종목코드의추천종목_사전 = new Dictionary<string, 매수추천종목>();
            _종목코드의보유종목_사전 = new Dictionary<string, 보유종목>();
            _종목코드의단계_사전 = new Dictionary<string, string>();

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

        private void 주식종합_Form_Load(object sender, EventArgs e)
        {
            _로봇종목코드_목록 = new List<string>();
            for (var i = 0; i < _로봇개수; i++)
            {
                _로봇종목코드_목록.Add("");
            }

            /* 자동매매 체크여부 확인 및 계좌번호 추가 */
            bool 자동매매 = 자동매매_checkBox.Checked;
            if (자동매매 == true)
            {
                자동매매_설정완료();
                _주식종합이야.Invoke(new Action(() => {
                    로그_listBox.Items.Add("자동매매 시작합니다.");
                }));
            }
            string accountList = axKHOpenAPI1.GetLoginInfo("ACCLIST");
            string[] accountArray = accountList.Split(';');
            bool isFirst = true;

            for (int i = 0; i < accountArray.Length; i++)
            {
                accountArray[i] = accountArray[i].Trim();
                if (accountArray[i] != "")
                {
                    계좌번호_comboBox.Items.Add(accountArray[i]);
                    if (isFirst == true)
                    {
                        isFirst = false;
                        계좌번호_comboBox.SelectedItem = accountArray[i];
                    }
                }
            }
            axKHOpenAPI1.SetInputValue("계좌번호", 계좌번호_comboBox.Text);
            axKHOpenAPI1.SetInputValue("비밀번호", "");
            axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");
            axKHOpenAPI1.SetInputValue("조회구분", "2");
            int 결과코드 = axKHOpenAPI1.CommRqData("계좌평가잔고내역", "opw00018", 0, "0018");

            _주식종합이야.Invoke(new Action(() => {
                _자동매매_매수금액 = (long)자동매매_매수금액_numericUpDown.Value;
                _자동매매_동시종목개수 = (int)자동매매_동시종목개수_numericUpDown.Value;
                _자동매매_매수시점 = 매수시점_알려줘(자동매매_매수시점_comboBox.Text);
                _자동매매_주도주익절시점 = (double)자동매매_주도주익절시점_numericUpDown.Value / (double)10;
                _자동매매_익절시점 = (double)자동매매_익절시점_numericUpDown.Value / (double)10;
                _자동매매_손절시점 = (double)자동매매_손절시점_numericUpDown.Value / (double)10;
            }));
        }

        public void 주식종합_Form_저장해줘(주식종합_Form 주식종합이야)
        {
            _주식종합이야 = 주식종합이야;
        }

        public void 실시간스레드_멈춰줘()
        {
            _실시간스레드.Abort();
        }

        public void 시작하자(AxKHOpenAPILib.AxKHOpenAPI _axKHOpenAPI1)
        {
            axKHOpenAPI1 = _axKHOpenAPI1;
            _실시간스레드 = new Thread(delegate ()
            {
                while (true)
                {
                    try
                    {
                        if (IsHandleCreated)
                        {
                            /*if (_작업중이니 == false)
                            {
                                _주식종합이야.Invoke(new Action(() => {
                                }));
                                //Thread.Sleep(_실시간주기);
                            }*/
                            while (요청작업큐.Count > 0)
                            {
                                요청작업큐.Dequeue().RunSynchronously();
                                Thread.Sleep(실시간주기);
                            }
                        }
                        Thread.Sleep(100);
                    }
                    catch (Exception EX)
                    {
                        Console.WriteLine("당일매수추천종목목록 실시간스레드 오류입니다. EX: {0}", EX);
                    }
                }
            });
            _실시간스레드.IsBackground = true;
            _실시간스레드.Start();
        }

        public void 트랜잭션_요청해줘(Task 작업)
        {
            요청작업큐.Enqueue(작업);
        }

        public void 빌리언스탁_저장해줘(빌리언스탁_Form 빌리언스탁)
        {
            _빌리언스탁 = 빌리언스탁;
        }

        private void 종목검색 ()
        {
            string 검색내용 = 종목검색_textBox.Text;
            if (검색내용 == "")
            {
                MessageBox.Show(String.Format("{0}는 존재하지 않는 종목입니다.\n다시 시도하세요.", 검색내용), "존재하지 않는 종목", MessageBoxButtons.OK);
                return;
            }

            /*
            for (int ㄱ = 0; ㄱ < _로봇개수; ㄱ++)
            {
                if (_로봇종목코드_목록[ㄱ] == "")
                {
                    선택된로봇_인덱스 = ㄱ;
                    break;
                }
            }
            */
            int 선택된로봇_인덱스 = 0;
            string 선택된스크린번호 = "";
            string 문자_임시 = "";

            if (선택된로봇_인덱스 != -1)
            {
                _로봇종목코드_목록[선택된로봇_인덱스] = 검색내용;
                선택된스크린번호 = String.Format("{0}", 2000 + 선택된로봇_인덱스);

                트랜잭션_요청해줘(new Task(() => {
                    axKHOpenAPI1.DisconnectRealData(선택된스크린번호);
                    axKHOpenAPI1.SetInputValue("종목코드", 검색내용);
                    int 결과코드 = axKHOpenAPI1.CommRqData("주식호가요청", "opt10004", 0, 선택된스크린번호);
                    Console.WriteLine(DateTime.Now + " - 주식종합에서 종목코드 " + 검색내용 + " 주식호가요청합니다.");
                }));
                /*
                axKHOpenAPI1.DisconnectRealData(선택된스크린번호);
                axKHOpenAPI1.SetInputValue("종목코드", 검색내용);
                int 결과코드 = axKHOpenAPI1.CommRqData("주식호가요청", "opt10004", 0, 선택된스크린번호);
                Console.WriteLine(DateTime.Now + " - 주식종합에서 종목코드 " + 검색내용 + " 주식호가요청합니다.");
                */

                _주식종합이야.Invoke(new Action(() => {
                    로그_listBox.Items.Add(DateTime.Now + " - 주식종합에서 종목코드 " + 검색내용 + " 주식호가요청합니다.");
                }));

                /*if (_작업중이니 == true) { return; } _작업중이니 = true;*/

                var builder = Builders<종목>.Filter;
                var query = builder.Eq(x => x._종목코드, 검색내용) | builder.Eq(x => x._종목명, 검색내용);
                var result = _종목컬렉션.Find(query).Limit(1).ToList();
                _선택된종목아 = result.FirstOrDefault();
                if (_선택된종목아 != null)
                {
                    _주식종합이야.Invoke(new Action(() => {
                        선택된종목명_label.Text = _선택된종목아._종목명;
                        매수_종목코드_textBox.Text = 매도_종목코드_textBox.Text = 정정_종목코드_textBox.Text = 선택된종목코드_label.Text = _선택된종목아._종목코드;

                        문자_임시 = Math.Floor((double)_자동매매_매수금액 / (double)(_선택된종목아._현재가 + 1)) + "";
                        매수_주문가격_numericUpDown.Value = 매도_주문가격_numericUpDown.Value = 정정_주문가격_numericUpDown.Value = _선택된종목아._현재가;
                        매수_주문수량_numericUpDown.Value = 매도_주문수량_numericUpDown.Value = 정정_주문수량_numericUpDown.Value = Int32.Parse(문자_임시);

                    }));
                }
                else
                {
                    로그_listBox.Items.Add(String.Format("{0}는 존재하지 않는 종목입니다.\n다시 시도하세요.", 검색내용));
                    MessageBox.Show(String.Format("{0}는 존재하지 않는 종목입니다.\n다시 시도하세요.", 검색내용), "존재하지 않는 종목", MessageBoxButtons.OK);
                }
            }
            else
            {
                Console.WriteLine("모든 로봇이 현재 매수중입니다. 기다려주세요. = {0}", DateTime.Now);
                로그_listBox.Items.Add(String.Format("모든 로봇이 현재 매수중입니다.기다려주세요. = {0}", DateTime.Now));

                // 로봇 초기화
                string 종목코드;
                for (int i = 0; i < _로봇종목코드_목록.Count; i++)
                {
                    선택된스크린번호 = String.Format("{0}", (2000 + i));
                    종목코드 = _로봇종목코드_목록[i];
                    트랜잭션_요청해줘(new Task(() => {
                        axKHOpenAPI1.SetRealRemove(선택된스크린번호, 종목코드);
                        Console.WriteLine(String.Format("로봇 초기화 위해 화면번호 {0} = 종목코드 {1} 시세해지하였습니다.\r\n\r\n", 선택된스크린번호, 종목코드));
                    }));
                    _로봇종목코드_목록[i] = "";
                }
            }
        }

        private void 종목검색_button_Click(object sender, EventArgs e)
        {
            bool 자동매매 = 자동매매_checkBox.Checked;
            if (자동매매 == true)
            {
                MessageBox.Show(String.Format("자동매매 시, 종목검색 사용 불가합니다.\n자동매매 비활성화 후 다시 시도하세요."), "자동매매 사용중", MessageBoxButtons.OK);
                return;
            }
            종목검색();
        }

        /*  Form1 - OnReceiveRealData에서 주식호가 업데이트 위해 호출. 종목코드 같을 경우, 업데이트. */
        public string 선택된종목코드_알려줘 ()
        {
            return 선택된종목코드_label.Text;
        }

        /*
         유형: 0 = 트랜잭션 | 1 = 실시간
         */
        public void 주식호가_업데이트해줘 (int 유형, Object 주식호가잔량아)
        {
            /*
            try
            {
                bool 실시간호가 = 실시간호가_checkBox.Checked;
                if (실시간호가 == false) return;

                if (_작업중이니 == true) return;
                _작업중이니 = true;

                주식호가잔량 최신주식호가잔량아 = (주식호가잔량)주식호가잔량아;
                if (
                    유형 == 0  ||
                    최신주식호가잔량아._종목코드 == "" ||
                    최신주식호가잔량아._종목코드 == null
                    ) {
                    최신주식호가잔량아._종목코드 = _선택된종목아._종목코드;
                }

                Console.WriteLine(DateTime.Now + " - 유형: " + 유형 + ", 호가 받고있는 종목코드: " + 최신주식호가잔량아._종목코드 + "\r\n");

                _주식종합이야.Invoke(new Action(() => {
                    로그_listBox.Items.Add(DateTime.Now + " - 유형: " + 유형 + ", 호가 받고있는 종목코드: " + 최신주식호가잔량아._종목코드);
                }));

                최신주식호가잔량아._생성날짜 = 날짜_알려줘();
                string 문자_임시 = "";
                string 호가시간_임시 = 최신주식호가잔량아._호가시간.ToString();
                string 호가시간;
                if (호가시간_임시 == "0")
                {
                    호가시간_임시 = "000000";
                }
                호가시간 = 호가시간_임시.Length > 5 ? String.Format("{0}:{1}:{2}", 호가시간_임시.Substring(0, 2), 호가시간_임시.Substring(2, 2), 호가시간_임시.Substring(4, 2)) : String.Format("{0}:{1}:{2}", 호가시간_임시.Substring(0, 1), 호가시간_임시.Substring(1, 2), 호가시간_임시.Substring(3, 2));
                
                _주식종합이야.Invoke(new Action(() => {
                    호가_시간_label.Text = 호가시간;
                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가10);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격10_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격10_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가9 < 0)
                            {
                                호가_매도가격10_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격10_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격10_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격10_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격10_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격10_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격10_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가9);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격9_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격9_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가8 < 0)
                            {
                                호가_매도가격9_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격9_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격9_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격9_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격9_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격9_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격9_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가8);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격8_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격8_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가7 < 0)
                            {
                                호가_매도가격8_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격8_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격8_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격8_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격8_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격8_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격8_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가7);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격7_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격7_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가6 < 0)
                            {
                                호가_매도가격7_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격7_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격7_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격7_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격7_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격7_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격7_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가6);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격6_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격6_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가5 < 0)
                            {
                                호가_매도가격6_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격6_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격6_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격6_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격6_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격6_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격6_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가5);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격5_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격5_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가4 < 0)
                            {
                                호가_매도가격5_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격5_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격5_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격5_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격5_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격5_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격5_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가4);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격4_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격4_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가3 < 0)
                            {
                                호가_매도가격4_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격4_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격4_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격4_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격4_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격4_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격4_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가3);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격3_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격3_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가2 < 0)
                            {
                                호가_매도가격3_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격3_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격3_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격3_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격3_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격3_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격3_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가2);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격2_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격2_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매도호가1 < 0)
                            {
                                호가_매도가격2_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격2_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격2_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격2_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격2_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격2_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격2_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가1);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도가격1_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도가격1_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가1 < 0)
                            {
                                호가_매도가격1_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매도가격1_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매도가격1_label.BackColor = _레드셀스타일.BackColor;
                                호가_매도가격1_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매도가격1_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도가격1_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도가격1_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가10);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격10_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격10_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            Console.WriteLine(String.Format("최신주식호가잔량아._매수호가10: {0}", 최신주식호가잔량아._매수호가10));
                            Console.WriteLine(String.Format("_선택된종목아._현재가: {0}", _선택된종목아._현재가));

                            if (_선택된종목아._현재가 == 최신주식호가잔량아._매수호가10)
                            {
                                호가_매수가격10_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격10_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격10_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격10_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격10_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격10_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격10_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가9);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격9_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격9_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가10 < 0)
                            {
                                호가_매수가격9_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격9_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격9_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격9_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격9_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격9_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격9_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가8);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격8_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격8_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가9 < 0)
                            {
                                호가_매수가격8_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격8_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격8_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격8_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격8_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격8_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격8_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가7);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격7_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격7_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가8 < 0)
                            {
                                호가_매수가격7_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격7_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격7_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격7_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격7_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격7_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격7_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가6);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격6_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격6_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가7 < 0)
                            {
                                호가_매수가격6_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격6_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격6_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격6_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격6_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격6_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격6_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가5);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격5_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격5_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가6 < 0)
                            {
                                호가_매수가격5_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격5_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격5_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격5_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격5_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격5_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격5_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가4);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격4_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격4_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가5 < 0)
                            {
                                호가_매수가격4_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격4_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격4_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격4_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격4_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격4_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격4_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가3);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격3_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격3_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가4 < 0)
                            {
                                호가_매수가격3_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격3_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격3_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격3_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격3_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격3_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격3_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가2);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격2_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격2_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가3 < 0)
                            {
                                호가_매수가격2_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격2_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격2_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격2_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격2_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격2_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격2_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가1);
                    if (문자_임시.Length > 0)
                    {
                        Console.WriteLine("호가_매수가격1 = " + 문자_임시.Substring(0));

                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수가격1_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수가격1_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            if (최신주식호가잔량아._매수호가2 < 0)
                            {
                                호가_매수가격1_label.BackColor = _블랙셀스타일.BackColor;
                                호가_매수가격1_label.ForeColor = _블랙셀스타일.ForeColor;
                            }
                            else
                            {
                                호가_매수가격1_label.BackColor = _레드셀스타일.BackColor;
                                호가_매수가격1_label.ForeColor = _레드셀스타일.ForeColor;
                            }
                        }
                    }
                    else
                    {
                        호가_매수가격1_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수가격1_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수가격1_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량10);
                    호가_매도잔량10_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량9);
                    호가_매도잔량9_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량8);
                    호가_매도잔량8_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량7);
                    호가_매도잔량7_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량6);
                    호가_매도잔량6_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량5);
                    호가_매도잔량5_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량4);
                    호가_매도잔량4_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량3);
                    호가_매도잔량3_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량2);
                    호가_매도잔량2_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가수량1);
                    호가_매도잔량1_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량10);
                    호가_매수잔량10_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량9);
                    호가_매수잔량9_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량8);
                    호가_매수잔량8_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량7);
                    호가_매수잔량7_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량6);
                    호가_매수잔량6_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량5);
                    호가_매수잔량5_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량4);
                    호가_매수잔량4_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량3);
                    호가_매수잔량3_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량2);
                    호가_매수잔량2_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가수량1);
                    호가_매수잔량1_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비10);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전10_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전10_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전10_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전10_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전10_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전10_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전10_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비9);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전9_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전9_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전9_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전9_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전9_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전9_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전9_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비8);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전8_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전8_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전8_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전8_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전8_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전8_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전8_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비7);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전7_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전7_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전7_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전7_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전7_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전7_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전7_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비6);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전6_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전6_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전6_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전6_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전6_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전6_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전6_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비5);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전5_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전5_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전5_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전5_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전5_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전5_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전5_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비4);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전4_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전4_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전4_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전4_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전4_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전4_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전4_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비3);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전3_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전3_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전3_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전3_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전3_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전3_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전3_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비2);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전2_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전2_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전2_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전2_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전2_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전2_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전2_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가직전대비1);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도직전1_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도직전1_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도직전1_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도직전1_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도직전1_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도직전1_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도직전1_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비10);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전10_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전10_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전10_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전10_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전10_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전10_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전10_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비9);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전9_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전9_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전9_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전9_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전9_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전9_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전9_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비8);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전8_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전8_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전8_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전8_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전8_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전8_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전8_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비7);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전7_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전7_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전7_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전7_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전7_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전7_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전7_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비6);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전6_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전6_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전6_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전6_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전6_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전6_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전6_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비5);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전5_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전5_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전5_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전5_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전5_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전5_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전5_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비4);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전4_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전4_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전4_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전4_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전4_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전4_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전4_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비3);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전3_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전3_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전3_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전3_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전3_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전3_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전3_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비2);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전2_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전2_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전2_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전2_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전2_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전2_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전2_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가직전대비1);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수직전1_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수직전1_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수직전1_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수직전1_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수직전1_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수직전1_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수직전1_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매도호가총잔량);
                    if (문자_임시.Length > 0)
                    {
                        호가_매도총잔량_label.BackColor = _블루셀스타일.BackColor;
                        호가_매도총잔량_label.ForeColor = _블루셀스타일.ForeColor;
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                    }
                    else
                    {
                        호가_매도총잔량_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도총잔량_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도총잔량_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,0}", 최신주식호가잔량아._매수호가총잔량);
                    if (문자_임시.Length > 0)
                    {
                        호가_매수총잔량_label.BackColor = _레드셀스타일.BackColor;
                        호가_매수총잔량_label.ForeColor = _레드셀스타일.ForeColor;
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                    }
                    else
                    {
                        호가_매수총잔량_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수총잔량_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수총잔량_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매도호가총잔량직전대비);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매도총직전_label.BackColor = _블루셀스타일.BackColor;
                            호가_매도총직전_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매도총직전_label.BackColor = _레드셀스타일.BackColor;
                            호가_매도총직전_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매도총직전_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매도총직전_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매도총직전_label.Text = 문자_임시;

                    문자_임시 = String.Format("{0:#,#}", 최신주식호가잔량아._매수호가총잔량직전대비);
                    if (문자_임시.Length > 0)
                    {
                        if (문자_임시.Substring(0, 1).Equals("-"))
                        {
                            호가_매수총직전_label.BackColor = _블루셀스타일.BackColor;
                            호가_매수총직전_label.ForeColor = _블루셀스타일.ForeColor;
                            문자_임시 = 문자_임시.Replace("-", "");
                        }
                        else
                        {
                            호가_매수총직전_label.BackColor = _레드셀스타일.BackColor;
                            호가_매수총직전_label.ForeColor = _레드셀스타일.ForeColor;
                        }
                    }
                    else
                    {
                        호가_매수총직전_label.BackColor = _블랙셀스타일.BackColor;
                        호가_매수총직전_label.ForeColor = _블랙셀스타일.ForeColor;
                    }
                    호가_매수총직전_label.Text = 문자_임시;

                    문자_임시 = 호가_매도가격1_label.Text;
                    int 매도가격1 = Int32.Parse(문자_임시.Replace(",", ""));
                    문자_임시 = 호가_매수가격1_label.Text;
                    int 매수가격1 = Int32.Parse(문자_임시.Replace(",", ""));

                    문자_임시 = 호가_매수가격1_label.Text;
                    int 정정가격1 = Int32.Parse(문자_임시.Replace(",", ""));

                    매수_주문가격_numericUpDown.Value = 매수가격1;
                    매도_주문가격_numericUpDown.Value = 매도가격1;
                    정정_주문가격_numericUpDown.Value = 매수가격1;

                    문자_임시 = Math.Floor((double)_자동매매_매수금액 / (double)(매수가격1 + 1)) + "";
                    매수_주문수량_numericUpDown.Value = 매도_주문수량_numericUpDown.Value = 정정_주문수량_numericUpDown.Value = Int32.Parse( 문자_임시 );
                }));
                _작업중이니 = false;
            } catch (Exception EX)
            {
                _작업중이니 = false;
                Console.WriteLine("주식호가_업데이트해줘 Exception: {0}", EX);
            }
            */
        }

        private int 날짜_알려줘()
        {
            DateTime date = DateTime.Now;
            int 연도 = date.Year;
            int 월 = date.Month;
            int 일 = date.Day;
            string 날짜 = String.Format("{0}{1}{2}", 연도, 월 < 10 ? ("0" + 월) : ("" + 월), (일 < 10 ? ("0" + 일) : ("" + 일)));
            return Int32.Parse(날짜);
        }

        private int 시간_알려줘()
        {
            DateTime date = DateTime.Now;
            int 시 = date.Hour;
            int 분 = date.Minute;
            int 초 = date.Second;
            string 시간 = String.Format("{0}{1}{2}", 시, (분 < 10 ? ("0" + 분) : ("" + 분)), (초 < 10 ? ("0" + 초) : ("" + 초)));
            return Int32.Parse(시간);
        }

        public void 접속중_변경해줘(string 접속중)
        {
            _주식종합이야.Invoke(new Action(() => { 접속중_label.Text = 접속중; }));
        }

        public void 로그_찍어줘 (string 로그내용)
        {
            _주식종합이야.Invoke(new Action(() => { 로그_listBox.Items.Add(로그내용);}));
        }

        public void 매수하자 ()
        {
            _주식종합이야.Invoke(new Action(() => {
                로그_listBox.Items.Add("현금매수 버튼을 클릭하였습니다.");
                string 사용자구분명 = "주식주문";
                string 화면번호 = "8844";
                string 계좌번호 = 계좌번호_comboBox.Text;
                int 주문유형 = 주문유형코드_알려줘(매수_주문유형_comboBox.Text);
                string 종목코드 = 매수_종목코드_textBox.Text;
                int 주문수량 = (int)매수_주문수량_numericUpDown.Value;
                int 주문가격 = (int)매수_주문가격_numericUpDown.Value;
                string 거래유형 = 거래유형코드_알려줘(매수_거래유형_comboBox.Text);
                string 원주문번호 = ""; //원주문번호_textBox.Text;
                int 주문요청결과;
                if (거래유형 == "03")
                {
                    주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 0, 거래유형, 원주문번호);
                } else
                {
                    주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 주문가격, 거래유형, 원주문번호);
                }

                if (주문요청결과 == 0)
                {
                    Console.WriteLine("현금매수 주문을 요청하였습니다.");
                    로그_listBox.Items.Add("현금매수 주문을 요청하였습니다.");
                    자동매매로그_listBox.Items.Add(String.Format("{0} : [{1}] 종목코드: {2}, 주문수량: {3}, 주문가격: {4}, 거래유형: {5}", DateTime.Now, 매수_주문유형_comboBox.Text, 종목코드, 주문수량, 주문가격, 매수_거래유형_comboBox.Text));
                    _빌리언스탁.효과음_들려줘("매수접수");

                    var builder = Builders<유보종목>.Filter;
                    var query = builder.Eq(x => x._종목코드, 종목코드);
                    var result = _유보종목컬렉션.Find(query).Limit(1).ToList();
                    유보종목 유보종목아 = result.FirstOrDefault();
                    if (유보종목아 == null)
                    {
                        유보종목아 = new 유보종목();
                        유보종목아._종목코드 = 종목코드;
                        _유보종목컬렉션.InsertOne(유보종목아);
                    }
                }
                else
                {
                    Console.WriteLine("현금매수 주문을 요청 실패 하였습니다.");
                    로그_listBox.Items.Add("현금매수 주문을 요청 실패 하였습니다.");
                }
            }));
        }

        public void 매도하자()
        {
            _주식종합이야.Invoke(new Action(() => {
                로그_listBox.Items.Add("현금매도 버튼을 클릭하였습니다.");
                string 사용자구분명 = "주식주문";
                string 화면번호 = "8844";
                string 계좌번호 = 계좌번호_comboBox.Text;
                int 주문유형 = 주문유형코드_알려줘(매도_주문유형_comboBox.Text);
                string 종목코드 = 매도_종목코드_textBox.Text;
                int 주문수량 = (int)매도_주문수량_numericUpDown.Value;
                int 주문가격 = (int)매도_주문가격_numericUpDown.Value;
                string 거래유형 = 거래유형코드_알려줘(매도_거래유형_comboBox.Text);
                string 원주문번호 = ""; //원주문번호_textBox.Text;
                int 주문요청결과;
                if (거래유형 == "03")
                {
                    주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 0, 거래유형, 원주문번호);
                }
                else
                {
                    주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 주문가격, 거래유형, 원주문번호);
                }
                if (주문요청결과 == 0)
                {
                    Console.WriteLine("현금매도 주문을 요청하였습니다.");
                    로그_listBox.Items.Add("현금매도 주문을 요청하였습니다.");
                    자동매매로그_listBox.Items.Add(String.Format("{0} : [{1}] 종목코드: {2}, 주문수량: {3}, 주문가격: {4}, 거래유형: {5}", DateTime.Now, 매도_주문유형_comboBox.Text, 종목코드, 주문수량, 주문가격, 매도_거래유형_comboBox.Text));
                    _빌리언스탁.효과음_들려줘("매도접수");
                    if (_종목코드의보유종목_사전.ContainsKey(종목코드) == true)
                    {
                        _종목코드의보유종목_사전.Remove(종목코드);
                    }

                }
                else
                {
                    Console.WriteLine("현금매도 주문을 요청 실패 하였습니다.");
                    로그_listBox.Items.Add("현금매도 주문을 요청 실패 하였습니다.");
                }
            }));
        }

        public void 정정하자()
        {
            _주식종합이야.Invoke(new Action(() => {
                int 주문유형 = 주문유형코드_알려줘(정정_주문유형_comboBox.Text);

                if (주문유형 == 3 || 주문유형 == 4)
                {
                    로그_listBox.Items.Add("정정 버튼을 클릭하였습니다. 매수취소/매도취소를 하시려면 취소 버튼을 클릭하세요.");
                    return;
                }
                
                로그_listBox.Items.Add("정정 버튼을 클릭하였습니다.");
                string 사용자구분명 = "주식주문";
                string 화면번호 = "8844";
                string 계좌번호 = 계좌번호_comboBox.Text;
                
                string 종목코드 = 정정_종목코드_textBox.Text;
                int 주문수량 = (int)정정_주문수량_numericUpDown.Value;
                int 주문가격 = (int)정정_주문가격_numericUpDown.Value;
                string 거래유형 = 거래유형코드_알려줘(정정_거래유형_comboBox.Text);
                string 원주문번호 = 정정_원주문번호_textBox.Text;
                int 주문요청결과;
                if (거래유형 == "03")
                {
                    주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 0, 거래유형, 원주문번호);
                }
                else
                {
                    주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 주문가격, 거래유형, 원주문번호);
                }
                if (주문요청결과 == 0)
                {
                    Console.WriteLine("정정 주문을 요청하였습니다.");
                    로그_listBox.Items.Add("정정 주문을 요청하였습니다.");
                    자동매매로그_listBox.Items.Add(String.Format("{0} : [{1}] 종목코드: {2}, 주문수량: {3}, 주문가격: {4}, 거래유형: {5}, 원주문번호: {6}", DateTime.Now, 정정_주문유형_comboBox.Text, 종목코드, 주문수량, 주문가격, 정정_거래유형_comboBox.Text, 원주문번호));
                    _빌리언스탁.효과음_들려줘("정정접수");
                    if (주문유형 == 6 && _종목코드의보유종목_사전.ContainsKey(종목코드) == true)
                    {
                        _종목코드의보유종목_사전.Remove(종목코드);
                    } else if (주문유형 == 5)
                    {
                        var builder = Builders<유보종목>.Filter;
                        var query = builder.Eq(x => x._종목코드, 종목코드);
                        var result = _유보종목컬렉션.Find(query).Limit(1).ToList();
                        유보종목 유보종목아 = result.FirstOrDefault();
                        if (유보종목아 == null)
                        {
                            유보종목아 = new 유보종목();
                            유보종목아._종목코드 = 종목코드;
                            _유보종목컬렉션.InsertOne(유보종목아);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("정정 주문을 요청 실패 하였습니다.");
                    로그_listBox.Items.Add("정정 주문을 요청 실패 하였습니다.");
                }
            }));
        }

        public void 취소하자()
        {
            _주식종합이야.Invoke(new Action(() => {
                int 주문유형 = 주문유형코드_알려줘(정정_주문유형_comboBox.Text);

                if (주문유형 == 5 || 주문유형 == 6)
                {
                    로그_listBox.Items.Add("취소 버튼을 클릭하였습니다. 매수정정/매도정정을 하시려면 정정 버튼을 클릭하세요.");
                    return;
                }

                로그_listBox.Items.Add("취소 버튼을 클릭하였습니다.");
                string 사용자구분명 = "주식주문";
                string 화면번호 = "8844";
                string 계좌번호 = 계좌번호_comboBox.Text;

                string 종목코드 = 정정_종목코드_textBox.Text;
                int 주문수량 = (int)정정_주문수량_numericUpDown.Value;
                int 주문가격 = (int)정정_주문가격_numericUpDown.Value;
                string 거래유형 = 거래유형코드_알려줘(정정_거래유형_comboBox.Text);
                string 원주문번호 = 정정_원주문번호_textBox.Text;
                int 주문요청결과;
                if (거래유형 == "03")
                {
                    주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 0, 거래유형, 원주문번호);
                }
                else
                {
                    주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 주문가격, 거래유형, 원주문번호);
                }
                if (주문요청결과 == 0)
                {
                    Console.WriteLine("취소 주문을 요청하였습니다.");
                    로그_listBox.Items.Add("취소 주문을 요청하였습니다.");
                    자동매매로그_listBox.Items.Add(String.Format("{0} : [{1}] 종목코드: {2}, 주문수량: {3}, 주문가격: {4}, 거래유형: {5}, 원주문번호: {6}", DateTime.Now, 정정_주문유형_comboBox.Text, 종목코드, 주문수량, 주문가격, 정정_거래유형_comboBox.Text, 원주문번호));
                    _빌리언스탁.효과음_들려줘("취소접수");
                }
                else
                {
                    Console.WriteLine("취소 주문을 요청 실패 하였습니다.");
                    로그_listBox.Items.Add("취소 주문을 요청 실패 하였습니다.");
                }
            }));
        }

        private void 매수_button_Click(object sender, EventArgs e)
        {
            매수하자();
        }

        private void 매도_button_Click(object sender, EventArgs e)
        {
            매도하자();
        }

        private void 정정_button_Click(object sender, EventArgs e)
        {
            정정하자();
        }

        private void 취소_button_Click(object sender, EventArgs e)
        {
            취소하자();
        }

        /* 효과음 플레이 */
        public void 효과음_들려줘(string 명령어)
        {
            string 효과음;
            if (명령어 == "환영")
            {
                효과음 = @"D:\\soundPlayer\\start2.wav";
            }
            else if (명령어 == "딩동")
            {
                효과음 = @"D:\\soundPlayer\\bell.wav";
            }
            else if (명령어 == "작업끝")
            {
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

        private int 주문유형코드_알려줘(string 주문유형)
        {
            switch (주문유형)
            {
                case "신규매수":
                    return 1;
                case "신규매도":
                    return 2;
                case "매수취소":
                    return 3;
                case "매도취소":
                    return 4;
                case "매수정정":
                    return 5;
                case "매도정정":
                    return 6;
                default:
                    return -1;
            }
        }

        private string 거래유형코드_알려줘(string 거래유형)
        {
            switch (거래유형)
            {
                case "지정가":
                    return "00";
                case "시장가":
                    return "03";
                case "조건부지정가":
                    return "05";
                case "최유리지정가":
                    return "06";
                case "최우선지정가":
                    return "07";
                case "지정가IOC":
                    return "10";
                case "시장가IOC":
                    return "13";
                case "최유리IOC":
                    return "16";
                case "지정가FOK":
                    return "20";
                case "시장가FOK":
                    return "23";
                case "최유리FOK":
                    return "26";
                case "장전시간외종가":
                    return "61";
                case "시간외단일가매매":
                    return "62";
                case "장후시간외종가":
                    return "81";
                default:
                    return "";
            }
        }

        private int 매수시점_알려줘(string 매수시점)
        {
            switch (매수시점)
            {
                case "새종목추천":
                    return 1;
                case "세력매수대금유입":
                    return 2;
                case "주도주만":
                    return 3;
                default:
                    return -1;
            }
        }

        private int 매도시점_알려줘(string 매도시점)
        {
            switch (매도시점)
            {
                case "고점대비틱개수-2":
                    return 2;
                case "고점대비틱개수-7":
                    return 7;
                case "고점대비틱개수-9":
                    return 9;
                case "고점대비틱개수-10":
                    return 10;
                case "고점대비-1%+틱개수-2":
                    return 12;
                case "고점대비-2%+틱개수-2":
                    return 22;
                case "고점대비-3%+틱개수-2":
                    return 32;
                case "지금당장":
                    return 0;
                default:
                    return -1;
            }
        }

        public void 평가손익금액_label_ChangeForeColor(Color _color)
        {
            _주식종합이야.Invoke(new Action(() => {
                평가손익금액_label.ForeColor = _color;
            }));
        }
        public void 수익률_label_ChangeForeColor(Color _color)
        {
            _주식종합이야.Invoke(new Action(() => {
                수익률_label.ForeColor = _color;
            }));
        }
        public void 매입금액_label_ChangeText(String _string)
        {
            _주식종합이야.Invoke(new Action(() => {
                매입금액_label.Text = _string;
            }));
        }
        public void 평가금액_label_ChangeText(String _string)
        {
            _주식종합이야.Invoke(new Action(() => {
                평가금액_label.Text = _string;
            }));
        }
        public void 평가손익금액_label_ChangeText(String _string)
        {
            _주식종합이야.Invoke(new Action(() => {
                평가손익금액_label.Text = _string;
            }));
        }
        public void 수익률_label_ChangeText(String _string)
        {
            try
            {
                _string = _string.Replace("%", "");
                Console.WriteLine("수익률_label_ChangeText - _string: " + _string);
                double 수익률 = _string == "" ? 0 : double.Parse(_string);
                수익률 = 수익률 / (double)100;

                _주식종합이야.Invoke(new Action(() =>
                {
                    수익률_label.Text = 수익률 + "%";
                }));
            } catch (Exception EX)
            {
                Console.WriteLine("수익률_label_ChangeText - EX: " + EX);
                _주식종합이야.Invoke(new Action(() =>
                {
                    수익률_label.Text = "0%";
                }));
            }
        }
        public void 추정예탁자산_label_ChangeText(String _string)
        {
            _주식종합이야.Invoke(new Action(() => {
                추정예탁자산_label.Text = _string;
            }));
        }
        public void 대출금_label_ChangeText(String _string)
        {
            _주식종합이야.Invoke(new Action(() => {
                대출금_label.Text = _string;
            }));
        }
        public void 융자금액_label_ChangeText(String _string)
        {
            _주식종합이야.Invoke(new Action(() => {
                융자금액_label.Text = _string;
            }));
        }
        public void 대주금액_label_ChangeText(String _string)
        {
            _주식종합이야.Invoke(new Action(() => {
                대주금액_label.Text = _string;
            }));
        }
        
        public void 보유종목_초기화해줘 ()
        {
            _자동매매_보유종목개수 = 0;
            보유종목_dataGridView.Rows.Clear();
        }

        public void 보유종목_업데이트해줘(
                            String 종목코드_임시
                            , String 종목명_임시
                            , String 평가손익_임시
                            , String 수익률_임시
                            , String 매입가_임시
                            , String 전일종가_임시
                            , String 보유수량_임시
                            , String 매매가능수량_임시
                            , String 현재가_임시
                            , String 전일매수수량_임시
                            , String 전일매도수량_임시
                            , String 금일매수수량_임시
                            , String 금일매도수량_임시
                            , String 매입금액_임시
                            , String 매입수수료_임시
                            , String 평가금액_임시
                            , String 평가수수료_임시
                            , String 세금_임시
                            , String 수수료합_임시
                            , String 보유비중_임시
                            , String 신용구분_임시
                            , String 신용구분명_임시
                            , String 대출일_임시
                            , int ㄱ
                            , int 보유종목개수)
        {
            _자동매매_보유종목개수 = 보유종목개수;

            if (ㄱ == 0) { 보유종목_dataGridView.Rows.Clear();  }

            if (보유수량_임시 != null && 보유수량_임시 != "0" && 보유수량_임시 != "")
            {
                보유종목_dataGridView.Rows.Add();

                bool 자동매매 = 자동매매_checkBox.Checked;

                string 종목코드 = 종목코드_임시.Length == 6 ? 종목코드_임시 : 종목코드_임시.Substring(종목코드_임시.Length - 6, 6);
                string 종목명 = 종목명_임시;
                long 평가손익 = long.Parse(평가손익_임시);
                double 수익률 = double.Parse(수익률_임시);
                수익률 = 수익률 / 100;

                int 매입가 = Int32.Parse(매입가_임시);
                int 전일종가 = Int32.Parse(전일종가_임시);
                long 보유수량 = long.Parse(보유수량_임시);
                long 매매가능수량 = long.Parse(매매가능수량_임시);
                int 현재가 = Int32.Parse(현재가_임시);
                /*long 전일매수수량 = long.Parse(전일매수수량_임시);
                long 전일매도수량 = long.Parse(전일매도수량_임시);
                long 금일매수수량 = long.Parse(금일매수수량_임시);
                long 금일매도수량 = long.Parse(금일매도수량_임시);*/
                long 매입금액 = long.Parse(매입금액_임시);
                long 매입수수료 = long.Parse(매입수수료_임시);
                long 평가금액 = long.Parse(평가금액_임시);
                long 평가수수료 = long.Parse(평가수수료_임시);
                long 세금 = long.Parse(세금_임시);
                long 수수료합 = long.Parse(수수료합_임시);
                double 보유비중 = double.Parse(보유비중_임시);
                int 신용구분 = Int32.Parse(신용구분_임시);
                string 신용구분명 = 신용구분명_임시;
                string 대출일 = 대출일_임시;

                보유종목 보유종목아 = new 보유종목();
                보유종목아._종목코드 = 종목코드;
                보유종목아._종목명 = 종목명;
                보유종목아._평가손익 = 평가손익;
                보유종목아._수익률 = 수익률;
                보유종목아._매입가 = 매입가;
                보유종목아._전일종가 = 전일종가;
                보유종목아._보유수량 = 보유수량;
                보유종목아._매매가능수량 = 매매가능수량;
                보유종목아._현재가 = 현재가;
                보유종목아._매입금액 = 매입금액;
                보유종목아._매입수수료 = 매입수수료;
                보유종목아._평가금액 = 평가금액;
                보유종목아._평가수수료 = 평가수수료;
                보유종목아._보유비중 = 보유비중;
                보유종목아._신용구분 = 신용구분;
                보유종목아._신용구분명 = 신용구분명;
                보유종목아._대출일 = 대출일;

                Console.WriteLine(DateTime.Now + " 보유종목_업데이트해줘 = 수익률: " + 수익률 + "\r\n");

                _종목코드의보유종목_사전[종목코드] = 보유종목아;

                double 익절시점 = _자동매매_익절시점;
                double 손절시점 = _자동매매_손절시점;

                int 순위 = -1;

                if (_종목코드의추천종목_사전.ContainsKey(종목코드))
                {
                    순위 = _종목코드의추천종목_사전[종목코드]._순위;
                }

                if (순위 == 1 || 순위 == 2)
                {
                    익절시점 = _자동매매_주도주익절시점;
                }

                if (자동매매 && (수익률 > 익절시점 || 수익률 < 손절시점))
                {
                    _주식종합이야.Invoke(new Action(() => {
                        로그_listBox.Items.Add("현재 수익률은 " + 수익률  + "이고, 종목 순위는 " + 순위 +  "위 입니다. 자동매매 현금매도 주문시작합니다. 종목코드: " + 종목코드);
                        string 사용자구분명 = "주식주문";
                        string 화면번호 = "8844";
                        string 계좌번호 = 계좌번호_comboBox.Text;
                        int 주문유형 = 주문유형코드_알려줘(매도_주문유형_comboBox.Text);
                        int 주문수량 = (int)보유수량;
                        int 주문가격 = 현재가;
                        string 거래유형 = 거래유형코드_알려줘(매도_거래유형_comboBox.Text);
                        string 원주문번호 = ""; //원주문번호_textBox.Text;
                        int 주문요청결과;
                        if (거래유형 == "03")
                        {
                            주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 0, 거래유형, 원주문번호);
                        }
                        else
                        {
                            주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 주문가격, 거래유형, 원주문번호);
                        }
                        if (주문요청결과 == 0)
                        {
                            Console.WriteLine("현금매도 주문을 요청하였습니다.");
                            로그_listBox.Items.Add("현금매도 주문을 요청하였습니다.");
                            자동매매로그_listBox.Items.Add(String.Format("{0} : [{1}] 종목코드: {2}, 주문수량: {3}, 주문가격: {4}, 거래유형: {5}", DateTime.Now, 매도_주문유형_comboBox.Text, 종목코드, 주문수량, 주문가격, 매도_거래유형_comboBox.Text));
                            _빌리언스탁.효과음_들려줘("매도접수");
                            if ( _종목코드의보유종목_사전.ContainsKey(종목코드) == true )
                            {
                                _종목코드의보유종목_사전.Remove(종목코드);
                            }
                        }
                        else
                        {
                            Console.WriteLine("현금매도 주문을 요청 실패 하였습니다.");
                            로그_listBox.Items.Add("현금매도 주문을 요청 실패 하였습니다.");
                        }
                    }));
                }
                보유종목_dataGridView["종목코드_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = 종목코드;

                보유종목_dataGridView["종목명_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = 종목명;

                보유종목_dataGridView["수익률_보유종목_DataGridViewTextBoxColumn", ㄱ].Style = 수익률 > 0 ? _레드셀스타일 : (수익률 < 0 ? _블루셀스타일 : _블랙셀스타일);
                보유종목_dataGridView["수익률_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.00}", 수익률);

                보유종목_dataGridView["보유수량_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 보유수량);

                보유종목_dataGridView["현재가_보유종목_DataGridViewTextBoxColumn", ㄱ].Style = 현재가 > 매입가 ? _레드셀스타일 : (현재가 < 매입가 ? _블루셀스타일 : _블랙셀스타일);
                보유종목_dataGridView["현재가_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 현재가);

                보유종목_dataGridView["매입가_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매입가);

                보유종목_dataGridView["전일종가_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 전일종가);

                보유종목_dataGridView["평가금액_보유종목_DataGridViewTextBoxColumn", ㄱ].Style = 평가금액 > 매입금액 ? _레드셀스타일 : (평가금액 < 매입금액 ? _블루셀스타일 : _블랙셀스타일);
                보유종목_dataGridView["평가금액_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 평가금액);

                보유종목_dataGridView["매입금액_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매입금액);

                보유종목_dataGridView["평가손익_보유종목_DataGridViewTextBoxColumn", ㄱ].Style = 평가손익 > 0 ? _레드셀스타일 : (평가손익 < 0 ? _블루셀스타일 : _블랙셀스타일);
                보유종목_dataGridView["평가손익_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 평가손익);

                보유종목_dataGridView["매매가능수량_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매매가능수량);

                보유종목_dataGridView["매입수수료_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매입수수료);

                보유종목_dataGridView["평가수수료_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 평가수수료);

                보유종목_dataGridView["세금_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 세금);

                보유종목_dataGridView["수수료합_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 수수료합);

                보유종목_dataGridView["보유비중_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.00}", 보유비중);

                보유종목_dataGridView["신용구분_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 신용구분);

                보유종목_dataGridView["신용구분명_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = 신용구분명;

                보유종목_dataGridView["대출일_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = 대출일;
            }
            
            /*
            자동매매종목_dataGridView["전일매수수량_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 전일매수수량);
            자동매매종목_dataGridView["전일매도수량_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 전일매도수량);
            자동매매종목_dataGridView["금일매수수량_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 금일매수수량);
            자동매매종목_dataGridView["금일매도수량_보유종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 금일매도수량);
            */
        }

        private void 호가_가격_label_Click(object sender, EventArgs e)
        {
            /*
            string 문자_임시 = "1,000";
            int 호가가격 = 1000;

            if (sender.Equals(호가_매도가격10_label))
            {
                문자_임시 = 호가_매도가격10_label.Text;
            }
            else if (sender.Equals(호가_매도가격9_label))
            {
                문자_임시 = 호가_매도가격9_label.Text;
            }
            else if (sender.Equals(호가_매도가격8_label))
            {
                문자_임시 = 호가_매도가격8_label.Text;
            }
            else if (sender.Equals(호가_매도가격7_label))
            {
                문자_임시 = 호가_매도가격7_label.Text;
            }
            else if (sender.Equals(호가_매도가격6_label))
            {
                문자_임시 = 호가_매도가격6_label.Text;
            }
            else if (sender.Equals(호가_매도가격5_label))
            {
                문자_임시 = 호가_매도가격5_label.Text;
            }
            else if (sender.Equals(호가_매도가격4_label))
            {
                문자_임시 = 호가_매도가격4_label.Text;
            }
            else if (sender.Equals(호가_매도가격3_label))
            {
                문자_임시 = 호가_매도가격3_label.Text;
            }
            else if (sender.Equals(호가_매도가격2_label))
            {
                문자_임시 = 호가_매도가격2_label.Text;
            }
            else if (sender.Equals(호가_매도가격1_label))
            {
                문자_임시 = 호가_매도가격1_label.Text;
            }
            else if (sender.Equals(호가_매수가격1_label))
            {
                문자_임시 = 호가_매수가격1_label.Text;
            }
            else if (sender.Equals(호가_매수가격2_label))
            {
                문자_임시 = 호가_매수가격2_label.Text;
            }
            else if (sender.Equals(호가_매수가격3_label))
            {
                문자_임시 = 호가_매수가격3_label.Text;
            }
            else if (sender.Equals(호가_매수가격4_label))
            {
                문자_임시 = 호가_매수가격4_label.Text;
            }
            else if (sender.Equals(호가_매수가격5_label))
            {
                문자_임시 = 호가_매수가격5_label.Text;
            }
            else if (sender.Equals(호가_매수가격6_label))
            {
                문자_임시 = 호가_매수가격6_label.Text;
            }
            else if (sender.Equals(호가_매수가격7_label))
            {
                문자_임시 = 호가_매수가격7_label.Text;
            }
            else if (sender.Equals(호가_매수가격8_label))
            {
                문자_임시 = 호가_매수가격8_label.Text;
            }
            else if (sender.Equals(호가_매수가격9_label))
            {
                문자_임시 = 호가_매수가격9_label.Text;
            }
            else if (sender.Equals(호가_매수가격10_label))
            {
                문자_임시 = 호가_매수가격10_label.Text;
            }
            호가가격 = Int32.Parse(문자_임시.Replace(",", ""));
            매수_주문가격_numericUpDown.Value = 매도_주문가격_numericUpDown.Value = 정정_주문가격_numericUpDown.Value = 호가가격;
            매수_주문수량_numericUpDown.Value = 매도_주문수량_numericUpDown.Value = 정정_주문수량_numericUpDown.Value = Int32.Parse(Math.Floor((double)_자동매매_매수금액 / (double)(호가가격 + 1)) + "");
            */
        }

        public string 새매수추천종목_매수해줘 (string 종목코드, 매수추천종목 매수추천종목아)
        {
            int 지금시간 = 시간_알려줘();
            bool 자동매매 = 자동매매_checkBox.Checked;
            string 단계 = "대기";
            
            if (
                /*
                 원래
                 (지금시간 > 090500 && 지금시간 < 100000) || 
                (지금시간 > 130000 && 지금시간 < 140000)
                 */
                (지금시간 > 090000 && 지금시간 < 150000)
                )
            {
                _주식종합이야.Invoke(new Action(() => {
                    로그_listBox.Items.Add(DateTime.Now + " 새매수추천종목_매수해줘 지금시간: " + 지금시간 + ", 종목코드: " + 종목코드);
                }));
                /* _종목코드의단계사전 */
                if (_종목코드의단계_사전.ContainsKey(종목코드) == true)
                {
                    단계 = "유보";
                }
                else
                {
                    var builder = Builders<유보종목>.Filter;
                    var query = builder.Eq(x => x._종목코드, 종목코드);
                    var result = _유보종목컬렉션.Find(query).Limit(1).ToList();
                    유보종목 유보종목아 = result.FirstOrDefault();
                    if ( 자동매매 &&
                        (_자동매매_동시종목개수 > _자동매매_보유종목개수) &&
                        ((_자동매매_매수시점 != 1) || (_자동매매_매수시점 == 1 && 유보종목아 == null)) &&
                        (( _자동매매_매수시점 != 3 ) || ( _자동매매_매수시점 == 3 && 유보종목아 == null && (매수추천종목아._순위 == 2 || 매수추천종목아._순위 == 1)))
                        )
                    {
                        _주식종합이야.Invoke(new Action(() => {
                            로그_listBox.Items.Add("자동매매 현금매수 주문시작합니다. 종목코드: " + 종목코드);
                            string 사용자구분명 = "주식주문";
                            string 화면번호 = "8844";
                            string 계좌번호 = 계좌번호_comboBox.Text;
                            int 주문유형 = 주문유형코드_알려줘(매수_주문유형_comboBox.Text);
                            int 주문가격 = (int)매수추천종목아._최우선매도호가;

                            if (주문가격 == 0)
                            {
                                주문가격 = (int)매수추천종목아._현재가;
                            }

                            int 주문수량 = Int32.Parse(Math.Floor((double)_자동매매_매수금액 / (double)(주문가격 + 1)) + "");
                            string 거래유형 = 거래유형코드_알려줘(매수_거래유형_comboBox.Text);
                            string 원주문번호 = "";
                            int 주문요청결과;
                            if (거래유형 == "03")
                            {
                                주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 0, 거래유형, 원주문번호);
                            }
                            else
                            {
                                주문요청결과 = axKHOpenAPI1.SendOrder(사용자구분명, 화면번호, 계좌번호, 주문유형, 종목코드, 주문수량, 주문가격, 거래유형, 원주문번호);
                            }
                            if (주문요청결과 == 0)
                            {
                                Console.WriteLine(종목코드 + "현금매수 주문을 요청하였습니다.");
                                로그_listBox.Items.Add(종목코드 + " 현금매수 주문을 요청하였습니다.");
                                자동매매로그_listBox.Items.Add(String.Format("{0} : [{1}] 종목코드: {2}, 주문수량: {3}, 주문가격: {4}, 거래유형: {5}", DateTime.Now, 매수_주문유형_comboBox.Text, 종목코드, 주문수량, 주문가격, 매수_거래유형_comboBox.Text));
                                _빌리언스탁.효과음_들려줘("매수접수");
                                유보종목아 = new 유보종목();
                                유보종목아._종목코드 = 종목코드;
                                _유보종목컬렉션.InsertOne(유보종목아);
                            }
                            else
                            {
                                Console.WriteLine(종목코드 + "현금매수 주문을 요청 실패 하였습니다.");
                                로그_listBox.Items.Add(종목코드 + "현금매수 주문을 요청 실패 하였습니다.");
                            }
                        }));
                        단계 = "매수";
                    } else
                    {
                        단계 = "유보";
                    }
                    
                }
            } else
            {
                단계 = "유보";
                var builder = Builders<유보종목>.Filter;
                var query = builder.Eq(x => x._종목코드, 종목코드);
                var result = _유보종목컬렉션.Find(query).Limit(1).ToList();
                유보종목 유보종목아 = result.FirstOrDefault();
                if (유보종목아 == null)
                {
                    유보종목아 = new 유보종목();
                    유보종목아._종목코드 = 종목코드;
                    _유보종목컬렉션.InsertOne(유보종목아);
                }
            }
            return 단계;
        }

        public void 추천종목_업데이트해줘 (List<매수추천종목> 정렬된_매수추천종목_목록)
        {
            _추천종목_똑같니 = true;
            bool 딩동 = false;
            if( _추천종목_처음이니 == true)
            {
                _추천종목_처음이니 = false;
                for (int ㄱ = 0; ㄱ < 정렬된_매수추천종목_목록.Count; ㄱ++)
                {
                    매수추천종목 새_매수추천종목아 = 정렬된_매수추천종목_목록[ㄱ];
                    정렬된_매수추천종목_목록[ㄱ]._단계 = 새매수추천종목_매수해줘(새_매수추천종목아._아이디, 새_매수추천종목아);
                    자동매매로그_listBox.Items.Add(String.Format("{0} : {1} ({2}) 추출되었습니다.", DateTime.Now, 새_매수추천종목아._종목명, 새_매수추천종목아._아이디));
                }
                딩동 = true;
            } else
            {
                for (int ㄱ = 0; ㄱ < 정렬된_매수추천종목_목록.Count; ㄱ++)
                {
                    매수추천종목 새_매수추천종목아 = 정렬된_매수추천종목_목록[ㄱ];
                    if (_종목코드의추천종목_사전.ContainsKey(새_매수추천종목아._아이디) == false)
                    {
                        _추천종목_똑같니 = false;
                        정렬된_매수추천종목_목록[ㄱ]._단계 = 새매수추천종목_매수해줘(새_매수추천종목아._아이디, 새_매수추천종목아);
                        자동매매로그_listBox.Items.Add(String.Format("{0} : {1} ({2}) 추출되었습니다.", DateTime.Now, 새_매수추천종목아._종목명, 새_매수추천종목아._아이디));
                        딩동 = true;
                    } else
                    {
                        매수추천종목 기존_매수추천종목아 = _종목코드의추천종목_사전[새_매수추천종목아._아이디];
                        if (
                            (새_매수추천종목아._종목명 != 기존_매수추천종목아._종목명) ||
                            (새_매수추천종목아._순위 != 기존_매수추천종목아._순위) ||
                            (새_매수추천종목아._시장 != 기존_매수추천종목아._시장) ||
                            (새_매수추천종목아._첫추출시간 != 기존_매수추천종목아._첫추출시간) ||
                            (새_매수추천종목아._첫추출등락율 != 기존_매수추천종목아._첫추출등락율) ||
                            (새_매수추천종목아._첫추출호가 != 기존_매수추천종목아._첫추출호가) ||
                            (새_매수추천종목아._끝추출시간 != 기존_매수추천종목아._끝추출시간) ||
                            (새_매수추천종목아._끝추출등락율 != 기존_매수추천종목아._끝추출등락율) ||
                            (새_매수추천종목아._끝추출호가 != 기존_매수추천종목아._끝추출호가) ||
                            (새_매수추천종목아._현재가 != 기존_매수추천종목아._현재가) ||
                            (새_매수추천종목아._최우선매도호가 != 기존_매수추천종목아._최우선매도호가) ||
                            (새_매수추천종목아._최우선매수호가 != 기존_매수추천종목아._최우선매수호가) ||
                            (새_매수추천종목아._추출개수 != 기존_매수추천종목아._추출개수) ||
                            (새_매수추천종목아._세력순수익률 != 기존_매수추천종목아._세력순수익률) ||
                            (새_매수추천종목아._세력매수거래대금 != 기존_매수추천종목아._세력매수거래대금) ||
                            (새_매수추천종목아._누적거래량 != 기존_매수추천종목아._누적거래량)
                            )
                        {
                            _추천종목_똑같니 = false;
                            정렬된_매수추천종목_목록[ㄱ]._단계 = 새매수추천종목_매수해줘(새_매수추천종목아._아이디, 새_매수추천종목아);
                            if (새_매수추천종목아._추출개수 != 기존_매수추천종목아._추출개수)
                            {
                                자동매매로그_listBox.Items.Add(String.Format("{0} : {1} ({2}) 추출되었습니다.", DateTime.Now, 새_매수추천종목아._종목명, 새_매수추천종목아._아이디));
                                딩동 = true;
                            }
                        }
                    }
                }
                if (_추천종목_똑같니 == true)
                {
                    return;
                }
            }
            if (딩동 == true)
            {
                효과음_들려줘("딩동");
            }

            추천종목_dataGridView.Rows.Clear();
            string 끝추출시간 = "";
            
            for (int ㄱ = 0; ㄱ < 정렬된_매수추천종목_목록.Count; ㄱ++)
            {
                추천종목_dataGridView.Rows.Add();
                매수추천종목 매수추천종목아 = 정렬된_매수추천종목_목록[ㄱ];
                _종목코드의추천종목_사전[매수추천종목아._아이디] = 매수추천종목아;

                추천종목_dataGridView["단계_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = 매수추천종목아._단계; // 대기 | 유보 | 매수 | 보유중 | 매도1회
                추천종목_dataGridView["순위_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = 매수추천종목아._순위;
                끝추출시간 = 매수추천종목아._끝추출시간.ToString();
                if (끝추출시간.Length == 5)
                {
                    추천종목_dataGridView["시간_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0}:{1}:{2}", 끝추출시간.Substring(0, 1), 끝추출시간.Substring(1, 2), 끝추출시간.Substring(3, 2));
                }
                else
                {
                    추천종목_dataGridView["시간_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0}:{1}:{2}", 끝추출시간.Substring(0, 2), 끝추출시간.Substring(2, 2), 끝추출시간.Substring(4, 2));
                }
                if (매수추천종목아._시장 == 0) /* 코스피 */
                {
                    추천종목_dataGridView["시장_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _코스피셀스타일;
                    추천종목_dataGridView["시장_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = "코스피";
                } else /* 코스닥 */
                {
                    추천종목_dataGridView["시장_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _코스닥셀스타일;
                    추천종목_dataGridView["시장_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = "코스닥";
                }

                추천종목_dataGridView["종목코드_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = 매수추천종목아._아이디;
                추천종목_dataGridView["종목명_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = 매수추천종목아._종목명;
                if (매수추천종목아._끝추출등락율 > 0)
                {
                    추천종목_dataGridView["등락율_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _레드셀스타일;
                    추천종목_dataGridView["현재가_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _레드셀스타일;
                } else if (매수추천종목아._끝추출등락율 < 0)
                {
                    추천종목_dataGridView["등락율_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _블루셀스타일;
                    추천종목_dataGridView["현재가_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _블루셀스타일;
                } else
                {
                    추천종목_dataGridView["등락율_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _블랙셀스타일;
                    추천종목_dataGridView["현재가_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _블랙셀스타일;
                }
                추천종목_dataGridView["등락율_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.##}%", 매수추천종목아._끝추출등락율);
                추천종목_dataGridView["현재가_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매수추천종목아._현재가);

                추천종목_dataGridView["최우선매도호가_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매수추천종목아._최우선매도호가);
                추천종목_dataGridView["최우선매수호가_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매수추천종목아._최우선매수호가);

                추천종목_dataGridView["추출개수_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매수추천종목아._추출개수);

                if (매수추천종목아._세력순수익률 > 0)
                {
                    추천종목_dataGridView["세력순수익률_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _레드셀스타일;
                }
                else if (매수추천종목아._세력순수익률 < 0)
                {
                    추천종목_dataGridView["세력순수익률_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _블루셀스타일;
                }
                else
                {
                    추천종목_dataGridView["세력순수익률_추천종목_DataGridViewTextBoxColumn", ㄱ].Style = _블랙셀스타일;
                }
                추천종목_dataGridView["세력순수익률_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.##}%", 매수추천종목아._세력순수익률);

                추천종목_dataGridView["세력매수거래대금_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매수추천종목아._세력매수거래대금);

                추천종목_dataGridView["누적거래량_추천종목_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매수추천종목아._누적거래량);
            }
        }

        private void 추천종목_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (추천종목_dataGridView.CurrentRow.Selected)
                {
                    int 선택된행 = 추천종목_dataGridView.CurrentCell.RowIndex;
                    string 종목코드 = 추천종목_dataGridView["종목코드_추천종목_DataGridViewTextBoxColumn", 선택된행].Value.ToString();
                    Clipboard.SetText(종목코드);
                    Console.WriteLine("추천종목 선택된 종목코드 = " + 종목코드);
                    _주식종합이야.Invoke(new Action(() => {
                        종목검색_textBox.Text = 종목코드;
                        로그_listBox.Items.Add(DateTime.Now + " - 추천종목 종목코드 = " + 종목코드 + "를 클릭하였습니다.");
                    }));

                    종목검색();

                    string 문자_임시 = 추천종목_dataGridView["최우선매도호가_추천종목_DataGridViewTextBoxColumn", 선택된행].Value.ToString();
                    문자_임시 = 문자_임시.Replace(",", "");
                    int 최우선매도호가 = Int32.Parse(문자_임시);

                    if (최우선매도호가 == 0)
                    {
                        문자_임시 = 추천종목_dataGridView["현재가_추천종목_DataGridViewTextBoxColumn", 선택된행].Value.ToString();
                        문자_임시 = 문자_임시.Replace(",", "");
                        최우선매도호가 = Int32.Parse(문자_임시);
                    }

                    매수_주문가격_numericUpDown.Value = 최우선매도호가;

                    문자_임시 = 추천종목_dataGridView["최우선매수호가_추천종목_DataGridViewTextBoxColumn", 선택된행].Value.ToString();
                    문자_임시 = 문자_임시.Replace(",", "");
                    int 최우선매수호가 = Int32.Parse(문자_임시);
                    매도_주문가격_numericUpDown.Value = 정정_주문가격_numericUpDown.Value = 최우선매수호가;

                    문자_임시 = Math.Floor((double)_자동매매_매수금액 / (double)(최우선매도호가 + 1)) + "";
                    매수_주문수량_numericUpDown.Value = 매도_주문수량_numericUpDown.Value = 정정_주문수량_numericUpDown.Value = Int32.Parse(문자_임시);

                }
            }
            catch (Exception EX)
            {
                _주식종합이야.Invoke(new Action(() => {
                    로그_listBox.Items.Add(String.Format("주식종합 - 추천종목_dataGridView_CellClick - Exception accured. EX: {0}", EX));
                }));
            }
        }
        private void 보유종목_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (보유종목_dataGridView.CurrentRow.Selected)
                {
                    int 선택된행 = 보유종목_dataGridView.CurrentCell.RowIndex;
                    string 종목코드 = 보유종목_dataGridView["종목코드_보유종목_DataGridViewTextBoxColumn", 선택된행].Value.ToString();
                    Clipboard.SetText(종목코드);
                    Console.WriteLine("보유종목 선택된 종목코드 = " + 종목코드);
                    _주식종합이야.Invoke(new Action(() => {
                        종목검색_textBox.Text = 종목코드;
                        로그_listBox.Items.Add(DateTime.Now + " - 보유종목 종목코드 = " + 종목코드 + "를 클릭하였습니다.");
                    }));

                    종목검색();

                    string 문자_임시 = 보유종목_dataGridView["현재가_보유종목_DataGridViewTextBoxColumn", 선택된행].Value.ToString();
                    문자_임시 = 문자_임시.Replace(",", "");
                    int 현재가 = Int32.Parse(문자_임시);
                    매수_주문가격_numericUpDown.Value = 매도_주문가격_numericUpDown.Value = 정정_주문가격_numericUpDown.Value = 현재가;

                    문자_임시 = 보유종목_dataGridView["보유수량_보유종목_DataGridViewTextBoxColumn", 선택된행].Value.ToString();
                    문자_임시 = 문자_임시.Replace(",", "");
                    int 보유수량 = Int32.Parse(문자_임시);
                    매수_주문수량_numericUpDown.Value = 매도_주문수량_numericUpDown.Value = 정정_주문수량_numericUpDown.Value = 보유수량;

                }
            } catch (Exception EX)
            {
                _주식종합이야.Invoke(new Action(() => {
                    로그_listBox.Items.Add(String.Format("주식종합 - 보유종목_dataGridView_CellClick - Exception accured. EX: {0}", EX));
                }));
            }
        }

        public bool 추천_혹은_보유종목이니 (string __종목코드)
        {
            /*
             Dictionary<string, 매수추천종목> _종목코드의추천종목_사전;
             Dictionary<string, 보유종목> _종목코드의보유종목_사전;
             */
            bool 맞니 = false;

            foreach (string 종목코드 in _종목코드의추천종목_사전.Keys)
            {
                if (종목코드 == __종목코드)
                {
                    맞니 = true;
                }
            }
            foreach (string 종목코드 in _종목코드의보유종목_사전.Keys)
            {
                if (종목코드 == __종목코드)
                {
                    맞니 = true;
                }
            }

            return 맞니;
        }

        public void 주식체결_전송해줘(주식체결 주식체결아)
        {
            try {
                트랜잭션_요청해줘(new Task(() => {
                    잔고_알려줘();
                }));
                /*
                if (_선택된종목아 != null && 주식체결아 != null && _선택된종목아._종목코드 == 주식체결아._종목코드)
                {
                    
                    _주식종합이야.Invoke(new Action(() => {
                        string 시간 = 주식체결아._체결시간.ToString();
                        주식체결_dataGridView.Rows.Insert(0);

                        시간 = 시간.Length == 5 ? String.Format("{0}:{1}:{2}", 시간.Substring(0, 1), 시간.Substring(1, 2), 시간.Substring(3, 2)) : String.Format("{0}:{1}:{2}", 시간.Substring(0, 2), 시간.Substring(2, 2), 시간.Substring(4, 2));
                        주식체결_dataGridView["시간_주식체결_DataGridViewTextBoxColumn", 0].Value = 시간;

                        if (주식체결아._전일대비 > 0)
                        {
                            주식체결_dataGridView["주가_주식체결_DataGridViewTextBoxColumn", 0].Style = _레드셀스타일;
                            주식체결_dataGridView["등락폭_주식체결_DataGridViewTextBoxColumn", 0].Style = _레드셀스타일;
                        }
                        else if (주식체결아._전일대비 < 0)
                        {
                            주식체결_dataGridView["주가_주식체결_DataGridViewTextBoxColumn", 0].Style = _블루셀스타일;
                            주식체결_dataGridView["등락폭_주식체결_DataGridViewTextBoxColumn", 0].Style = _블루셀스타일;
                        }
                        else
                        {
                            주식체결_dataGridView["주가_주식체결_DataGridViewTextBoxColumn", 0].Style = _블랙셀스타일;
                            주식체결_dataGridView["등락폭_주식체결_DataGridViewTextBoxColumn", 0].Style = _블랙셀스타일;
                        }
                        주식체결_dataGridView["주가_주식체결_DataGridViewTextBoxColumn", 0].Value = String.Format("{0:#,0}", 주식체결아._현재가);
                        주식체결_dataGridView["등락폭_주식체결_DataGridViewTextBoxColumn", 0].Value = String.Format("{0:#,0}", 주식체결아._전일대비);


                        if (주식체결아._매수했니 == true)
                        {
                            주식체결_dataGridView["체결량_주식체결_DataGridViewTextBoxColumn", 0].Style = _레드셀스타일;
                        }
                        else
                        {
                            주식체결_dataGridView["체결량_주식체결_DataGridViewTextBoxColumn", 0].Style = _블루셀스타일;
                        }
                        주식체결_dataGridView["체결량_주식체결_DataGridViewTextBoxColumn", 0].Value = String.Format("{0:#,0}", 주식체결아._거래량);
                    }));
                    
                }
                */
            }
            catch (Exception EX)
            {
                Console.WriteLine("주식종합 - 주식체결_전송해줘 - EX: " + EX);
            }
        }

        private void 자동매매_설정완료 ()
        {
            _주식종합이야.Invoke(new Action(() => {
                _자동매매_매수금액 = (long)자동매매_매수금액_numericUpDown.Value;
                _자동매매_동시종목개수 = (int)자동매매_동시종목개수_numericUpDown.Value;
                _자동매매_매수시점 = 매수시점_알려줘(자동매매_매수시점_comboBox.Text);
                _자동매매_주도주익절시점 = (double)자동매매_주도주익절시점_numericUpDown.Value / (double)10;
                _자동매매_익절시점 = (double)자동매매_익절시점_numericUpDown.Value / (double)10;
                _자동매매_손절시점 = (double)자동매매_손절시점_numericUpDown.Value / (double)10;

                로그_listBox.Items.Add(String.Format("자동매매 설정완료하였습니다. 매수금액: {0:#,0}원, 동시종목개수: {1:#,0}, 매수시점: {2}, 주도주익절시점: {3}, 익절시점: {4}, 손절시점: {5}.", _자동매매_매수금액, _자동매매_동시종목개수, _자동매매_매수시점, _자동매매_주도주익절시점, _자동매매_익절시점, _자동매매_손절시점));
            }));
        }

        private void 자동매매_설정완료_button_Click(object sender, EventArgs e)
        {
            자동매매_설정완료();
        }

        public void 잔고_알려줘()
        {
            _주식종합이야.Invoke(new Action(() => {
                axKHOpenAPI1.SetInputValue("계좌번호", 계좌번호_comboBox.Text);
                axKHOpenAPI1.SetInputValue("비밀번호", "");
                axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");
                axKHOpenAPI1.SetInputValue("조회구분", "2");
                int 결과코드 = axKHOpenAPI1.CommRqData("계좌평가잔고내역", "opw00018", 0, "0018");
                로그_찍어줘(DateTime.Now + " - 보유종목을 업데이트합니다.");
                로그_찍어줘("결과코드: " + 결과코드);
            }));
        }

        private void 잔고확인_button_Click(object sender, EventArgs e)
        {
            로그_찍어줘(DateTime.Now + " - 잔고확인을 클릭하였습니다.");
            잔고_알려줘();
        }
    }
}
