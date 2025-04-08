namespace Renty.Server.Global
{
    public class TimeHelper
    {
        public static DateTime GetKoreanTime()
        {
            // 1. 현재 UTC 시간을 가져옵니다.
            DateTime utcNow = DateTime.UtcNow;

            // 2. 한국 시간대 정보를 가져옵니다.
            //    - Windows: "Korea Standard Time"
            //    - Linux/macOS: "Asia/Seoul" (도커 컨테이너는 주로 Linux 기반이므로 이걸 우선 시도)
            TimeZoneInfo kstZone = null;
            try
            {
                // Linux/macOS 환경에서 시도
                kstZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Seoul");
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    // Windows 환경에서 시도
                    kstZone = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
                }
                catch (Exception ex)
                {
                    // 원하는 시간대를 찾을 수 없는 경우 예외 처리
                    Console.WriteLine($"Error finding KST time zone: {ex.Message}");
                    // 기본값으로 UTC를 반환하거나, 적절한 예외를 던질 수 있습니다.
                    // 여기서는 예시로 UTC를 반환합니다. 실제 상황에 맞게 수정하세요.
                    return utcNow;
                }
            }
            catch (Exception ex) // 기타 예외 처리
            {
                Console.WriteLine($"Error finding KST time zone: {ex.Message}");
                return utcNow; // 예시: UTC 반환
            }


            // 3. UTC 시간을 한국 시간으로 변환합니다.
            DateTime koreanTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, kstZone);

            return koreanTime;
        }
    }
}
