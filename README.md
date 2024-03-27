# .NET 8 Web API Projesinde Keycloak Kullanımı
# KeyCloak Web API Projesi

Bu proje, KeyCloak ile birlikte ASP.NET Core Web API kullanarak kimlik doğrulama ve yetkilendirme işlemlerini göstermektedir.

## Kurulum

1. Projeyi klonlayın:

```bash
git clone https://github.com/kullanici/adresi.git
cd ad-resi
```

2. Gerekli paketleri yükleyin:

```bash
dotnet restore
```

3. `appsettings.json` dosyasını güncelleyin:

   - KeyCloak sunucu, istemci kimliği, istemci gizli anahtarı ve diğer yapılandırma bilgilerini burada ayarlayın.

4. Projeyi çalıştırın:

```bash
dotnet run
```

5. Tarayıcıda `https://localhost:5001/swagger` adresine giderek API belgelerine erişebilirsiniz.

## Kullanım

- **Home:** `/api/home/home` endpoint'ine GET isteği yaparak ana sayfaya erişebilirsiniz.

- **AuthenticationAdmin:** `/api/home/authenticationadmin` endpoint'ine GET isteği yaparak yönetici kimlik doğrulamasını test edebilirsiniz.

- **AuthenticationNoAccess:** `/api/home/authenticationnoaccess` endpoint'ine GET isteği yaparak erişim izni olmayan bir kullanıcı için kimlik doğrulamasını test edebilirsiniz.

- **Authentication:** `/api/home/authentication` endpoint'ine GET isteği yaparak kullanıcı kimlik doğrulamasını test edebilirsiniz.

- **Privacy:** `/api/home/privacy` endpoint'ine GET isteği yaparak gizlilik politikasını görebilirsiniz.

- **Error:** `/api/home/error` endpoint'ine GET isteği yaparak bir hata sayfası alabilirsiniz.

## Lisans

Bu proje [MIT lisansı](LICENSE) altında lisanslanmıştır.
