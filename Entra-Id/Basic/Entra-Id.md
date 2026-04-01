# Ćwiczenie: utworzenie nowego tenantu Microsoft Entra ID i podstawowa konfiguracja

## Cel ćwiczenia

W tym ćwiczeniu utworzysz nowy tenant Microsoft Entra ID, skonfigurujesz MFA dla swojego konta administracyjnego, dodasz nowego użytkownika i na końcu wrócisz do katalogu szkoleniowego.

To ćwiczenie jest celowo proste, ale warto od razu rozumieć dwie rzeczy:

- `tenant` to katalog tożsamości, użytkowników, grup i metod logowania,
- `subscription` to warstwa rozliczeniowa i organizacyjna dla zasobów Azure.

To nie jest to samo. W praktyce w tym ćwiczeniu tworzysz nowy katalog tożsamości, ale w formularzu możesz też wskazać subskrypcję `Microsoft Sponsorship` i `Resource group`, aby powiązać tenant z zasobem widocznym w Azure.

## Zanim zaczniesz

Przygotuj:

- dostęp do [https://portal.azure.com](https://portal.azure.com),
- konto z uprawnieniami pozwalającymi utworzyć nowy tenant,
- telefon z iOS lub Androidem,
- możliwość zainstalowania aplikacji `Microsoft Authenticator`.

Dobrze też mieć otwarty notatnik, bo w trakcie ćwiczenia przyda się zapisanie:

- nazwy nowego tenantu,
- domeny `onmicrosoft.com`,
- nazwy nowego użytkownika,
- hasła tymczasowego, jeśli portal je pokaże.

## Krok 1. Utwórz nowy tenant Microsoft Entra ID

1. Zaloguj się do [https://portal.azure.com](https://portal.azure.com).
2. W górnym polu wyszukiwania wpisz `Microsoft Entra ID`.
3. Otwórz usługę `Microsoft Entra ID`.
4. Przejdź do `Overview` > `Manage tenants`.
5. Kliknij `Create`.

Jeżeli portal zapyta o typ tenantu:

- wybierz zwykły tenant `Microsoft Entra ID` / `Workforce`,
- nie wybieraj `B2C`, bo to inny scenariusz.

W wariancie używanym w tym ćwiczeniu formularz zawiera pola subskrypcji i grupy zasobów. Uzupełnij go tak:

- `Subscription`: wybierz `Microsoft Sponsorship`,
- `Resource group`: wybierz istniejącą grupę szkoleniową albo utwórz nową, jeśli prowadzący tak zalecił,
- `Organization name`: wpisz czytelną nazwę organizacji, np. `Lab Entra Jan Kowalski`,
- `Initial domain name`: wpisz unikalny prefiks domeny, np. `lab-entra-jan-kowalski`,
- `Country/Region`: wybierz kraj zgodny z instrukcją szkolenia.

Uwaga: Microsoft dość często zmienia układ kreatora. Jeżeli nie widzisz pól `Subscription` i `Resource group`, najprawdopodobniej jesteś w innym wariancie tworzenia tenantu niż ten używany na szkoleniu.

Następnie:

1. Kliknij `Review + create`.
2. Sprawdź, czy nie ma błędów walidacji.
3. Kliknij `Create`.
4. Poczekaj na zakończenie tworzenia tenantu.

Po poprawnym utworzeniu powinieneś zobaczyć nowy katalog z domeną w formacie:

`twoja-nazwa.onmicrosoft.com`

## Krok 2. Przełącz się do nowego tenantu

Samo utworzenie tenantu nie zawsze powoduje automatyczne przełączenie kontekstu. Dlatego wykonaj to ręcznie:

1. W prawym górnym rogu portalu kliknij ikonę konta albo `Settings`.
2. Otwórz `Directories + subscriptions` albo użyj opcji `Switch directory`.
3. Na liście katalogów znajdź tenant utworzony przed chwilą.
4. Wybierz go jako aktywny katalog.

Jak sprawdzić, że jesteś we właściwym miejscu:

- w nagłówku lub w ustawieniach portalu widać nazwę nowego katalogu,
- po wejściu do `Microsoft Entra ID` na stronie `Overview` widzisz nową domenę `onmicrosoft.com`,
- `Tenant ID` jest inny niż w katalogu szkoleniowym.

Jeżeli nie widzisz nowego tenantu od razu, odśwież portal albo wyloguj się i zaloguj ponownie.

## Krok 3. Skonfiguruj MFA dla swojego konta administracyjnego

W tym kroku konfigurujesz metodę wieloskładnikowego logowania dla własnego konta w nowym tenancie. Najczęściej portal sam wyświetli kreator typu `More information required` lub `Keep your account secure`. Jeśli nie wyświetli się automatycznie, przejdź ręcznie do:

[https://mysignins.microsoft.com/security-info](https://mysignins.microsoft.com/security-info)

### 3.1. Zainstaluj Microsoft Authenticator

1. Na telefonie otwórz `App Store` albo `Google Play`.
2. Wyszukaj `Microsoft Authenticator`.
3. Zainstaluj aplikację.
4. Przy pierwszym uruchomieniu zezwól na powiadomienia.
5. Jeżeli telefon o to poprosi, zezwól też na dostęp do aparatu, bo będzie potrzebny do zeskanowania kodu QR.

### 3.2. Połącz konto z aplikacją

1. W przeglądarce, w nowym tenancie, rozpocznij konfigurację metody logowania.
2. Wybierz `Authenticator app` jako metodę weryfikacji.
3. Na telefonie otwórz `Microsoft Authenticator`.
4. W aplikacji wybierz `Add account`.
5. Wybierz `Work or school account`.
6. Wybierz skanowanie kodu QR.
7. W przeglądarce kliknij `Next`, aby wyświetlić kod QR.
8. Zeskanuj kod QR telefonem.
9. Po zeskanowaniu wróć do przeglądarki i kliknij `Next`.
10. Zaakceptuj testowe powiadomienie w aplikacji.

Jeżeli test zakończy się powodzeniem, metoda `Microsoft Authenticator` zostanie przypisana do Twojego konta.

### 3.3. Sprawdź efekt

Po zakończeniu konfiguracji na stronie `Security info` powinieneś widzieć co najmniej jedną metodę:

- `Microsoft Authenticator`

W niektórych tenantach portal może poprosić także o dodanie drugiej metody, np. numeru telefonu. Jeżeli taki krok się pojawi, wykonaj go zgodnie z komunikatem na ekranie.

## Krok 4. Dodaj nowego użytkownika

Wciąż będąc w nowym tenancie:

1. Otwórz `Microsoft Entra ID`.
2. Przejdź do `Users`.
3. Kliknij `New user`.
4. Wybierz `Create new user`.

Na formularzu uzupełnij przynajmniej pola podstawowe:

- `User principal name`: np. `lab.user01@twoja-domena.onmicrosoft.com`
- `Display name`: np. `Lab User 01`
- `Password`: zostaw automatyczne generowanie albo wpisz hasło tymczasowe, jeśli taka jest instrukcja na szkoleniu

Dodatkowe wskazówki:

- dla prostego ćwiczenia wybierz zwykłego użytkownika typu `Member`,
- nie nadawaj temu użytkownikowi ról administracyjnych, jeśli nie jest to osobny element zadania,
- jeżeli portal pokaże hasło tymczasowe, zapisz je od razu, bo później może nie być już widoczne.

Następnie:

1. Kliknij `Review + create`.
2. Kliknij `Create`.
3. Poczekaj, aż użytkownik pojawi się na liście.

Po poprawnym utworzeniu zobaczysz konto na liście użytkowników w nowym tenancie.

## Krok 5. Przełącz katalog z powrotem na tenant szkoleniowy

Na koniec wróć do katalogu, w którym zaczynałeś ćwiczenie:

1. Kliknij ikonę konta albo `Settings` w prawym górnym rogu portalu.
2. Otwórz `Directories + subscriptions` albo użyj `Switch directory`.
3. Wybierz tenant szkoleniowy.
4. Potwierdź przełączenie.

Sprawdź, czy wróciłeś do właściwego katalogu:

- nazwa tenantu w portalu zgadza się z tenantem szkoleniowym,
- widzisz z powrotem znane zasoby lub subskrypcje szkoleniowe,
- `Tenant ID` w `Microsoft Entra ID` jest inny niż w nowo utworzonym katalogu.

## Checklista końcowa

Po zakończeniu ćwiczenia powinieneś mieć:

- utworzony nowy tenant Microsoft Entra ID,
- nowy tenant widoczny na liście katalogów,
- skonfigurowany `Microsoft Authenticator` dla swojego konta,
- dodanego co najmniej jednego nowego użytkownika,
- aktywny z powrotem tenant szkoleniowy.

## Typowe problemy

### Nie mogę utworzyć tenantu

Najczęstsze przyczyny:

- konto nie ma uprawnień do tworzenia nowych tenantów,
- otworzył się inny wariant kreatora niż ten używany na szkoleniu,
- wpisana nazwa domeny `Initial domain name` nie jest unikalna.

Najprostsza poprawka:

- zmień `Initial domain name` na bardziej unikalny,
- upewnij się, że wybrana jest właściwa subskrypcja `Microsoft Sponsorship`,
- jeśli problem dotyczy uprawnień, poproś prowadzącego o weryfikację roli.

### Nie widzę nowego tenantu na liście katalogów

- odśwież portal,
- wyloguj się i zaloguj ponownie,
- sprawdź, czy tworzenie tenantu zakończyło się sukcesem, a nie błędem walidacji lub uprawnień.

### MFA nie uruchamia się automatycznie

To nie musi oznaczać błędu. Przejdź ręcznie do:

[https://mysignins.microsoft.com/security-info](https://mysignins.microsoft.com/security-info)

i dodaj metodę `Microsoft Authenticator` samodzielnie.

## Źródła referencyjne

Instrukcja została rozpisana na podstawie aktualnej dokumentacji Microsoft Learn i Microsoft Support:

- [Quickstart: Create a new tenant in Microsoft Entra ID](https://learn.microsoft.com/en-us/entra/fundamentals/create-new-tenant)
- [How to create, invite, and delete users](https://learn.microsoft.com/en-us/entra/fundamentals/how-to-create-delete-users)
- [Set up Security info from a sign-in page](https://support.microsoft.com/en-us/account-billing/set-up-security-info-from-a-sign-in-page-28180870-c256-4ebf-8bd7-5335571bf9a8)
