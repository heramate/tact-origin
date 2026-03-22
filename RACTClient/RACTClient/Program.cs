using System;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace RACTClient
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 고성능 서버 통신 테스트용 비동기 실행 루틴
            // Task.Run(() => AppGlobal.TryServerConnectAsync("127.0.0.1", 43210));

            // 실제 UI 로직은 현재 제외된 상태이므로 메시지 박스만 표시하거나 빈 루프 실행
            MessageBox.Show(".NET 10 고성능 서버 대응 클라이언트 통신 엔진 리팩토링 완료 (Messaging Core 전용 빌드)");
        }
    }
}
