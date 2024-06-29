using UnityEngine;

public class ContinuousDayNightCycle : MonoBehaviour
{
    public Light sunLight; // Güneş ışığı
    public Light moonLight; // Ay ışığı

    public float dayLengthInMinutes = 24 * 60; // Toplam gün uzunluğu dakika cinsinden
    public float daySpeedMultiplier = 1f; // Gün hızı çarpanı

    private float dayDuration; // Gün uzunluğu saniye cinsinden
    private float elapsedTime = 0f; // Geçen süre

    private int currentHour = 0; // Şu anki saat
    private int currentMinute = 0; // Şu anki dakika

    void Start()
    {
        dayDuration = dayLengthInMinutes * 60; // Gün uzunluğu saniye cinsinden

        // Güneş ve Ay ışığı başlangıçta aynı yerde başlat
        sunLight.transform.rotation = Quaternion.Euler(CalculateSunAngleX(0f), 0f, 0f);
        moonLight.transform.rotation = Quaternion.Euler(CalculateMoonAngleX(0f), 0f, 0f);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime * daySpeedMultiplier;

        // Saat ve dakika bilgisini güncelle
        UpdateTime();

        // Güneş ve Ay'ın dairesel yolunu hesapla
        float sunAngleX = CalculateSunAngleX(elapsedTime / dayDuration);
        float moonAngleX = CalculateMoonAngleX(elapsedTime / dayDuration);

        // Güneş ve Ay'ın yön ışığını güncelle
        sunLight.transform.rotation = Quaternion.Euler(sunAngleX, 0f, 0f);
        moonLight.transform.rotation = Quaternion.Euler(moonAngleX, 0f, 0f);
    }

    void UpdateTime()
    {
        // Geçen süreyi saat ve dakika olarak hesapla
        float totalMinutes = elapsedTime / 60f;
        currentHour = Mathf.FloorToInt(totalMinutes / 60f) % 24;
        currentMinute = Mathf.FloorToInt(totalMinutes % 60f);
    }

    float CalculateSunAngleX(float timeProgress)
    {
        // Güneş X eksenindeki açısını hesapla
        return Mathf.Lerp(0f, 180f, timeProgress); // Sabit bir yükseliş ve batış açısı kabul ediyoruz
    }

    float CalculateMoonAngleX(float timeProgress)
    {
        // Ay X eksenindeki açısını hesapla (güneşe zıt)
        return Mathf.Lerp(180f, 360f, timeProgress); // Güneşin batışından doğuşuna zıt hareket eder
    }

    // Public metotlarla saat ve dakika bilgilerine erişim sağlarız
    public int GetCurrentHour()
    {
        return currentHour;
    }

    public int GetCurrentMinute()
    {
        return currentMinute;
    }
}
