# Ćwiczenie: Azure DevOps od zera z prostym pipeline'em i demonstracją wycieku sekretu

## Cel ćwiczenia

W tym ćwiczeniu:

- zalogujesz się do `Azure DevOps`,
- upewnisz się, że pracujesz w tenantcie z poprzedniego ćwiczenia,
- utworzysz nową organizację,
- utworzysz nowy projekt,
- dodasz bardzo prosty kod aplikacji, który da się zbudować w pipeline,
- utworzysz `Environment` w Azure DevOps,
- skonfigurujesz sekret dla pipeline'u,
- uruchomisz pipeline i zobaczysz, że sekret można ujawnić w logach mimo tego, że został oznaczony jako `secret`.

To ćwiczenie jest celowo proste i jednocześnie trochę przewrotne: końcówka pokazuje zły wzorzec bezpieczeństwa, ale właśnie dlatego nadaje się na szkolenie.

## Ważna uwaga techniczna

Warto od razu skorygować jedno założenie, bo łatwo tu pomylić pojęcia:

- `Environment` w Azure DevOps to logiczny cel wdrożenia i historia deploymentów,
- `Environment` w Azure DevOps nie jest natywnym magazynem sekretów takim jak `environment secrets` w GitHub Actions,
- sekret w tym ćwiczeniu dodamy jako `secret variable` pipeline'u,
- sam deployment skierujemy do environment o nazwie `lab-dev`.

To jest technicznie poprawny wariant dla Azure DevOps Services.

## Zanim zaczniesz

Przygotuj:

- konto Microsoft albo konto służbowe/szkoleniowe, którego używałeś w poprzednim ćwiczeniu,
- prawdziwy adres e-mail tego konta,
- dostęp do tenantu z poprzedniego ćwiczenia,
- przeglądarkę internetową,
- kilka minut cierpliwości, bo pierwszy pipeline może chwilę się kolejkować.

Dobrze też zapisać sobie:

- nazwę tenantu z poprzedniego ćwiczenia,
- nazwę organizacji Azure DevOps,
- nazwę projektu,
- wartość sekretu używanego w pipeline.

## Krok 1. Zaloguj się do Azure DevOps i uzupełnij dane

1. Otwórz stronę:

   [https://aex.dev.azure.com/me?mkt=en-US](https://aex.dev.azure.com/me?mkt=en-US)

2. Zaloguj się kontem używanym na szkoleniu.
3. Jeżeli portal poprosi o uzupełnienie danych profilowych, wpisz dane zgodne z instrukcją szkoleniową.
4. Adres e-mail podaj prawdziwy, czyli taki, na który faktycznie masz dostęp albo który naprawdę należy do Twojego konta szkoleniowego.
5. Dokończ proces rejestracji lub pierwszego logowania.

Po tym kroku powinieneś móc wejść do swojego profilu Azure DevOps.

## Krok 2. Upewnij się, że wybrany jest tenant z poprzedniego ćwiczenia

To ważne, bo konto służbowe może być widoczne w więcej niż jednym tenantcie.

1. Jeśli podczas logowania Microsoft pokaże wybór organizacji lub katalogu, wybierz tenant używany w poprzednim ćwiczeniu.
2. Jeżeli Azure DevOps zaloguje Cię do niewłaściwego kontekstu, wyloguj się i zaloguj ponownie.
3. Po wejściu do Azure DevOps sprawdź, czy używasz właściwego konta i właściwego kontekstu szkoleniowego.

Jak to rozpoznać w praktyce:

- nazwa konta i adres e-mail zgadzają się z danymi szkoleniowymi,
- nie widzisz obcej organizacji lub starego środowiska testowego,
- jeśli później w ustawieniach organizacji będzie widoczna sekcja `Microsoft Entra ID`, to powinna wskazywać tenant z poprzedniego ćwiczenia.

Jeżeli nie masz pewności, zatrzymaj się tutaj i poproś prowadzącego o potwierdzenie. Lepiej poprawić to teraz niż budować organizację w złym tenantcie.

## Krok 3. Utwórz nową organizację

1. W Azure DevOps kliknij `Create a new organization` albo `New organization`.
2. Wpisz nazwę organizacji.
3. Wybierz geograficzną lokalizację hostingu. Dla szkolenia najczęściej sensowna będzie Europa, jeśli prowadzący nie podał innej.
4. Kliknij `Continue`.

Praktyczne wskazówki:

- nazwa organizacji musi być unikalna,
- najlepiej użyć prostego schematu, np. `ado-lab-imie-nazwisko`,
- jeżeli nazwa jest zajęta, dodaj krótki sufiks, np. numer albo inicjały.

Po poprawnym utworzeniu organizacji jej adres będzie wyglądał mniej więcej tak:

`https://dev.azure.com/twoja-organizacja`

## Krok 4. Utwórz projekt

1. Po wejściu do nowej organizacji kliknij `New project`.
2. Nazwę projektu wybierz dowolną, na przykład:

   `ado-lab`

3. Ustaw:

- `Visibility`: `Private`
- `Version control`: `Git`
- `Work item process`: `Basic`

4. Kliknij `Create`.

Dlaczego taki zestaw:

- `Private`, bo to najbezpieczniejszy domyślny wybór,
- `Git`, bo to standard dla współczesnych repozytoriów,
- `Basic`, bo dla początkującego jest prostszy niż bardziej rozbudowane procesy.

## Krok 5. Zainicjalizuj repozytorium

1. Wejdź do `Repos`.
2. Jeżeli projekt jest pusty, wybierz opcję inicjalizacji repozytorium z plikiem `README`.
3. Zatwierdź utworzenie repo.

Po tym kroku powinieneś mieć:

- repozytorium Git,
- domyślną gałąź, najczęściej `main`,
- pierwszy plik `README.md`.

Uwaga: jeśli Twoja domyślna gałąź nazywa się `master`, zapamiętaj to. W kroku z YAML trzeba będzie zamienić `main` na `master`.

## Krok 6. Dodaj prosty kod, który pipeline będzie budował

W tym ćwiczeniu użyjemy minimalnej aplikacji `Node.js`. To dobry wybór dla początkującego, bo:

- pliki są krótkie,
- build jest prosty,
- nie trzeba znać .NET ani Javy,
- łatwo pokazać, jak sekret trafia do procesu budowania.

### 6.1. Utwórz plik `package.json`

1. W `Repos` wybierz `Files`.
2. Kliknij `New file`.
3. Nazwij plik:

   `package.json`

4. Wklej poniższą zawartość:

```json
{
  "name": "ado-secret-demo",
  "version": "1.0.0",
  "private": true,
  "scripts": {
    "build": "node build.js"
  }
}
```

5. Zapisz plik bezpośrednio do domyślnej gałęzi.

### 6.2. Utwórz plik `build.js`

1. Ponownie kliknij `New file`.
2. Nazwij plik:

   `build.js`

3. Wklej poniższy kod:

```javascript
const fs = require('fs');

const secret = process.env.LAB_SECRET;

if (!secret) {
  console.error('LAB_SECRET environment variable is missing.');
  process.exit(1);
}

const splitPoint = Math.ceil(secret.length / 2);
const part1 = secret.slice(0, splitPoint);
const part2 = secret.slice(splitPoint);

console.log('Starting demo build...');
console.log(`Secret part 1: ${part1}`);
console.log(`Secret part 2: ${part2}`);

fs.mkdirSync('dist', { recursive: true });
fs.writeFileSync(
  'dist/build-report.txt',
  [
    'Build completed successfully.',
    `Secret part 1: ${part1}`,
    `Secret part 2: ${part2}`
  ].join('\n')
);

console.log('Artifact created: dist/build-report.txt');
```

4. Zapisz plik do repozytorium.

### Co robi ten kod

Ten skrypt:

- oczekuje zmiennej środowiskowej `LAB_SECRET`,
- kończy działanie błędem, jeśli sekretu nie ma,
- dzieli sekret na dwie części,
- wypisuje obie części do logu,
- zapisuje obie części także do pliku `dist/build-report.txt`.

To jest celowo zły wzorzec. W prawdziwym projekcie nie wolno tak robić.

## Krok 7. Utwórz environment w Azure DevOps

1. Przejdź do `Pipelines`.
2. Wybierz `Environments`.
3. Kliknij `Create environment`.
4. Jako nazwę wpisz:

   `lab-dev`

5. Utwórz pusty environment, bez dodawania zasobów.

To wystarczy do ćwiczenia. Azure DevOps zapisze potem historię deploymentów do `lab-dev`.

## Krok 8. Utwórz pipeline YAML

1. Przejdź do `Pipelines`.
2. Kliknij `New pipeline`.
3. Jako źródło kodu wybierz `Azure Repos Git`.
4. Wskaż bieżące repozytorium.
5. Jeżeli kreator zapyta o typ konfiguracji, wybierz `Starter pipeline`.
6. Usuń domyślną zawartość i wklej ten plik:

```yaml
trigger:
- main

pool:
  vmImage: ubuntu-latest

stages:
- stage: Build
  displayName: Build demo application
  jobs:
  - job: BuildJob
    displayName: Run demo build
    steps:
    - task: NodeTool@0
      displayName: Use Node.js 20
      inputs:
        versionSpec: '20.x'

    - script: |
        echo "Direct secret output should be masked:"
        echo "$LAB_SECRET"
      displayName: Check secret masking
      env:
        LAB_SECRET: $(labSecret)

    - script: npm run build
      displayName: Build app with secret
      env:
        LAB_SECRET: $(labSecret)

    - task: PublishPipelineArtifact@1
      displayName: Publish build output
      inputs:
        targetPath: dist
        artifact: drop

- stage: Deploy
  displayName: Record deployment to environment
  dependsOn: Build
  jobs:
  - deployment: DeployToDev
    displayName: Deploy to lab-dev
    environment: lab-dev
    strategy:
      runOnce:
        deploy:
          steps:
          - download: current
            artifact: drop
          - script: |
              echo "Deployment to environment lab-dev completed."
              echo "Artifact content:"
              cat "$(Pipeline.Workspace)/drop/build-report.txt"
            displayName: Show deployment artifact
```

7. Zapisz pipeline.

Jeżeli Twoja domyślna gałąź nazywa się `master`, to w sekcji `trigger` zamień:

```yaml
- main
```

na:

```yaml
- master
```

Na tym etapie jeszcze nie uruchamiaj pipeline'u, jeśli nie dodałeś sekretu. Pierwszy run skończy się błędem.

## Krok 9. Dodaj sekret do pipeline'u

To jest najważniejszy krok z punktu widzenia demonstracji bezpieczeństwa.

1. Otwórz zapisany pipeline.
2. Wejdź w jego edycję.
3. Odszukaj sekcję `Variables`.
4. Dodaj nową zmienną:

- `Name`: `labSecret`
- `Value`: wpisz własną wartość, np. `AdoLabSecret2026`

5. Zaznacz opcję `Keep this value secret` albo ikonę kłódki.
6. Zapisz zmiany.

Dlaczego robimy to tak:

- sekret nie trafia do repozytorium,
- YAML odwołuje się tylko do `$(labSecret)`,
- agent pipeline dostaje sekret jako zmienną środowiskową `LAB_SECRET`.

## Krok 10. Uruchom pipeline

1. Kliknij `Run pipeline`.
2. Potwierdź uruchomienie.
3. Poczekaj, aż zakończy się etap `Build`, a potem `Deploy`.

Jeżeli Azure DevOps poprosi o autoryzację dostępu do zasobu lub environment:

- kliknij `Permit` albo odpowiednik zgody,
- uruchom pipeline ponownie, jeśli będzie to konieczne.

## Krok 11. Przeanalizuj logi i odszukaj sekret

To jest sedno ćwiczenia.

### 11.1. Sprawdź krok `Check secret masking`

Wejdź do logu kroku:

`Check secret masking`

Powinieneś zobaczyć, że bezpośrednie wypisanie sekretu jest maskowane. Zamiast prawdziwej wartości zwykle widać:

`***`

To jest normalne zachowanie Azure DevOps dla secret variables.

### 11.2. Sprawdź krok `Build app with secret`

Teraz otwórz log kroku:

`Build app with secret`

Tam powinny pojawić się dwa wpisy podobne do tych:

- `Secret part 1: AdoLab`
- `Secret part 2: Secret2026`

Po połączeniu tych dwóch fragmentów dostajesz pełną wartość sekretu.

To właśnie pokazuje problem:

- sekret został poprawnie oznaczony jako `secret`,
- Azure DevOps zamaskował jego dokładną wartość przy prostym `echo`,
- ale kod aplikacji wypisał fragmenty sekretu,
- a Azure DevOps nie maskuje podciągów sekretu.

### 11.3. Sprawdź etap deploymentu

Otwórz etap:

`Deploy`

oraz krok:

`Show deployment artifact`

Zobaczysz tam zawartość pliku `build-report.txt`, czyli ponownie dwie części sekretu. Jednocześnie deployment zostanie zapisany w environment `lab-dev`.

## Krok 12. Sprawdź environment

1. Wejdź do `Pipelines` > `Environments`.
2. Otwórz `lab-dev`.
3. Sprawdź historię deploymentów.

Po tym kroku powinieneś widzieć:

- nazwę pipeline'u,
- numer uruchomienia,
- informację, że deployment został wykonany do `lab-dev`.

## Dlaczego to ćwiczenie jest ważne

To ćwiczenie uczy dwóch rzeczy naraz.

### Rzecz 1. Jak działa podstawowy przepływ w Azure DevOps

Masz już pełny mini-scenariusz:

- organizacja,
- projekt,
- repo,
- kod,
- pipeline,
- artifact,
- environment.

### Rzecz 2. Dlaczego samo oznaczenie zmiennej jako `secret` nie rozwiązuje problemu

To, że sekret jest ukryty w prostym logu, nie oznacza jeszcze bezpieczeństwa.

Jeżeli aplikacja albo skrypt:

- wypisze fragment sekretu,
- przekształci go,
- zapisze go do artefaktu,
- przekaże go do innego narzędzia, które pokaże go w logu,

to sekret nadal może zostać ujawniony.

## Checklista końcowa

Po zakończeniu ćwiczenia powinieneś mieć:

- konto zalogowane do Azure DevOps prawdziwym adresem e-mail,
- poprawnie wybrany tenant z poprzedniego ćwiczenia,
- nową organizację Azure DevOps,
- nowy projekt,
- repozytorium z plikami `package.json` i `build.js`,
- environment `lab-dev`,
- pipeline YAML,
- sekret `labSecret` zapisany jako `secret variable`,
- co najmniej jeden zakończony run pipeline'u,
- możliwość odtworzenia sekretu z logów przez połączenie dwóch fragmentów.

## Typowe problemy

### Nie mogę utworzyć organizacji

Najczęstsze powody:

- nazwa organizacji jest już zajęta,
- konto nie ma wymaganych uprawnień,
- organizacja została utworzona w innym tenantcie niż planowano.

Najprostsza poprawka:

- zmień nazwę na bardziej unikalną,
- sprawdź ponownie tenant,
- poproś prowadzącego o weryfikację uprawnień.

### Pipeline kończy się błędem, że `LAB_SECRET` nie istnieje

To zwykle znaczy, że:

- nie dodałeś zmiennej `labSecret`,
- nie została oznaczona jako sekret,
- pipeline został uruchomiony przed zapisaniem zmiennej.

### Pipeline nie startuje albo długo czeka w kolejce

To zwykle oznacza problem z agentem hostowanym albo limitem równoległych jobów. W nowej organizacji szkoleniowej czasami trzeba chwilę poczekać albo poprosić prowadzącego o pomoc.

### Environment istnieje, ale deployment nie przechodzi

Sprawdź:

- czy environment nazywa się dokładnie `lab-dev`,
- czy pipeline dostał zgodę na użycie tego zasobu,
- czy etap `Build` zakończył się powodzeniem.

### Nie widzę sekretu w logu

To akurat może oznaczać, że coś zrobiłeś lepiej niż w instrukcji.

Sprawdź wtedy:

- czy uruchomił się krok `Build app with secret`,
- czy plik `build.js` ma dokładnie taką treść jak w instrukcji,
- czy sekret ma więcej niż kilka znaków,
- czy analizujesz log kroku z buildem, a nie tylko krok z prostym `echo`.

## Co byłoby poprawnym wzorcem produkcyjnym

W prawdziwym projekcie:

- nie wypisuj sekretów ani ich fragmentów do logów,
- nie zapisuj sekretów do artefaktów,
- trzymaj sekrety poza repozytorium,
- najlepiej pobieraj je z dedykowanego systemu, np. `Azure Key Vault`,
- traktuj masking logów jako ostatnią warstwę ochrony, a nie główny mechanizm bezpieczeństwa.

## Źródła referencyjne

Instrukcja została oparta na aktualnej dokumentacji Microsoft Learn sprawdzonej 1 kwietnia 2026:

- [Sign up for Azure DevOps](https://learn.microsoft.com/en-us/azure/devops/user-guide/sign-up-invite-teammates?view=azure-devops)
- [Create an organization](https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/create-organization?view=azure-devops-2022)
- [Create a project in Azure DevOps](https://learn.microsoft.com/en-us/azure/devops/organizations/projects/create-project?view=azure-devops)
- [Create and target Azure DevOps environments](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/environments?view=azure-devops-2022)
- [Define variables](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/variables?view=azure-devops)
- [Set secret variables](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/set-secret-variables?view=azure-devops)
