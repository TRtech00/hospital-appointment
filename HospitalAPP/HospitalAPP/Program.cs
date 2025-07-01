using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace HospitalConsoleApp
{
    class Program
    {
        static string adminUsername = "admin";
        static string adminPassword = "admin123";
        static string personnelUsername = "personel";
        static string personnelPassword = "personel123";

        static List<Patient> patients = new List<Patient>();
        static List<Appointment> appointments = new List<Appointment>();
        static List<string> clinics = new List<string> { "Dahiliye", "KBB", "Ortopedi" };

        static void Main(string[] args)
        {
            LoadData();

            while (true)
            {
                Console.Clear();
                Console.Title = "🏥 Hastane Yönetim Sistemi";
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("=== HASTANE YÖNETİM SİSTEMİ ===\n");
                Console.WriteLine("1 - Admin Girişi");
                Console.WriteLine("2 - Personel Girişi");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçiminizi yapınız (1/2/0): ");
                string choice = Console.ReadLine();

                if (choice == "1") AdminLogin();
                else if (choice == "2") PersonnelLogin();
                else if (choice == "0") break;
                else Console.WriteLine("Geçersiz seçim.");
            }

            SaveData();
        }

        static void AdminLogin()
        {
            Console.Clear();
            Console.Write("Kullanıcı Adı: ");
            string username = Console.ReadLine();
            Console.Write("Şifre: ");
            string password = Console.ReadLine();

            if (username == adminUsername && password == adminPassword)
            {
                Console.WriteLine("\nAdmin girişi başarılı.\n");
                AdminPanel();
            }
            else
            {
                Console.WriteLine("Hatalı kullanıcı adı veya şifre.");
            }
        }

        static void PersonnelLogin()
        {
            Console.Clear();
            Console.Write("Kullanıcı Adı: ");
            string username = Console.ReadLine();
            Console.Write("Şifre: ");
            string password = Console.ReadLine();

            if (username == personnelUsername && password == personnelPassword)
            {
                Console.WriteLine("\nPersonel girişi başarılı.\n");
                PersonnelPanel();
            }
            else
            {
                Console.WriteLine("Hatalı kullanıcı adı veya şifre.");
            }
        }

        static void AdminPanel()
        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Paneli ---");
                Console.WriteLine("1 - Hasta Ekle / Güncelle / Sil");
                Console.WriteLine("2 - Randevu Ekle / Güncelle / Sil");
                Console.WriteLine("3 - Randevu Filtreleme (TC / Ad Soyad)");
                Console.WriteLine("4 - Personel Ekle / Güncelle / Sil");
                Console.WriteLine("5 - Poliklinik Ekle / Güncelle / Sil");
                Console.WriteLine("6 - Geri Ana Menüye Dön");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();

                if (choice == "1") { PatientOperations(true); }
                else if (choice == "2") { AppointmentOperations(true); }
                else if (choice == "3") { FilterAppointments(); }
                else if (choice == "4") { PersonnelManagement(); }
                else if (choice == "5") { ClinicManagement(); }
                else if (choice == "6") break;
                else Console.WriteLine("Geçersiz seçim");
            }
        }

        static void ClinicManagement()
        {
            while (true)
            {
                Console.WriteLine("\n--- Poliklinik İşlemleri ---");
                Console.WriteLine("1 - Poliklinik Ekle");
                Console.WriteLine("2 - Poliklinik Güncelle");
                Console.WriteLine("3 - Poliklinik Sil");
                Console.WriteLine("4 - Poliklinikleri Listele");
                Console.WriteLine("0 - Geri Dön");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Yeni poliklinik adı: ");
                    string yeni = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(yeni) || clinics.Contains(yeni, StringComparer.OrdinalIgnoreCase))
                        Console.WriteLine("Geçersiz veya mevcut poliklinik!");
                    else
                    {
                        clinics.Add(yeni);
                        Console.WriteLine("Poliklinik eklendi.");
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("--- Poliklinikler ---");
                    for (int i = 0; i < clinics.Count; i++)
                        Console.WriteLine($"{i + 1}. {clinics[i]}");
                    Console.Write("Güncellenecek poliklinik numarası: ");
                    if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= clinics.Count)
                    {
                        Console.Write("Yeni isim: ");
                        string yeniIsim = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(yeniIsim) || clinics.Contains(yeniIsim, StringComparer.OrdinalIgnoreCase))
                            Console.WriteLine("Geçersiz veya mevcut poliklinik adı!");
                        else
                        {
                            clinics[idx - 1] = yeniIsim;
                            Console.WriteLine("Poliklinik güncellendi.");
                        }
                    }
                    else Console.WriteLine("Geçersiz numara!");
                }
                else if (choice == "3")
                {
                    Console.WriteLine("--- Poliklinikler ---");
                    for (int i = 0; i < clinics.Count; i++)
                        Console.WriteLine($"{i + 1}. {clinics[i]}");
                    Console.Write("Silinecek poliklinik numarası: ");
                    if (int.TryParse(Console.ReadLine(), out int sidx) && sidx > 0 && sidx <= clinics.Count)
                    {
                        Console.WriteLine($"{clinics[sidx - 1]} silindi.");
                        clinics.RemoveAt(sidx - 1);
                    }
                    else Console.WriteLine("Geçersiz numara!");
                }
                else if (choice == "4")
                {
                    Console.WriteLine("--- Poliklinikler ---");
                    foreach (var c in clinics)
                        Console.WriteLine($"- {c}");
                }
                else if (choice == "0") break;
                else Console.WriteLine("Geçersiz seçim.");
            }
        }

        static void PersonnelPanel()
        {
            while (true)
            {
                Console.WriteLine("\n--- Personel Paneli ---");
                Console.WriteLine("1 - Hasta Ekle / Güncelle");
                Console.WriteLine("2 - Randevu Ekle / Güncelle / Sil");
                Console.WriteLine("3 - Randevu Filtreleme (TC / Ad Soyad)");
                Console.WriteLine("4 - Geri Ana Menüye Dön");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();

                if (choice == "1") { /* hasta işlemleri */ }
                else if (choice == "2") { /* randevu işlemleri */ }
                else if (choice == "3") { /* filtreleme */ }
                else if (choice == "4") break;
                else Console.WriteLine("Geçersiz seçim");
                if (choice == "1") { PatientOperations(false); }
                else if (choice == "2") { AppointmentOperations(true); }
                else if (choice == "3") { FilterAppointments(); }
                else if (choice == "4") break;

            }
        }
        static void PatientOperations(bool allowDelete = true)
        {
            while (true)
            {
                Console.WriteLine("\n--- Hasta İşlemleri ---");
                Console.WriteLine("1 - Hasta Ekle");
                Console.WriteLine("2 - Hasta Güncelle");
                if (allowDelete) Console.WriteLine("3 - Hasta Sil");
                Console.WriteLine("4 - Listele");
                Console.WriteLine("0 - Geri Dön");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    string tc;
                    while (true)
                    {
                        Console.Write("TC (11 hane): "); tc = Console.ReadLine();
                        if (tc.Length == 11 && tc.All(char.IsDigit)) break;
                        Console.WriteLine("TC 11 haneli ve rakamlardan oluşmalı!");
                    }
                    if (patients.Any(p => p.TC == tc))
                    {
                        Console.WriteLine("Bu TC ile kayıtlı hasta zaten var!");
                        continue;
                    }
                    Console.Write("Ad: "); string ad = Console.ReadLine();
                    Console.Write("Soyad: "); string soyad = Console.ReadLine();
                    patients.Add(new Patient { TC = tc, Name = ad, Surname = soyad });
                    Console.WriteLine("Hasta eklendi.");
                }
                else if (choice == "2")
                {
                    Console.Write("Güncellenecek hastanın TC'si: ");
                    string gtc = Console.ReadLine();
                    var hasta = patients.FirstOrDefault(p => p.TC == gtc);
                    if (hasta != null)
                    {
                        Console.Write("Yeni Ad: "); hasta.Name = Console.ReadLine();
                        Console.Write("Yeni Soyad: "); hasta.Surname = Console.ReadLine();
                        Console.WriteLine("Hasta güncellendi.");
                    }
                    else Console.WriteLine("Hasta bulunamadı.");
                }
                else if (choice == "3" && allowDelete)
                {
                    Console.Write("Silinecek hastanın TC'si: ");
                    string stc = Console.ReadLine();
                    var hasta = patients.FirstOrDefault(p => p.TC == stc);
                    if (hasta != null)
                    {
                        patients.Remove(hasta);
                        Console.WriteLine("Hasta silindi.");
                    }
                    else Console.WriteLine("Hasta bulunamadı.");
                }
                else if (choice == "4")
                {
                    Console.WriteLine("--- Tüm Hastalar ---");
                    foreach (var p in patients)
                        Console.WriteLine($"TC: {p.TC}, Ad: {p.Name}, Soyad: {p.Surname}");
                }
                else if (choice == "0") break;
                else Console.WriteLine("Geçersiz seçim.");
            }
        }
        static List<TimeSpan> AvailableTimes = new List<TimeSpan>
{
    new TimeSpan(9,0,0), new TimeSpan(9,15,0), new TimeSpan(9,30,0), new TimeSpan(9,45,0),
    new TimeSpan(10,0,0), new TimeSpan(10,15,0), new TimeSpan(10,30,0), new TimeSpan(10,45,0),
    new TimeSpan(11,0,0), new TimeSpan(11,15,0), new TimeSpan(11,30,0), new TimeSpan(11,45,0),
    new TimeSpan(13,30,0), new TimeSpan(13,45,0),
    new TimeSpan(14,0,0), new TimeSpan(14,15,0), new TimeSpan(14,30,0), new TimeSpan(14,45,0),
    new TimeSpan(15,0,0), new TimeSpan(15,15,0), new TimeSpan(15,30,0), new TimeSpan(15,45,0),
    new TimeSpan(16,0,0)
};

        static void AppointmentOperations(bool allowDelete = true)
        {
             while (true)
    {
        Console.WriteLine("\n--- Randevu İşlemleri ---");
        Console.WriteLine("1 - Randevu Ekle");
        Console.WriteLine("2 - Randevu Güncelle");
        if (allowDelete) Console.WriteLine("3 - Randevu Sil");
        Console.WriteLine("4 - Listele");
        Console.WriteLine("0 - Geri Dön");
        Console.Write("Seçiminiz: ");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            string tc;
            while (true)
            {
                Console.Write("TC (11 hane): "); tc = Console.ReadLine();
                if (tc.Length == 11 && tc.All(char.IsDigit)) break;
                Console.WriteLine("TC 11 haneli ve rakamlardan oluşmalı!");
            }
            if (!patients.Any(p => p.TC == tc))
            {
                Console.WriteLine("Bu TC ile kayıtlı hasta yok! Önce hastayı ekleyin.");
                continue;
            }

            int clinicIdx;
            while (true)
            {
                Console.WriteLine("Poliklinikler:");
                for (int i = 0; i < clinics.Count; i++)
                    Console.WriteLine($"{i + 1}. {clinics[i]}");
                Console.Write("Poliklinik numarası: ");
                if (int.TryParse(Console.ReadLine(), out clinicIdx) && clinicIdx > 0 && clinicIdx <= clinics.Count)
                {
                    clinicIdx -= 1;
                    break;
                }
                Console.WriteLine("Geçersiz poliklinik seçimi!");
            }


            DateTime date;
            while (true)
            {
                Console.Write("Tarih (GG.AA.YYYY): ");
                if (DateTime.TryParse(Console.ReadLine(), out date))
                    break;
                Console.WriteLine("Geçersiz tarih formatı!");
            }

            if (appointments.Any(a => a.Date.Date == date.Date && a.Clinic == clinics[clinicIdx] && a.TC == tc))
            {
                Console.WriteLine("Bu hasta o gün bu poliklinikte zaten randevu almış!");
                continue;
            }
            appointments.Add(new Appointment { TC = tc, Clinic = clinics[clinicIdx], Date = date });
            Console.WriteLine("Randevu eklendi.");
                    if (appointments.Any(a =>
    a.Clinic == clinics[clinicIdx] &&
    a.Date.Date == date.Date &&
    a.Date.TimeOfDay == date.TimeOfDay))
                    {
                        Console.WriteLine("Bu tarih ve saatte bu poliklinikte zaten randevu var! Lütfen başka bir zaman seçin.");
                        continue;
                    }

                    if (appointments.Any(a =>
                        a.TC == tc &&
                        a.Date.Date == date.Date &&
                        a.Clinic == clinics[clinicIdx]))
                    {
                        Console.WriteLine("Bu hasta, aynı gün ve poliklinikte zaten randevuya sahip!");
                        continue;
                    }

                }
                else if (choice == "2")
        {
            Console.Write("Güncellenecek randevunun TC'si: ");
            string uptc = Console.ReadLine();
            var randevu = appointments.FirstOrDefault(a => a.TC == uptc);
            if (randevu != null)
            {
                int newIndex;
                while (true)
                {
                    Console.WriteLine("Poliklinikler:");
                    for (int i = 0; i < clinics.Count; i++)
                        Console.WriteLine($"{i + 1}. {clinics[i]}");
                    Console.Write("Yeni Poliklinik numarası: ");
                    if (int.TryParse(Console.ReadLine(), out newIndex) && newIndex > 0 && newIndex <= clinics.Count)
                    {
                        newIndex -= 1;
                        break;
                    }
                    Console.WriteLine("Geçersiz poliklinik seçimi!");
                }

                DateTime newDate;
                while (true)
                {
                    Console.Write("Yeni Tarih (GG.AA.YYYY): ");
                    if (DateTime.TryParse(Console.ReadLine(), out newDate))
                        break;
                    Console.WriteLine("Geçersiz tarih formatı!");
                }

                randevu.Clinic = clinics[newIndex];
                randevu.Date = newDate;
                Console.WriteLine("Randevu güncellendi.");
                        if (appointments.Any(a =>
    a != randevu &&
    a.Clinic == clinics[newIndex] &&
    a.Date.Date == newDate.Date &&
    a.Date.TimeOfDay == newDate.TimeOfDay))

                        {
                            Console.WriteLine("Bu tarih ve saatte bu poliklinikte zaten randevu var! Lütfen başka bir zaman seçin.");
                            continue;
                        }


                        if (appointments.Any(a =>
                            a != randevu &&
                            a.TC == uptc &&
                            a.Clinic == clinics[newIndex] &&
                            a.Date.Date == newDate.Date))
                        {
                            Console.WriteLine("Bu hasta, aynı gün ve poliklinikte zaten randevuya sahip!");
                            continue;
                        }


                    }
                    else Console.WriteLine("Randevu bulunamadı.");
        }

        else if (choice == "3" && allowDelete)
        {
            Console.Write("Silinecek randevunun TC'si: ");
            string deltc = Console.ReadLine();
            var rs = appointments.FirstOrDefault(a => a.TC == deltc);
            if (rs != null)
            {
                appointments.Remove(rs);
                Console.WriteLine("Randevu silindi.");
            }
            else Console.WriteLine("Randevu bulunamadı.");
        }
        else if (choice == "4")
        {
            Console.WriteLine("--- Tüm Randevular ---");
            foreach (var a in appointments)
                Console.WriteLine($"TC: {a.TC}, Klinik: {a.Clinic}, Tarih: {a.Date:dd.MM.yyyy}");
        }
        else if (choice == "0") break;
        else Console.WriteLine("Geçersiz seçim.");
                if (choice == "1")
                {
                    string tc;
                    while (true)
                    {
                        Console.Write("TC (11 hane): "); tc = Console.ReadLine();
                        if (tc.Length == 11 && tc.All(char.IsDigit)) break;
                        Console.WriteLine("TC 11 haneli ve rakamlardan oluşmalı!");
                    }
                    if (!patients.Any(p => p.TC == tc))
                    {
                        Console.WriteLine("Bu TC ile kayıtlı hasta yok! Önce hastayı ekleyin.");
                        continue;
                    }

                    int clinicIdx;
                    while (true)
                    {
                        Console.WriteLine("Poliklinikler:");
                        for (int i = 0; i < clinics.Count; i++)
                            Console.WriteLine($"{i + 1}. {clinics[i]}");
                        Console.Write("Poliklinik numarası: ");
                        if (int.TryParse(Console.ReadLine(), out clinicIdx) && clinicIdx > 0 && clinicIdx <= clinics.Count)
                        {
                            clinicIdx -= 1;
                            break;
                        }
                        Console.WriteLine("Geçersiz poliklinik seçimi!");
                    }

                    DateTime date;
                    while (true)
                    {
                        Console.Write("Tarih (GG.AA.YYYY): ");
                        if (DateTime.TryParse(Console.ReadLine(), out date))
                            break;
                        Console.WriteLine("Geçersiz tarih formatı!");
                    }

                    // SAAT SEÇME BLOĞU
                    // Seçilen poliklinik ve tarih için dolu saatleri bul
                    var takenTimes = appointments
                        .Where(a => a.Clinic == clinics[clinicIdx] && a.Date.Date == date.Date)
                        .Select(a => a.Date.TimeOfDay)
                        .ToList();

                    var availableTimes = AvailableTimes
                        .Where(t => !takenTimes.Contains(t))
                        .ToList();

                    if (availableTimes.Count == 0)
                    {
                        Console.WriteLine("Bu gün ve poliklinikte uygun saat kalmadı!");
                        continue;
                    }

                    Console.WriteLine("Seçilebilecek saatler:");
                    for (int i = 0; i < availableTimes.Count; i++)
                        Console.WriteLine($"{i + 1}. {availableTimes[i]:hh\\:mm}");

                    int timeIdx;
                    while (true)
                    {
                        Console.Write("Saat seçimi: ");
                        if (int.TryParse(Console.ReadLine(), out timeIdx) && timeIdx > 0 && timeIdx <= availableTimes.Count)
                        {
                            break;
                        }
                        Console.WriteLine("Geçersiz saat seçimi!");
                    }

                    // Seçilen zaman
                    var chosenTime = availableTimes[timeIdx - 1];
                    var chosenDateTime = date.Date + chosenTime;

                    // Çakışma kontrolü
                    if (appointments.Any(a =>
                        a.Clinic == clinics[clinicIdx] &&
                        a.Date == chosenDateTime))
                    {
                        Console.WriteLine("Bu saat için zaten randevu alınmış!");
                        continue;
                    }
                    if (appointments.Any(a =>
                        a.TC == tc &&
                        a.Clinic == clinics[clinicIdx] &&
                        a.Date.Date == date.Date))
                    {
                        Console.WriteLine("Bu hasta, aynı gün ve poliklinikte zaten randevuya sahip!");
                        continue;
                    }

                    appointments.Add(new Appointment { TC = tc, Clinic = clinics[clinicIdx], Date = chosenDateTime });
                    Console.WriteLine("Randevu eklendi.");
                }

            }
        }
        static void FilterAppointments()
        {
            while (true)
            {
                Console.WriteLine("\n--- Randevu Filtreleme ---");
                Console.WriteLine("1 - TC ile");
                Console.WriteLine("2 - Ad Soyad ile");
                Console.WriteLine("0 - Geri Dön");
                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine();

                if (secim == "1")
                {
                    Console.Write("TC: ");
                    string ftc = Console.ReadLine();
                    var filtered = appointments.Where(a => a.TC == ftc);
                    foreach (var a in filtered)
                    {
                        var p = patients.FirstOrDefault(x => x.TC == a.TC);
                        string ad = p != null ? $"{p.Name} {p.Surname}" : "";
                        Console.WriteLine($"TC: {a.TC} Ad Soyad: {ad} Klinik: {a.Clinic} Tarih: {a.Date:dd.MM.yyyy}");
                    }
                }
                else if (secim == "2")
                {
                    Console.Write("Ad: "); string ad = Console.ReadLine();
                    Console.Write("Soyad: "); string soyad = Console.ReadLine();
                    var tcList = patients.Where(x => x.Name.Equals(ad, StringComparison.OrdinalIgnoreCase) && x.Surname.Equals(soyad, StringComparison.OrdinalIgnoreCase)).Select(x => x.TC).ToList();
                    var filtered = appointments.Where(a => tcList.Contains(a.TC));
                    foreach (var a in filtered)
                    {
                        Console.WriteLine($"TC: {a.TC} Ad Soyad: {ad} {soyad} Klinik: {a.Clinic} Tarih: {a.Date:dd.MM.yyyy}");
                    }
                }
                else if (secim == "0") break;
                else Console.WriteLine("Geçersiz seçim.");
            }
        }
        static void PersonnelManagement()
        {
            while (true)
            {
                Console.WriteLine("\n--- Personel İşlemleri ---");
                Console.WriteLine("1 - Personel Ekle");
                Console.WriteLine("2 - Personel Güncelle");
                Console.WriteLine("3 - Personel Sil");
                Console.WriteLine("0 - Geri Dön");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Yeni kullanıcı adı: ");
                    personnelUsername = Console.ReadLine();
                    Console.Write("Yeni şifre: ");
                    personnelPassword = Console.ReadLine();
                    Console.WriteLine("Personel eklendi.");
                }
                else if (choice == "2")
                {
                    Console.Write("Mevcut kullanıcı adı: ");
                    string mevcut = Console.ReadLine();
                    if (mevcut == personnelUsername)
                    {
                        Console.Write("Yeni kullanıcı adı: ");
                        personnelUsername = Console.ReadLine();
                        Console.Write("Yeni şifre: ");
                        personnelPassword = Console.ReadLine();
                        Console.WriteLine("Personel bilgisi güncellendi.");
                    }
                    else
                    {
                        Console.WriteLine("Personel bulunamadı.");
                    }
                }
                else if (choice == "3")
                {
                    Console.Write("Silinecek kullanıcı adı: ");
                    string silinecek = Console.ReadLine();
                    if (silinecek == personnelUsername)
                    {
                        personnelUsername = "";
                        personnelPassword = "";
                        Console.WriteLine("Personel silindi.");
                    }
                    else
                    {
                        Console.WriteLine("Personel bulunamadı.");
                    }
                }
                else if (choice == "0") break;
                else Console.WriteLine("Geçersiz seçim.");
            }
        }


        static void SaveData()
        {
            File.WriteAllText("patients.json", JsonConvert.SerializeObject(patients));
            File.WriteAllText("appointments.json", JsonConvert.SerializeObject(appointments));
        }

        static void LoadData()
        {
            if (File.Exists("patients.json"))
                patients = JsonConvert.DeserializeObject<List<Patient>>(File.ReadAllText("patients.json"));
            if (File.Exists("appointments.json"))
                appointments = JsonConvert.DeserializeObject<List<Appointment>>(File.ReadAllText("appointments.json"));
        }
    }

    class Patient
    {
        public string TC { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    class Appointment
    {
        public string TC { get; set; }
        public string Clinic { get; set; }
        public DateTime Date { get; set; }
    }
}
