using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using MongoDB.Bson;
using MongoDB.Driver;

namespace billionStock
{
    public partial class BS_Main : Form
    {
        private const int _maximum_progress = 100;
        static MongoClient _client = new MongoClient("mongodb://billionStock:82822424@localhost:27017/billionStock");
        static IMongoDatabase _db = _client.GetDatabase("billionStock");
        static IMongoCollection<MongoStock> _stockCollection = _db.GetCollection<MongoStock>("stocks");
        private int _timerCount = 0;
        private List<string> _marketList;
        private Dictionary<int, string> _errorCodeDictionary;
        private Dictionary<string, Stock> _stockDictionary;
        private Dictionary<string, string> _stockMarketDictionary;
        private Dictionary<string, int> _stockMarketCountDictionary;
        private SoundPlayer _soundPlayer;

        public BS_Main()
        {
            InitializeComponent();
            InitializeCustomComponent();
            axKHOpenAPI1.OnEventConnect += OnEventConnect;
            PlaySound("Start");
        }

        public void InitializeCustomComponent() {
            _marketList = new List<string>();
            _marketList.Add("0");
            _marketList.Add("10");
            _marketList.Add("3");
            _marketList.Add("8");
            _marketList.Add("50");
            _marketList.Add("4");
            _marketList.Add("5");
            _marketList.Add("6");
            _marketList.Add("9");
            _marketList.Add("30");

            _errorCodeDictionary = new Dictionary<int, string>() {
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
            _stockMarketDictionary = new Dictionary<string, string>()
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
            _stockMarketCountDictionary = new Dictionary<string, int>()
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

            //   var titleBar = ApplicationView.GetForCurrentView().TitleBar;
        }
        // 로그인 후 시작
        public void Run()
        {
            
        }

        /**** 커스텀 메서드 ****/
        /* 프로그레스 바 */
        public void StartProgressBar ()
        {
            _timerCount = 0;
            BS_ProgressBar.Value = 0;
            BS_ProgressBarTimer.Enabled = true;
        }

        public void StopProgressBar()
        {
            _timerCount = _maximum_progress;
            BS_ProgressBar.Value = _maximum_progress;
            BS_ProgressBarTimer.Enabled = false;
        }

        /* 효과음 플레이 */
        public void PlaySound(string type)
        {
            string fileName;
            if (type == "Start")
            {
                fileName = "start2.wav";
            } else if (type == "WorkEnd") {
                fileName = "trumpet.wav";
            } else
            {
                fileName = "button.wav";
            }
            _soundPlayer = new SoundPlayer(fileName);
            _soundPlayer.Play();
        }        

        /*
         * 로그인 정보
          "ACCOUNT_CNT" : 보유계좌 수를 반환합니다.
          "ACCLIST" 또는 "ACCNO" : 구분자 ';'로 연결된 보유계좌 목록을 반환합니다.
          "USER_ID" : 사용자 ID를 반환합니다.
          "USER_NAME" : 사용자 이름을 반환합니다.
          "KEY_BSECGB" : 키보드 보안 해지여부를 반환합니다.(0 : 정상, 1 : 해지)
          "FIREW_SECGB" : 방화벽 설정여부를 반환합니다.(0 : 미설정, 1 : 설정, 2 : 해지)
          "GetServerGubun" : 접속서버 구분을 반환합니다.(1 : 모의투자, 나머지 : 실서버)
         */
        public void GetLoginInfo()
        {
            string ACCOUNT_CNT = axKHOpenAPI1.GetLoginInfo("ACCOUNT_CNT");
            List<string> ACCLIST = ParseStringToList(axKHOpenAPI1.GetLoginInfo("ACCLIST"));
            string USER_ID = axKHOpenAPI1.GetLoginInfo("USER_ID");
            string USER_NAME = axKHOpenAPI1.GetLoginInfo("USER_NAME");
            string KEY_BSECGB = axKHOpenAPI1.GetLoginInfo("KEY_BSECGB");
            string FIREW_SECGB = axKHOpenAPI1.GetLoginInfo("FIREW_SECGB");
            string GetServerGubun = axKHOpenAPI1.GetLoginInfo("GetServerGubun");
            LoginInfo loginInfo = new LoginInfo(ACCOUNT_CNT, ACCLIST, USER_ID, USER_NAME, KEY_BSECGB, FIREW_SECGB, GetServerGubun);
            loginInfo.Show();
        }
        /*
         * 시장에 따른 종목 목록 가져오기
         [시장구분값]
          0 : 장내
          10 : 코스닥
          3 : ELW
          8 : ETF
          50 : KONEX
          4 :  뮤추얼펀드
          5 : 신주인수권
          6 : 리츠
          9 : 하이얼펀드
          30 : K-OTC
        */
        public void GetCodeListByMarket(int marketIndex) {
            string code = axKHOpenAPI1.GetCodeListByMarket(_marketList[marketIndex]);
            string name;
            List<String> market;
            List<string> codeList = ParseStringToList(code);
            Stock stock;

            foreach (string singleCode in codeList)
            {
                if ( _stockDictionary.ContainsKey(singleCode) )
                {
                    _stockDictionary[singleCode].AppendMarket(_marketList[marketIndex]);
                    Console.WriteLine(_stockDictionary[singleCode].ToString() + "\n");
                } else
                {
                    name = axKHOpenAPI1.GetMasterCodeName(singleCode);
                    market = new List<String>();
                    market.Add(_marketList[marketIndex]);
                    stock = new Stock(singleCode, name, market);
                    _stockDictionary.Add( singleCode, stock );
                }
            }
            _stockMarketCountDictionary[_marketList[marketIndex]] = codeList.Count;
            marketIndex++;
            if (marketIndex < _marketList.Count) {
                GetCodeListByMarket(marketIndex);
            } else
            {
                // Finished
                //var bulk = _stockCollection.InitializeUnorderedBulkOperation();
                foreach ( Stock newStock in _stockDictionary.Values )
                {

                //    var builder = Builders<MongoStock>.Filter;
                //    var query = builder.Eq("code", newStock.Code);
                //    var update = Builders<MongoStock>.Update.Set("code", newStock.Code).Set("name", newStock.Name).Set("market", newStock.Market);

                //    _stockCollection.UpdateOneAsync(query, update, builder.UpdateFlags.Upsert, builer.SafeMode.false);



                //    bulk.Find( ).Upsert().UpdateOne(update);
                    
                    //MongoStock s = new MongoStock( newStock.Code, newStock.Name, newStock.Market );
                    //var query = new QueryDocument("code", newStock.Code);

                    //_stockCollection.UpdateOne(query, update, UpdateFlags.Upsert, SafeMode.False);
                    //_stockCollection.InsertOne(s);
                }
                // BulkWriteResult bwr = bulk.Execute();
            }
        }

        /* string을 ;로 parsing하여 List로 반환 */
        public List<string> ParseStringToList (string code) {
            List<string> list = code.Split(';').ToList();
            if (list.Count <= 0)
            {
                return list;
            }
            int lastIndex = list.Count - 1;
            if (list[lastIndex] == "") {
                list.RemoveAt(list.Count - 1);
            }
            return list;
        }

        /**** 이벤트 메소드 ****/

        /* 종목 다운로드 */
        private void BS_BtnUpdateStocks_OnClick(object sender, EventArgs e)
        {
            /* Background Thread */
            if ( !BS_UpdateStock_BackgroundWorker.IsBusy )
            {
                PlaySound("Click");
                BS_BtnUpdateStocks.Text = "종목 다운로드중";
                _stockDictionary = new Dictionary<string, Stock>();
                StartProgressBar();
                BS_UpdateStock_BackgroundWorker.RunWorkerAsync();
            }
        }

        /* Input - 종목을 입력하세요 Placeholder  */
        private void BS_InputPanelSearchStock_OnEnter(object sender, EventArgs e)
        {
            BS_InputSearchStock.Text = "";
        }
        private void BS_InputPanelSearchStock_OnLeave(object sender, EventArgs e)
        {
            if (BS_InputSearchStock.Text == "") {
                BS_InputSearchStock.Text = "종목을 입력하세요";
            }
        }

        /* 프로그래스바 타이머 */
        private void BS_ProgressBarTimer_OnTick(object sender, EventArgs e)
        {
            if ( _timerCount < _maximum_progress) {
                _timerCount++;
            } else {
                _timerCount =0;
            }
            BS_ProgressBar.Value = _timerCount;
        }

        /* 시계 */
        private void BS_ClockTimer_OnTick(object sender, EventArgs e)
        {
            int YEAR = DateTime.Now.Year;
            string MONTH;
            string DAY;
            string DAYOFWEEK;
            int HH = DateTime.Now.Hour;
            int MM = DateTime.Now.Minute;
            int SS = DateTime.Now.Second;
            StringBuilder sb = new StringBuilder();

            if (DateTime.Now.Month < 10)
            {
                MONTH = "0" + DateTime.Now.Month;
            } else
            {
                MONTH = "" + DateTime.Now.Month;
            }

            if (DateTime.Now.Day < 10)
            {
                DAY = "0" + DateTime.Now.Day;
            }
            else
            {
                DAY = "" + DateTime.Now.Day;
            }

            switch ((int)DateTime.Now.DayOfWeek)
            {
                case 0:
                    DAYOFWEEK = "일";
                    break;
                case 1:
                    DAYOFWEEK = "월";
                    break;
                case 2:
                    DAYOFWEEK = "화";
                    break;
                case 3:
                    DAYOFWEEK = "수";
                    break;
                case 4:
                    DAYOFWEEK = "목";
                    break;
                case 5:
                    DAYOFWEEK = "금";
                    break;
                case 6:
                    DAYOFWEEK = "토";
                    break;
                default:
                    DAYOFWEEK = "??";
                    break;
            }

            sb.Append(string.Format("{0}년 {1}월 {2}일 {3}요일 ", YEAR, MONTH, DAY, DAYOFWEEK));

            if (HH < 12)
            {
                sb.Append("오전 ");
            } else
            {
                sb.Append("오후 ");
                HH -= 12;
            }

            if ( HH < 10 )
            {
                sb.Append( "0" + HH );
            }
            else
            {
                sb.Append(HH);
            }
            sb.Append(":");
            if (MM < 10)
            {
                sb.Append("0" + MM);
            }
            else
            {
                sb.Append(MM);
            }
            sb.Append(":");
            if (SS < 10)
            {
                sb.Append("0" + SS);
            }
            else
            {
                sb.Append(SS);
            }

            BS_Clock.Text = sb.ToString();
        }

        /* 키보드 키 감지 이벤트 */
        private void BS_Main_OnKeyDown(object sender, KeyEventArgs e)
        {
            //e.KeyboardDevice.Modifiers;
            // Ctrl + N
            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.N)
            {
                MessageBox.Show("New");
            }

            // Ctrl + O
            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.O)
            {
                MessageBox.Show("Open");
            }

            // Ctrl + S
            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.S)
            {
                MessageBox.Show("Save");
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
        private void MenuExit_OnClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /* 사용자 - 로그인 */
        private void Login() {
            if (BS_LoginState.Text == "로그인하세요") {
                StartProgressBar();
                PlaySound("Click");
                axKHOpenAPI1.CommConnect();
            }
        }
        private void MenuLogin_OnClick(object sender, EventArgs e)
        {
            Login();
        }
        private void BS_LoginState_OnClick(object sender, EventArgs e)
        {
            Login();
        }
        /* 사용자 - 로그인 정보 */
        private void MenuLoginInfo_OnClick(object sender, EventArgs e)
        {
            PlaySound("Click");
            StartProgressBar();
            long state = axKHOpenAPI1.GetConnectState();
            if (state == 0)
            {
                BS_LoginState.Text = "로그인하세요";
                MessageBox.Show(String.Format("로그인하지 않은 상태입니다.\n로그인하세요."), "로그인 정보", MessageBoxButtons.OK);
            }
            else
            {
                GetLoginInfo();
            }
            StopProgressBar();
        }
        /* 로그인 상태 */
        private void BS_LoginState_OnTextChanged(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.TopMost = false;
        }
        private void OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            StopProgressBar();
            // sender.Equals(loginButton)
            if (e.nErrCode != 0)
            {
                BS_LoginState.Text = "로그인하세요";
                if (_errorCodeDictionary.ContainsKey(e.nErrCode))
                {
                    if (MessageBox.Show(String.Format("로그인 실패하였습니다.\n원인: {0}\n\n안전을 위해 프로그램을 종료합니다.", _errorCodeDictionary[e.nErrCode]), "로그인 실패", MessageBoxButtons.OK) == DialogResult.OK )
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    if (MessageBox.Show(String.Format("로그인 실패하였습니다.\n원인: 알수없음\n\n안전을 위해 프로그램을 종료합니다."), "로그인 실패", MessageBoxButtons.OK) == DialogResult.OK )
                    {
                        Application.Exit();
                    }
                }
            }
            else
            {
                if (axKHOpenAPI1.GetLoginInfo("GetServerGubun") == "1")
                {
                    BS_LoginState.Text = "모의투자 접속중";
                }
                else
                {
                    BS_LoginState.Text = "실서버 접속중";
                }
                Run();
            }
        }

        private void BS_UpdateStock_BackgroundWorker_OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BS_UpdateStock_BackgroundWorker_OnCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PlaySound("WorkEnd");
            StopProgressBar();
            BS_BtnUpdateStocks.Text = "종목 다운로드";
            BS_ShowBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
            BS_ShowBox.AppendText("[전체 종목 다운로드 완료] - 모든 종목정보를 가져왔습니다.");
            BS_ShowBox.AppendText(Environment.NewLine);
            BS_ShowBox.AppendText("\r\n전체 종목: " +  string.Format("{0:0,0}", _stockDictionary.Count)  + "개");
            foreach (string key in _stockMarketDictionary.Keys)
            {
                BS_ShowBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
                BS_ShowBox.AppendText(String.Format("{0} 종목: {1}개\r\n", _stockMarketDictionary[key], string.Format("{0:0,0}", _stockMarketCountDictionary[key])));
            }
            int codeDuplicatingMarketCount = 0;
            StringBuilder codeDuplicatingMarket = new StringBuilder();
            StringBuilder tempStringBuilder;

            foreach (Stock stock in _stockDictionary.Values)
            {
                if (stock.Market.Count > 1)
                {
                    tempStringBuilder = new StringBuilder();
                    stock.Market.ForEach(delegate (String key)
                    {
                        tempStringBuilder.Append(_stockMarketDictionary[key] + "\r\n");
                    });
                    codeDuplicatingMarketCount++;
                    codeDuplicatingMarket.Append("\r\n\r\n---------------------------------------------\r\n\r\n");
                    codeDuplicatingMarket.Append(String.Format( "{0}\r\n{1}\r\n{2}", stock.Code, stock.Name, tempStringBuilder.ToString()));
                }
            }
            BS_ShowBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
            BS_ShowBox.AppendText(String.Format("마켓 중복 종목: {0}개", string.Format("{0:0,0}", codeDuplicatingMarketCount)));
            BS_ShowBox.AppendText(codeDuplicatingMarket.ToString());
            /*
            foreach (Stock newStock in _stockDictionary.Values)
            {

                BS_ShowBox.AppendText(newStock.ToString());
                BS_ShowBox.AppendText(Environment.NewLine);
            }
            BS_ShowBox.AppendText(Environment.NewLine);
            */
        }

        private void BS_UpdateStock_BackgroundWorker_OnDoWork(object sender, DoWorkEventArgs e)
        {
            GetCodeListByMarket(0);
        }
    }
}