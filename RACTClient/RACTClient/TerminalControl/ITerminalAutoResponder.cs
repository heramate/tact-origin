using System.Collections.Generic;

namespace RACTClient
{
    /// <summary>
    /// 자동 응답 규칙을 정의하는 클래스입니다.
    /// </summary>
    public class AutoResponseRule
    {
        /// <summary>
        /// 매칭할 패턴 (일반 문자열 또는 정규표현식)
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 패턴이 정규표현식인지 여부
        /// </summary>
        public bool IsRegex { get; set; }

        /// <summary>
        /// 패턴 매칭 시 전송할 응답 문자열 (예: " ", "\r")
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// 규칙에 대한 설명
        /// </summary>
        public string Description { get; set; }

        public AutoResponseRule() { }

        public AutoResponseRule(string pattern, string response, bool isRegex = false, string description = "")
        {
            Pattern = pattern;
            Response = response;
            IsRegex = isRegex;
            Description = description;
        }
    }

    /// <summary>
    /// 터미널 데이터 수신 시 자동으로 응답하는 기능을 정의하는 인터페이스입니다.
    /// </summary>
    public interface ITerminalAutoResponder
    {
        /// <summary>
        /// 자동 응답 기능 활성화 여부
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 새로운 응답 규칙을 추가합니다.
        /// </summary>
        void AddRule(AutoResponseRule rule);

        /// <summary>
        /// 규칙 목록을 초기화합니다.
        /// </summary>
        void ClearRules();

        /// <summary>
        /// 특정 패턴과 응답을 가진 규칙이 이미 존재하는지 확인합니다.
        /// </summary>
        bool HasRule(string pattern, string response);

        /// <summary>
        /// 수신된 텍스트를 분석하여 매칭되는 규칙이 있으면 응답 문자열을 반환합니다.
        /// </summary>
        /// <param name="receivedText">수신된 텍스트</param>
        /// <returns>매칭된 응답 문자열, 매칭되는 규칙이 없으면 null</returns>
        string Process(string receivedText);
    }
}
