using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
1초 5회 제한 사용시: 20초과. 예: 25
1분 100회 제한 사용시: 600초과. 예: 610
1시간 1000회 제한 사용시: 3600초과. 예: 3610
*/
namespace billionStock
{
    class 요청데이터관리자
    {
        public static 요청데이터관리자 요청데이터관리자야;
        private static AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        Queue<Task> 요청작업큐 = new Queue<Task>(); // TR요청 작업 큐
        Thread 요청스레드;
        public int 딜레이 = 3610;
        private Dictionary<int, string> _오류코드의오류메시지_사전;

        private 요청데이터관리자 ()
        {
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
            요청스레드 = new Thread(delegate ()
            {
                while (true)
                {
                    try
                    {
                        /*
                        if (요청작업큐.Count == 0)
                        {
                            트랜잭션_요청해줘(new Task(() =>
                            {
                                axKHOpenAPI1.SetInputValue("시장구분", "000");
                                axKHOpenAPI1.SetInputValue("정렬구분", "1");
                                axKHOpenAPI1.SetInputValue("관리종목포함", "1");
                                int 결과코드 = axKHOpenAPI1.CommRqData("당일거래량상위", "OPT10030", 0, "7777");
                                if (결과코드 != 0)
                                {
                                    if (_오류코드의오류메시지_사전.ContainsKey(결과코드))
                                    {
                                        Console.WriteLine(String.Format("{0}\r\n당일거래량상위요청 실패하였습니다.\r\n원인: {1}\r\n\r\n", DateTime.Now, _오류코드의오류메시지_사전[결과코드]));
                                    }
                                    else
                                    {
                                        Console.WriteLine(String.Format("{0}\r\n당일거래량상위요청 실패하였습니다.\r\n원인: 알수없음\r\n\r\n", DateTime.Now));
                                    }
                                }
                            }));
                        }*/
                        while (요청작업큐.Count > 0)
                        {
                            요청작업큐.Dequeue().RunSynchronously();
                            Thread.Sleep(딜레이);
                        }
                        Thread.Sleep(100);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("요청데이터관리자 요청작업큐 오류메시지 : " + exception.Message);
                    }
                }
            });
        }

        public void 시작하자(AxKHOpenAPILib.AxKHOpenAPI _axKHOpenAPI1)
        {
            axKHOpenAPI1 = _axKHOpenAPI1;
            요청스레드.IsBackground = true;
            요청스레드.Start();
        }

        public static 요청데이터관리자 GetInstance()
        {
            if (요청데이터관리자야 == null )
            {
                요청데이터관리자야 = new 요청데이터관리자();
            }
            return 요청데이터관리자야;
        }

        public void 트랜잭션_요청해줘 (Task 작업)
        {
            요청작업큐.Enqueue(작업);
        }
    }
}