namespace FluentDemo.Web.Services;

public enum Language { En, Tr }

public class LocalizationService
{
    private static readonly Dictionary<string, (string En, string Tr)> Strings = new()
    {
        // Brand / generic
        ["app.title"]            = ("FluentUI Demo", "FluentUI Demo"),
        ["app.subtitle"]         = ("Incentive Management", "Teşvik Yönetimi"),
        ["app.version"]          = (".NET 10 · v2.0", ".NET 10 · v2.0"),
        ["app.footer"]           = ("© 2026 FluentUI Blazor Demo", "© 2026 FluentUI Blazor Demo"),

        // Login
        ["login.title"]          = ("FluentUI Demo", "FluentUI Demo"),
        ["login.subtitle"]       = ("Incentive Management System · v2.0", "Teşvik Yönetim Sistemi · v2.0"),
        ["login.username"]       = ("Username", "Kullanıcı Adı"),
        ["login.password"]       = ("Password", "Şifre"),
        ["login.submit"]         = ("Sign In", "Giriş Yap"),
        ["login.hint"]           = ("Demo user: <b>admin</b> · Password: <b>123</b>", "Demo kullanıcı: <b>admin</b> · Şifre: <b>123</b>"),
        ["login.fail"]           = ("Login failed.", "Giriş başarısız."),
        ["login.connectError"]   = ("Could not connect to server: ", "Sunucuya bağlanılamadı: "),

        // Menu
        ["menu.label"]           = ("Menu", "Menü"),
        ["menu.home"]            = ("Home", "Ana Sayfa"),
        ["menu.incentives"]      = ("Incentives", "Teşvikler"),
        ["menu.reports"]         = ("Reports", "Raporlar"),
        ["menu.users"]           = ("Users", "Kullanıcılar"),
        ["menu.settings"]        = ("Settings", "Ayarlar"),
        ["nav.group.work"]       = ("Work", "İş"),
        ["nav.group.manage"]     = ("Manage", "Yönetim"),

        // Header / common
        ["common.logout"]        = ("Sign out", "Çıkış"),
        ["common.refresh"]       = ("Refresh", "Yenile"),
        ["common.all"]           = ("All", "Tümü"),
        ["common.loading"]       = ("Loading...", "Yükleniyor..."),
        ["common.records"]       = ("records", "kayıt"),
        ["common.total"]         = ("Total", "Toplam"),
        ["common.searchPlaceholder"] = ("Search by name, employee no or department...", "Ad, sicil veya departman ara..."),
        ["common.edit"]          = ("Edit", "Düzenle"),
        ["common.delete"]        = ("Delete", "Sil"),
        ["common.view"]          = ("View", "Görüntüle"),

        // Home page
        ["home.welcome"]         = ("Welcome, {0} 👋", "Hoş geldin, {0} 👋"),
        ["home.subtitle"]        = ("Here is the current overview of the incentive system.", "İşte teşvik sisteminin güncel özeti."),
        ["home.totalIncentives"] = ("Total Incentives", "Toplam Teşvik"),
        ["home.approved"]        = ("Approved", "Onaylanan"),
        ["home.pending"]         = ("Pending", "Bekleyen"),
        ["home.rejected"]        = ("Rejected", "Reddedilen"),
        ["home.trendUp"]         = ("▲ +12% this month", "▲ +12% bu ay"),
        ["home.trendApproved"]   = ("▲ +2 this week", "▲ +2 bu hafta"),
        ["home.trendPending"]    = ("▼ -1 yesterday", "▼ -1 dün"),
        ["home.trendFlat"]       = ("▬ no change", "▬ değişim yok"),
        ["home.cardTitle"]       = ("🚀 FluentUI Blazor Demo", "🚀 FluentUI Blazor Demo"),
        ["home.cardText"]        = (
            "A showcase application built with <b>.NET 10</b>, <b>Blazor Server</b> and <b>Microsoft FluentUI Blazor</b> components. Use it to explore reports, dashboards, forms and other UI patterns.",
            "<b>.NET 10</b>, <b>Blazor Server</b> ve <b>Microsoft FluentUI Blazor</b> bileşenleri ile hazırlanmış bir gösterim uygulamasıdır. Raporları, panoları, formları ve diğer arayüz desenlerini keşfedin."),
        ["home.goToList"]        = ("Go to incentive list →", "Teşvik listesine git →"),

        // Incentives page
        ["inc.title"]            = ("Incentive List", "Teşvik Listesi"),
        ["inc.subtitle"]         = ("View, search and filter all incentive records.", "Tüm teşvik kayıtlarını görüntüle, ara ve filtrele."),
        ["inc.col.id"]           = ("ID", "ID"),
        ["inc.col.employeeNo"]   = ("Employee No", "Sicil"),
        ["inc.col.fullName"]     = ("Full Name", "Ad Soyad"),
        ["inc.col.department"]   = ("Department", "Departman"),
        ["inc.col.type"]         = ("Incentive Type", "Teşvik Türü"),
        ["inc.col.amount"]       = ("Amount", "Tutar"),
        ["inc.col.date"]         = ("Date", "Tarih"),
        ["inc.col.status"]       = ("Status", "Durum"),
        ["inc.loadError"]        = ("Failed to load data: ", "Veri yüklenemedi: "),

        // Statuses (server returns English; we translate for display)
        ["status.Approved"]      = ("Approved", "Onaylandı"),
        ["status.Pending"]       = ("Pending", "Beklemede"),
        ["status.Rejected"]      = ("Rejected", "Reddedildi"),

        // Reports page
        ["rep.title"]            = ("Reports & Analytics", "Raporlar ve Analiz"),
        ["rep.subtitle"]         = ("Visual insights about incentive distribution and trends.", "Teşvik dağılımı ve trendlere ilişkin görsel analizler."),
        ["rep.totalAmount"]      = ("Total Paid", "Toplam Ödenen"),
        ["rep.avgAmount"]        = ("Average / Incentive", "Ortalama / Teşvik"),
        ["rep.topDept"]          = ("Top Department", "En İyi Departman"),
        ["rep.approvalRate"]     = ("Approval Rate", "Onay Oranı"),
        ["rep.byDeptTitle"]      = ("Distribution by Department", "Departmana Göre Dağılım"),
        ["rep.byTypeTitle"]      = ("Incentive Types Share", "Teşvik Türü Payı"),
        ["rep.byDateTitle"]      = ("Monthly Trend", "Aylık Trend"),
        ["rep.col.department"]   = ("Department", "Departman"),
        ["rep.col.count"]        = ("# of Incentives", "Teşvik Adedi"),
        ["rep.col.total"]        = ("Total Amount", "Toplam Tutar"),
        ["rep.col.avg"]          = ("Avg / Person", "Kişi Başı"),
        ["rep.col.share"]        = ("Share", "Pay"),

        // Users page
        ["usr.title"]            = ("User Management", "Kullanıcı Yönetimi"),
        ["usr.subtitle"]         = ("Add, edit and deactivate application users.", "Uygulama kullanıcılarını ekle, düzenle ve devre dışı bırak."),
        ["usr.new"]              = ("New User", "Yeni Kullanıcı"),
        ["usr.edit"]             = ("Edit User", "Kullanıcıyı Düzenle"),
        ["usr.delete"]           = ("Delete", "Sil"),
        ["usr.deleteConfirm"]    = ("Are you sure you want to delete this user?", "Bu kullanıcıyı silmek istediğinize emin misiniz?"),
        ["usr.save"]             = ("Save", "Kaydet"),
        ["usr.cancel"]           = ("Cancel", "İptal"),
        ["usr.col.username"]     = ("Username", "Kullanıcı Adı"),
        ["usr.col.fullName"]     = ("Full Name", "Ad Soyad"),
        ["usr.col.email"]        = ("Email", "E-posta"),
        ["usr.col.department"]   = ("Department", "Departman"),
        ["usr.col.role"]         = ("Role", "Rol"),
        ["usr.col.status"]       = ("Status", "Durum"),
        ["usr.col.created"]      = ("Created", "Oluşturulma"),
        ["usr.col.actions"]      = ("Actions", "İşlemler"),
        ["usr.active"]           = ("Active", "Aktif"),
        ["usr.inactive"]         = ("Inactive", "Pasif"),
        ["usr.total"]            = ("users", "kullanıcı"),

        // Settings page
        ["set.title"]            = ("Settings", "Ayarlar"),
        ["set.subtitle"]         = ("Customize your demo experience.", "Demo deneyiminizi özelleştirin."),
        ["set.tab.profile"]      = ("Profile", "Profil"),
        ["set.tab.appearance"]   = ("Appearance", "Görünüm"),
        ["set.tab.notifications"]= ("Notifications", "Bildirimler"),
        ["set.tab.security"]     = ("Security", "Güvenlik"),
        ["set.tab.system"]       = ("System", "Sistem"),

        ["set.profile.title"]    = ("Personal Information", "Kişisel Bilgiler"),
        ["set.profile.fullName"] = ("Full Name", "Ad Soyad"),
        ["set.profile.email"]    = ("Email", "E-posta"),
        ["set.profile.phone"]    = ("Phone", "Telefon"),
        ["set.profile.dept"]     = ("Department", "Departman"),

        ["set.appearance.title"]    = ("Theme & Display", "Tema ve Görünüm"),
        ["set.theme"]               = ("Theme", "Tema"),
        ["set.theme.light"]         = ("Light", "Açık"),
        ["set.theme.dark"]          = ("Dark", "Koyu"),
        ["set.theme.system"]        = ("System", "Sistem"),
        ["set.density"]             = ("Density", "Yoğunluk"),
        ["set.density.compact"]     = ("Compact", "Sıkı"),
        ["set.density.normal"]      = ("Normal", "Normal"),
        ["set.density.comfortable"] = ("Comfortable", "Geniş"),
        ["set.fontSize"]            = ("Font Size", "Yazı Boyutu"),
        ["set.accent"]              = ("Accent Color", "Vurgu Rengi"),
        ["set.lang"]                = ("Language", "Dil"),

        ["set.notif.title"]      = ("Notification Preferences", "Bildirim Tercihleri"),
        ["set.notif.email"]      = ("Email notifications", "E-posta bildirimleri"),
        ["set.notif.push"]       = ("Push notifications", "Anlık bildirimler"),
        ["set.notif.weekly"]     = ("Weekly summary", "Haftalık özet"),
        ["set.notif.approved"]   = ("Notify when an incentive is approved", "Teşvik onaylandığında bildir"),
        ["set.notif.pending"]    = ("Notify on pending incentives", "Bekleyen teşviklerde bildir"),

        ["set.sec.title"]        = ("Security", "Güvenlik"),
        ["set.sec.2fa"]          = ("Two-factor authentication", "İki adımlı doğrulama"),
        ["set.sec.session"]      = ("Session timeout (minutes)", "Oturum zaman aşımı (dakika)"),
        ["set.sec.changePw"]     = ("Change Password", "Şifre Değiştir"),
        ["set.sec.sessions"]     = ("Active Sessions", "Aktif Oturumlar"),

        ["set.sys.title"]        = ("System", "Sistem"),
        ["set.sys.version"]      = ("Version", "Sürüm"),
        ["set.sys.environment"]  = ("Environment", "Ortam"),
        ["set.sys.api"]          = ("API endpoint", "API uç noktası"),
        ["set.sys.support"]      = ("Support email", "Destek e-postası"),
        ["set.sys.clearCache"]   = ("Clear local cache", "Önbelleği temizle"),

        ["set.saved"]            = ("Your changes have been saved.", "Değişiklikleriniz kaydedildi."),
        ["set.save"]             = ("Save Changes", "Değişiklikleri Kaydet"),

        // Roles
        ["role.Admin"]           = ("Admin", "Yönetici"),
        ["role.Manager"]         = ("Manager", "Müdür"),
        ["role.User"]            = ("User", "Kullanıcı"),

        // Top bar / shell
        ["shell.search"]         = ("Search resources, services and docs", "Kaynak, servis ve doküman ara"),
        ["shell.notifications"]  = ("Notifications", "Bildirimler"),
        ["shell.settings"]       = ("Settings", "Ayarlar"),
        ["shell.help"]           = ("Help", "Yardım"),
        ["shell.account"]        = ("Account", "Hesap"),
        ["shell.signOut"]        = ("Sign out", "Çıkış yap"),
        ["shell.viewProfile"]    = ("View profile", "Profili görüntüle"),
        ["shell.directory"]      = ("Demo Directory", "Demo Dizini"),
        ["shell.markRead"]       = ("Mark all as read", "Tümünü okundu işaretle"),
        ["shell.viewAll"]        = ("View all", "Tümünü görüntüle"),
        ["shell.recent"]         = ("Recent", "Son kullanılan"),
        ["shell.favorites"]      = ("Favorites", "Favoriler"),
        ["shell.create"]         = ("Create a resource", "Kaynak oluştur"),
        ["shell.cloudShell"]     = ("Cloud Shell", "Bulut Kabuğu"),

        // Menu (extra)
        ["menu.dashboard"]       = ("Dashboard", "Panel"),
        ["menu.resources"]       = ("All resources", "Tüm kaynaklar"),
        ["menu.activity"]        = ("Activity log", "İşlem geçmişi"),
        ["menu.createIncentive"] = ("Create incentive", "Teşvik oluştur"),

        // Breadcrumb root
        ["bc.home"]              = ("Home", "Ana Sayfa"),

        // Resources page
        ["res.title"]            = ("All resources", "Tüm kaynaklar"),
        ["res.subtitle"]         = ("Browse incentives, departments, users and groups.", "Teşvikleri, departmanları, kullanıcıları ve grupları gözat."),
        ["res.tree.root"]        = ("Demo Workspace", "Demo Çalışma Alanı"),
        ["res.tree.incentives"]  = ("Incentives", "Teşvikler"),
        ["res.tree.depts"]       = ("Departments", "Departmanlar"),
        ["res.tree.users"]       = ("Users", "Kullanıcılar"),
        ["res.tree.groups"]      = ("Groups", "Gruplar"),
        ["res.details"]          = ("Details", "Detaylar"),
        ["res.empty"]            = ("Select an item from the tree to see details", "Detayları görmek için ağaçtan bir öğe seçin"),

        // Activity log
        ["act.title"]            = ("Activity log", "İşlem geçmişi"),
        ["act.subtitle"]         = ("All operations performed in your workspace in the last 7 days.", "Çalışma alanınızda son 7 günde yapılan tüm işlemler."),
        ["act.filter"]           = ("Filter", "Filtrele"),
        ["act.export"]           = ("Export to CSV", "CSV olarak dışa aktar"),

        // Create incentive wizard
        ["wiz.title"]            = ("Create incentive", "Teşvik oluştur"),
        ["wiz.subtitle"]         = ("Define a new incentive in 4 easy steps.", "4 kolay adımda yeni bir teşvik tanımlayın."),
        ["wiz.s1"]               = ("Basics", "Temel bilgiler"),
        ["wiz.s2"]               = ("Recipient", "Alıcı"),
        ["wiz.s3"]               = ("Amount & date", "Tutar ve tarih"),
        ["wiz.s4"]               = ("Review + create", "Gözden geçir + oluştur"),
        ["wiz.next"]             = ("Next", "İleri"),
        ["wiz.prev"]             = ("Previous", "Geri"),
        ["wiz.create"]           = ("Create", "Oluştur"),
        ["wiz.f.type"]           = ("Incentive type", "Teşvik türü"),
        ["wiz.f.desc"]           = ("Description", "Açıklama"),
        ["wiz.f.dept"]           = ("Department", "Departman"),
        ["wiz.f.employee"]       = ("Employee", "Çalışan"),
        ["wiz.f.amount"]         = ("Amount (₺)", "Tutar (₺)"),
        ["wiz.f.date"]           = ("Effective date", "Yürürlük tarihi"),
        ["wiz.f.priority"]       = ("Priority", "Öncelik"),
        ["wiz.f.notify"]         = ("Notify recipient by email", "Alıcıyı e-posta ile bilgilendir"),
        ["wiz.created"]          = ("✓ Incentive created successfully", "✓ Teşvik başarıyla oluşturuldu"),

        // Common extras
        ["common.new"]           = ("New", "Yeni"),
        ["common.edit"]          = ("Edit", "Düzenle"),
        ["common.delete"]        = ("Delete", "Sil"),
        ["common.export"]        = ("Export", "Dışa aktar"),
        ["common.filter"]        = ("Filter", "Filtrele"),
        ["common.columns"]       = ("Columns", "Sütunlar"),
        ["common.cancel"]        = ("Cancel", "İptal"),
        ["common.save"]          = ("Save", "Kaydet"),
        ["common.close"]         = ("Close", "Kapat"),
    };

    public Language Current { get; private set; } = Language.En;
    public event Action? Changed;

    public void SetLanguage(Language lang)
    {
        if (Current == lang) return;
        Current = lang;
        Changed?.Invoke();
    }

    public string T(string key)
    {
        if (!Strings.TryGetValue(key, out var pair)) return key;
        return Current == Language.En ? pair.En : pair.Tr;
    }

    public string T(string key, params object[] args) => string.Format(T(key), args);
}
